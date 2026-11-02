using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Strategy;
using Facebook.Models;
using Facebook.Models.PhoneContacts;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.ContactsMessengerSync;
using Facebook.Services.Api.Methods.FetchChangedPhoneContacts;
using Facebook.Services.Api.Methods.FetchContactsById;
using Facebook.Services.Api.Methods.FetchFriends;
using Facebook.Services.Api.Methods.RemovePhoneContacts;
using Facebook.Services.Api.Methods.SyncContacts;
using Facebook.Services.Api.Methods.SyncFriends;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Friends;

public class Service : BackgroundService
{
	public enum SyncContactTypes
	{
		Full,
		Incremental
	}

	private const int RefetchIntervalMs = 86400000;

	private const int StaleIntervalMs = 3600000;

	private const int MaxContactSyncRequests = 3;

	private const int ContactSyncBatchSize = 50;

	private const int ContactSyncingBlockedErrorCode = 5700;

	private readonly ContactCache _contactCache;

	private FacebookContext _context;

	private bool _syncing;

	private readonly Facebook.Services.Api.Service _apiService;

	private CancellationTokenSource _cancelTokenSource;

	private readonly ExponentialFallback<Tuple<Facebook.Services.Api.Methods.SyncContacts.Request, Facebook.Services.Api.Service>, Facebook.Services.Api.Methods.SyncContacts.Response> _syncContactsWithRetries;

	public Service(IDispatcher ui, Facebook.Services.Api.Service service, ContactCache contactCache)
		: base(ui)
	{
		_apiService = service;
		_contactCache = contactCache;
		_cancelTokenSource = new CancellationTokenSource();
		Register<Facebook.Services.Api.Methods.FetchFriends.Request, Facebook.Services.Api.Methods.FetchFriends.Response>(CreateFetchFriendsTask);
		_syncContactsWithRetries = new ExponentialFallback<Tuple<Facebook.Services.Api.Methods.SyncContacts.Request, Facebook.Services.Api.Service>, Facebook.Services.Api.Methods.SyncContacts.Response>(SyncContactsWithRetries);
	}

	private async Task<Facebook.Services.Api.Methods.FetchFriends.Response> CreateFetchFriendsTask(Facebook.Services.Api.Methods.FetchFriends.Request request)
	{
		return await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchFriends.Request, Facebook.Services.Api.Methods.FetchFriends.Response>(request);
	}

	public async Task<List<FacebookUser>> SyncContacts(List<FBPhoneContact> contacts, SyncContactTypes syncType, Action<double> progressAction = null)
	{
		if (_syncing)
		{
			return null;
		}
		_syncing = true;
		List<FacebookUser> matchedContacts = ((syncType != SyncContactTypes.Full) ? (await SyncContactsIncremental(contacts, progressAction)) : (await SyncContactsFull(contacts, progressAction)));
		_syncing = false;
		return matchedContacts;
	}

	public async Task RemovePhoneContacts()
	{
		Facebook.Services.Api.Methods.RemovePhoneContacts.Request removeRequest = new Facebook.Services.Api.Methods.RemovePhoneContacts.Request();
		if ((await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.RemovePhoneContacts.Request, Facebook.Services.Api.Methods.RemovePhoneContacts.Response>(removeRequest)).FriendsUpdated)
		{
			Signal(new FriendsUpdatedSignal()).DoNotWait();
		}
	}

	public async Task<bool> ToggleContactsMessengerSync(bool enabled)
	{
		Facebook.Services.Api.Methods.ContactsMessengerSync.Request toggleRequest = new Facebook.Services.Api.Methods.ContactsMessengerSync.Request
		{
			Enabled = enabled,
			AccessToken = _context.Session.Credentials.AccessToken,
			CancelToken = _cancelTokenSource.Token
		};
		return (await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.ContactsMessengerSync.Request, BoolResponse>(toggleRequest)).Succeeded;
	}

	private async Task<StrategyResponse<Facebook.Services.Api.Methods.SyncContacts.Response>> SyncContactsWithRetries(int attempt, CancellationToken token, Tuple<Facebook.Services.Api.Methods.SyncContacts.Request, Facebook.Services.Api.Service> parameters)
	{
		if (attempt > 3)
		{
			Error error = Error.FromException(new TaskCanceledException("Max sync contacts retry attempts"));
			return StrategyResponse.Create(ExecutionResult.Cancelled, new Facebook.Services.Api.Methods.SyncContacts.Response
			{
				Error = error
			});
		}
		Facebook.Services.Api.Methods.SyncContacts.Request request = parameters.Item1;
		Facebook.Services.Api.Service service = parameters.Item2;
		Facebook.Services.Api.Methods.SyncContacts.Response response = await service.ExecuteAsync<Facebook.Services.Api.Methods.SyncContacts.Request, Facebook.Services.Api.Methods.SyncContacts.Response>(request);
		ExecutionResult state = ((response != null && (!response.HasError() || response.Error.Code == 5700)) ? ExecutionResult.Success : ExecutionResult.Failed);
		return StrategyResponse.Create(state, response);
	}

	private async Task<List<FacebookUser>> SyncContactsFull(List<FBPhoneContact> allContacts, Action<double> progressAction = null)
	{
		List<ContactChange> changes = GetChangesFromPhoneContacts(allContacts);
		List<string> matchedContactIds = await SendContactChanges(changes, progressAction);
		if (!matchedContactIds.Any())
		{
			return new List<FacebookUser>();
		}
		return await ProcessChangedContacts(matchedContactIds, SyncContactTypes.Full);
	}

	private async Task<List<FacebookUser>> SyncContactsIncremental(List<FBPhoneContact> allContacts, Action<double> progressAction = null)
	{
		Facebook.Services.Api.Methods.FetchChangedPhoneContacts.Request request = new Facebook.Services.Api.Methods.FetchChangedPhoneContacts.Request
		{
			PhoneContacts = allContacts,
			CancelToken = _cancelTokenSource.Token,
			AccessToken = _context.Session.Credentials.AccessToken
		};
		List<ContactChange> changes = GetChangesFromPhoneContacts((await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchChangedPhoneContacts.Request, Facebook.Services.Api.Methods.FetchChangedPhoneContacts.Response>(request)).ChangedContacts);
		List<string> matchedContactIds = await SendContactChanges(changes, progressAction);
		if (!matchedContactIds.Any())
		{
			return new List<FacebookUser>();
		}
		return await ProcessChangedContacts(matchedContactIds, SyncContactTypes.Incremental);
	}

	private async Task<List<string>> SendContactChanges(List<ContactChange> changes, Action<double> progressAction = null)
	{
		List<string> contactsWithFBAccount = new List<string>();
		int totalChanges = changes.Count;
		while (changes.Any())
		{
			List<ContactChange> batchChanges = changes.Take(50).ToList();
			changes.RemoveRange(0, batchChanges.Count);
			progressAction?.Invoke(100.0 * (double)(totalChanges - batchChanges.Count) / (double)totalChanges);
			Facebook.Services.Api.Methods.SyncContacts.Response response = await SyncContactsBatch(batchChanges);
			if (response == null)
			{
				return new List<string>();
			}
			if (response.HasError() || response.ContactChanges == null)
			{
				if (response.Error.Code == 5700)
				{
					_context.SetContactSyncEnabled(value: false);
					await RemovePhoneContacts();
				}
				return new List<string>();
			}
			contactsWithFBAccount.AddRange(response.ContactChanges.Values.Select((ContactChangeResult v) => v.RemoteContactId).ToList());
		}
		progressAction?.Invoke(100.0);
		return contactsWithFBAccount;
	}

	private async Task<List<FacebookUser>> ProcessChangedContacts(List<string> contactsWithFBAccount, SyncContactTypes contactType)
	{
		Facebook.Services.Api.Methods.FetchContactsById.Request fetchContactsRequest = new Facebook.Services.Api.Methods.FetchContactsById.Request
		{
			ContactIds = contactsWithFBAccount,
			RequestType = contactType,
			CancelToken = _cancelTokenSource.Token,
			AccessToken = _context.Session.Credentials.AccessToken
		};
		Facebook.Services.Api.Methods.FetchContactsById.Response addedContactsResponse = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchContactsById.Request, Facebook.Services.Api.Methods.FetchContactsById.Response>(fetchContactsRequest);
		if (!addedContactsResponse.HasError() && addedContactsResponse.FriendsUpdated)
		{
			Signal(new FriendsUpdatedSignal()).DoNotWait();
		}
		return addedContactsResponse.NewUsers;
	}

	private List<ContactChange> GetChangesFromPhoneContacts(List<FBPhoneContact> contacts)
	{
		return contacts.Select((FBPhoneContact contact) => new ContactChange
		{
			ChangeType = "add",
			ClientContactId = contact.ContactId,
			Contact = contact
		}).ToList();
	}

	private async Task<Facebook.Services.Api.Methods.SyncContacts.Response> SyncContactsBatch(List<ContactChange> batchChanges)
	{
		Facebook.Services.Api.Methods.SyncContacts.Request request = new Facebook.Services.Api.Methods.SyncContacts.Request
		{
			AccessToken = _context.Session.Credentials.AccessToken,
			CancelToken = _cancelTokenSource.Token,
			ContactChanges = batchChanges
		};
		return (await _syncContactsWithRetries.Execute(request.CancelToken, Tuple.Create(request, _apiService), 1000, 5000)).ReturnValue;
	}

	private async Task SyncFriends()
	{
		if (_syncing)
		{
			return;
		}
		_syncing = true;
		long lastFullSync = await _contactCache.GetLastFullSyncTime();
		long lastSync = await _contactCache.GetLastSyncTime();
		long now = DateExtensions.MSecsSinceEpoch();
		bool reachedFullRefetchInterval = Math.Abs(now - lastFullSync) >= 86400000;
		bool reachedIncrementalRefetchInterval = Math.Abs(now - lastSync) >= 3600000;
		if (!reachedFullRefetchInterval && !reachedIncrementalRefetchInterval)
		{
			_syncing = false;
			Signal(new FriendsSyncComplete()).DoNotWait();
			return;
		}
		if (reachedFullRefetchInterval ? (await SyncFull()) : (await SyncIncremental(lastSync)))
		{
			await SaveSuccessfulSync(reachedFullRefetchInterval);
		}
		Signal(new FriendsSyncComplete()).DoNotWait();
		_syncing = false;
	}

	private async Task<bool> SyncFull()
	{
		Facebook.Services.Api.Methods.SyncFriends.Request request = CreateSyncRequest();
		bool incrementalSyncReturnedResults;
		do
		{
			Facebook.Services.Api.Methods.SyncFriends.Response response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.SyncFriends.Request, Facebook.Services.Api.Methods.SyncFriends.Response>(request);
			if (response.HasError() || _cancelTokenSource.IsCancellationRequested)
			{
				return false;
			}
			incrementalSyncReturnedResults = response.NewFriends.Any();
			request.Offset += response.NewFriends.Count;
		}
		while (incrementalSyncReturnedResults);
		return true;
	}

	private async Task<bool> SyncIncremental(long lastUpdated)
	{
		Facebook.Services.Api.Methods.SyncFriends.Request request = CreateSyncRequest();
		request.LastUpdated = lastUpdated;
		return !(await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.SyncFriends.Request, Facebook.Services.Api.Methods.SyncFriends.Response>(request)).HasError() && !_cancelTokenSource.IsCancellationRequested;
	}

	private Facebook.Services.Api.Methods.SyncFriends.Request CreateSyncRequest()
	{
		Facebook.Services.Api.Methods.SyncFriends.Request request = new Facebook.Services.Api.Methods.SyncFriends.Request();
		request.AccessToken = _context.Session.Credentials.AccessToken;
		request.CancelToken = _cancelTokenSource.Token;
		request.Limit = Facebook.Services.Api.Methods.SyncFriends.Request.FetchUserSize;
		return request;
	}

	private async Task SaveSuccessfulSync(bool fullSync)
	{
		long syncTime = DateExtensions.MSecsSinceEpoch();
		if (fullSync)
		{
			await _contactCache.SetLastFullSyncTime(syncTime);
		}
		await _contactCache.SetLastSyncTime(syncTime);
		Signal(new FriendsUpdatedSignal()).DoNotWait();
	}

	public override void StartService(FacebookContext context)
	{
		_context = context;
		_cancelTokenSource = new CancellationTokenSource();
		Messenger.Default.Register<LoginSignal>(this, OnLogin);
		Messenger.Default.Register<LogoutSignal>(this, OnLogout);
		if (_context.IsAuthenticated())
		{
			SyncFriends().DoNotWait();
		}
	}

	private void OnLogin(LoginSignal signal)
	{
		_cancelTokenSource = new CancellationTokenSource();
		SyncFriends().DoNotWait();
	}

	private void OnLogout(LogoutSignal signal)
	{
		if (!_cancelTokenSource.IsCancellationRequested)
		{
			_cancelTokenSource.Cancel();
		}
	}

	public override void StopService()
	{
		Messenger.Default.Unregister(this);
	}
}

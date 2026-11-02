using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.AddContact;
using Facebook.Services.Api.Methods.DeleteContact;
using Facebook.Services.Api.Methods.FetchContactsById;
using Facebook.Services.Friends;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.UserData;

namespace Facebook.Messenger.WP8.Utilities.PhoneContacts;

public class PhoneContactsManager : FacebookMessengerViewModel
{
	private const int RefetchIntervalMs = 604800000;

	private const string LastContactSyncTimeKey = "LastContactSyncTime";

	private readonly Facebook.Services.Friends.Service _friendsService;

	private readonly ContactSyncFeatureEnabledProvider _contactSyncFeatureEnabledProvider;

	public ContactSyncStatus ContactSyncStatus;

	public event EventHandler<ContactSyncStatus> SyncStatusUpdate;

	public PhoneContactsManager(Facebook.Services.Api.Service apiService, Facebook.Services.Friends.Service friendsService, ContactSyncFeatureEnabledProvider contactSyncFeatureEnabledProvider, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
		_friendsService = friendsService;
		_contactSyncFeatureEnabledProvider = contactSyncFeatureEnabledProvider;
		ContactSyncStatus = new ContactSyncStatus();
		ContactSyncStatus.PropertyChanged += ContactSyncStatusOnPropertyChanged;
	}

	private void ContactSyncStatusOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
	{
		if (propertyChangedEventArgs.PropertyName == "Status" && SyncStatusUpdate != null)
		{
			SyncStatusUpdate(this, ContactSyncStatus);
		}
	}

	public void Register()
	{
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<FriendsSyncComplete>(this, OnFriendsSyncComplete);
	}

	private void OnFriendsSyncComplete(FriendsSyncComplete obj)
	{
		FetchPhoneContacts();
	}

	public async void StartContactSync()
	{
		ContactSyncStatus.OnContactSyncStarted();
		if (await _friendsService.ToggleContactsMessengerSync(enabled: true))
		{
			Context.SetContactSyncEnabled(value: true);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ContactSyncToggled(isEnabled: true));
			FetchPhoneContacts(force: true);
		}
		else
		{
			Logger.Error("Unable to turn on contacts messenger syncing", "StartContactSync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\PhoneContacts\\PhoneContactsManager.cs", 153);
			ContactSyncStatus.OnContactSyncCancelled();
		}
	}

	public void FetchPhoneContacts(bool force = false)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected Obj, but got Unknown
		if (_contactSyncFeatureEnabledProvider.Enabled && Context.IsContactSyncEnabled())
		{
			ContactSyncStatus.OnContactSyncStarted();
			Contacts val = new Contacts();
			val.SearchCompleted += OnContactFetchCompleted;
			val.SearchAsync(string.Empty, (FilterKind)0, (object)((!ContactsAreStale() && !force) ? Facebook.Services.Friends.Service.SyncContactTypes.Incremental : Facebook.Services.Friends.Service.SyncContactTypes.Full));
		}
	}

	private bool ContactsAreStale()
	{
		long setting = Context.Settings.GetSetting<long>("LastContactSyncTime");
		long num = DateExtensions.MSecsSinceEpoch();
		return num - setting > 604800000;
	}

	private async void OnContactFetchCompleted(object sender, ContactsSearchEventArgs e)
	{
		Contacts contacts = (Contacts)((sender is Contacts) ? sender : null);
		if (contacts != null)
		{
			contacts.SearchCompleted -= OnContactFetchCompleted;
		}
		Facebook.Services.Friends.Service.SyncContactTypes syncType = (Facebook.Services.Friends.Service.SyncContactTypes)Enum.Parse(typeof(Facebook.Services.Friends.Service.SyncContactTypes), e.State.ToString());
		List<FacebookUser> contactsSynced = await _friendsService.SyncContacts(e.Results.Select(PhoneContactUtils.CreatePhoneContact).ToList(), syncType, (double d) =>
		{
			Dispatcher.RunAsync(() =>
			{
				ContactSyncStatus.OnProgressUpdate(d);
			});
		});
		if (contactsSynced != null)
		{
			Context.Settings.SaveSetting("LastContactSyncTime", DateExtensions.MSecsSinceEpoch());
			ContactSyncStatus.OnContactSyncComplete(contactsSynced);
		}
	}

	public async void StopContactSync()
	{
		ContactSyncStatus.OnContactSyncStarted();
		if (await _friendsService.ToggleContactsMessengerSync(enabled: false))
		{
			Context.SetContactSyncEnabled(value: false);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ContactSyncToggled(isEnabled: false));
			await _friendsService.RemovePhoneContacts();
			ContactSyncStatus.OnContactSyncReset();
		}
		else
		{
			Logger.Error("Unable to stop contacts messenger syncing.", "StopContactSync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\PhoneContacts\\PhoneContactsManager.cs", 217);
			ContactSyncStatus.OnContactSyncCancelled();
		}
	}

	public async Task<Facebook.Services.Api.Methods.FetchContactsById.Response> AddPhoneContactAsync(string phoneNumber, string countryCode)
	{
		if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(countryCode))
		{
			return null;
		}
		Facebook.Services.Api.Methods.AddContact.Request request = new Facebook.Services.Api.Methods.AddContact.Request(phoneNumber, countryCode)
		{
			AccessToken = Context.Session.Credentials.AccessToken,
			CancelToken = CancelTokenSource.Token
		};
		Facebook.Services.Api.Methods.AddContact.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.AddContact.Request, Facebook.Services.Api.Methods.AddContact.Response>(request);
		if (response != null && !response.HasError() && !string.IsNullOrEmpty(response.ContactId))
		{
			Facebook.Services.Api.Methods.FetchContactsById.Request fetchRequest = new Facebook.Services.Api.Methods.FetchContactsById.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = CancelTokenSource.Token,
				ContactIds = new List<string> { response.ContactId },
				RequestType = Facebook.Services.Friends.Service.SyncContactTypes.Incremental
			};
			Facebook.Services.Api.Methods.FetchContactsById.Response fetchResponse = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchContactsById.Request, Facebook.Services.Api.Methods.FetchContactsById.Response>(fetchRequest);
			if (fetchResponse != null && !fetchResponse.HasError() && fetchResponse.FriendsUpdated)
			{
				GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new FriendsUpdatedSignal());
			}
			return fetchResponse;
		}
		Facebook.Services.Api.Methods.FetchContactsById.Response response2 = new Facebook.Services.Api.Methods.FetchContactsById.Response();
		response2.Error = Error.FromResponse(response);
		return response2;
	}

	public async Task DeletePhoneContactAsync(FacebookUser user)
	{
		Facebook.Services.Api.Methods.DeleteContact.Request deleteContactRequest = new Facebook.Services.Api.Methods.DeleteContact.Request
		{
			AccessToken = Context.Session.Credentials.AccessToken,
			CancelToken = CancelTokenSource.Token,
			ContactWriteId = user.GraphApiWriteId,
			ContactId = user.Id
		};
		await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.DeleteContact.Request, Facebook.Services.Api.Methods.DeleteContact.Response>(deleteContactRequest);
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new FriendsUpdatedSignal());
	}
}

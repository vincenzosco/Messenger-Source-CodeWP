using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchFriendLastActive;
using Facebook.Services.Api.Methods.FetchFriends;
using Facebook.Services.Api.Methods.FetchPresence;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Presence
{
	public class UserPresenceHelper
	{
		private const int PresenceExpiredIntervalMs = 480000;

		private const int PresenceStatusUpdateIntervalMs = 60000;

		private const int PresenceStaleIntervalMs = 300000;

		private readonly IDispatcher _uiDispatcher;

		private readonly Facebook.Services.Api.Service _apiService;

		private readonly AutoRenewCancellationTokenSource _presenceTimerCancelSource = new AutoRenewCancellationTokenSource();

		private readonly AutoRenewCancellationTokenSource _sessionCancelSource = new AutoRenewCancellationTokenSource();

		private FacebookContext _context;

		private HashSet<string> _onlineIds;

		private List<FacebookUser> _onlineUsers;

		private Dictionary<string, int> _uidToClientType;

		private Dictionary<string, long> _uidToLastActiveTime;

		private long _lastPresenceUpdate;

		private bool _fetchingPresence;

		public UserPresenceHelper(IDispatcher uiDispatcher, Facebook.Services.Api.Service apiService)
		{
			_uiDispatcher = uiDispatcher;
			_apiService = apiService;
			_onlineIds = new HashSet<string>();
			_onlineUsers = new List<FacebookUser>();
			_uidToClientType = new Dictionary<string, int>();
			_uidToLastActiveTime = new Dictionary<string, long>();
		}

		public void Start(FacebookContext context)
		{
			_context = context;
			ResetTimers();
		}

		public void Stop()
		{
			StopTimers();
		}

		public void Cleanup()
		{
			StopTimers();
			_sessionCancelSource.CancelAndRenew();
			_onlineIds.Clear();
			_onlineUsers.Clear();
			_uidToClientType.Clear();
			_uidToLastActiveTime.Clear();
			_lastPresenceUpdate = 0L;
		}

		public async Task<List<FacebookUser>> GetOnlineUsers()
		{
			await FetchAndUpdatePresence();
			return new List<FacebookUser>(_onlineUsers);
		}

		public async Task<LastActiveParams> GetLastActiveForUser(string id)
		{
			FacebookUser onlineUser = _onlineUsers.FirstOrDefault((FacebookUser u) => u.Id == id);
			if (onlineUser != null)
			{
				LastActiveParams lastActiveParams = new LastActiveParams();
				lastActiveParams.IsMessengerUser = onlineUser.IsMessengerUser;
				lastActiveParams.LastActive = onlineUser.LastActive ?? new LastActive
				{
					Active = true
				};
				return lastActiveParams;
			}
			return await FetchLastActive(id);
		}

		public async Task FetchAndUpdatePresence()
		{
			await UpdateAndSignalPresence(UpdateOnlineUsers);
			await UpdateAndSignalPresence(FetchPresence);
		}

		public async Task UpdatePresence(HashSet<string> onlineIds, HashSet<string> offlineIds, Dictionary<string, int> uidToClientType, Dictionary<string, long> uidToLastActiveTime, PresenceUpdateType type)
		{
			ResetTimers();
			_lastPresenceUpdate = DateExtensions.MSecsSinceEpoch();
			if (type == PresenceUpdateType.Incremental)
			{
				_onlineIds.UnionWith(onlineIds);
				_onlineIds.ExceptWith(offlineIds);
				foreach (KeyValuePair<string, int> item in uidToClientType)
				{
					_uidToClientType[item.Key] = item.Value;
				}
				foreach (KeyValuePair<string, long> item2 in uidToLastActiveTime)
				{
					_uidToLastActiveTime[item2.Key] = item2.Value;
				}
			}
			else
			{
				_onlineIds = onlineIds ?? new HashSet<string>();
				_uidToClientType = uidToClientType ?? new Dictionary<string, int>();
				_uidToLastActiveTime = uidToLastActiveTime ?? new Dictionary<string, long>();
			}
			await UpdateAndSignalPresence(UpdateOnlineUsers);
		}

		private async Task LoopTimer(int interval, Action action, CancellationToken token)
		{
			while (!token.IsCancellationRequested && !(await Task.Delay(interval, token).RunAndDetermineIfCancelled()))
			{
				action();
			}
		}

		private async Task LoopPresenceTimers()
		{
			CancellationToken token = _presenceTimerCancelSource.Token;
			Task presenceStatusTask = LoopTimer(60000, SignalPresenceStateIfEnabled, token);
			Task presenceExpiredTask = LoopTimer(480000, () =>
			{
				UpdateAndSignalPresence(FetchAndUpdatePresence).DoNotWait();
			}, token);
			await Task.WhenAll(presenceStatusTask, presenceExpiredTask);
		}

		private void SignalPresenceStateIfEnabled()
		{
			if (_context.IsChatEnabled())
			{
				Messenger.Default.Send(new ChatSettingSignal(true));
			}
		}

		private async Task UpdateAndSignalPresence(Func<Task> updateFunc)
		{
			await updateFunc();
			SignalPresenceChanged();
		}

		private void SignalPresenceChanged()
		{
			_uiDispatcher.RunAsync(() =>
			{
				Messenger.Default.Send(new OnlineUserSignal
				{
					OnlineIds = new HashSet<string>(_onlineIds),
					OnlineUsers = new List<FacebookUser>(_onlineUsers)
				});
			});
		}

		private void StopTimers()
		{
			_presenceTimerCancelSource.CancelAndRenew();
		}

		private void ResetTimers()
		{
			StopTimers();
			LoopPresenceTimers().DoNotWait();
		}

		private async Task FetchPresence()
		{
			if (_fetchingPresence || !_context.IsAuthenticated())
			{
				return;
			}
			long now = DateExtensions.MSecsSinceEpoch();
			long timeSinceLastUpdate = Math.Abs(now - _lastPresenceUpdate);
			if (timeSinceLastUpdate < 300000)
			{
				return;
			}
			_fetchingPresence = true;
			Facebook.Services.Api.Methods.FetchPresence.Request request = new Facebook.Services.Api.Methods.FetchPresence.Request
			{
				AccessToken = _context.Session.Credentials.AccessToken,
				CancelToken = _sessionCancelSource.Token
			};
			Facebook.Services.Api.Methods.FetchPresence.Response response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchPresence.Request, Facebook.Services.Api.Methods.FetchPresence.Response>(request);
			if (!response.HasError())
			{
				ResetTimers();
				_lastPresenceUpdate = DateExtensions.MSecsSinceEpoch();
				_onlineIds = new HashSet<string>(response.OnlineProfiles.Select((FacebookUser p) => p.Id));
				await UpdateOnlineUsers();
			}
			_fetchingPresence = false;
		}

		private async Task<LastActiveParams> FetchLastActive(string id)
		{
			Facebook.Services.Api.Methods.FetchFriendLastActive.Request request = new Facebook.Services.Api.Methods.FetchFriendLastActive.Request
			{
				Id = id,
				CancelToken = _sessionCancelSource.Token,
				AccessToken = _context.Session.Credentials.AccessToken,
				ForceToServer = true
			};
			Facebook.Services.Api.Methods.FetchFriendLastActive.Response response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchFriendLastActive.Request, Facebook.Services.Api.Methods.FetchFriendLastActive.Response>(request);
			if (response.HasError())
			{
				return null;
			}
			List<LastActiveParams> lastActiveInfo = response.LastActiveInfo;
			if (lastActiveInfo == null || !lastActiveInfo.Any() || lastActiveInfo[0].LastActive == null)
			{
				return null;
			}
			LastActiveParams lastActiveParams = lastActiveInfo[0];
			if (lastActiveParams.LastActive != null && lastActiveParams.LastActive.Active && _onlineIds.Add(id))
			{
				_uidToLastActiveTime[id] = lastActiveParams.LastActive.Timestamp;
				UpdateOnlineUsers().DoNotWait();
			}
			return lastActiveParams;
		}

		private async Task UpdateOnlineUsers()
		{
			Facebook.Services.Api.Methods.FetchFriends.Request friendRequest = new Facebook.Services.Api.Methods.FetchFriends.Request();
			Facebook.Services.Api.Methods.FetchFriends.Response response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchFriends.Request, Facebook.Services.Api.Methods.FetchFriends.Response>(friendRequest);
			if (response.HasError())
			{
				return;
			}
			_onlineUsers = response.Friends.Where((FacebookUser f) => _onlineIds.Contains(f.Id)).ToList();
			foreach (FacebookUser onlineUser in _onlineUsers)
			{
				if (_uidToClientType.ContainsKey(onlineUser.Id))
				{
					onlineUser.ClientType = _uidToClientType[onlineUser.Id];
				}
			}
		}
	}
}

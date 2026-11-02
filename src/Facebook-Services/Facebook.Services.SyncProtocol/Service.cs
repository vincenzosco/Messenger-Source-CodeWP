using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Mqtt;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Models.SyncProtocol;
using Facebook.Models.Utilities;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.FetchInitialThreadListSyncData;
using Facebook.Services.Api.Methods.FetchThreads;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.SyncProtocol
{
	public class Service : BackgroundService
	{
		private const double IgnoreNotificationDurationMs = 10000.0;

		private const int FirstSyncingDurationMs = 5000;

		private readonly DisposableMutex _syncMutex = new DisposableMutex();

		private readonly Facebook.Services.Api.Service _apiService;

		private readonly ThreadCache _threadCache;

		private readonly DeltaHandlerManager _deltaHandlerManager;

		private readonly AutoRenewCancellationTokenSource _timerCancelTokenSource = new AutoRenewCancellationTokenSource();

		private readonly ApplicationState _applicationState;

		private CancellationTokenSource _cancellationToken;

		private FacebookContext _context;

		private SyncConnectionHelper _syncConnectionHelper;

		public Service(IDispatcher ui, Facebook.Services.Api.Service apiService, ThreadCache threadCache, LoggedInUserUtils loggedInUserUtils, ApplicationState applicationState, SyncConnectionHelper syncConnectionHelper)
			: base(ui)
		{
			_apiService = apiService;
			_threadCache = threadCache;
			_applicationState = applicationState;
			_syncConnectionHelper = syncConnectionHelper;
			_deltaHandlerManager = new DeltaHandlerManager(threadCache, apiService, loggedInUserUtils);
		}

		public override void StartService(FacebookContext context)
		{
			_context = context;
			Messenger.Default.Register<LoginSignal>(this, OnLogin);
			Messenger.Default.Register(this, async (LogoutSignal signal) =>
			{
				await OnLogout(signal);
			});
			Messenger.Default.Register(this, async (SyncMessageSignal signal) =>
			{
				await OnSyncMessage(signal);
			});
			Messenger.Default.Register(this, async (MqttConnectionStateChangedSignal signal) =>
			{
				await OnMqttConnectionStateChanged(signal);
			});
			ScheduleConnect();
		}

		public override async void StopService()
		{
			Messenger.Default.Unregister(this);
			using (await _syncMutex.WaitAsync())
			{
				UnlockedDisconnect("Stopping service");
			}
		}

		public async Task SimulateSyncError()
		{
			using (await _syncMutex.WaitAsync())
			{
				await UnlockedHandleError("Simulating sync error");
			}
		}

		private void OnLogin(LoginSignal signal)
		{
			ScheduleConnect();
		}

		private async Task OnLogout(LogoutSignal signal)
		{
			using (await _syncMutex.WaitAsync())
			{
				await UnlockedTeardownSync();
			}
		}

		private async Task UnlockedTeardownSync()
		{
			UnlockedDisconnect("Tearing down, need to reset everything");
			await _threadCache.ClearInboxData();
			SyncProtocolSettings.ClearSystemSyncSettings(_context);
		}

		private async Task OnMqttConnectionStateChanged(MqttConnectionStateChangedSignal mqttSignal)
		{
			using (await _syncMutex.WaitAsync())
			{
				switch (mqttSignal.State)
				{
				case ClientState.Disconnected:
					UnlockedDisconnect(string.Format("MQTT state changed to: {0}", new object[1] { mqttSignal.State }));
					break;
				case ClientState.Connected:
					ScheduleConnect();
					break;
				}
			}
		}

		private void ScheduleConnect()
		{
			Task.Run((Func<Task>)Connect).DoNotWait();
		}

		private async Task Connect()
		{
			using (await _syncMutex.WaitAsync())
			{
				if (_applicationState.IsShuttingDown || !UnlockedCanConnect())
				{
					return;
				}
				if (_cancellationToken != null && !_cancellationToken.IsCancellationRequested)
				{
					Logger.Error(this, "Creating new token but old is not null or cancelled", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 139);
				}
				_cancellationToken = new CancellationTokenSource();
				_syncConnectionHelper.SyncConnectionState = SyncConnectionState.Connecting;
				long largestSequenceId = await UnlockedAttemptToGetLargestSequenceId();
				if (!SequenceIdUtil.IsValidSequenceId(largestSequenceId))
				{
					Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Response initialThreads = await FetchInitialThreads();
					if (initialThreads.HasError() || initialThreads.Count > 0)
					{
						UnlockedDisconnect("Connect failed, do not have valid sequence Id");
						return;
					}
					largestSequenceId = initialThreads.SyncSequenceId;
				}
				if (await _syncConnectionHelper.ConnectToSync(largestSequenceId, _cancellationToken.Token))
				{
					Logger.Info(this, "Puback received, Sync Connected", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 160);
					SyncStateTimer().DoNotWait();
				}
				else
				{
					UnlockedDisconnect(string.Format("Failed to connect, unable to start queue"));
				}
			}
		}

		private async Task<Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Response> FetchInitialThreads()
		{
			Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Request request = new Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Request
			{
				AccessToken = _context.Session.Credentials.AccessToken
			};
			return await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Request, Facebook.Services.Api.Methods.FetchInitialThreadListSyncData.Response>(request);
		}

		private bool UnlockedCanConnect()
		{
			if (!_context.IsAuthenticated())
			{
				Logger.Info(this, "Not connecting to sync - no session", "UnlockedCanConnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 184);
				return false;
			}
			if (_syncConnectionHelper.SyncConnectionState != SyncConnectionState.Disconnected)
			{
				Logger.Info(this, "Not connecting to sync - not disconnected", "UnlockedCanConnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 190);
				return false;
			}
			return true;
		}

		private async Task<long> UnlockedAttemptToGetLargestSequenceId()
		{
			long largestSequenceId = await _threadCache.GetLargestSequenceId();
			if (!SequenceIdUtil.IsValidSequenceId(largestSequenceId))
			{
				Logger.Warning(this, "Sync sequence ID is invalid attempting fetch before creating queue", "UnlockedAttemptToGetLargestSequenceId", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 203);
				Facebook.Services.Api.Methods.FetchThreads.Request fetchThreadsRequest = new Facebook.Services.Api.Methods.FetchThreads.Request
				{
					AccessToken = _context.Session.Credentials.AccessToken,
					ForceToServer = true,
					RequestFromSyncProtocolService = true
				};
				if ((await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchThreads.Request, Facebook.Services.Api.Methods.FetchThreads.Response>(fetchThreadsRequest)).HasError())
				{
					Logger.Error(this, "Failed to fetch messages, sync will be broken", "UnlockedAttemptToGetLargestSequenceId", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 215);
				}
				largestSequenceId = await _threadCache.GetLargestSequenceId();
				if (!SequenceIdUtil.IsValidSequenceId(largestSequenceId))
				{
					Logger.Error(this, "Sync sequence ID is still invalid, giving up", "UnlockedAttemptToGetLargestSequenceId", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 221);
				}
			}
			return largestSequenceId;
		}

		private void UnlockedDisconnect(string reason)
		{
			Logger.Info(this, string.Format("Disconnecting Sync, reason: {0}", new object[1] { reason }), "UnlockedDisconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 230);
			if (_syncConnectionHelper.SyncConnectionState == SyncConnectionState.Disconnected)
			{
				Logger.Info(this, "Skipping disconnect, already disconnected", "UnlockedDisconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 234);
				return;
			}
			_cancellationToken.Cancel();
			_syncConnectionHelper.SyncConnectionState = SyncConnectionState.Disconnected;
		}

		private async Task OnSyncMessage(SyncMessageSignal signal)
		{
			Logger.Info(this, string.Format("Delta Payload Received"), "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 244);
			using (await _syncMutex.WaitAsync())
			{
				if (_applicationState.IsShuttingDown)
				{
					return;
				}
				Logger.Info(this, string.Format("Processing payload"), "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 253);
				ClientPayload payload = signal.Payload;
				if (_syncConnectionHelper.SyncConnectionState == SyncConnectionState.Disconnected)
				{
					Logger.Warning(this, "Sync received payload but not connected or connecting", "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 259);
				}
				else if (!string.IsNullOrEmpty(payload.ErrorCode))
				{
					Logger.Error(this, "Sync error code received", "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 265);
					await UnlockedHandleError(payload.ErrorCode);
				}
				else
				{
					if (!_syncConnectionHelper.ValidatePayloadToken(payload.SyncToken))
					{
						return;
					}
					if (payload.Deltas == null)
					{
						Logger.Error(this, "No deltas received", "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 278);
						return;
					}
					Logger.Info(this, string.Format("Handling Delta Payload - Delta Count: {0}", new object[1] { payload.Deltas.Count }), "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 282);
					_timerCancelTokenSource.CancelAndRenew();
					bool isFirstBulkPayloadAfterConnection = (double)DateTime.UtcNow.ToUnixTimeMilliseconds() < (double)_syncConnectionHelper.LastConnectedTimeStamp.ToUnixTimeMilliseconds() + 10000.0;
					DeltaHandlerManager.Result result = await _deltaHandlerManager.HandleDeltas(payload.Deltas, payload.FirstDeltaSeqId, _context.Session.Credentials.AccessToken, _cancellationToken.Token, _syncConnectionHelper.SyncConnectionState, !isFirstBulkPayloadAfterConnection);
					if (result == DeltaHandlerManager.Result.Success)
					{
						Logger.Info(this, "Successfully processed all deltas", "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 297);
						_syncConnectionHelper.SyncConnectionState = SyncConnectionState.Connected;
						return;
					}
					switch (result)
					{
					case DeltaHandlerManager.Result.RecreateQueue:
						Logger.Info(this, "Something unexpected happened, restarting queue.", "OnSyncMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 307);
						await UnlockedResetQueue();
						break;
					case DeltaHandlerManager.Result.ResetEverything:
						await UnlockedHandleError("Failure handling payload");
						break;
					default:
						throw new NotImplementedException(string.Concat("Result state ", result, "cannot be handled"));
					case DeltaHandlerManager.Result.Success:
						break;
					}
				}
			}
		}

		private async Task UnlockedHandleError(string errorMessage)
		{
			Logger.Error(this, errorMessage, "UnlockedHandleError", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 321);
			await UnlockedTeardownSync();
			ScheduleConnect();
		}

		private async Task UnlockedResetQueue()
		{
			await UnlockedTeardownSync();
			ScheduleConnect();
		}

		private async Task SyncStateTimer()
		{
			CancellationToken canceltoken = _timerCancelTokenSource.Token;
			await Task.Delay(5000, canceltoken);
			using (await _syncMutex.WaitAsync())
			{
				if (!canceltoken.IsCancellationRequested && _syncConnectionHelper.SyncConnectionState == SyncConnectionState.Syncing)
				{
					Logger.Info(this, string.Format("Syncing state timeout fired, no payload received after {0}ms", new object[1] { 5000 }), "SyncStateTimer", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Service.cs", 341);
					_syncConnectionHelper.SyncConnectionState = SyncConnectionState.Connected;
				}
			}
		}
	}
}

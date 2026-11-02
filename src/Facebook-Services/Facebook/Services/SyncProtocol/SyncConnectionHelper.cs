using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Mqtt;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Mqtt;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.SyncProtocol
{
	public class SyncConnectionHelper
	{
		private const int SyncApiVersion = 2;

		private const int SyncMaxConnectionRetryAttempts = 5;

		private readonly Facebook.Services.Mqtt.Service _mqttService;

		private readonly IDispatcher _uiDispatcher;

		private readonly FacebookContext _context;

		private SyncConnectionState _syncConnectionState;

		public DateTime LastConnectedTimeStamp { get; private set; }

		public SyncConnectionState SyncConnectionState
		{
			get
			{
				return _syncConnectionState;
			}
			set
			{
				if (_syncConnectionState != value)
				{
					_syncConnectionState = value;
					_uiDispatcher.RunAsync(() =>
					{
						Messenger.Default.Send(new SyncConnectionStateChangedSignal(_syncConnectionState));
					});
				}
			}
		}

		public SyncConnectionHelper(IDispatcher ui, Facebook.Services.Mqtt.Service mqttService, FacebookContext context)
		{
			_uiDispatcher = ui;
			_mqttService = mqttService;
			_context = context;
		}

		public bool ValidatePayloadToken(string payloadToken)
		{
			SyncProtocolSettings syncSettings = SyncProtocolSettings.GetSyncSettings(_context, 2);
			bool flag = !string.IsNullOrWhiteSpace(syncSettings.SyncToken);
			bool flag2 = !string.IsNullOrWhiteSpace(payloadToken);
			if (!flag && !flag2)
			{
				Logger.Info(this, "We don't have a sync token and neither does the payload so we're dropping it as it may be cancelled", "ValidatePayloadToken", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 57);
				return false;
			}
			if (flag2)
			{
				if (flag && !syncSettings.SyncToken.Equals(payloadToken))
				{
					Logger.Error(this, string.Format("We just got a sync token even though we already have a current one. Existing ({0}) and new ({1}).", new object[2] { syncSettings.SyncToken, payloadToken }), "ValidatePayloadToken", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 66);
					return false;
				}
				Logger.Info(this, "Setting SyncToken and Connecting", "ValidatePayloadToken", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 70);
				SyncProtocolSettings syncProtocolSettings = new SyncProtocolSettings(payloadToken, 2);
				syncProtocolSettings.SaveSyncSettings(_context);
			}
			return true;
		}

		public async Task<bool> ConnectToSync(long largestSequenceId, CancellationToken token)
		{
			SyncProtocolSettings syncSettings = SyncProtocolSettings.GetSyncSettings(_context, 2);
			bool hasSyncVersionChanged = syncSettings.SyncApiVersion != 2;
			bool hasValidQueue = !string.IsNullOrWhiteSpace(syncSettings.SyncToken) || hasSyncVersionChanged;
			if (hasSyncVersionChanged)
			{
				SyncProtocolSettings.ClearSystemSyncSettings(_context);
				syncSettings = SyncProtocolSettings.GetSyncSettings(_context, 2);
			}
			for (int numAttempt = 0; numAttempt < 5; numAttempt++)
			{
				Task<bool> queueTask = GetQueueTask(hasValidQueue, largestSequenceId, syncSettings.SyncToken, token);
				if (await queueTask)
				{
					SyncConnectionState = SyncConnectionState.Syncing;
					LastConnectedTimeStamp = DateTime.UtcNow;
					return true;
				}
			}
			return false;
		}

		private Task<bool> GetQueueTask(bool hasValidQueue, long largestSequenceId, string syncToken, CancellationToken token)
		{
			return hasValidQueue ? ResumeQueue(largestSequenceId, syncToken, token) : CreateQueue(largestSequenceId, token);
		}

		private async Task<bool> ResumeQueue(long largestSequenceId, string syncToken, CancellationToken token)
		{
			CreateSyncDiffsRequest request = new CreateSyncDiffsRequest(syncToken, largestSequenceId);
			SyncResponse response = await Task.Run(() => _mqttService.ExecuteAsync<CreateSyncDiffsRequest, SyncResponse>(request), token);
			if (response.HasError())
			{
				SyncResponse.FailureReason reason = response.Reason;
				if (reason == SyncResponse.FailureReason.NotConnected)
				{
					Logger.Warning(this, "[SYNC] Create sync diffs failed because we're not connected to MQTT", "ResumeQueue", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 120);
				}
				else
				{
					Logger.Error(this, string.Format("[SYNC] Create sync diffs (resume) operation failed, reason: {0}, error: {1}", new object[2] { response.Reason, response.Error }), "ResumeQueue", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 123);
				}
			}
			else if (token.IsCancellationRequested)
			{
				Logger.Error(this, $"[SYNC] Create sync diffs (resume) operation cancelled", "ResumeQueue", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 129);
				return false;
			}
			return !response.HasError();
		}

		private async Task<bool> CreateQueue(long largestSequenceId, CancellationToken token)
		{
			CreateSyncQueueRequest request = new CreateSyncQueueRequest(largestSequenceId, 2);
			SyncResponse response = await Task.Run(() => _mqttService.ExecuteAsync<CreateSyncQueueRequest, SyncResponse>(request), token);
			if (response.HasError())
			{
				SyncResponse.FailureReason reason = response.Reason;
				if (reason == SyncResponse.FailureReason.NotConnected)
				{
					Logger.Warning(this, "[SYNC] Create sync queue failed because we're not connected to MQTT", "CreateQueue", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 146);
				}
				else
				{
					Logger.Error(this, string.Format("[SYNC] Create sync queue (create) operation failed, reason: {0}, error: {1}", new object[2] { response.Reason, response.Error }), "CreateQueue", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 149);
				}
			}
			else if (token.IsCancellationRequested)
			{
				Logger.Error(this, $"[SYNC] Create sync diffs (create) operation cancelled", "CreateQueue", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\SyncConnectionHelper.cs", 155);
				return false;
			}
			return !response.HasError();
		}
	}
}

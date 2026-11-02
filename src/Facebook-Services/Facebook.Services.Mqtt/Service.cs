using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Mqtt;
using Facebook.Base.Mqtt.Messages;
using Facebook.Base.Mqtt.Serialization.Payloads;
using Facebook.Base.Network;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Analytics;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.Mqtt.Sync;
using Facebook.Services.Mqtt.TopicSubscriptions;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;

namespace Facebook.Services.Mqtt
{
	public class Service : BackgroundService
	{
		private const bool UseCompressPublishPayload = true;

		private const string PublishFormatCompressed = "JSON+zlib";

		private const string PublishFormatUncompressed = "JSON";

		private const int MqttPublishMessageTimeoutMs = 10000;

		private static readonly Error ErrorNotConnected = new Error
		{
			Message = "Client is not connected"
		};

		private readonly Facebook.Services.Analytics.Service _analyticsService;

		private readonly IUserAgentGenerator _userAgentGenerator;

		private readonly AbstractMqttClient _mqttClient;

		private readonly INetworkStatus _network;

		private readonly ApplicationState _applicationState;

		private readonly PublishAcknowledgementAwaiter _publishAcknowledgementAwaiter;

		private readonly DeltaSentMessageEnabledProvider _deltaSentMessageEnabledProvider;

		private readonly ConnectionStrategy _connectionStategy;

		private readonly AutoResetEvent _reconnectionWaitEvent = new AutoResetEvent(false);

		private readonly SendMessageResponse _sendMessageResponseSubscription;

		private readonly IDictionary<string, Subscription> _mqttSubscriptions;

		private FacebookContext _context;

		private readonly AutoRenewCancellationTokenSource _cancellationTokenSource = new AutoRenewCancellationTokenSource();

		private bool _attemptingReconnected;

		private bool _isSubscribed;

		private void RegisterSyncMethods()
		{
			Register<CreateSyncQueueRequest, SyncResponse>(CreateSyncQueueTask);
			Register<CreateSyncDiffsRequest, SyncResponse>(CreateSyncDiffsTasks);
		}

		private async Task<SyncResponse> CreateSyncQueueTask(CreateSyncQueueRequest request)
		{
			Logger.Info(this, string.Format("Attempting to create sync queue with sequence id: {0}, api version: {1}", new object[2] { request.LargestSequenceId, request.SyncApiVersion }), "CreateSyncQueueTask", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service-Sync.cs", 22);
			return await ExecuteSyncPublish("/messenger_sync_create_queue", new CreateQueueMessage(request.LargestSequenceId, request.SyncApiVersion));
		}

		private async Task<SyncResponse> CreateSyncDiffsTasks(CreateSyncDiffsRequest request)
		{
			Logger.Info(this, string.Format("Attempting to get diffs with sync token \"{0}\" and sequence id \"{1}\"", new object[2] { request.SyncToken, request.LastSequenceId }), "CreateSyncDiffsTasks", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service-Sync.cs", 28);
			return await ExecuteSyncPublish("/messenger_sync_get_diffs", new CreateSyncDiffsMessage(request.SyncToken, request.LastSequenceId));
		}

		private async Task<SyncResponse> ExecuteSyncPublish(string topic, object payloadObject)
		{
			if (!IsConnected())
			{
				return new SyncResponse(SyncResponse.FailureReason.NotConnected, ErrorNotConnected);
			}
			try
			{
				string payload = JsonConvert.SerializeObject(payloadObject);
				return (await SendMessageAndWait(topic, payload, QualityofService.AcknowledgedDelivery)) ? new SyncResponse() : new SyncResponse(SyncResponse.FailureReason.NoPubAckReceived, new Error
				{
					Message = "Failed to receive a PubAck"
				});
			}
			catch (Exception exception)
			{
				return new SyncResponse(SyncResponse.FailureReason.Exception, new Error
				{
					Exception = exception
				});
			}
		}

		private async Task<Facebook.Services.Api.Methods.SendMessage.Response> CreateSendMessageTask(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			if (!IsConnected())
			{
				Facebook.Services.Api.Methods.SendMessage.Response response = new Facebook.Services.Api.Methods.SendMessage.Response();
				response.Error = new Error
				{
					Message = "Client is not connected"
				};
				return response;
			}
			StringWriter sw = new StringWriter();
			JsonTextWriter jw = new JsonTextWriter(sw);
			string to = (request.HasSingleRecipientId() ? request.SingleRecipientId : request.ServerThreadId);
			jw.WriteStartObject();
			jw.WriteProperty("to", to);
			jw.WriteProperty("body", request.Message);
			jw.WriteProperty("msgid", request.OfflineThreadingId);
			if (!string.IsNullOrWhiteSpace(request.ObjectAttachmentId))
			{
				jw.WriteProperty("object_attachment", request.ObjectAttachmentId);
			}
			if (request.Location != null && request.Location.IsValidLocation())
			{
				jw.WritePropertyName("coordinates");
				jw.WriteRawValue(request.Location.CreateLocationObject());
			}
			if (request.CopyMessageId != null)
			{
				jw.WriteProperty("copy_message", request.CopyMessageId);
			}
			if (request.CopyAttachmentId != null)
			{
				jw.WriteProperty("copy_attachment", request.CopyAttachmentId);
			}
			jw.WriteEndObject();
			string payload = sw.ToString();
			try
			{
				if (!(await SendMessageAndWait("/send_message2", payload, QualityofService.AcknowledgedDelivery)))
				{
					Logger.Error(this, "Publishing message failed", "CreateSendMessageTask", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service-Messages.cs", 69);
					Facebook.Services.Api.Methods.SendMessage.Response response2 = new Facebook.Services.Api.Methods.SendMessage.Response();
					response2.Error = new Facebook.Base.Error
					{
						Message = "Failed to receive a PubAck"
					};
					return response2;
				}
				return CreateResponse(await _sendMessageResponseSubscription.WaitForResponse(request.OfflineThreadingId, _cancellationTokenSource.Token), request);
			}
			catch (Exception ex)
			{
				Logger.Error(this, ex.ToString(), "CreateSendMessageTask", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service-Messages.cs", 84);
				Facebook.Services.Api.Methods.SendMessage.Response response3 = new Facebook.Services.Api.Methods.SendMessage.Response();
				response3.Error = Facebook.Base.Error.FromException(ex);
				return response3;
			}
		}

		private Facebook.Services.Api.Methods.SendMessage.Response CreateResponse(SendResponse sendResponse, Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			Facebook.Services.Api.Methods.SendMessage.Response response = new Facebook.Services.Api.Methods.SendMessage.Response();
			if (sendResponse == null)
			{
				response.Error = new Facebook.Base.Error
				{
					Message = "No message response was received from mqtt"
				};
			}
			else if (sendResponse.Succeeded)
			{
				response.MessageId = sendResponse.MessageId;
				response.OfflineId = request.OfflineThreadingId;
				response.ThreadId = request.ServerThreadId;
			}
			else if (!sendResponse.Succeeded)
			{
				Logger.Error(this, string.Format("Message failed to send via api {0}", new object[1] { sendResponse.ErrNo }), "CreateResponse", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service-Messages.cs", 108);
				response.Error = new Facebook.Base.Error
				{
					Message = "Message failed to send over mqtt"
				};
			}
			return response;
		}

		private void OnTyping(MessageTypingSentSignal signal)
		{
			if (IsConnected())
			{
				StringWriter stringWriter = new StringWriter();
				JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
				jsonTextWriter.WriteStartObject();
				jsonTextWriter.WriteProperty("to", signal.ToId);
				jsonTextWriter.WriteProperty("state", signal.State ? 1 : 0);
				jsonTextWriter.WriteEndObject();
				string payload = stringWriter.ToString();
				Logger.Info(this, string.Format("Sending MQTT typing {0} to {1}", new object[2]
				{
					signal.State ? "start" : "end",
					signal.ToId
				}), "OnTyping", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service-Messages.cs", 130);
				SendMessageAndWait("/typing", payload, QualityofService.FireAndForget).DoNotWait();
			}
		}

		private void OnChatSettingChanged(ChatSettingSignal chatSettingSignal)
		{
			SendMessageAndWait("/set_client_settings", "{\"make_user_available_when_in_foreground\": " + (chatSettingSignal.IsChatEnabled ? 1 : 0) + "}", QualityofService.AcknowledgedDelivery).DoNotWait();
		}

		public Service(IDispatcher ui, IUserAgentGenerator userAgentGenerator, Facebook.Services.Analytics.Service analytics, DeltaSentMessageEnabledProvider deltaSentMessageEnabledProvider, AbstractMqttClient client, INetworkStatus network, ApplicationState applicationState)
			: base(ui)
		{
			_userAgentGenerator = userAgentGenerator;
			_analyticsService = analytics;
			_applicationState = applicationState;
			_mqttClient = client;
			_network = network;
			_deltaSentMessageEnabledProvider = deltaSentMessageEnabledProvider;
			_connectionStategy = new ConnectionStrategy();
			_attemptingReconnected = false;
			Register<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(CreateSendMessageTask);
			RegisterSyncMethods();
			_mqttSubscriptions = CreateSubscriptions(UiDispatcher);
			_publishAcknowledgementAwaiter = new PublishAcknowledgementAwaiter();
			_sendMessageResponseSubscription = (SendMessageResponse)_mqttSubscriptions["/send_message_response"];
		}

		private static Dictionary<string, Subscription> CreateSubscriptions(IDispatcher uiDispatcher)
		{
			Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>();
			Action<Subscription> action = (Subscription s) =>
			{
				subscriptions[s.Topic] = s;
			};
			action(new SendMessageResponse(uiDispatcher));
			action(new PresenceSubscription(uiDispatcher));
			action(new Typing(uiDispatcher));
			action(new Facebook.Services.Mqtt.TopicSubscriptions.Sync(uiDispatcher));
			return subscriptions;
		}

		private void OnNetworkStatusChanged(INetworkStatus sender, bool connected)
		{
			Task.Run(() =>
			{
				Logger.Info(this, "Network status updated, connected: " + _network.IsConnected(), "OnNetworkStatusChanged", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 93);
				if (_network.IsConnected())
				{
					ResetRetryDelayAndReconnect();
				}
				else
				{
					CancelTasksAndDisconnect();
				}
			});
		}

		private void AttemptReconnect()
		{
			if (_attemptingReconnected || _applicationState.IsShuttingDown)
			{
				return;
			}
			if (!_network.IsConnected())
			{
				Logger.Info(this, "Skipping MQTT retry since this is no network", "AttemptReconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 111);
				return;
			}
			_attemptingReconnected = true;
			if (_context == null || !_context.IsAuthenticated())
			{
				Logger.Info(this, "Skipping MQTT retry since there is no active session", "AttemptReconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 119);
			}
			else
			{
				Logger.Info(this, string.Format("Waiting {0} before attempting mqtt connection, phone network is connected: {1}", new object[2]
				{
					_connectionStategy.CurrentRetryDelay,
					_network.IsConnected()
				}), "AttemptReconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 123);
				bool flag = _reconnectionWaitEvent.WaitOne(_connectionStategy.CurrentRetryDelay);
				Logger.Info(this, string.Format("Woken from reconnection sleep, was woken prematurely: {0}, phone network is connected: {1}", new object[2]
				{
					flag,
					_network.IsConnected()
				}), "AttemptReconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 127);
				Task.Run((Action)Connect);
			}
			_attemptingReconnected = false;
		}

		private void ResetRetryDelayAndReconnect()
		{
			_connectionStategy.Reset();
			_reconnectionWaitEvent.Set();
			AttemptReconnect();
		}

		private async void MqttClientOnStateChanged(AbstractMqttClient sender, StateChangedEventArgs args)
		{
			Logger.Info(this, string.Format("MQTT State Changed: {0}", new object[1] { args.State }), "MqttClientOnStateChanged", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 144);
			switch (args.State)
			{
			case ClientState.Connected:
				await OnClientConnected();
				Logger.Info(this, string.Format("MQTT Subscriptions Completed"), "MqttClientOnStateChanged", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 149);
				break;
			case ClientState.Disconnected:
				OnClientDisconnected(args);
				break;
			}
			PerformSignal(new MqttConnectionStateChangedSignal(args.State));
		}

		private async Task OnClientConnected()
		{
			_connectionStategy.Reset();
			foreach (Subscription mqttSubscription in _mqttSubscriptions.Values)
			{
				await _mqttClient.Subscribe(mqttSubscription.Topic, mqttSubscription.QoS);
			}
			_isSubscribed = true;
		}

		private void OnClientDisconnected(StateChangedEventArgs args)
		{
			CancelTasks();
			_publishAcknowledgementAwaiter.Reset();
			_isSubscribed = false;
			if (args is ConnectionFailedEventArgs)
			{
				OnConnectionFailed(args as ConnectionFailedEventArgs);
			}
			if (args is ConnectionLostEventArgs)
			{
				OnConnectionLost(args as ConnectionLostEventArgs);
			}
			AttemptReconnect();
		}

		private void OnConnectionFailed(ConnectionFailedEventArgs args)
		{
			Logger.Warning(this, string.Format("Reason: {0}\n{1}", new object[2] { args.FailureReason, args.FailureException }), "OnConnectionFailed", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 190);
			_connectionStategy.Increment();
		}

		private void OnConnectionLost(ConnectionLostEventArgs args)
		{
			Logger.Warning(this, string.Format("Service state {0}, reason {1}", new object[2]
			{
				_applicationState.IsShuttingDown ? "deactive" : "active",
				args.Message ?? string.Empty
			}), "OnConnectionLost", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 196);
		}

		private void OnSessionExpired(AbstractMqttClient sender)
		{
			_context.Logout(true);
		}

		private async void OnMessageReceived(AbstractMqttClient sender, MqttMessage msg)
		{
			Logger.Info(this, string.Format("Message received with type: {0}", new object[1] { msg.MessageType }), "OnMessageReceived", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 206);
			switch (msg.MessageType)
			{
			case MessageType.Publish:
				await HandlePublishMessage((PublishMessage)msg);
				break;
			case MessageType.PublishAck:
				_publishAcknowledgementAwaiter.Received(msg as PublishAckMessage);
				break;
			default:
				Logger.Info(this, "Message received but not processed: " + msg.MessageType, "OnMessageReceived", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 217);
				break;
			}
		}

		private async Task HandlePublishMessage(PublishMessage message)
		{
			Logger.Info(this, string.Format("Publish Message received with topic name: {0}", new object[1] { message.TopicName }), "HandlePublishMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 224);
			if (_mqttSubscriptions.ContainsKey(message.TopicName))
			{
				Subscription handler = _mqttSubscriptions[message.TopicName];
				await handler.HandleMessage(message);
			}
			DataConsumptionLogger.LogDataEvent(downloadedBytes: message.RawPayloadSize, service: _analyticsService, type: DataType.Mqtt, uploadedBytes: 0L);
		}

		private async Task<bool> SendMessageAndWait(string topic, string payload, QualityofService qos)
		{
			if (!IsConnected())
			{
				Logger.Warning(this, "Skipping publish because MQTT not connected", "SendMessageAndWait", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 240);
				return false;
			}
			SendPublishMessageResponse response = await _mqttClient.SendPublishMessage(topic, payload, qos, 10000);
			DataConsumptionLogger.LogDataEvent(uploadedBytes: response.RawPayloadSize, service: _analyticsService, type: DataType.Mqtt, downloadedBytes: 0L);
			if (qos == QualityofService.AcknowledgedDelivery)
			{
				return _publishAcknowledgementAwaiter.Await(response.MessageId);
			}
			return true;
		}

		private void AddEventHandlers()
		{
			_mqttClient.MessageReceived += OnMessageReceived;
			_mqttClient.StateChanged += MqttClientOnStateChanged;
			_mqttClient.SessionExpired += OnSessionExpired;
			_network.AddHandler(OnNetworkStatusChanged);
		}

		private void RemoveEventHandlers()
		{
			_mqttClient.MessageReceived -= OnMessageReceived;
			_mqttClient.StateChanged -= MqttClientOnStateChanged;
			_mqttClient.SessionExpired -= OnSessionExpired;
			_network.RemoveHandler(OnNetworkStatusChanged);
		}

		public override void StartService(FacebookContext context)
		{
			_context = context;
			_isSubscribed = false;
			Messenger.Default.Register(this, (LoginSignal signal) =>
			{
				RunTask(OnLogin, signal);
			});
			Messenger.Default.Register(this, (LogoutSignal signal) =>
			{
				RunTask(OnLogout, signal);
			});
			Messenger.Default.Register(this, (MessageTypingSentSignal signal) =>
			{
				RunTask(OnTyping, signal);
			});
			Messenger.Default.Register(this, (ChatSettingSignal signal) =>
			{
				RunTask(OnChatSettingChanged, signal);
			});
			AddEventHandlers();
			_connectionStategy.Reset();
			Task.Run((Action)Connect);
		}

		private static void RunTask<TSignal>(Action<TSignal> function, TSignal signal)
		{
			Task.Run(() =>
			{
				function(signal);
			});
		}

		public override void StopService()
		{
			Messenger.Default.Unregister(this);
			RemoveEventHandlers();
			CancelTasksAndDisconnect();
		}

		private void OnLogout(LogoutSignal obj)
		{
			CancelTasksAndDisconnect();
		}

		private void OnLogin(LoginSignal obj)
		{
			AttemptReconnect();
		}

		private void Connect()
		{
			if (_context.IsAuthenticated() && _mqttClient != null && _mqttClient.State != ClientState.Connected && _mqttClient.State != ClientState.Connecting && !_applicationState.IsShuttingDown)
			{
				_cancellationTokenSource.CancelAndRenew();
				MqttConnectUser mqttConnectUser = new MqttConnectUser();
				mqttConnectUser.Uid = _context.Session.UserId;
				mqttConnectUser.Capabilities = GetCapabilities();
				mqttConnectUser.UserAgent = string.Format("[{0}]", new object[1] { _userAgentGenerator.GetSystemInformation() });
				mqttConnectUser.DeviceId = _context.GlobalSettings.DeviceIdentifier;
				mqttConnectUser.PublishFormat = "JSON+zlib";
				MqttConnectUser value = mqttConnectUser;
				MqttParameters mqttParameters = new MqttParameters();
				mqttParameters.UserName = JsonConvert.SerializeObject(value);
				mqttParameters.Password = _context.Session.Credentials.AccessToken;
				mqttParameters.KeepaliveSeconds = 60;
				mqttParameters.ClientId = GetClientId();
				mqttParameters.HostName = "orcart.facebook.com";
				mqttParameters.DefaultPort = 443;
				mqttParameters.UseUnsecureConnection = false;
				mqttParameters.PublishPayloadIsCompressed = true;
				MqttParameters parameters = mqttParameters;
				_mqttClient.Connect(parameters, _cancellationTokenSource.Token).DoNotWait();
			}
		}

		private string GetClientId()
		{
			string deviceIdentifier = _context.GlobalSettings.DeviceIdentifier;
			return deviceIdentifier.Substring(0, 20);
		}

		private bool IsConnected()
		{
			if (_mqttClient.State == ClientState.Connected)
			{
				return _isSubscribed;
			}
			return false;
		}

		private int GetCapabilities()
		{
			int num = 3;
			if (_deltaSentMessageEnabledProvider.Enabled)
			{
				num += 16;
			}
			return num;
		}

		private void CancelTasks()
		{
			_cancellationTokenSource.CancelAndRenew();
		}

		private void CancelTasksAndDisconnect()
		{
			Logger.LogLocation(this, LogLevel.Info, "CancelTasksAndDisconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Mqtt\\Service.cs", 378);
			CancelTasks();
			_mqttClient.Disconnect();
		}
	}
}

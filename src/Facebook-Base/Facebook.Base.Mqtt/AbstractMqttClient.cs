using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Mqtt.Messages;
using Facebook.Base.Mqtt.Serialization;
using Ionic.Zlib;

namespace Facebook.Base.Mqtt
{
	public abstract class AbstractMqttClient
	{
		private readonly object _stateLock = new object();

		private ClientState _state;

		private int _lastMsgId;

		private readonly AutoRenewCancellationTokenSource _cancellationTokenSource = new AutoRenewCancellationTokenSource();

		private CancellationToken _linkedCancellationToken;

		protected IMqttStreamReader Reader { get; set; }

		protected IMqttStreamWriter Writer { get; set; }

		public ClientState State
		{
			get
			{
				lock (_stateLock)
				{
					return _state;
				}
			}
			private set
			{
				lock (_stateLock)
				{
					_state = value;
					HandleStateChanged();
				}
			}
		}

		protected MqttParameters Parameters { get; set; }

		public event MqttEventHandler<PublishEventArgs> PublishArrived;

		public event MqttEventHandler<int> PublishAcknowledged;

		public event MqttEventHandler<int> MessageSent;

		public event MqttEventHandler PingSent;

		public event MqttEventHandler<MqttMessage> MessageReceived;

		public event MqttEventHandler<StateChangedEventArgs> StateChanged;

		public event MqttEventHandler SessionExpired;

		public abstract Task SendMessage(MqttMessage msg, CancellationToken token);

		protected abstract Task OpenSocket();

		protected abstract void CloseSocket();

		protected abstract void InitializeReaderAndWriter();

		protected AbstractMqttClient()
		{
			Logger.Info(this, "Client Constructed", ".ctor", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 147);
			_lastMsgId = 0;
			State = ClientState.Initialized;
			PublishArrived += OnPublishArrived;
		}

		private void OnPublishArrived(AbstractMqttClient sender, PublishEventArgs args)
		{
			if (args.QoS == QualityofService.AcknowledgedDelivery)
			{
				FixedHeader fixedHeader = new FixedHeader();
				fixedHeader.MessageType = MessageType.PublishAck;
				FixedHeader header = fixedHeader;
				PublishAckMessage publishAckMessage = new PublishAckMessage(header);
				publishAckMessage.MessageId = args.MessageId;
				PublishAckMessage msg = publishAckMessage;
				SendMessage(msg, _linkedCancellationToken);
			}
		}

		public Task SendMessage(MqttMessage msg, CancellationToken token, int timeoutMs)
		{
			if (timeoutMs <= 0)
			{
				return SendMessage(msg, token);
			}
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(timeoutMs);
			CancellationTokenSource cancellationTokenSource2 = CancellationTokenSource.CreateLinkedTokenSource(token, cancellationTokenSource.Token);
			return SendMessage(msg, cancellationTokenSource2.Token);
		}

		private async Task LoopMessages()
		{
			CancellationToken token = _linkedCancellationToken;
			try
			{
				while (!token.IsCancellationRequested)
				{
					MqttMessage msg = await MessageDecoder.ReadNextMessage(Reader, Parameters.PublishPayloadIsCompressed, token);
					if (msg == null)
					{
						break;
					}
					switch (msg.MessageType)
					{
					case MessageType.Publish:
					{
						PublishMessage publishMessage = msg as PublishMessage;
						RaisePublishArrived(new PublishEventArgs
						{
							MessageId = publishMessage.MessageId,
							Payload = publishMessage.Payload,
							TopicName = publishMessage.TopicName,
							QoS = (QualityofService)publishMessage.FixedHeader.QosLevel,
							Retained = publishMessage.FixedHeader.Retain
						});
						break;
					}
					case MessageType.PublishAck:
					{
						PublishAckMessage publishAckMessage = msg as PublishAckMessage;
						RaisePublishAcknowledged(publishAckMessage.MessageId);
						break;
					}
					}
					RaiseMessageReceived(msg);
				}
			}
			catch (TaskCanceledException)
			{
				Logger.Info("Cancellation token was fired while looping messages", "LoopMessages", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 221);
			}
			catch (Exception ex2)
			{
				if (token.IsCancellationRequested)
				{
					Logger.Warning("Exception thrown before TaskCanceledException: " + ex2, "LoopMessages", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 227);
					return;
				}
				HandleConnectionLost(ex2.Message);
				Logger.Warning("Exception occurred looping mqtt messages: " + ex2, "LoopMessages", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 231);
			}
		}

		private void CreateLinkedToken(CancellationToken token)
		{
			_cancellationTokenSource.CancelAndRenew();
			_linkedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(token, _cancellationTokenSource.Token).Token;
		}

		public void Disconnect()
		{
			_cancellationTokenSource.CancelAndRenew();
			CloseSocket();
			lock (_stateLock)
			{
				State = ClientState.Disconnected;
			}
		}

		public async Task Connect(MqttParameters parameters, CancellationToken token)
		{
			CreateLinkedToken(token);
			Parameters = parameters;
			Logger.Info(this, string.Format("Connect called for host - {0}:{1}", new object[2] { parameters.HostName, parameters.DefaultPort }), "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 256);
			lock (_stateLock)
			{
				if (State == ClientState.Connected || State == ClientState.Connecting)
				{
					Logger.Info(this, "Already connected or connecting. Ignore call.", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 261);
					return;
				}
				State = ClientState.Connecting;
			}
			try
			{
				Logger.Info(this, "Opening a socket", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 270);
				await OpenSocket();
				Logger.Info(this, "Initializing reader and writer", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 273);
				InitializeReaderAndWriter();
				Logger.Info(this, "Send a connect message", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 276);
				await SendConnectMessage();
				Logger.Info(this, "Waiting for Ack", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 279);
				ConnectAckMessage connectAckMessage = (await MessageDecoder.ReadNextMessage(Reader, Parameters.PublishPayloadIsCompressed, _linkedCancellationToken)) as ConnectAckMessage;
				ConnectAckMessage ack = connectAckMessage;
				if (ack == null || ack.MessageType != MessageType.ConnectAck)
				{
					throw new MqttException(ConnectionFailureReason.FailedInvalidConnectAck);
				}
				if (ack.ReturnCode != 0)
				{
					if (ack.ReturnCode == 5)
					{
						throw new MqttException(ConnectionFailureReason.FailedConnectRefusedNotAuthorized);
					}
					throw new MqttException(ConnectionFailureReason.FailedConnectRefused);
				}
				lock (_stateLock)
				{
					if (State == ClientState.Disconnected)
					{
						throw new MqttException(ConnectionFailureReason.FailedUnexpectedDisconnect);
					}
					State = ClientState.Connected;
				}
			}
			catch (TaskCanceledException)
			{
				Logger.Info("Cancellation token was fired while connecting to mqtt", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 308);
			}
			catch (MqttException ex2)
			{
				ConnectionFailureReason reason = ((!ex2.ConnectionFailureReason.HasValue) ? ConnectionFailureReason.FailedConnectMessage : ex2.ConnectionFailureReason.Value);
				HandleConnectionFailed(reason, ex2);
				Logger.Warning(this, "Failed to connect", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 317);
				Logger.Warning(this, ex2.ToString(), "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 318);
			}
			catch (Exception ex3)
			{
				HandleConnectionFailed(ConnectionFailureReason.FailedConnectMessage, ex3);
				Logger.Warning(this, "Failed to connect", "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 324);
				Logger.Warning(this, ex3.ToString(), "Connect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 325);
			}
		}

		private void HandleConnectionFailed(ConnectionFailureReason reason, Exception ex)
		{
			Logger.Info(this, "Connection failed", "HandleConnectionFailed", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 331);
			HandleAutoDisconnect(new ConnectionFailedEventArgs(reason, ex));
			if (reason == ConnectionFailureReason.FailedConnectRefusedNotAuthorized && SessionExpired != null)
			{
				SessionExpired(this);
			}
		}

		private void HandleConnectionLost(string connectionMessage)
		{
			Logger.Info(this, "Connection lost", "HandleConnectionLost", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 345);
			HandleAutoDisconnect(new ConnectionLostEventArgs(connectionMessage));
		}

		private void HandleAutoDisconnect(StateChangedEventArgs args)
		{
			Logger.Info(this, "Closing Socket", "HandleAutoDisconnect", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 351);
			CloseSocket();
			lock (_stateLock)
			{
				_state = ClientState.Disconnected;
			}
			SignalStateChanged(args);
		}

		private void HandleStateChanged()
		{
			ClientState state = State;
			if (state == ClientState.Connected)
			{
				StartPingRequests().DoNotWait();
				LoopMessages().DoNotWait();
			}
			SignalStateChanged(new StateChangedEventArgs(State));
		}

		private void SignalStateChanged(StateChangedEventArgs args)
		{
			Logger.Info(this, string.Format("MqttClient state changed to: {0}", new object[1] { args.State }), "SignalStateChanged", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 375);
			if (StateChanged != null)
			{
				StateChanged(this, args);
			}
		}

		private async Task StartPingRequests()
		{
			try
			{
				CancellationToken token = _linkedCancellationToken;
				while (!token.IsCancellationRequested)
				{
					FixedHeader header = new FixedHeader
					{
						MessageType = MessageType.PingRequest,
						RemainingLength = 0
					};
					MqttMessage msg = new MqttMessage(header);
					Logger.Info(this, "Sending MQTT Ping", "StartPingRequests", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 397);
					await SendMessage(msg, token);
					RaisePingSent();
					await Task.Delay(Parameters.KeepaliveSeconds * 1000, token);
				}
			}
			catch (TaskCanceledException)
			{
				Logger.Info(this, "Ping requests task cancelled", "StartPingRequests", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 407);
			}
			catch (Exception ex2)
			{
				Logger.Warning("Exception occurred sending ping requests: " + ex2, "StartPingRequests", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 411);
			}
		}

		public async Task<int> Subscribe(string topic, QualityofService qos)
		{
			int id = NextMessageId();
			try
			{
				FixedHeader header = new FixedHeader
				{
					MessageType = MessageType.Subscribe,
					QosLevel = (int)qos
				};
				List<SubscribeTopic> list = new List<SubscribeTopic>
				{
					new SubscribeTopic(topic, (int)qos)
				};
				SubscribeMessage msg = new SubscribeMessage(header)
				{
					MessageId = id,
					TopicList = list
				};
				await SendMessage(msg, _linkedCancellationToken);
				RaiseMessageSent(msg.MessageId);
			}
			catch (TaskCanceledException)
			{
				Logger.Info("Cancellation while subscribing to mqtt topic: " + topic, "Subscribe", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 442);
			}
			catch (Exception ex2)
			{
				Logger.Warning(string.Format("Exception occurred while subscribing to topic {0}: {1}", new object[2] { topic, ex2 }), "Subscribe", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 446);
			}
			return id;
		}

		protected int NextMessageId()
		{
			lock (this)
			{
				_lastMsgId++;
				return _lastMsgId;
			}
		}

		private async Task SendConnectMessage()
		{
			FixedHeader header = new FixedHeader
			{
				MessageType = MessageType.Connect
			};
			ConnectMessage connectMsg = new ConnectMessage(header)
			{
				Version = 3,
				HasUserName = true,
				HasPassword = true,
				KeepAliveTimeSeconds = 60,
				CleanSession = true,
				UserName = Parameters.UserName,
				Password = Parameters.Password,
				ClientIdentifier = Parameters.ClientId
			};
			await SendMessage(connectMsg, _linkedCancellationToken);
		}

		protected void RaisePublishArrived(PublishEventArgs args)
		{
			if (PublishArrived != null)
			{
				PublishArrived(this, args);
			}
		}

		protected void RaisePublishAcknowledged(int msgId)
		{
			if (PublishAcknowledged != null)
			{
				PublishAcknowledged(this, msgId);
			}
		}

		protected void RaiseMessageSent(int msgId)
		{
			if (MessageSent != null)
			{
				MessageSent(this, msgId);
			}
		}

		protected void RaisePingSent()
		{
			if (PingSent != null)
			{
				PingSent(this);
			}
		}

		protected void RaiseMessageReceived(MqttMessage msg)
		{
			if (MessageReceived != null)
			{
				MessageReceived(this, msg);
			}
		}

		public async Task<SendPublishMessageResponse> SendPublishMessage(string topic, string payloadString, QualityofService qos, int timeoutMs)
		{
			if (State != ClientState.Connected)
			{
				Logger.Info(this, string.Format("Attempted to publish message while not connected, topic: {0}", new object[1] { topic }), "SendPublishMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 536);
				throw new MqttException(ConnectionFailureReason.FailedUnexpectedDisconnect);
			}
			int id = NextMessageId();
			long payloadBytes = 0L;
			try
			{
				FixedHeader header = new FixedHeader
				{
					MessageType = MessageType.Publish,
					QosLevel = (int)qos
				};
				byte[] payload = (Parameters.PublishPayloadIsCompressed ? ZlibStream.CompressString(payloadString) : Encoding.UTF8.GetBytes(payloadString));
				payloadBytes = payload.Length;
				PublishMessage msg = new PublishMessage(header)
				{
					TopicName = topic,
					Payload = payload,
					MessageId = id
				};
				await SendMessage(msg, _linkedCancellationToken, timeoutMs);
			}
			catch (Exception ex)
			{
				Logger.Warning("Exception occurred while sending mqtt publish message", "SendPublishMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 568);
				Logger.Warning(this, ex.ToString(), "SendPublishMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Mqtt\\AbstractMqttClient.cs", 569);
			}
			return new SendPublishMessageResponse
			{
				MessageId = id,
				RawPayloadSize = payloadBytes
			};
		}
	}
}

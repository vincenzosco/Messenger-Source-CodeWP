using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Base.Mqtt;
using Facebook.Base.Mqtt.Messages;
using Facebook.Base.Mqtt.Serialization;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Facebook.Messenger.Mqtt
{
	public class MqttClient : AbstractMqttClient
	{
		public class MqttStreamReader : IMqttStreamReader
		{
			private readonly IDataReader _reader;

			public MqttStreamReader(IDataReader reader)
			{
				_reader = reader;
			}

			public async Task<byte> ReadByte(CancellationToken token)
			{
				await ((IAsyncOperation<uint>)(object)_reader.LoadAsync(1u)).AsTask<uint>(token);
				return _reader.ReadByte();
			}

			public async Task ReadBytes(byte[] buff)
			{
				await (IAsyncOperation<uint>)(object)_reader.LoadAsync((uint)buff.Length);
				_reader.ReadBytes(buff);
			}
		}

		public class MqttStreamWriter : IMqttStreamWriter
		{
			private readonly IDataWriter _writer;

			public async Task WriteMessageAndStoreAsync(MqttMessage msg)
			{
				MessageEncoder.Write(this, msg);
				await (IAsyncOperation<uint>)(object)_writer.StoreAsync();
				await _writer.FlushAsync();
			}

			public MqttStreamWriter(IDataWriter writer)
			{
				_writer = writer;
			}

			public void WriteChar(char value)
			{
				_writer.WriteByte((byte)value);
			}

			public void WriteInt16(int value)
			{
				_writer.WriteInt16((short)value);
			}

			public void WriteByte(int value)
			{
				_writer.WriteByte((byte)value);
			}

			public void WriteByte(byte value)
			{
				_writer.WriteByte(value);
			}

			public void WriteBytes(byte[] value)
			{
				_writer.WriteBytes(value);
			}
		}

		private readonly SemaphoreSlim _sendingSemaphore = new SemaphoreSlim(1);

		private StreamSocket _socket;

		public override Task SendMessage(MqttMessage msg, CancellationToken token)
		{
			Logger.Info(this, $"Sending Message of type {msg.FixedHeader.MessageType}", "SendMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 21);
			return Task.Run(async () =>
			{
				_sendingSemaphore.Wait(token);
				try
				{
					MqttStreamWriter writer = Writer as MqttStreamWriter;
					if (writer != null)
					{
						await writer.WriteMessageAndStoreAsync(msg);
					}
				}
				finally
				{
					_sendingSemaphore.Release();
				}
			}, token);
		}

		protected override async Task OpenSocket()
		{
			Logger.Info(this, "Attempt to close existing socket", "OpenSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 44);
			CloseSocket();
			HostName host = new HostName(Parameters.HostName);
			string port = Parameters.DefaultPort.ToString();
			Logger.Info(this, "Initialize socket", "OpenSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 50);
			StreamSocket socket = new StreamSocket();
			socket.Control.put_KeepAlive(false);
			socket.Control.put_NoDelay(true);
			try
			{
				SocketProtectionLevel socketProtection = (SocketProtectionLevel)(!Parameters.UseUnsecureConnection);
				Logger.Info(this, $"Try to connect on socket {Parameters.HostName}:{port} - {socketProtection}", "OpenSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 60);
				await socket.ConnectAsync(host, port, socketProtection);
				Logger.Info(this, "Socket opened successfully", "OpenSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 62);
				_socket = socket;
			}
			catch (Exception ex)
			{
				try
				{
					Logger.Info(this, $"Exception occurred, disposing socket, {ex}", "OpenSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 69);
					socket.Dispose();
				}
				catch (Exception arg)
				{
					Logger.Info(this, $"Error while disposing socket, {arg}", "OpenSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 74);
				}
				throw new MqttException(ConnectionFailureReason.FailedSocketConnectError, ex);
			}
		}

		protected override void InitializeReaderAndWriter()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected Obj, but got Unknown
			Writer = new MqttStreamWriter((IDataWriter)new DataWriter(_socket.OutputStream));
			Reader = new MqttStreamReader((IDataReader)new DataReader(_socket.InputStream));
		}

		protected override void CloseSocket()
		{
			if (_socket == null)
			{
				Logger.Info(this, "Trying to close socket that does not exist", "CloseSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 91);
				return;
			}
			try
			{
				_socket.Dispose();
			}
			catch (Exception arg)
			{
				Logger.Info(this, $"Error while closing socket, {arg}", "CloseSocket", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Mqtt\\MqttClient.cs", 101);
			}
			_socket = null;
		}
	}
}

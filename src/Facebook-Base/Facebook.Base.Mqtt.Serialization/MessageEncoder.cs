using System;
using System.Text;
using Facebook.Base.Mqtt.Messages;

namespace Facebook.Base.Mqtt.Serialization
{
	public static class MessageEncoder
	{
		public static void Write(IMqttStreamWriter writer, MqttMessage message)
		{
			switch (message.FixedHeader.MessageType)
			{
			case MessageType.Connect:
				WriteConnect(message as ConnectMessage, writer);
				break;
			case MessageType.ConnectAck:
				WriteConnectAck(message as ConnectAckMessage, writer);
				break;
			case MessageType.Publish:
				WritePublish(message as PublishMessage, writer);
				break;
			case MessageType.PublishAck:
				WritePublishAck(message as PublishAckMessage, writer);
				break;
			case MessageType.Subscribe:
				WriteSubscribe(message as SubscribeMessage, writer);
				break;
			case MessageType.SubscribeAck:
				WriteSubscribeAck(message as SubscribeAckMessage, writer);
				break;
			case MessageType.Unsubscribe:
				WriteUnsubscribe(message as UnsubscribeMessage, writer);
				break;
			case MessageType.UnsubscribeAck:
				WriteUnsubscribeAck(message as UnsubscribeAckMessage, writer);
				break;
			case MessageType.PingRequest:
				WriteFixedHeader(message, writer);
				break;
			case MessageType.PingResponse:
				WriteFixedHeader(message, writer);
				break;
			default:
				throw new ArgumentException("Unknown MQTT message type {0}", message.FixedHeader.MessageType.ToString());
			}
		}

		private static void WriteUnsubscribeAck(UnsubscribeAckMessage message, IMqttStreamWriter writer)
		{
			int num = 2;
			WriteFixedHeaderByte1(message, writer);
			WriteVariableLengthInt(num, writer);
			writer.WriteInt16(message.MessageId);
		}

		private static void WriteUnsubscribe(UnsubscribeMessage message, IMqttStreamWriter writer)
		{
			int num = 2;
			int num2 = 0;
			foreach (string topic in message.TopicList)
			{
				byte[] array = EncodeStringUtf8(topic);
				num2 += 2 + array.Length;
			}
			int num3 = num + num2;
			WriteFixedHeaderByte1(message, writer);
			WriteVariableLengthInt(num3, writer);
			writer.WriteInt16(message.MessageId);
			foreach (string topic2 in message.TopicList)
			{
				byte[] array2 = EncodeStringUtf8(topic2);
				writer.WriteInt16(array2.Length);
				writer.WriteBytes(array2);
			}
		}

		private static void WriteSubscribe(SubscribeMessage message, IMqttStreamWriter writer)
		{
			int num = 2;
			int num2 = 0;
			foreach (SubscribeTopic topic in message.TopicList)
			{
				byte[] array = EncodeStringUtf8(topic.Name);
				num2 += 2 + array.Length;
				num2++;
			}
			int num3 = num + num2;
			WriteFixedHeaderByte1(message, writer);
			WriteVariableLengthInt(num3, writer);
			writer.WriteInt16(message.MessageId);
			foreach (SubscribeTopic topic2 in message.TopicList)
			{
				byte[] array2 = EncodeStringUtf8(topic2.Name);
				writer.WriteInt16(array2.Length);
				writer.WriteBytes(array2);
				writer.WriteByte(topic2.QualityOfService);
			}
		}

		private static void WriteSubscribeAck(SubscribeAckMessage message, IMqttStreamWriter writer)
		{
			int num = 2;
			int count = message.GrantedQualityOfServices.Count;
			int num2 = num + count;
			WriteFixedHeaderByte1(message, writer);
			WriteVariableLengthInt(num2, writer);
			writer.WriteInt16(message.MessageId);
			foreach (int grantedQualityOfService in message.GrantedQualityOfServices)
			{
				writer.WriteByte(grantedQualityOfService);
			}
		}

		private static void WriteFixedHeader(MqttMessage message, IMqttStreamWriter writer)
		{
			WriteFixedHeaderByte1(message, writer);
			writer.WriteByte(0);
		}

		private static void WritePublishAck(PublishAckMessage message, IMqttStreamWriter writer)
		{
			WriteFixedHeaderByte1(message, writer);
			WriteVariableLengthInt(2, writer);
			writer.WriteInt16(message.MessageId);
		}

		private static void WritePublish(PublishMessage message, IMqttStreamWriter writer)
		{
			byte[] payload = message.Payload;
			byte[] array = EncodeStringUtf8(message.TopicName);
			int num = 2 + array.Length + ((message.FixedHeader.QosLevel > 0) ? 2 : 0);
			int num2 = payload.Length;
			int num3 = num + num2;
			WriteFixedHeaderByte1(message, writer);
			WriteVariableLengthInt(num3, writer);
			writer.WriteInt16(array.Length);
			writer.WriteBytes(array);
			if (message.FixedHeader.QosLevel > 0)
			{
				writer.WriteInt16(message.MessageId);
			}
			writer.WriteBytes(payload);
		}

		private static void WriteConnectAck(ConnectAckMessage message, IMqttStreamWriter writer)
		{
			WriteFixedHeaderByte1(message, writer);
			writer.WriteByte(2);
			writer.WriteByte(0);
			writer.WriteByte(message.ReturnCode);
		}

		private static void WriteConnect(ConnectMessage message, IMqttStreamWriter writer)
		{
			int num = 12;
			int num2 = 0;
			string clientIdentifier = message.ClientIdentifier;
			byte[] array = EncodeStringUtf8(clientIdentifier);
			num2 += 2 + array.Length;
			string willTopic = message.WillTopic;
			byte[] array2 = ((willTopic != null) ? EncodeStringUtf8(willTopic) : new byte[0]);
			string willMessage = message.WillMessage;
			byte[] array3 = ((willMessage != null) ? EncodeStringUtf8(willMessage) : new byte[0]);
			if (message.WillFlag)
			{
				num2 += 2 + array2.Length;
				num2 += 2 + array3.Length;
			}
			string userName = message.UserName;
			byte[] array4 = ((userName != null) ? EncodeStringUtf8(userName) : new byte[0]);
			if (message.HasUserName)
			{
				num2 += 2 + array4.Length;
			}
			string password = message.Password;
			byte[] array5 = ((password != null) ? EncodeStringUtf8(password) : new byte[0]);
			if (message.HasPassword)
			{
				num2 += 2 + array5.Length;
			}
			int num3 = num + num2;
			WriteFixedHeaderByte1(message, writer);
			WriteVariableLengthInt(num3, writer);
			writer.WriteByte(0);
			writer.WriteByte(6);
			writer.WriteChar('M');
			writer.WriteChar('Q');
			writer.WriteChar('I');
			writer.WriteChar('s');
			writer.WriteChar('d');
			writer.WriteChar('p');
			writer.WriteByte(message.Version);
			writer.WriteByte(GetConnVariableHeaderFlag(message));
			writer.WriteInt16(message.KeepAliveTimeSeconds);
			writer.WriteInt16(array.Length);
			writer.WriteBytes(array);
			if (message.WillFlag)
			{
				writer.WriteInt16(array2.Length);
				writer.WriteBytes(array2);
				writer.WriteInt16(array3.Length);
				writer.WriteBytes(array3);
			}
			if (message.HasUserName)
			{
				writer.WriteInt16(array4.Length);
				writer.WriteBytes(array4);
			}
			if (message.HasPassword)
			{
				writer.WriteInt16(array5.Length);
				writer.WriteBytes(array5);
			}
		}

		private static void WriteFixedHeaderByte1(MqttMessage message, IMqttStreamWriter writer)
		{
			int num = 0;
			num |= message.FixedHeader.MessageTypeValue << 4;
			if (message.FixedHeader.DupFlag)
			{
				num |= 8;
			}
			num |= message.FixedHeader.QosLevel << 1;
			if (message.FixedHeader.Retain)
			{
				num |= 1;
			}
			writer.WriteByte(num);
		}

		private static byte GetConnVariableHeaderFlag(ConnectMessage message)
		{
			int num = 0;
			if (message.HasUserName)
			{
				num |= 0x80;
			}
			if (message.HasPassword)
			{
				num |= 0x40;
			}
			if (message.WillRetain)
			{
				num |= 0x20;
			}
			num |= (message.WillQos & 3) << 3;
			if (message.WillFlag)
			{
				num |= 4;
			}
			if (message.CleanSession)
			{
				num |= 2;
			}
			return (byte)num;
		}

		private static void WriteVariableLengthInt(int num, IMqttStreamWriter writer)
		{
			do
			{
				int num2 = num % 128;
				num /= 128;
				if (num > 0)
				{
					num2 |= 0x80;
				}
				writer.WriteByte(num2);
			}
			while (num > 0);
		}

		private static byte[] EncodeStringUtf8(string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}

		private static void WriteUtf8(byte[] bytes, IMqttStreamWriter writer)
		{
			int value = bytes.Length;
			writer.WriteInt16(value);
			writer.WriteBytes(bytes);
		}
	}
}

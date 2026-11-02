using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Mqtt.Messages;
using Ionic.Zlib;

namespace Facebook.Base.Mqtt.Serialization
{
	public static class MessageDecoder
	{
		public static async Task<MqttMessage> ReadNextMessage(IMqttStreamReader reader, bool publishPayloadIsCompressed, CancellationToken token)
		{
			byte b1 = await reader.ReadByte(token);
			MessageType messageType = TypeFromInt(b1 >> 4);
			bool dupFlag = (b1 & 8) == 8;
			int qosLevel = (b1 & 6) >> 1;
			bool retain = (b1 & 1) != 0;
			int remainingLength = 0;
			int multiplier = 1;
			int digit;
			do
			{
				digit = await reader.ReadByte(token);
				remainingLength += (digit & 0x7F) * multiplier;
				multiplier *= 128;
			}
			while ((digit & 0x80) != 0 && !token.IsCancellationRequested);
			FixedHeader header = new FixedHeader
			{
				DupFlag = dupFlag,
				MessageType = messageType,
				QosLevel = qosLevel,
				RemainingLength = remainingLength,
				Retain = retain
			};
			switch (header.MessageType)
			{
			case MessageType.Connect:
				return await ReadConnect(reader, header, token);
			case MessageType.ConnectAck:
				return await ReadConnectAck(reader, header, token);
			case MessageType.Publish:
				return await ReadPublish(reader, header, publishPayloadIsCompressed, token);
			case MessageType.PublishAck:
				return await ReadPublishAck(reader, header, token);
			case MessageType.Subscribe:
				return await ReadSubscribe(reader, header, token);
			case MessageType.SubscribeAck:
				return await ReadSubscribeAck(reader, header, token);
			case MessageType.Unsubscribe:
				return await ReadUnsubscribe(reader, header, token);
			case MessageType.UnsubscribeAck:
				return await ReadUnsubscribeAck(reader, header, token);
			case MessageType.PingRequest:
				return new MqttMessage(header);
			case MessageType.PingResponse:
				return new MqttMessage(header);
			default:
				return null;
			}
		}

		private static async Task<Tuple<string, int>> DecodeString(IMqttStreamReader reader, CancellationToken token)
		{
			Tuple<int, int> result = await DecodeMsbLsb(reader, token);
			int size = result.Item1;
			byte[] buf = new byte[size];
			await reader.ReadBytes(buf);
			int bytesRead = result.Item2 + size;
			string decodedString = Encoding.UTF8.GetString(buf, 0, buf.Length);
			return new Tuple<string, int>(decodedString, bytesRead);
		}

		private static async Task<Tuple<int, int>> DecodeMsbLsb(IMqttStreamReader reader, CancellationToken token)
		{
			int data = (await reader.ReadByte(token) << 8) | await reader.ReadByte(token);
			int bytesRead = 2;
			return new Tuple<int, int>(data, bytesRead);
		}

		private static async Task<MqttMessage> ReadUnsubscribeAck(IMqttStreamReader reader, FixedHeader header, CancellationToken token)
		{
			int remainingLength = header.RemainingLength;
			return new UnsubscribeAckMessage(header)
			{
				MessageId = (await DecodeMsbLsb(reader, token)).Item1
			};
		}

		private static async Task<MqttMessage> ReadUnsubscribe(IMqttStreamReader reader, FixedHeader header, CancellationToken token)
		{
			int bytesRemaining = header.RemainingLength;
			UnsubscribeMessage msg = new UnsubscribeMessage(header);
			Tuple<int, int> headerResult = await DecodeMsbLsb(reader, token);
			msg.MessageId = headerResult.Item1;
			bytesRemaining -= headerResult.Item2;
			msg.TopicList = new List<string>();
			while (bytesRemaining > 0)
			{
				Tuple<string, int> payloadResult = await DecodeString(reader, token);
				string topicName = payloadResult.Item1;
				bytesRemaining -= payloadResult.Item2;
				msg.TopicList.Add(topicName);
			}
			return msg;
		}

		private static async Task<MqttMessage> ReadSubscribeAck(IMqttStreamReader reader, FixedHeader header, CancellationToken token)
		{
			int bytesRemaining = header.RemainingLength;
			SubscribeAckMessage msg = new SubscribeAckMessage(header);
			Tuple<int, int> headerResult = await DecodeMsbLsb(reader, token);
			msg.MessageId = headerResult.Item1;
			bytesRemaining -= headerResult.Item2;
			msg.GrantedQualityOfServices = new List<int>();
			while (bytesRemaining > 0)
			{
				int qos = await reader.ReadByte(token) & -4;
				bytesRemaining--;
				msg.GrantedQualityOfServices.Add(qos);
			}
			return msg;
		}

		private static async Task<MqttMessage> ReadSubscribe(IMqttStreamReader reader, FixedHeader header, CancellationToken token)
		{
			int bytesRemaining = header.RemainingLength;
			SubscribeMessage msg = new SubscribeMessage(header);
			Tuple<int, int> headerResult = await DecodeMsbLsb(reader, token);
			msg.MessageId = headerResult.Item1;
			bytesRemaining -= headerResult.Item2;
			msg.TopicList = new List<SubscribeTopic>();
			while (bytesRemaining > 0)
			{
				Tuple<string, int> payloadResult = await DecodeString(reader, token);
				string topicName = payloadResult.Item1;
				bytesRemaining -= payloadResult.Item2;
				int qos = await reader.ReadByte(token);
				bytesRemaining--;
				msg.TopicList.Add(new SubscribeTopic(topicName, qos));
			}
			return msg;
		}

		private static async Task<MqttMessage> ReadPublishAck(IMqttStreamReader reader, FixedHeader header, CancellationToken token)
		{
			return new PublishAckMessage(header)
			{
				MessageId = (await DecodeMsbLsb(reader, token)).Item1
			};
		}

		private static async Task<MqttMessage> ReadPublish(IMqttStreamReader reader, FixedHeader header, bool compressed, CancellationToken token)
		{
			Tuple<string, int> payloadResult = await DecodeString(reader, token);
			string topic = payloadResult.Item1;
			int rawPayloadBytes = header.RemainingLength - payloadResult.Item2;
			int messageId = -1;
			if (header.QosLevel > 0)
			{
				Tuple<int, int> headerResult = await DecodeMsbLsb(reader, token);
				messageId = headerResult.Item1;
				rawPayloadBytes -= headerResult.Item2;
			}
			byte[] rawPayload = new byte[rawPayloadBytes];
			await reader.ReadBytes(rawPayload);
			byte[] payload = (compressed ? ZlibStream.UncompressBuffer(rawPayload) : rawPayload);
			return new PublishMessage(header)
			{
				MessageId = messageId,
				TopicName = topic,
				Payload = payload,
				RawPayloadSize = rawPayloadBytes
			};
		}

		private static async Task<MqttMessage> ReadConnectAck(IMqttStreamReader reader, FixedHeader header, CancellationToken token)
		{
			await reader.ReadByte(token);
			byte returnCode = await reader.ReadByte(token);
			return new ConnectAckMessage(header)
			{
				ReturnCode = returnCode
			};
		}

		private static async Task<MqttMessage> ReadConnect(IMqttStreamReader reader, FixedHeader header, CancellationToken token)
		{
			int bytesRemaining = header.RemainingLength;
			Tuple<string, int> strResult = await DecodeString(reader, token);
			string protoString = strResult.Item1;
			bytesRemaining -= strResult.Item2;
			if ("MQIsdp" != protoString)
			{
				throw new IOException("Invalid input - missing header");
			}
			byte version = await reader.ReadByte(token);
			int b1 = await reader.ReadByte(token);
			bytesRemaining -= 2;
			Tuple<int, int> result = await DecodeMsbLsb(reader, token);
			int keepAlive = result.Item1;
			bytesRemaining -= result.Item2;
			ConnectMessage msg = new ConnectMessage(header)
			{
				Version = version,
				HasUserName = ((b1 & 0x80) == 128),
				HasPassword = ((b1 & 0x40) == 64),
				WillRetain = ((b1 & 0x20) == 32),
				WillQos = (b1 & 0x18) >> 3,
				WillFlag = ((b1 & 4) == 4),
				CleanSession = ((b1 & 2) == 2),
				KeepAliveTimeSeconds = keepAlive
			};
			strResult = await DecodeString(reader, token);
			bytesRemaining -= strResult.Item2;
			msg.ClientIdentifier = strResult.Item1;
			if (msg.WillFlag)
			{
				strResult = await DecodeString(reader, token);
				bytesRemaining -= strResult.Item2;
				msg.WillTopic = strResult.Item1;
				strResult = await DecodeString(reader, token);
				bytesRemaining -= strResult.Item2;
				msg.WillMessage = strResult.Item1;
			}
			if (bytesRemaining > 0)
			{
				if (msg.HasUserName)
				{
					strResult = await DecodeString(reader, token);
					bytesRemaining -= strResult.Item2;
					msg.UserName = strResult.Item1;
				}
				if (msg.HasPassword)
				{
					strResult = await DecodeString(reader, token);
					int num = bytesRemaining - strResult.Item2;
					msg.Password = strResult.Item1;
				}
			}
			return msg;
		}

		private static MessageType TypeFromInt(int val)
		{
			return (MessageType)val;
		}
	}
}

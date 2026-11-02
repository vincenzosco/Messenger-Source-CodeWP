using System.Text;

namespace Facebook.Base.Mqtt.Messages;

public class PublishMessage : MqttMessage
{
	public string TopicName { get; set; }

	public int MessageId { get; set; }

	public byte[] Payload { get; set; }

	public long RawPayloadSize { get; set; }

	public PublishMessage(FixedHeader header)
		: base(header)
	{
	}

	public string GetPayloadAsString()
	{
		return Encoding.UTF8.GetString(Payload, 0, Payload.Length);
	}
}

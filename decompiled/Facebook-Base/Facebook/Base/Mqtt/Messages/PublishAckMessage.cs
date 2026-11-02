namespace Facebook.Base.Mqtt.Messages;

public class PublishAckMessage : MqttMessage
{
	public int MessageId { get; set; }

	public PublishAckMessage(FixedHeader header)
		: base(header)
	{
	}
}

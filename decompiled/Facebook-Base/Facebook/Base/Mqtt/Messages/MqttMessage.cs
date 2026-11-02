namespace Facebook.Base.Mqtt.Messages;

public class MqttMessage
{
	public FixedHeader FixedHeader { get; private set; }

	public MessageType MessageType => FixedHeader.MessageType;

	public MqttMessage(FixedHeader header)
	{
		FixedHeader = header;
	}
}

namespace Facebook.Base.Mqtt.Messages
{
	public class UnsubscribeAckMessage : MqttMessage
	{
		public int MessageId { get; set; }

		public UnsubscribeAckMessage(FixedHeader header)
			: base(header)
		{
		}
	}
}

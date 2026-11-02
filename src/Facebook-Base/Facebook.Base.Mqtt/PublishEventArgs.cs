namespace Facebook.Base.Mqtt
{
	public class PublishEventArgs
	{
		public int MessageId { get; set; }

		public string TopicName { get; set; }

		public byte[] Payload { get; set; }

		public QualityofService QoS { get; set; }

		public bool Retained { get; set; }
	}
}

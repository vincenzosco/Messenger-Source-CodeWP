namespace Facebook.Base.Mqtt.Messages
{
	public class SubscribeTopic
	{
		public string Name { get; set; }

		public int QualityOfService { get; set; }

		public SubscribeTopic(string name, int qos)
		{
			Name = name;
			QualityOfService = qos;
		}
	}
}

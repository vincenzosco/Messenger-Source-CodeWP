using System.Collections.Generic;

namespace Facebook.Base.Mqtt.Messages
{
	public class SubscribeMessage : MqttMessage
	{
		public int MessageId { get; set; }

		public List<SubscribeTopic> TopicList { get; set; }

		public SubscribeMessage(FixedHeader header)
			: base(header)
		{
		}
	}
}

using System.Collections.Generic;

namespace Facebook.Base.Mqtt.Messages
{
	public class UnsubscribeMessage : MqttMessage
	{
		public int MessageId { get; set; }

		public List<string> TopicList { get; set; }

		public UnsubscribeMessage(FixedHeader header)
			: base(header)
		{
		}
	}
}

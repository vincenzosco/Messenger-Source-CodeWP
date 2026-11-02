using System.Collections.Generic;

namespace Facebook.Base.Mqtt.Messages;

public class SubscribeAckMessage : MqttMessage
{
	public int MessageId { get; set; }

	public List<int> GrantedQualityOfServices { get; set; }

	public SubscribeAckMessage(FixedHeader header)
		: base(header)
	{
	}
}

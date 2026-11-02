namespace Facebook.Base.Mqtt.Messages;

public enum MessageType
{
	Connect = 1,
	ConnectAck,
	Publish,
	PublishAck,
	PublishReceived,
	PublishReleased,
	PublishComplete,
	Subscribe,
	SubscribeAck,
	Unsubscribe,
	UnsubscribeAck,
	PingRequest,
	PingResponse,
	Disconnect
}

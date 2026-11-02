namespace Facebook.Services.Mqtt
{
	internal enum MqttConnectionCapabilities
	{
		AcknowledgedDelivery = 1,
		ProcessingLastActivePresenceInfo = 2,
		ExactKeepAlive = 4,
		RequiresJsonUnicodeEscapes = 8,
		DeltaSentMessageEnabled = 0x10
	}
}

namespace Facebook.Base.Mqtt.Messages
{
	public class ConnectAckMessage : MqttMessage
	{
		public const byte CONNECTION_ACCEPTED = 0;

		public const byte CONNECTION_REFUSED_BAD_PROTOCOL_VERSION = 1;

		public const byte CONNECTION_REFUSED_BAD_IDENTIFIER = 2;

		public const byte CONNECTION_REFUSED_SERVER_UNAVAILABLE = 3;

		public const byte CONNECTION_REFUSED_BAD_USER_NAME_OR_PASSWORD = 4;

		public const byte CONNECTION_REFUSED_NOT_AUTHORIZED = 5;

		public byte ReturnCode { get; set; }

		public ConnectAckMessage(FixedHeader header)
			: base(header)
		{
		}
	}
}

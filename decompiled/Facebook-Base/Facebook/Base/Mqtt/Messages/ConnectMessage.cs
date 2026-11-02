namespace Facebook.Base.Mqtt.Messages;

public class ConnectMessage : MqttMessage
{
	public int Version { get; set; }

	public bool HasUserName { get; set; }

	public bool HasPassword { get; set; }

	public bool WillRetain { get; set; }

	public int WillQos { get; set; }

	public bool WillFlag { get; set; }

	public bool CleanSession { get; set; }

	public int KeepAliveTimeSeconds { get; set; }

	public string ClientIdentifier { get; set; }

	public string WillTopic { get; set; }

	public string WillMessage { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public ConnectMessage(FixedHeader header)
		: base(header)
	{
	}
}

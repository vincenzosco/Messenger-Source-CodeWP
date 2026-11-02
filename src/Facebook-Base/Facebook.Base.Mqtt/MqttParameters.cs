namespace Facebook.Base.Mqtt
{
	public class MqttParameters
	{
		public bool MqttCleanStart
		{
			get
			{
				return true;
			}
		}

		public string HostName { get; set; }

		public int DefaultPort { get; set; }

		public string ClientId { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public int KeepaliveSeconds { get; set; }

		public bool UseUnsecureConnection { get; set; }

		public bool PublishPayloadIsCompressed { get; set; }
	}
}

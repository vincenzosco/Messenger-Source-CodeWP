using Newtonsoft.Json;

namespace Facebook.Services.Mqtt
{
	[JsonObject(MemberSerialization.OptIn)]
	internal class MqttConnectUser
	{
		[JsonProperty("uid")]
		public string Uid { get; set; }

		[JsonProperty("capabilities")]
		public int Capabilities { get; set; }

		[JsonProperty("agent")]
		public string UserAgent { get; set; }

		[JsonProperty("device_id")]
		public string DeviceId { get; set; }

		[JsonProperty("publish_format")]
		public string PublishFormat { get; set; }
	}
}

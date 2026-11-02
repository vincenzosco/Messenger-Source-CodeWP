using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Share
	{
		[JsonProperty("sticker_id")]
		[DataMember]
		public string StickerId { get; set; }
	}
}

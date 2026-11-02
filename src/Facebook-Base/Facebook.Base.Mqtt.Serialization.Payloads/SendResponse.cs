using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class SendResponse
	{
		[DataMember]
		[JsonProperty("succeeded")]
		public bool Succeeded { get; set; }

		[JsonProperty("msgid")]
		[DataMember]
		public string MessageId { get; set; }

		[JsonProperty("errno")]
		[DataMember]
		public long ErrNo { get; set; }

		[DataMember]
		[JsonProperty("isRetryable")]
		public string IsRetryable { get; set; }

		[JsonProperty("fbtrace_meta")]
		[DataMember]
		public string FBTraceMeta { get; set; }
	}
}

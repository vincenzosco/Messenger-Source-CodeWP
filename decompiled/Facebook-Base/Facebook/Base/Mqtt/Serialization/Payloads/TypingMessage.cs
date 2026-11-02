using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class TypingMessage
{
	[JsonProperty("type")]
	[DataMember]
	public string Type { get; set; }

	[JsonProperty("sender_fbid")]
	[DataMember]
	public string SenderId { get; set; }

	[DataMember]
	[JsonProperty("state")]
	public bool State { get; set; }
}

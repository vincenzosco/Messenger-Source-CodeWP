using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class PrivacyOption
{
	[DataMember]
	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }

	[JsonProperty(PropertyName = "type")]
	[DataMember]
	public string Type { get; set; }
}

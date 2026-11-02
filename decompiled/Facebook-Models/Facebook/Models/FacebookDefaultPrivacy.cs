using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class FacebookDefaultPrivacy
{
	[DataMember]
	[JsonProperty(PropertyName = "name")]
	public string Name { get; set; }

	[JsonProperty(PropertyName = "value")]
	[DataMember]
	public string Value { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "description")]
	public string Description { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "allow")]
	public string Allow { get; set; }

	[JsonProperty(PropertyName = "deny")]
	[DataMember]
	public string Deny { get; set; }

	[JsonProperty(PropertyName = "friends")]
	[DataMember]
	public string Friends { get; set; }
}

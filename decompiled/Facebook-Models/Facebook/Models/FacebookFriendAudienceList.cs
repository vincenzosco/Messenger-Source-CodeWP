using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class FacebookFriendAudienceList
{
	[JsonProperty(PropertyName = "filter_key")]
	[DataMember]
	public string FilterKey { get; set; }

	[JsonProperty(PropertyName = "value")]
	[DataMember]
	public string Value { get; set; }

	[JsonProperty(PropertyName = "name")]
	[DataMember]
	public string Name { get; set; }

	[JsonProperty(PropertyName = "type")]
	[DataMember]
	public string Type { get; set; }
}

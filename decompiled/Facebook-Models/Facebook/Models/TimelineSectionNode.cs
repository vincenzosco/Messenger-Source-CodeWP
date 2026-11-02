using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class TimelineSectionNode
{
	[DataMember]
	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }

	[JsonProperty(PropertyName = "label")]
	[DataMember]
	public string Label { get; set; }
}

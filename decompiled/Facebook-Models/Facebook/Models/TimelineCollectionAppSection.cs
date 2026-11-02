using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class TimelineCollectionAppSection
{
	[DataMember]
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("name")]
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	[JsonProperty("collections")]
	public TimelineCollectionNode Collection { get; set; }
}

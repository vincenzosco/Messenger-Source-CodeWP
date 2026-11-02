using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class TimelineObject
{
	[DataMember]
	[JsonProperty("id")]
	public string Id { get; set; }

	[DataMember]
	[JsonProperty("name")]
	public string Name { get; set; }
}

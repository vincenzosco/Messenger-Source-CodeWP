using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class EventUserNode
{
	[JsonProperty("count")]
	[DataMember]
	public int? Count { get; set; }

	[JsonProperty("nodes")]
	[DataMember]
	public List<Actor> Users { get; set; }
}

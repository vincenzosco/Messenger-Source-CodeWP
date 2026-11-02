using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class TimelineCollectionNode
{
	[DataMember]
	[JsonProperty("nodes")]
	public List<TimelineObject> Nodes { get; set; }
}

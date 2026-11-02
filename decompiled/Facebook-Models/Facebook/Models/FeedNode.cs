using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class FeedNode
{
	[JsonProperty(PropertyName = "section_feed")]
	[DataMember]
	public SectionFeed Feed { get; set; }
}

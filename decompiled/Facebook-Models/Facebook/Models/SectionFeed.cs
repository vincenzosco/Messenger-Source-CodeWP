using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class SectionFeed
{
	[DataMember]
	[JsonProperty(PropertyName = "edges")]
	public List<FeedItem> Items { get; set; }
}

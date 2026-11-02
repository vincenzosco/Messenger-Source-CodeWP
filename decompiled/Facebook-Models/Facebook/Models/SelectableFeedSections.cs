using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class SelectableFeedSections
{
	[DataMember]
	[JsonProperty(PropertyName = "nodes")]
	public List<FeedSection> Sections { get; set; }
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class ObjectComments
{
	[DataMember]
	[JsonProperty(PropertyName = "count")]
	public int? Count;

	[DataMember]
	[JsonProperty(PropertyName = "page_info")]
	public PageInfo PageInfo;

	[DataMember]
	[JsonProperty(PropertyName = "comments")]
	public List<Comment> Comments { get; set; }
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

public class FacebookMedia
{
	[DataMember]
	[JsonProperty(PropertyName = "nodes")]
	public List<FacebookPhoto> Photos { get; set; }

	[JsonProperty(PropertyName = "page_info")]
	[DataMember]
	public PageInfo PageInfo { get; set; }

	[JsonProperty(PropertyName = "count")]
	[DataMember]
	public int Count { get; set; }
}

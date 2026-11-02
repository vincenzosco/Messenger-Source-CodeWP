using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class TimelineItems
	{
		[DataMember]
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("page_info")]
		[DataMember]
		public PageInfo PageInfo { get; set; }

		[DataMember]
		[JsonProperty("nodes")]
		public List<TimelineNode> Nodes { get; set; }
	}
}

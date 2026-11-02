using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class ActorCollectionNode
	{
		[JsonProperty(PropertyName = "count")]
		[DataMember]
		public int? Count { get; set; }

		[JsonProperty(PropertyName = "page_info")]
		[DataMember]
		public PageInfo PageInfo { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "edges")]
		public List<ActorNode> Actors { get; set; }
	}
}

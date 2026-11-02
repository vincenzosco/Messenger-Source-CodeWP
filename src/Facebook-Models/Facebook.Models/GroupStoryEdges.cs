using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class GroupStoryEdges
	{
		[JsonProperty("edges")]
		[DataMember]
		public List<GroupStoryNodes> Edges { get; set; }
	}
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FeaturedContent
	{
		[DataMember]
		[JsonProperty(PropertyName = "edges")]
		public List<FeaturedNode> Nodes { get; set; }
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FeaturedNode
	{
		[DataMember]
		[JsonProperty(PropertyName = "node")]
		public FeaturedObject Object { get; set; }
	}
}

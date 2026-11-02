using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FeedItem
	{
		[JsonProperty(PropertyName = "node")]
		[DataMember]
		public Story Node { get; set; }
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class RecentPhoto
	{
		[DataMember]
		[JsonProperty(PropertyName = "photo")]
		public FeedPhoto Photo { get; set; }
	}
}

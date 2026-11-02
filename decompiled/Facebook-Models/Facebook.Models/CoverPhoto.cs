using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class CoverPhoto
	{
		[JsonProperty(PropertyName = "focus")]
		[DataMember]
		public Focus Focus { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "photo")]
		public FeedPhoto Photo { get; set; }
	}
}

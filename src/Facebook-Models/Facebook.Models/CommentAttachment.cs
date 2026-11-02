using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class CommentAttachment
	{
		[DataMember]
		[JsonProperty(PropertyName = "description")]
		public StringEntity Description;

		[DataMember]
		[JsonProperty(PropertyName = "media")]
		public FeedMedia Media;

		[DataMember]
		[JsonProperty(PropertyName = "source")]
		public StringEntity Source;
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FeedAlbum
	{
		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id;

		[JsonProperty(PropertyName = "title")]
		[DataMember]
		public StringEntity Title;
	}
}

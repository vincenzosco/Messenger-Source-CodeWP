using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FacebookPostResponse
	{
		[JsonProperty(PropertyName = "body")]
		[JsonConverter(typeof(BodyConverter))]
		[DataMember]
		public Body Body { get; set; }

		[JsonProperty(PropertyName = "code")]
		[DataMember]
		public string Code { get; set; }
	}
}

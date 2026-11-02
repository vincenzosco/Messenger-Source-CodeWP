using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Phone
	{
		[JsonProperty(PropertyName = "is_verified")]
		[DataMember]
		public bool? IsVerified { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "phone_number")]
		public PhoneNumber IconImage { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "phone_type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public PhoneType? PhoneType { get; set; }
	}
}

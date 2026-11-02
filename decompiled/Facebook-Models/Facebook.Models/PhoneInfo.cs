using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class PhoneInfo
	{
		[DataMember]
		[JsonProperty(PropertyName = "number")]
		public string Number { get; set; }

		[JsonProperty(PropertyName = "country_code")]
		[DataMember]
		public string CountryCode { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "privacy")]
		public string Privacy { get; set; }
	}
}

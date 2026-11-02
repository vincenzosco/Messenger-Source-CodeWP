using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class Address
	{
		[JsonProperty(PropertyName = "city")]
		[DataMember]
		public string City { get; set; }

		[JsonProperty(PropertyName = "country")]
		[DataMember]
		public string Country { get; set; }

		[JsonProperty(PropertyName = "postal_code")]
		[DataMember]
		public string PostalCode { get; set; }

		[JsonProperty(PropertyName = "street")]
		[DataMember]
		public string Street { get; set; }
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class PhoneNumber
	{
		[DataMember]
		[JsonProperty(PropertyName = "display_number")]
		public string DisplayNumber { get; set; }

		[JsonProperty(PropertyName = "universal_number")]
		[DataMember]
		public string UniversalNumber { get; set; }
	}
}

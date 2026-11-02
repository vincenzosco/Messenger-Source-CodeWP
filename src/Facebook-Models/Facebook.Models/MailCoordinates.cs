using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class MailCoordinates
	{
		[DataMember]
		[JsonProperty("latitude")]
		public double Latitude { get; set; }

		[DataMember]
		[JsonProperty("longitude")]
		public double Longitude { get; set; }

		[DataMember]
		[JsonProperty("accuracy")]
		public double Accuracy { get; set; }
	}
}

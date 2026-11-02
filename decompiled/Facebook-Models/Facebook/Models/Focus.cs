using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Focus
	{
		[DataMember]
		[JsonProperty(PropertyName = "x")]
		public double? X { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "y")]
		public double? Y { get; set; }
	}
}

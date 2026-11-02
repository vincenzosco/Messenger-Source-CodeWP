using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class BaseRange
	{
		[JsonProperty(PropertyName = "length")]
		[DataMember]
		public int Length { get; set; }

		[JsonProperty(PropertyName = "offset")]
		[DataMember]
		public int Offset { get; set; }
	}
}

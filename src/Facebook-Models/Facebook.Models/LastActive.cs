using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class LastActive
	{
		[DataMember]
		[JsonProperty(PropertyName = "time")]
		public long Timestamp { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "active")]
		public bool Active { get; set; }
	}
}

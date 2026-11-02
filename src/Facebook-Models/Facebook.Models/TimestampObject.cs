using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class TimestampObject
	{
		[DataMember]
		[JsonProperty("time")]
		public long Timestamp { get; set; }
	}
}

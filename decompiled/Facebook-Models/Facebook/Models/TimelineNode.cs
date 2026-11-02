using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class TimelineNode
	{
		[JsonProperty("node")]
		[DataMember]
		public TimelineObject TimelineObject { get; set; }
	}
}

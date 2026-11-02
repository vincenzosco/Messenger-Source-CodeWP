using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class TimelineAppSection
	{
		[DataMember]
		[JsonProperty("id")]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty("items")]
		public TimelineItems Items { get; set; }

		[JsonProperty("name")]
		[DataMember]
		public string Name { get; set; }
	}
}

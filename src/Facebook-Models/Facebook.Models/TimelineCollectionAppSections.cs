using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class TimelineCollectionAppSections
	{
		[JsonProperty("nodes")]
		[DataMember]
		public List<TimelineCollectionAppSection> TimeLineCollectionAppSectionNodes { get; set; }
	}
}

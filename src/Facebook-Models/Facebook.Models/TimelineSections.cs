using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class TimelineSections
	{
		[DataMember]
		[JsonProperty(PropertyName = "edges")]
		public List<TimelineSection> Sections { get; set; }
	}
}

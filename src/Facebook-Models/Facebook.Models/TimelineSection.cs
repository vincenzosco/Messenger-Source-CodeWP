using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class TimelineSection
	{
		[DataMember]
		[JsonProperty(PropertyName = "node")]
		public TimelineSectionNode Node { get; set; }
	}
}

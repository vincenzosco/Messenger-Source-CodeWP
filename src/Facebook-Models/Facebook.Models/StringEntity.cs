using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class StringEntity
	{
		[DataMember]
		[JsonProperty(PropertyName = "aggregated_ranges")]
		public List<MessageAggregatedRange> AggregatedRanges { get; set; }

		[JsonProperty(PropertyName = "ranges")]
		[DataMember]
		public List<MessageRange> Ranges { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }
	}
}

using System;
using System.Runtime.Serialization;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models;

public class TimeRange
{
	[JsonConverter(typeof(FormattedDateTimeConverter))]
	[JsonProperty(PropertyName = "start")]
	[DataMember]
	public DateTime Start { get; set; }

	[JsonConverter(typeof(FormattedDateTimeConverter))]
	[JsonProperty(PropertyName = "end")]
	[DataMember]
	public DateTime End { get; set; }

	[JsonProperty(PropertyName = "timezone")]
	[DataMember]
	public string TimeZone { get; set; }
}

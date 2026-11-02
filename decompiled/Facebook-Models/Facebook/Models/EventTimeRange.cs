using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

public class EventTimeRange
{
	[JsonProperty("start")]
	[DataMember]
	public string Start { get; set; }

	[DataMember]
	[JsonProperty("end")]
	public string End { get; set; }

	[JsonProperty("timezone")]
	[DataMember]
	public string TimeZone { get; set; }

	public bool IsDateOnly
	{
		get
		{
			if (Start != null)
			{
				return Start.Length == 10;
			}
			return false;
		}
	}
}

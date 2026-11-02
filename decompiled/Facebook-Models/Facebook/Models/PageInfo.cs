using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class PageInfo
{
	[JsonProperty(PropertyName = "start_cursor")]
	[DataMember]
	public string StartCursor { get; set; }

	[JsonProperty(PropertyName = "end_cursor")]
	[DataMember]
	public string EndCursor { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "has_next_page")]
	public bool? HasNextPage { get; set; }

	[JsonProperty(PropertyName = "has_previous_page")]
	[DataMember]
	public bool? HasPreviousPage { get; set; }
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SearchImages;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class ImageSearchResult
{
	[JsonProperty("Version")]
	public string Version { get; set; }

	[JsonProperty("Total")]
	public long Total { get; set; }

	[JsonProperty("Offset")]
	public long Offset { get; set; }

	[JsonProperty("Results")]
	public List<Image> Results { get; set; }
}

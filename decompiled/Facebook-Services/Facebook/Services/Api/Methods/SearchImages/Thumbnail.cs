using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SearchImages;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class Thumbnail
{
	[JsonProperty("Url")]
	public string Url { get; set; }

	[JsonProperty("ContentType")]
	public string ContentType { get; set; }

	[JsonProperty("Width")]
	public int Width { get; set; }

	[JsonProperty("Height")]
	public int Height { get; set; }

	[JsonProperty("FileSize")]
	public int FileSize { get; set; }
}

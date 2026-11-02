using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SearchImages
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Image
	{
		[JsonProperty("Title")]
		public string Title { get; set; }

		[JsonProperty("MediaUrl")]
		public string MediaUrl { get; set; }

		[JsonProperty("Url")]
		public string Url { get; set; }

		[JsonProperty("DisplayUrl")]
		public string DisplayUrl { get; set; }

		[JsonProperty("Width")]
		public int Width { get; set; }

		[JsonProperty("Height")]
		public int Height { get; set; }

		[JsonProperty("FileSize")]
		public int FileSize { get; set; }

		[JsonProperty("Thumbnail")]
		public Thumbnail Thumbnail { get; set; }
	}
}

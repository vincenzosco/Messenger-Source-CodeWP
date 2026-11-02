using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SearchImages
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class SearchResponse
	{
		[JsonProperty("Version")]
		public string Version { get; set; }

		[JsonProperty("Query")]
		public Query Query { get; set; }

		[JsonProperty("Image")]
		public ImageSearchResult ImageResult { get; set; }
	}
}

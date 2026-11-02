using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SearchImages
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Response : Facebook.Base.Response
	{
		[JsonProperty("SearchResponse")]
		public SearchResponse SearchResponse { get; set; }
	}
}

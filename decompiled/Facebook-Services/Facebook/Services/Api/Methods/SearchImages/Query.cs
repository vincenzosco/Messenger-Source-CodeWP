using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SearchImages
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Query
	{
		[JsonProperty("SearchTerms")]
		public string SearchTerms { get; set; }
	}
}

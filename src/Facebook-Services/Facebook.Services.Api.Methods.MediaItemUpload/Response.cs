using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.MediaItemUpload
{
	[JsonConverter(typeof(FbidResponseConverter))]
	public class Response : FacebookApiResponse
	{
		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Fbid { get; set; }
	}
}

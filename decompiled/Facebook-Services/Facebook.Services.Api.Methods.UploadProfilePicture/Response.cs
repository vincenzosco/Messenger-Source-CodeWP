using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.UploadProfilePicture
{
	public class Response : FacebookApiResponse
	{
		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }
	}
}

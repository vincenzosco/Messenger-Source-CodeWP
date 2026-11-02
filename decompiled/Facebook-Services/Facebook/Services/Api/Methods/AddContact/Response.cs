using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.AddContact;

public class Response : FacebookApiResponse
{
	[DataMember]
	[JsonProperty(PropertyName = "id")]
	public string ContactId { get; set; }
}

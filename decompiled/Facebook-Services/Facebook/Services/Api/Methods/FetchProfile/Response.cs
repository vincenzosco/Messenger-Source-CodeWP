using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchProfile;

public class Response : FacebookApiResponse
{
	[JsonProperty(PropertyName = "data")]
	[DataMember]
	public List<FacebookProfile> Profiles;
}

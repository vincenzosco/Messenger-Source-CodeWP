using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchPresence
{
	public class Response : FacebookApiResponse
	{
		[DataMember]
		[JsonProperty(PropertyName = "data")]
		public List<FacebookUser> OnlineProfiles;
	}
}

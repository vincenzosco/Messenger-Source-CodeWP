using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchSearchability
{
	public class Response : FacebookApiResponse
	{
		[DataMember]
		[JsonProperty(PropertyName = "data")]
		public List<PrivacyOption> PrivacyOptions;

		public PhoneSearchabilityTypes Searchability;
	}
}

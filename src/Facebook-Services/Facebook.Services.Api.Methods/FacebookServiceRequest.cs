using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FacebookServiceRequest : Request
	{
		[DataMember]
		[JsonProperty(PropertyName = "access_token")]
		public string AccessToken { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "locale")]
		public string Locale { get; set; }
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.Login
{
	public class AuthResponse : FacebookApiResponse
	{
		[JsonProperty(PropertyName = "access_token")]
		[DataMember]
		public string AccessToken { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "session_key")]
		public string SessionKey { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "secret")]
		public string Secret { get; set; }

		[JsonProperty(PropertyName = "uid")]
		[DataMember]
		public string UserId { get; set; }

		[JsonProperty(PropertyName = "machine_id")]
		[DataMember]
		public string MachineId { get; set; }
	}
}

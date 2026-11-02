using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchAppConfig
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class ClientConfig
	{
		[JsonProperty("min_version")]
		public string MinVersion;

		[JsonProperty("current_version")]
		public string CurrentVersion;

		[JsonProperty("new_version_url")]
		public string NewVersionUrl;
	}
}

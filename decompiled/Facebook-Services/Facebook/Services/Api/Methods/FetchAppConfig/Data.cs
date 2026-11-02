using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchAppConfig;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class Data
{
	[JsonProperty("client_config")]
	public ClientConfig ClientConfig;
}

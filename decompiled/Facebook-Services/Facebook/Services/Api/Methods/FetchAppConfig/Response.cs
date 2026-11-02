using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchAppConfig;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class Response : FacebookApiResponse
{
	[JsonProperty("data")]
	public List<Data> Data;

	public ClientConfig ClientConfig
	{
		get
		{
			if (Data == null || Data.Count != 1)
			{
				return null;
			}
			return Data[0].ClientConfig;
		}
	}
}

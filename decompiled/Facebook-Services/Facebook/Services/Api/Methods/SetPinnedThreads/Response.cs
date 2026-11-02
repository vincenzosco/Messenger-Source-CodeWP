using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SetPinnedThreads;

public class Response : FacebookApiResponse
{
	[JsonProperty("updated_time")]
	public long? UpdatedTime;
}

using Facebook.Services.Api.Methods;
using Newtonsoft.Json;

namespace Facebook.Services.Common
{
	[JsonConverter(typeof(BoolResponseConverter))]
	public class BoolResponse : FacebookApiResponse
	{
		public bool Succeeded { get; set; }
	}
}

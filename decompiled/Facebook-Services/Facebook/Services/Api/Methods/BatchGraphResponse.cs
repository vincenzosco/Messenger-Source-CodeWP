using System.Collections.Generic;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods
{
	[JsonConverter(typeof(BatchGraphResponseConverter))]
	public class BatchGraphResponse : FacebookApiResponse
	{
		public List<GraphResponse> Responses { get; set; }
	}
}

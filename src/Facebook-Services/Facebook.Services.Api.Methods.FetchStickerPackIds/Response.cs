using System.Collections.Generic;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchStickerPackIds
{
	[JsonConverter(typeof(StickerPacksIdConverter))]
	public class Response : FacebookApiResponse
	{
		public List<string> StickerPackIds { get; set; }
	}
}

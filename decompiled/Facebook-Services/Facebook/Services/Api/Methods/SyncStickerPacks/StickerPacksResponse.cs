using System.Collections.Generic;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SyncStickerPacks;

[JsonConverter(typeof(ResponseConverter))]
public class StickerPacksResponse : FacebookApiResponse
{
	public IList<StickerPack> StickerPacks { get; set; }
}

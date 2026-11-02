using System.Collections.Generic;
using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchStickerPacks;

public class Response : Facebook.Base.Response
{
	public List<StickerPack> StickerPacks { get; set; }

	public List<string> DownloadingStickerPackIds { get; set; }

	public List<Sticker> RecentStickers { get; set; }
}

using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchStickerPack;

public class Response : Facebook.Base.Response
{
	public StickerPack StickerPack { get; set; }

	public bool IsDownloading { get; set; }
}

using Facebook.Base;

namespace Facebook.Services.Api.Methods.DownloadStickerPack;

public class Request : Facebook.Base.Request
{
	public string StickerPackId { get; set; }
}

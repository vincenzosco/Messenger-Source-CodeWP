using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchStickerPacks;

[Queue(QueueType.Local)]
public class Request : Facebook.Base.Request
{
	public StickerPackType StickerPackType { get; set; }
}

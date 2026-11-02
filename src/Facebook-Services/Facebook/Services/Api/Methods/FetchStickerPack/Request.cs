using Facebook.Base;

namespace Facebook.Services.Api.Methods.FetchStickerPack
{
	[Queue(QueueType.Local)]
	public class Request : Facebook.Base.Request
	{
		public string StickerPackId { get; set; }
	}
}

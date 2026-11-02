namespace Facebook.Services.Api.Methods.AddStickerPack
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string StickerPackId { get; set; }
	}
}

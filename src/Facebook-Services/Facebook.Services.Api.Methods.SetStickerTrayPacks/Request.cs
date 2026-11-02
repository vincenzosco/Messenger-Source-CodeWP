using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.SetStickerTrayPacks
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public List<string> StickerPackIds { get; set; }
	}
}

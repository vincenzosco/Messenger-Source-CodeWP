using System.Collections.Generic;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.SyncStickerPacks
{
	public class Response : FacebookApiResponse
	{
		public IList<StickerPack> AvailableStickerPacks { get; set; }

		public IList<StickerPack> TrayStickerPacks { get; set; }

		public IList<StickerPack> OwnedStickerPacks { get; set; }
	}
}

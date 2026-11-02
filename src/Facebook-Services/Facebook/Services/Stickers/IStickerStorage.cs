using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Models;

namespace Facebook.Services.Stickers
{
	public interface IStickerStorage
	{
		Task<Uri> GenerateStickerUri(string stickerPackId, string stickerId, bool animatedSticker);

		Task<Uri> GetStickerUri(string stickerId, bool animated);

		Task<bool> DownloadStickerPack(StickerPack sp, CancellationToken cancellationToken, Action<double> progressUpdate = null);

		Task DeleteStickerPack(StickerPack sp);

		void Reset();
	}
}

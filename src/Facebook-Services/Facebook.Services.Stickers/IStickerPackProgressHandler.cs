namespace Facebook.Services.Stickers
{
	public interface IStickerPackProgressHandler
	{
		void DownloadStarted(string stickerPackId);

		void DownloadFailed(string stickerPackId);

		void ProgressUpdated(string stickerPackId, double progress);
	}
}

using System;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class StickerSelectedArgs : EventArgs
	{
		public readonly string StickerId;

		public readonly string StickerUrl;

		public StickerSelectedArgs(string stickerId, string stickerUrl)
		{
			StickerId = stickerId;
			StickerUrl = stickerUrl;
		}
	}
}

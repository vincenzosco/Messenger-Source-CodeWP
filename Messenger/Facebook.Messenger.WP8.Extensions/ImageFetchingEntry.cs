using System;
using System.Threading;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Cache;

namespace Facebook.Messenger.WP8.Extensions
{
	internal class ImageFetchingEntry : FetchingEntry<Image>
	{
		public byte[] ImageData { get; set; }

		public ImageFetchingEntry(Uri uri, CancellationToken token)
			: base(uri, token)
		{
		}
	}
}

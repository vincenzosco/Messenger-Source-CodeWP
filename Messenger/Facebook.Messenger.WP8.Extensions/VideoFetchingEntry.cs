using System;
using System.Threading;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Cache;

namespace Facebook.Messenger.WP8.Extensions
{
	internal class VideoFetchingEntry : FetchingEntry<MediaElement>
	{
		public Uri LocalUri { get; set; }

		public VideoFetchingEntry(Uri uri, CancellationToken token)
			: base(uri, token)
		{
		}
	}
}

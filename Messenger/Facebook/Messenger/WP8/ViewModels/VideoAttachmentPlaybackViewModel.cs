using System;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Extensions;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class VideoAttachmentPlaybackViewModel : MediaPlaybackViewModel
	{
		public void SetupMediaElement(PlaybackListener listener, Panel container, double width, double height, Uri source)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected Obj, but got Unknown
			if (Media != null)
			{
				if ((object)((FrameworkElement)Media).Parent == container)
				{
					Uri videoUri = MediaElementCacheExtensions.GetVideoUri((FrameworkElement)(object)Media);
					if (videoUri == null || videoUri != source || Media.Source == null)
					{
						if (videoUri == source)
						{
							MediaElementCacheExtensions.SetVideoUri((FrameworkElement)(object)Media, null);
						}
						MediaElementCacheExtensions.SetVideoUri((FrameworkElement)(object)Media, source);
						MediaElementCacheExtensions.SetVideoDownloadedHandler((FrameworkElement)(object)Media, VideoDownloaded);
						((FrameworkElement)Media).Width = width;
						((FrameworkElement)Media).Height = height;
						NotifyState(State.Loading);
					}
					return;
				}
				Reset();
				RemoveMediaFromVisualTree();
			}
			SetListener(listener);
			MediaElement val = new MediaElement();
			((FrameworkElement)val).Width = width;
			((FrameworkElement)val).Height = height;
			((UIElement)val).Opacity = 0.0;
			Media = val;
			MediaElementCacheExtensions.SetVideoUri((FrameworkElement)(object)Media, source);
			MediaElementCacheExtensions.SetVideoDownloadedHandler((FrameworkElement)(object)Media, VideoDownloaded);
			((PresentationFrameworkCollection<UIElement>)(object)container.Children).Add((UIElement)(object)Media);
			NotifyState(State.Loading);
		}

		public override void Reset()
		{
			if (Media != null)
			{
				MediaElementCacheExtensions.SetVideoUri((FrameworkElement)(object)Media, null);
				Media.Source = null;
			}
			base.Reset();
		}

		private void VideoDownloaded(Uri uri)
		{
			ToggleCurrentMedia();
		}
	}
}

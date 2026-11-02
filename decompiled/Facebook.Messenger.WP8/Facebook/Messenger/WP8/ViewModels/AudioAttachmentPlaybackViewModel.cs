using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Controls;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class AudioAttachmentPlaybackViewModel : MediaPlaybackViewModel
	{
		private string _messageId;

		public AudioAttachmentPlaybackViewModel()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected Obj, but got Unknown
			MediaElement val = new MediaElement();
			((UIElement)val).Visibility = (Visibility)1;
			val.AutoPlay = true;
			Media = val;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void PlayPauseMedia(string messageId, string attachmentId, PlaybackListener listener)
		{
			if (messageId == _messageId)
			{
				ToggleCurrentMedia();
			}
			else
			{
				PlayNewMedia(messageId, attachmentId, listener);
			}
		}

		private void PlayNewMedia(string messageId, string attachmentId, PlaybackListener listener)
		{
			Reset();
			SetListener(listener);
			_messageId = messageId;
			Media.Source = new Uri(AttachmentUriGenerator.UrlForAttachment(attachmentId, messageId));
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void RegisterIfPlaying(string messageId, PlaybackListener listener)
		{
			if (_messageId == messageId)
			{
				SetListener(listener);
				NotifyState();
			}
		}

		public override void Reset()
		{
			if (Media != null)
			{
				Media.Source = null;
			}
			base.Reset();
			SetListener(null);
			_messageId = null;
		}
	}
}

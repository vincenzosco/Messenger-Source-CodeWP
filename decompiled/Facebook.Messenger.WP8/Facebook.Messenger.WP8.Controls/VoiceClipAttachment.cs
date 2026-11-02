using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls
{
	public class VoiceClipAttachment : UserControl, MediaPlaybackViewModel.PlaybackListener, IResetableView
	{
		private const double MaxBarLengthFromLoggedInUser = 213.0;

		private const double MaxBarLengthFromOther = 193.0;

		private const double MinBarLength = 40.0;

		private static readonly ImageSource PlayButtonAsset;

		private static readonly ImageSource PauseButtonAsset;

		private static AudioAttachmentPlaybackViewModel AudioAttachmentPlayback;

		private TimeSpan _voiceClipDuration;

		private string _messageId;

		private string _attachmentId;

		private readonly SolidColorBrush FromMeTextBrush;

		private readonly SolidColorBrush FromFriendTextBrush;

		internal StackPanel LayoutRoot;

		internal Path PauseButton;

		internal Path PlayButton;

		internal ProgressBar VoiceClipProgressBar;

		internal TextBlock ProgressText;

		private bool _contentLoaded;

		static VoiceClipAttachment()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected Obj, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
			PlayButtonAsset = (ImageSource)new BitmapImage(new Uri("/Assets/AppBar/Play.png", UriKind.Relative));
			PauseButtonAsset = (ImageSource)new BitmapImage(new Uri("/Assets/AppBar/Pause.png", UriKind.Relative));
			AudioAttachmentPlayback = ServiceLocator.Current.GetInstance<AudioAttachmentPlaybackViewModel>();
		}

		public VoiceClipAttachment()
		{
			object obj = Application.Current.Resources[(object)"MessengerWhiteBrush"];
			FromMeTextBrush = (SolidColorBrush)((obj is SolidColorBrush) ? obj : null);
			object obj2 = Application.Current.Resources[(object)"MessengerBlackBrush"];
			FromFriendTextBrush = (SolidColorBrush)((obj2 is SolidColorBrush) ? obj2 : null);
			((UserControl)this)._002Ector();
			InitializeComponent();
		}

		public void ResetView(bool recycled)
		{
			AudioAttachmentPlayback.ClearListener(this);
			_voiceClipDuration = TimeSpan.Zero;
			StateChanged(MediaPlaybackViewModel.State.Stopped, TimeSpan.Zero);
		}

		private static double ComputeWidthFromDuration(TimeSpan duration, double lowWidth, double maxWidth)
		{
			double num = Math.Max(5.0, duration.TotalSeconds);
			double num2 = (1.0 - Math.Pow(10.0, num / -30.0)) * (maxWidth - lowWidth);
			return num2 + lowWidth;
		}

		public void UpdateView(MessageViewModel m, bool fromLoggedInUser)
		{
			((UIElement)LayoutRoot).IsHitTestVisible = m.IsMessageSent();
			MailAttachment mailAttachment = m.AudioAttachments.ElementAt(0);
			_voiceClipDuration = mailAttachment.GetAudioClipDuration();
			double maxWidth = (fromLoggedInUser ? 213.0 : 193.0);
			((FrameworkElement)VoiceClipProgressBar).Width = ComputeWidthFromDuration(_voiceClipDuration, 40.0, maxWidth);
			_messageId = m.Id;
			_attachmentId = m.AudioAttachments.ElementAt(0).Id;
			SetDisplayTime(_voiceClipDuration);
			AudioAttachmentPlayback.RegisterIfPlaying(_messageId, this);
			ProgressText.Foreground = (Brush)(object)(fromLoggedInUser ? FromMeTextBrush : FromFriendTextBrush);
			((Shape)PlayButton).Fill = (Brush)(object)(fromLoggedInUser ? FromMeTextBrush : FromFriendTextBrush);
			((Shape)PauseButton).Fill = (Brush)(object)(fromLoggedInUser ? FromMeTextBrush : FromFriendTextBrush);
			((Control)VoiceClipProgressBar).Foreground = (Brush)(object)(fromLoggedInUser ? FromMeTextBrush : FromFriendTextBrush);
			ProgressBar voiceClipProgressBar = VoiceClipProgressBar;
			object background;
			if (!fromLoggedInUser)
			{
				object obj = Application.Current.Resources[(object)"VoiceClipProgressBarBackgroundGrayBrush"];
				background = ((obj is SolidColorBrush) ? obj : null);
			}
			else
			{
				object obj2 = Application.Current.Resources[(object)"VoiceClipProgressBarBackgroundBlueBrush"];
				background = ((obj2 is SolidColorBrush) ? obj2 : null);
			}
			((Control)voiceClipProgressBar).Background = (Brush)background;
		}

		public void StateChanged(MediaPlaybackViewModel.State state, TimeSpan position)
		{
			bool isIndeterminate = false;
			double value = position.TotalMilliseconds / _voiceClipDuration.TotalMilliseconds;
			TimeSpan timeSpan = position - _voiceClipDuration;
			TimeSpan displayTime;
			switch (state)
			{
			case MediaPlaybackViewModel.State.Loading:
				isIndeterminate = true;
				displayTime = _voiceClipDuration;
				value = 0.0;
				break;
			case MediaPlaybackViewModel.State.Playing:
				displayTime = timeSpan;
				break;
			case MediaPlaybackViewModel.State.Paused:
				displayTime = timeSpan;
				break;
			case MediaPlaybackViewModel.State.Stopped:
				displayTime = _voiceClipDuration;
				value = 0.0;
				break;
			default:
				throw new Exception("Invalid state");
			}
			((UIElement)PlayButton).Visibility = (Visibility)(state == MediaPlaybackViewModel.State.Playing);
			((UIElement)PauseButton).Visibility = (Visibility)((state != MediaPlaybackViewModel.State.Playing) ? 1 : 0);
			SetDisplayTime(displayTime);
			((RangeBase)VoiceClipProgressBar).Value = value;
			VoiceClipProgressBar.IsIndeterminate = isIndeterminate;
		}

		private void SetDisplayTime(TimeSpan span)
		{
			string text = span.ToString("m\\:ss");
			ProgressText.Text = text;
		}

		private void PlayPauseButton_OnTap(object sender, GestureEventArgs e)
		{
			AudioAttachmentPlayback.PlayPauseMedia(_messageId, _attachmentId, this);
			e.Handled = true;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/VoiceClipAttachment.xaml", UriKind.Relative));
				LayoutRoot = (StackPanel)((FrameworkElement)this).FindName("LayoutRoot");
				PauseButton = (Path)((FrameworkElement)this).FindName("PauseButton");
				PlayButton = (Path)((FrameworkElement)this).FindName("PlayButton");
				VoiceClipProgressBar = (ProgressBar)((FrameworkElement)this).FindName("VoiceClipProgressBar");
				ProgressText = (TextBlock)((FrameworkElement)this).FindName("ProgressText");
			}
		}
	}
}

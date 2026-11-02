using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class VideoAttachmentControl : BaseControl, MediaPlaybackViewModel.PlaybackListener, IResetableView
	{
		private const double MaxVideoWidth = 280.0;

		private const double MaxVideoHeight = 280.0;

		private Uri _videoSource;

		private int _angle;

		private static VideoAttachmentPlaybackViewModel VideoAttachmentPlayback;











		public event EventHandler<bool> OnVideoTouch;

		static VideoAttachmentControl()
		{
			VideoAttachmentPlayback = new VideoAttachmentPlaybackViewModel();
		}

		public VideoAttachmentControl()
		{
			InitializeComponent();
			base.ManipulationStarted += OnManipulationStarted;
			base.ManipulationCompleted += OnManipulationCompleted;
		}

		private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			if (OnVideoTouch != null)
			{
				OnVideoTouch(this, false);
			}
		}

		private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			if (OnVideoTouch != null)
			{
				OnVideoTouch(this, true);
			}
		}

		public void UpdateView(MessageViewModel m)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (m != null)
			{
				bool flag = m.State == MailMessage.MessageState.Queued;
				bool flag2 = m.IsMessageSent();
				bool flag3 = m.State == MailMessage.MessageState.Failed;
				((UIElement)MediaCanvas).Visibility = VisibilityUtil.FromBool(flag2);
				((UIElement)LayoutRoot).IsHitTestVisible = flag2;
				UpdatePlayButton(!flag, !flag3);
				MailAttachment.VideoData videoData = GetVideoData(m);
				if (videoData != null)
				{
					TimeSpan timeSpan = TimeSpan.FromSeconds(videoData.Length);
					DurationTextBlock.Text = $"{timeSpan.Minutes}:{timeSpan.Seconds:00}";
					double width;
					double height;
					CalculateVideoWidthAndHeight(videoData, out width, out height);
					SetupVideo(videoData, width, height);
					ResetPreviewImage();
					UpdatePreviewImage(videoData, width, height);
				}
			}
		}

		private static void CalculateVideoWidthAndHeight(MailAttachment.VideoData videoData, out double width, out double height)
		{
			width = videoData.Width;
			height = videoData.Height;
			if (videoData.Width > videoData.Height)
			{
				if ((double)videoData.Width > 280.0)
				{
					width = 280.0;
					height = height * 280.0 / (double)videoData.Width;
				}
			}
			else if (videoData.Height > videoData.Width)
			{
				if ((double)videoData.Height > 280.0)
				{
					height = 280.0;
					width = width * 280.0 / (double)videoData.Height;
				}
			}
			else if ((double)videoData.Height > 280.0)
			{
				width = 280.0;
				height = 280.0;
			}
		}

		private void UpdatePreviewImage(MailAttachment.VideoData vd, double width, double height)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected Obj, but got Unknown
			Uri result;
			Uri.TryCreate(vd.PreviewUrl, UriKind.Absolute, out result);
			Image val = new Image();
			((FrameworkElement)val).MaxWidth = width;
			((FrameworkElement)val).MaxHeight = height;
			val.Stretch = (Stretch)1;
			Image val2 = val;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)PreviewImageContainer).Children).Add((UIElement)(object)val2);
			ImageCacheExtensions.SetImageUri((FrameworkElement)(object)val2, result);
		}

		private MailAttachment.VideoData GetVideoData(MessageViewModel m)
		{
			MailAttachment mailAttachment = m.Attachments.FirstOrDefault();
			if (mailAttachment == null || !mailAttachment.IsVideo())
			{
				return null;
			}
			return mailAttachment.Video;
		}

		private void SetupVideo(MailAttachment.VideoData vd, double width, double height)
		{
			Canvas mediaCanvas = MediaCanvas;
			double width2 = (((FrameworkElement)SizeGrid).Width = width);
			((FrameworkElement)mediaCanvas).Width = width2;
			Canvas mediaCanvas2 = MediaCanvas;
			double height2 = (((FrameworkElement)SizeGrid).Height = height);
			((FrameworkElement)mediaCanvas2).Height = height2;
			_angle = vd.Rotation;
			Uri result;
			Uri.TryCreate(vd.Url, UriKind.Absolute, out result);
			_videoSource = result;
		}

		private void PreviewImage_OnTap(object sender, GestureEventArgs e)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected Obj, but got Unknown
			int num = 10;
			VideoAttachmentPlayback.SetupMediaElement(this, (Panel)(object)MediaElementContainer, num, num, _videoSource);
			VideoBrush.SetSource(VideoAttachmentPlayback.Media);
			RotateTransform val = new RotateTransform();
			val.CenterX = 0.5;
			val.CenterY = 0.5;
			val.Angle = (double)_angle + 0.001;
			RotateTransform relativeTransform = val;
			((Brush)VideoBrush).RelativeTransform = (Transform)(object)relativeTransform;
			VideoAttachmentPlayback.ToggleCurrentMedia();
		}

		public void ResetView(bool recycled)
		{
			if (recycled)
			{
				((UIElement)PlayButton).Visibility = Visibility.Visible;
				ProgressBar.IsIndeterminate = false;
				((UIElement)ProgressBar).Visibility = Visibility.Collapsed;
				((UIElement)DurationTextBlock).Visibility = Visibility.Visible;
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)MediaElementContainer).Children).Clear();
				VideoAttachmentPlayback.ClearListener(this);
				ResetPreviewImage();
			}
		}

		private void ResetPreviewImage()
		{
			if (((PresentationFrameworkCollection<UIElement>)(object)((Panel)PreviewImageContainer).Children).Count != 0)
			{
				UIElement val = ((PresentationFrameworkCollection<UIElement>)(object)((Panel)PreviewImageContainer).Children)[0];
				Image val2 = (Image)(object)((val is Image) ? val : null);
				if (val2 != null)
				{
					ResetImage(val2);
				}
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)PreviewImageContainer).Children).Clear();
			}
		}

		private static void ResetImage(Image image)
		{
			if (image != null)
			{
				ImageSource source = image.Source;
				BitmapImage val = (BitmapImage)(object)((source is BitmapImage) ? source : null);
				if (val != null)
				{
					val.UriSource = null;
				}
				image.Source = null;
			}
		}

		public void StateChanged(MediaPlaybackViewModel.State state, TimeSpan position)
		{
			UpdatePlayButton(state != MediaPlaybackViewModel.State.Playing && state != MediaPlaybackViewModel.State.Loading);
			UpdateProgressBar(state == MediaPlaybackViewModel.State.Loading);
			UpdateDurationText(state != MediaPlaybackViewModel.State.Playing);
		}

		private void UpdateDurationText(bool visible)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)DurationTextBlock).Visibility = VisibilityUtil.FromBool(visible);
		}

		private void UpdatePlayButton(bool visible, bool enabled = true)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)PlayButton).Visibility = VisibilityUtil.FromBool(visible);
			PlayButton.BackgroundSource = new Uri(enabled ? "/Assets/Camera/ViewfinderButtonBackground.png" : "/Assets/Camera/ViewfinderDisabledButtonBackground.png", UriKind.Relative);
		}

		private void UpdateProgressBar(bool visible)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			ProgressBar.IsIndeterminate = visible;
			((UIElement)ProgressBar).Visibility = VisibilityUtil.FromBool(visible);
		}

	}
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls
{
	public class NotificationDrawer : UserControl
	{
		private readonly Popup _notificationPopup;

		internal Storyboard Animation;

		internal EasingDoubleKeyFrame ShowKeyFrame;

		internal Grid LayoutRoot;

		internal UserTile ProfilePhoto;

		internal TextBlock SenderName;

		internal TextBlock Snippet;

		private bool _contentLoaded;

		public event EventHandler<NotificationViewModel> NotificationTapped;

		public NotificationDrawer()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected Obj, but got Unknown
			InitializeComponent();
			Popup val = new Popup();
			val.Child = (UIElement)(object)this;
			_notificationPopup = val;
		}

		public void SetNotification(NotificationViewModel vm)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			((FrameworkElement)this).DataContext = vm;
			_notificationPopup.IsOpen = true;
			if ((int)Animation.GetCurrentState() == 0)
			{
				Storyboard animation = Animation;
				KeyTime keyTime = ((DoubleKeyFrame)ShowKeyFrame).KeyTime;
				animation.Seek(keyTime.TimeSpan);
			}
			else
			{
				Animation.Begin();
			}
		}

		private void OnCancelClicked(object sender, RoutedEventArgs e)
		{
			Animation.SkipToFill();
		}

		private void NotificationDrawer_OnTap(object sender, GestureEventArgs e)
		{
			Animation.SkipToFill();
			NotificationViewModel notificationViewModel = ((FrameworkElement)this).DataContext as NotificationViewModel;
			if (NotificationTapped != null && notificationViewModel != null)
			{
				NotificationTapped(this, notificationViewModel);
			}
		}

		private void Animation_OnCompleted(object sender, EventArgs e)
		{
			((FrameworkElement)this).DataContext = null;
			_notificationPopup.IsOpen = false;
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
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/NotificationDrawer.xaml", UriKind.Relative));
				Animation = (Storyboard)((FrameworkElement)this).FindName("Animation");
				ShowKeyFrame = (EasingDoubleKeyFrame)((FrameworkElement)this).FindName("ShowKeyFrame");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				ProfilePhoto = (UserTile)((FrameworkElement)this).FindName("ProfilePhoto");
				SenderName = (TextBlock)((FrameworkElement)this).FindName("SenderName");
				Snippet = (TextBlock)((FrameworkElement)this).FindName("Snippet");
			}
		}
	}
}

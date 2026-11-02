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
	public partial class NotificationDrawer : UserControl
	{
		private readonly Popup _notificationPopup;








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
			base.DataContext = vm;
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
			NotificationViewModel notificationViewModel = base.DataContext as NotificationViewModel;
			if (NotificationTapped != null && notificationViewModel != null)
			{
				NotificationTapped(this, notificationViewModel);
			}
		}

		private void Animation_OnCompleted(object sender, EventArgs e)
		{
			base.DataContext = null;
			_notificationPopup.IsOpen = false;
		}

	}
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Facebook.Messenger.WP8.Controls.MainPanorama
{
	public class SwitchChatPopup : FacebookPopup
	{
		internal Grid SwitchChatPopupGrid;

		private bool _contentLoaded;

		public event EventHandler ChatEnabled;

		public event EventHandler ChatDisabled;

		public SwitchChatPopup()
		{
			InitializeComponent();
		}

		private void EnableChatTapped(object sender, GestureEventArgs e)
		{
			if (ChatEnabled != null)
			{
				ChatEnabled(sender, (EventArgs)(object)e);
			}
		}

		private void DisableChatTapped(object sender, GestureEventArgs e)
		{
			if (ChatDisabled != null)
			{
				ChatDisabled(sender, (EventArgs)(object)e);
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/MainPanorama/SwitchChatPopup.xaml", UriKind.Relative));
				SwitchChatPopupGrid = (Grid)((FrameworkElement)this).FindName("SwitchChatPopupGrid");
			}
		}
	}
}

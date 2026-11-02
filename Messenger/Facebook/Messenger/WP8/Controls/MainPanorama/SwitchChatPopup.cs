using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Facebook.Messenger.WP8.Controls.MainPanorama
{
	public partial class SwitchChatPopup : FacebookPopup
	{


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

	}
}

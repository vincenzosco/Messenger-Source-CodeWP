using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class GroupsNuxControl : UserControl
	{



		public event EventHandler OnDismissClick;

		public GroupsNuxControl()
		{
			InitializeComponent();
		}

		private void ButtonOnClick(object sender, GestureEventArgs e)
		{
			if (OnDismissClick != null)
			{
				OnDismissClick(this, (EventArgs)(object)e);
			}
		}

		public void SetButtonLabel(bool hasPinnedThreads)
		{
			if (hasPinnedThreads)
			{
				NuxButtonText.Text = StringLibrary.Groups_NUX_Button_Text;
			}
			else
			{
				NuxButtonText.Text = StringLibrary.Groups_NUX_NoThread_Button_Text;
			}
		}

	}
}

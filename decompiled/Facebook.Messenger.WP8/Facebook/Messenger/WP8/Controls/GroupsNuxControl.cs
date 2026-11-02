using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Controls
{
	public class GroupsNuxControl : UserControl
	{
		internal Grid LayoutRoot;

		internal TextBlock NuxButtonText;

		private bool _contentLoaded;

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

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/GroupsNuxControl.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				NuxButtonText = (TextBlock)((FrameworkElement)this).FindName("NuxButtonText");
			}
		}
	}
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Facebook.Messenger.Localization.Resources;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls.Contacts
{
	internal class EnableContactSyncMessageBox
	{
		public static void Show(RelayCommand learnMoreCommand, Action acceptAction, Action cancelAction)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected Obj, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			HyperlinkButton val = new HyperlinkButton();
			object obj = Application.Current.Resources[(object)"HyperlinkButtonStyle"];
			((FrameworkElement)val).Style = (Style)((obj is Style) ? obj : null);
			((FrameworkElement)val).HorizontalAlignment = (HorizontalAlignment)0;
			((Control)val).Padding = new Thickness(0.0, 12.0, 0.0, 0.0);
			((ContentControl)val).Content = StringLibrary.SyncContacts_MessageBox_LinkLabel;
			((ButtonBase)val).Command = learnMoreCommand;
			HyperlinkButton learnMoreLink = val;
			CustomMessageBox customMessageBox = new CustomMessageBox();
			customMessageBox.Caption = StringLibrary.Settings_Sync_Contacts_Title;
			customMessageBox.Message = StringLibrary.Settings_Sync_Contacts_Body;
			customMessageBox.LeftButtonContent = StringLibrary.Settings_Sync_Contacts_Confirmation;
			customMessageBox.RightButtonContent = StringLibrary.MessageBox_Popup_Cancel;
			((ContentControl)customMessageBox).Content = learnMoreLink;
			customMessageBox.IsFullScreen = false;
			CustomMessageBox customMessageBox2 = customMessageBox;
			customMessageBox2.Dismissed += (object o, DismissedEventArgs args) =>
			{
				if (args.Result == CustomMessageBoxResult.LeftButton)
				{
					if (acceptAction != null)
					{
						acceptAction();
					}
				}
				else if (cancelAction != null)
				{
					cancelAction();
				}
				((ButtonBase)learnMoreLink).Command = null;
			};
			customMessageBox2.Show();
		}
	}
}

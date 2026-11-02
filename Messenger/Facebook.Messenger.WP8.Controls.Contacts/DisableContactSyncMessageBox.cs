using System;
using Facebook.Messenger.Localization.Resources;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls.Contacts
{
	internal class DisableContactSyncMessageBox
	{
		public static void Show(Action acceptAction, Action cancelAction)
		{
			CustomMessageBox customMessageBox = new CustomMessageBox();
			customMessageBox.Caption = StringLibrary.Settings_Remove_Contacts_Title;
			customMessageBox.Message = StringLibrary.Settings_Remove_Contacts_Body;
			customMessageBox.LeftButtonContent = StringLibrary.Settings_Remove_Contacts_Confirm;
			customMessageBox.RightButtonContent = StringLibrary.MessageBox_Popup_Cancel;
			customMessageBox.IsFullScreen = false;
			CustomMessageBox customMessageBox2 = customMessageBox;
			customMessageBox2.Dismissed += (object sender, DismissedEventArgs args) =>
			{
				if (args.Result == CustomMessageBoxResult.LeftButton)
				{
					acceptAction();
				}
				else
				{
					cancelAction();
				}
			};
			customMessageBox2.Show();
		}
	}
}

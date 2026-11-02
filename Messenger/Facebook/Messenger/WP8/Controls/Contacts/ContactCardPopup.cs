using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization.Resources;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls.Contacts
{
	public partial class ContactCardPopup : FacebookPopup
	{







		public event EventHandler<FacebookUser> SendMessageClicked;

		public event EventHandler<FacebookUser> CreateGroupClicked;

		public event EventHandler<FacebookUser> RemoveContactClicked;

		public ContactCardPopup()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
		}

		protected override void ViewModelUpdated(object oldViewModel)
		{
			base.ViewModelUpdated(oldViewModel);
			FacebookUser facebookUser = base.DataContext as FacebookUser;
			if (facebookUser != null)
			{
				CreateGroupTextBlock.Text = string.Format(StringLibrary.ContactCard_MenuLabel_CreateGroup, facebookUser.FirstName);
				SendMessageTextBlock.Text = string.Format(StringLibrary.ContactCard_MenuLabel_SendMessage, facebookUser.FirstName);
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)ContactPopupControl).Width = MaxContentPanelWidth;
			((FrameworkElement)ContactPopupControl).Height = MaxContentPanelHeight;
		}

		private void SendMessage_OnClick(object sender, RoutedEventArgs e)
		{
			if (SendMessageClicked != null)
			{
				SendMessageClicked(this, base.DataContext as FacebookUser);
			}
		}

		private void CreateGroup_OnClick(object sender, RoutedEventArgs e)
		{
			if (CreateGroupClicked != null)
			{
				CreateGroupClicked(this, base.DataContext as FacebookUser);
			}
		}

		private void RemoveContact_OnClick(object sender, RoutedEventArgs e)
		{
			if (RemoveContactClicked != null)
			{
				RemoveContactClicked(this, base.DataContext as FacebookUser);
			}
		}

	}
}

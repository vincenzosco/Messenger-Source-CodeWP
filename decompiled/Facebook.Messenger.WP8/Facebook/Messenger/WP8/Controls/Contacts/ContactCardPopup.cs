using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization.Resources;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls.Contacts
{
	public class ContactCardPopup : FacebookPopup
	{
		internal Grid ContactPopupControl;

		internal Image Logo;

		internal TextBlock PhoneNumberTextBlock;

		internal TextBlock SendMessageTextBlock;

		internal TextBlock CreateGroupTextBlock;

		internal TextBlock RemoveContactTextBlock;

		private bool _contentLoaded;

		public event EventHandler<FacebookUser> SendMessageClicked;

		public event EventHandler<FacebookUser> CreateGroupClicked;

		public event EventHandler<FacebookUser> RemoveContactClicked;

		public ContactCardPopup()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			InitializeComponent();
			((FrameworkElement)this).Loaded += OnLoaded;
		}

		protected override void ViewModelUpdated(object oldViewModel)
		{
			base.ViewModelUpdated(oldViewModel);
			FacebookUser facebookUser = ((FrameworkElement)this).DataContext as FacebookUser;
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
				SendMessageClicked(this, ((FrameworkElement)this).DataContext as FacebookUser);
			}
		}

		private void CreateGroup_OnClick(object sender, RoutedEventArgs e)
		{
			if (CreateGroupClicked != null)
			{
				CreateGroupClicked(this, ((FrameworkElement)this).DataContext as FacebookUser);
			}
		}

		private void RemoveContact_OnClick(object sender, RoutedEventArgs e)
		{
			if (RemoveContactClicked != null)
			{
				RemoveContactClicked(this, ((FrameworkElement)this).DataContext as FacebookUser);
			}
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
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Contacts/ContactCardPopup.xaml", UriKind.Relative));
				ContactPopupControl = (Grid)((FrameworkElement)this).FindName("ContactPopupControl");
				Logo = (Image)((FrameworkElement)this).FindName("Logo");
				PhoneNumberTextBlock = (TextBlock)((FrameworkElement)this).FindName("PhoneNumberTextBlock");
				SendMessageTextBlock = (TextBlock)((FrameworkElement)this).FindName("SendMessageTextBlock");
				CreateGroupTextBlock = (TextBlock)((FrameworkElement)this).FindName("CreateGroupTextBlock");
				RemoveContactTextBlock = (TextBlock)((FrameworkElement)this).FindName("RemoveContactTextBlock");
			}
		}
	}
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class AdminMessageControl : UserControl
	{
		private const string AdminPhotoId = "1";






		public event EventHandler<NavigationEventArgs> NavigateEvent;

		public AdminMessageControl()
		{
			InitializeComponent();
		}

		private void AdminImageBlock_OnTap(object sender, GestureEventArgs e)
		{
			MessageViewModel messageViewModel = base.DataContext as MessageViewModel;
			if (messageViewModel != null)
			{
				FacebookUri attachmentsUri = FacebookUri.GetAttachmentsUri(messageViewModel.Id, "1", null, 0);
				if (NavigateEvent != null)
				{
					NavigateEvent(this, new NavigationEventArgs(attachmentsUri));
				}
			}
		}

	}
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ActiveUsersListControl : UserControl
	{



		public ActiveUsersListControl()
		{
			InitializeComponent();
		}

		private void OnItemTapped(object sender, GestureEventArgs e)
		{
			UserControl val = (UserControl)((sender is UserControl) ? sender : null);
			if (val == null)
			{
				return;
			}
			ActiveUsersViewModel activeUsersViewModel = base.DataContext as ActiveUsersViewModel;
			if (activeUsersViewModel != null)
			{
				FacebookUser facebookUser = ((FrameworkElement)val).DataContext as FacebookUser;
				if (facebookUser != null)
				{
					activeUsersViewModel.LaunchUserThread(facebookUser);
				}
			}
		}

	}
}

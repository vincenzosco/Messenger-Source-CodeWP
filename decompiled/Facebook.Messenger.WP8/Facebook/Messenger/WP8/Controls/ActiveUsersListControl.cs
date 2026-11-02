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
	public class ActiveUsersListControl : UserControl
	{
		internal Grid LayoutRoot;

		internal FacebookListView ActiveUsersList;

		private bool _contentLoaded;

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
			ActiveUsersViewModel activeUsersViewModel = ((FrameworkElement)this).DataContext as ActiveUsersViewModel;
			if (activeUsersViewModel != null)
			{
				FacebookUser facebookUser = ((FrameworkElement)val).DataContext as FacebookUser;
				if (facebookUser != null)
				{
					activeUsersViewModel.LaunchUserThread(facebookUser);
				}
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
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ActiveUsersListControl.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				ActiveUsersList = (FacebookListView)((FrameworkElement)this).FindName("ActiveUsersList");
			}
		}
	}
}

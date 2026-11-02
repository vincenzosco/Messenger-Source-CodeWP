using System;
using System.Collections.Generic;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public class UserSelectedEventArgs : EventArgs
	{
		public List<FacebookUser> SelectedFacebookUsers { get; private set; }

		public UserSelectedEventArgs(List<FacebookUser> selectedUsersList)
		{
			SelectedFacebookUsers = selectedUsersList;
		}
	}
}

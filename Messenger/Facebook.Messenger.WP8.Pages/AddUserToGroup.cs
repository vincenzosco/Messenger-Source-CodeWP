using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class AddUserToGroup : GroupContactsViewModelPage
	{
		private ApplicationBarIconButton _searchFriendsButton;

		private ApplicationBarIconButton _multiSelectButton;

		private ApplicationBarIconButton _checkButton;










		public AddUserToGroup()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected Obj, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBar();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
			ContactListUserControl contactList = ContactList;
			EventHandler<EventArgs> value = (object sender, EventArgs args) =>
			{
				base.Dispatcher.BeginInvoke((Action)UpdateAppBarPresentation);
			};
			contactList.UserSelectionChanged += value;
		}

		private void InitializeAppBar()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected Obj, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected Obj, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected Obj, but got Unknown
			ApplicationBarIconButton val = new ApplicationBarIconButton();
			val.IconUri = new Uri("/Assets/AppBar/list.png", UriKind.Relative);
			val.Text = StringLibrary.Group_AppBar_SelectMany;
			_multiSelectButton = val;
			_multiSelectButton.Click += MultiSelectOnClick;
			base.ApplicationBar.Buttons.Add(_multiSelectButton);
			ApplicationBarIconButton val2 = new ApplicationBarIconButton();
			val2.IconUri = new Uri("/Assets/AppBar/Search.png", UriKind.Relative);
			val2.Text = StringLibrary.People_AppBar_Search;
			_searchFriendsButton = val2;
			_searchFriendsButton.Click += SearchFriendsOnClick;
			base.ApplicationBar.Buttons.Add(_searchFriendsButton);
			ApplicationBarIconButton val3 = new ApplicationBarIconButton();
			val3.IconUri = new Uri("/Assets/AppBar/Check.png", UriKind.Relative);
			val3.Text = StringLibrary.AppBar_Ok;
			_checkButton = val3;
			_checkButton.Click += CheckOnClick;
		}

		private void MultiSelectOnClick(object sender, EventArgs e)
		{
			base.ApplicationBar.Buttons.Remove(_searchFriendsButton);
			base.ApplicationBar.Buttons.Remove(_multiSelectButton);
			base.ApplicationBar.Buttons.Add(_checkButton);
			ContactList.AllowMultipleSelection = true;
		}

		private void SearchFriendsOnClick(object sender, EventArgs e)
		{
			WithViewModel((GroupContactsViewModel vm) =>
			{
				vm.IsSearching = true;
				base.ApplicationBar.IsVisible = false;
				((Control)SearchTextBox).Focus();
				object obj = base.Resources[(object)"AnimateContactSearchIn"];
				Storyboard val = (Storyboard)((obj is Storyboard) ? obj : null);
				if (val != null)
				{
					val.Begin();
				}
			});
		}

		private void ResetPageState()
		{
			WithViewModel((GroupContactsViewModel vm) =>
			{
				base.ApplicationBar.IsVisible = true;
				SearchTextBox.Text = string.Empty;
				vm.IsSearching = false;
				object obj = base.Resources[(object)"AnimateContactSearchOut"];
				Storyboard val = (Storyboard)((obj is Storyboard) ? obj : null);
				if (val != null)
				{
					val.Begin();
				}
			});
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			WithViewModel((GroupContactsViewModel vm) =>
			{
				if (vm.IsSearching)
				{
					ResetPageState();
					e.Cancel = true;
				}
			});
			base.OnBackKeyPress(e);
		}

		private void CheckOnClick(object sender, EventArgs e)
		{
			WithViewModel((GroupContactsViewModel vm) =>
			{
				vm.AddGroupParticipantsCommand.Execute(null);
			});
		}

		private void OnItemTapped(object sender, GestureEventArgs e)
		{
			ContactUserControl contactUserControl = sender as ContactUserControl;
			if (contactUserControl == null)
			{
				return;
			}
			FacebookUser user = ((FrameworkElement)contactUserControl).DataContext as FacebookUser;
			if (user != null)
			{
				WithViewModel((GroupContactsViewModel vm) =>
				{
					List<FacebookUser> selectedUsersList = new List<FacebookUser> { user };
					UserSelectedEventArgs parameter = new UserSelectedEventArgs(selectedUsersList);
					vm.UserSelectedCommand.Execute(parameter);
				});
			}
		}

		private void ContactSearchFilterChanged(object sender, TextChangedEventArgs e)
		{
			((FrameworkElement)SearchTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
		}

		private void SearchTextBox_OnGotFocus(object sender, RoutedEventArgs e)
		{
			double remainingSpace = InputHelper.GetRemainingSpace(((FrameworkElement)ContactSearch).ActualHeight);
			((FrameworkElement)ContactsSearchResults).MaxHeight = remainingSpace;
		}

		private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null && val.Text == string.Empty)
			{
				ResetPageState();
			}
			WithViewModel((GroupContactsViewModel vm) =>
			{
				ObservableCollection<FacebookUser> allUsers = vm.SearchResults.AllUsers;
				if (allUsers.Count > 3 && allUsers.Count < 8)
				{
					ContactsSearchResults.ScrollToTop();
				}
				((FrameworkElement)ContactsSearchResults).MaxHeight = Application.Current.Host.Content.ActualHeight;
			});
		}

		private void OnOverlayGridTapped(object sender, GestureEventArgs e)
		{
			ResetPageState();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((GroupContactsViewModel vm) =>
			{
				vm.SearchResults.FirstElementLoaded += SearchResultsOnFirstElementLoaded;
			});
			UpdateAppBarPresentation();
			((UIElement)ContactList.ContactSyncPromoGrid).Visibility = Visibility.Collapsed;
		}

		private void UpdateAppBarPresentation()
		{
			WithViewModel((GroupContactsViewModel vm) =>
			{
				_checkButton.IsEnabled = vm.AnyUserSelected;
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((GroupContactsViewModel vm) =>
			{
				vm.SearchResults.FirstElementLoaded -= SearchResultsOnFirstElementLoaded;
			});
		}

		private void SearchResultsOnFirstElementLoaded(object sender, EventArgs eventArgs)
		{
			ContactsSearchResults.ScrollToTop();
		}

		private void ContactList_OnContactSourceChanged(object sender, IList list)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)NoFriendsTextBlock).Visibility = VisibilityUtil.FromBool(list.Count == 0);
		}

	}
}

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

namespace Facebook.Messenger.WP8.Pages;

public class AddUserToGroup : GroupContactsViewModelPage
{
	private ApplicationBarIconButton _searchFriendsButton;

	private ApplicationBarIconButton _multiSelectButton;

	private ApplicationBarIconButton _checkButton;

	internal Grid LayoutRoot;

	internal TextBlock NoFriendsTextBlock;

	internal ContactListUserControl ContactList;

	internal Grid SearchGrid;

	internal Border ContactSearch;

	internal TextBox SearchTextBox;

	internal Grid SearchOverlayGrid;

	internal FacebookListView ContactsSearchResults;

	private bool _contentLoaded;

	public AddUserToGroup()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected Obj, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected Obj, but got Unknown
		InitializeComponent();
		InitializeAppBar();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
		ContactListUserControl contactList = ContactList;
		EventHandler<EventArgs> value = (object sender, EventArgs args) =>
		{
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)UpdateAppBarPresentation);
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
		((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_multiSelectButton);
		ApplicationBarIconButton val2 = new ApplicationBarIconButton();
		val2.IconUri = new Uri("/Assets/AppBar/Search.png", UriKind.Relative);
		val2.Text = StringLibrary.People_AppBar_Search;
		_searchFriendsButton = val2;
		_searchFriendsButton.Click += SearchFriendsOnClick;
		((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_searchFriendsButton);
		ApplicationBarIconButton val3 = new ApplicationBarIconButton();
		val3.IconUri = new Uri("/Assets/AppBar/Check.png", UriKind.Relative);
		val3.Text = StringLibrary.AppBar_Ok;
		_checkButton = val3;
		_checkButton.Click += CheckOnClick;
	}

	private void MultiSelectOnClick(object sender, EventArgs e)
	{
		((PhoneApplicationPage)this).ApplicationBar.Buttons.Remove(_searchFriendsButton);
		((PhoneApplicationPage)this).ApplicationBar.Buttons.Remove(_multiSelectButton);
		((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_checkButton);
		ContactList.AllowMultipleSelection = true;
	}

	private void SearchFriendsOnClick(object sender, EventArgs e)
	{
		WithViewModel((GroupContactsViewModel vm) =>
		{
			vm.IsSearching = true;
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
			((Control)SearchTextBox).Focus();
			object obj = ((FrameworkElement)this).Resources[(object)"AnimateContactSearchIn"];
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
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
			SearchTextBox.Text = string.Empty;
			vm.IsSearching = false;
			object obj = ((FrameworkElement)this).Resources[(object)"AnimateContactSearchOut"];
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
		if (!(sender is ContactUserControl contactUserControl))
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
		((UIElement)ContactList.ContactSyncPromoGrid).Visibility = (Visibility)1;
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

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected Obj, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/AddUserToGroup.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			NoFriendsTextBlock = (TextBlock)((FrameworkElement)this).FindName("NoFriendsTextBlock");
			ContactList = (ContactListUserControl)((FrameworkElement)this).FindName("ContactList");
			SearchGrid = (Grid)((FrameworkElement)this).FindName("SearchGrid");
			ContactSearch = (Border)((FrameworkElement)this).FindName("ContactSearch");
			SearchTextBox = (TextBox)((FrameworkElement)this).FindName("SearchTextBox");
			SearchOverlayGrid = (Grid)((FrameworkElement)this).FindName("SearchOverlayGrid");
			ContactsSearchResults = (FacebookListView)((FrameworkElement)this).FindName("ContactsSearchResults");
		}
	}
}

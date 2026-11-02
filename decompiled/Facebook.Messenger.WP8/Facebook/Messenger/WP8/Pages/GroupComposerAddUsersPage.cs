using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Common;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Windows.Foundation;

namespace Facebook.Messenger.WP8.Pages;

public class GroupComposerAddUsersPage : GroupComposerAddUsersViewModelPage
{
	private AppBarHelper _appBarHelper;

	private IconBarEntry<bool> _checkButton;

	internal Grid LayoutRoot;

	internal Border ContactSearch;

	internal TextBox SearchTextBox;

	internal Grid SearchGrid;

	internal FacebookListView SelectedUsers;

	internal Grid SearchOverlayGrid;

	internal FacebookListView ContactsSearchResults;

	internal FullscreenProgressIndicator FullscreenProgressIndicator;

	private bool _contentLoaded;

	public GroupComposerAddUsersPage()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected Obj, but got Unknown
		InitializeComponent();
		InitializeAppBarItems();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
		RegisterInputElements(new List<UIElement> { (UIElement)(object)SearchTextBox });
	}

	private void InitializeAppBarItems()
	{
		_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
		_checkButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(state: false, StringLibrary.AppBar_Ok, CheckButton_Clicked, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
		_checkButton.IsEnabled = false;
	}

	private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		((DependencyObject)this).Dispatcher.BeginInvoke((Action)(async () =>
		{
			await Task.Delay(500);
			((Control)SearchTextBox).Focus();
		}));
		WithViewModel((GroupComposerAddUsersViewModel vm) =>
		{
			vm.PropertyChanged += OnPropertyChanged;
		});
	}

	private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
	{
		WithViewModel((GroupComposerAddUsersViewModel vm) =>
		{
			vm.PropertyChanged -= OnPropertyChanged;
		});
	}

	protected override void OnErrorOccurred(object sender, Error error)
	{
		base.OnErrorOccurred(sender, error);
		((UIElement)FullscreenProgressIndicator).Visibility = (Visibility)1;
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		WithViewModel((GroupComposerAddUsersViewModel vm) =>
		{
			if (e.PropertyName == "IsCreatingGroup")
			{
				if (vm.IsCreatingGroup)
				{
					((Control)this).Focus();
					((UIElement)FullscreenProgressIndicator).Visibility = (Visibility)0;
					((Control)SearchTextBox).IsEnabled = false;
				}
				else
				{
					((Control)SearchTextBox).IsEnabled = true;
				}
			}
		});
	}

	private void CheckButton_Clicked(object sender, EventArgs e)
	{
		SearchTextBox.Text = string.Empty;
		WithViewModel((GroupComposerAddUsersViewModel vm) =>
		{
			vm.CreateGroupCommand.Execute(null);
		});
	}

	private void ContactSearchFilterChanged(object sender, TextChangedEventArgs e)
	{
		((FrameworkElement)SearchTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
	}

	private void ResetPageState()
	{
		SearchTextBox.Text = string.Empty;
	}

	private void OnItemTapped(object sender, GestureEventArgs e)
	{
		if (!(sender is ContactUserControl contactUserControl))
		{
			return;
		}
		FacebookUser user = ((FrameworkElement)contactUserControl).DataContext as FacebookUser;
		if (user == null)
		{
			return;
		}
		WithViewModel((GroupComposerAddUsersViewModel vm) =>
		{
			List<FacebookUser> selectedUsersList = new List<FacebookUser> { user };
			UserSelectedEventArgs parameter = new UserSelectedEventArgs(selectedUsersList);
			vm.UserSelectedCommand.Execute(parameter);
			if (vm.SelectedUsers.Count > 1)
			{
				_checkButton.IsEnabled = true;
			}
			ResetPageState();
		});
		((Control)SearchTextBox).Focus();
	}

	private void DeselectedMember(object sender, GestureEventArgs e)
	{
		if (!(sender is ContactUserControl contactUserControl))
		{
			return;
		}
		FacebookUser user = ((FrameworkElement)contactUserControl).DataContext as FacebookUser;
		if (user == null)
		{
			return;
		}
		WithViewModel((GroupComposerAddUsersViewModel vm) =>
		{
			vm.UserDeselectedCommand.Execute(user);
			if (vm.SelectedUsers.Count < 2)
			{
				_checkButton.IsEnabled = false;
			}
		});
	}

	private void SearchTextBox_OnGotFocus(object sender, RoutedEventArgs e)
	{
		((UIElement)SearchOverlayGrid).Visibility = (Visibility)0;
	}

	private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
	{
		((UIElement)SearchOverlayGrid).Visibility = (Visibility)1;
	}

	private void UpdateSearchGridBottom(double bottom)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		((FrameworkElement)SearchGrid).Margin = new Thickness(20.0, 10.0, 20.0, bottom);
	}

	private void OnOverlayGridTapped(object sender, GestureEventArgs e)
	{
		((UIElement)SearchOverlayGrid).Visibility = (Visibility)1;
		((Control)SelectedUsers).Focus();
	}

	protected override void OnKeyboardVisibilityChanged(bool showing, Rect occludedRect)
	{
		double bottom = (showing ? occludedRect.Height : 0.0);
		UpdateSearchGridBottom(bottom);
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
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/GroupComposerAddUsersPage.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			ContactSearch = (Border)((FrameworkElement)this).FindName("ContactSearch");
			SearchTextBox = (TextBox)((FrameworkElement)this).FindName("SearchTextBox");
			SearchGrid = (Grid)((FrameworkElement)this).FindName("SearchGrid");
			SelectedUsers = (FacebookListView)((FrameworkElement)this).FindName("SelectedUsers");
			SearchOverlayGrid = (Grid)((FrameworkElement)this).FindName("SearchOverlayGrid");
			ContactsSearchResults = (FacebookListView)((FrameworkElement)this).FindName("ContactsSearchResults");
			FullscreenProgressIndicator = (FullscreenProgressIndicator)((FrameworkElement)this).FindName("FullscreenProgressIndicator");
		}
	}
}

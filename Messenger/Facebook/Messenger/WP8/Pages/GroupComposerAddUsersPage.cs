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
using Rect = System.Windows.Rect;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class GroupComposerAddUsersPage : GroupComposerAddUsersViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _checkButton;










		public GroupComposerAddUsersPage()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBarItems();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
			RegisterInputElements(new List<UIElement> { (UIElement)(object)SearchTextBox });
		}

		private void InitializeAppBarItems()
		{
			_appBarHelper = new AppBarHelper(base.ApplicationBar);
			_checkButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.AppBar_Ok, CheckButton_Clicked, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
			_checkButton.IsEnabled = false;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			base.Dispatcher.BeginInvoke((Action)(async () =>
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
			((UIElement)FullscreenProgressIndicator).Visibility = Visibility.Collapsed;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			WithViewModel((GroupComposerAddUsersViewModel vm) =>
			{
				if (e.PropertyName == "IsCreatingGroup")
				{
					if (vm.IsCreatingGroup)
					{
						base.Focus();
						((UIElement)FullscreenProgressIndicator).Visibility = Visibility.Visible;
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
			ContactUserControl contactUserControl = sender as ContactUserControl;
			if (contactUserControl == null)
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
			ContactUserControl contactUserControl = sender as ContactUserControl;
			if (contactUserControl == null)
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
			((UIElement)SearchOverlayGrid).Visibility = Visibility.Visible;
		}

		private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			((UIElement)SearchOverlayGrid).Visibility = Visibility.Collapsed;
		}

		private void UpdateSearchGridBottom(double bottom)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			((FrameworkElement)SearchGrid).Margin = new Thickness(20.0, 10.0, 20.0, bottom);
		}

		private void OnOverlayGridTapped(object sender, GestureEventArgs e)
		{
			((UIElement)SearchOverlayGrid).Visibility = Visibility.Collapsed;
			((Control)SelectedUsers).Focus();
		}

		protected override void OnKeyboardVisibilityChanged(bool showing, Rect occludedRect)
		{
			double bottom = (showing ? occludedRect.Height : 0.0);
			UpdateSearchGridBottom(bottom);
		}

	}
}

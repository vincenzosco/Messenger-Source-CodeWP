using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class NameConversationPage : NameConversationViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private MenuBarEntry<bool> _clearNameMenuItem;

		private IconBarEntry<bool> _okButton;





		public NameConversationPage()
		{
			InitializeComponent();
			InitializeAppBarItems();
		}

		private void NameConversationPage_OnLoaded(object sender, RoutedEventArgs e)
		{
			((Control)ConversationNameTextBox).Focus();
			ConversationNameTextBox.SelectionStart = ConversationNameTextBox.Text.Length;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ConversationNameTextBox.MaxLength = 100;
			WithViewModel((NameConversationViewModel vm) =>
			{
				_clearNameMenuItem.IsVisible = !string.IsNullOrEmpty(vm.ThreadName);
			});
		}

		private void InitializeAppBarItems()
		{
			_appBarHelper = new AppBarHelper(base.ApplicationBar);
			_okButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.AppBar_Ok, OkButton_Clicked, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
			_okButton.IsEnabled = false;
			_appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(true, StringLibrary.AppBar_Cancel, CancelButton_Clicked, new Uri("/Assets/AppBar/Skip.png", UriKind.Relative)));
			_clearNameMenuItem = _appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(true, StringLibrary.NameConversation_Menu_Clear, ClearButton_Clicked));
		}

		private void SubmitName()
		{
			WithViewModel((NameConversationViewModel vm) =>
			{
				if (!ConversationNameTextBox.Text.Equals(vm.OriginalThreadName ?? string.Empty))
				{
					((FrameworkElement)ConversationNameTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
					vm.SubmitNameCommand.Execute(null);
				}
			});
		}

		private void OkButton_Clicked(object sender, EventArgs e)
		{
			SubmitName();
		}

		private void CancelButton_Clicked(object sender, EventArgs e)
		{
			base.NavigationService.GoBack();
		}

		private void ClearButton_Clicked(object sender, EventArgs e)
		{
			ConversationNameTextBox.Text = string.Empty;
			SubmitName();
		}

		private void ConversationNameTextBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)e.Key == 3 && _okButton.IsEnabled)
			{
				SubmitName();
			}
		}

		private void ConversationNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			WithViewModel((NameConversationViewModel vm) =>
			{
				bool flag = !string.Equals(ConversationNameTextBox.Text.Trim(), vm.ThreadName);
				bool flag2 = string.IsNullOrWhiteSpace(ConversationNameTextBox.Text);
				_okButton.IsEnabled = !flag2 && flag;
			});
		}

		private void OnBackgroundClick(object sender, RoutedEventArgs e)
		{
			((Control)ConversationNameTextBox).Focus();
		}

	}
}

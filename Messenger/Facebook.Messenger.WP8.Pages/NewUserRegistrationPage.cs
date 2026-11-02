using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class NewUserRegistrationPage : NewUserRegistrationViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _nextButton;






		public NewUserRegistrationPage()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBar();
			base.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			UpdateAppBar();
		}

		private void InitializeAppBar()
		{
			_appBarHelper = new AppBarHelper(base.ApplicationBar);
			_nextButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.Attachments_AppBar_Next, NextButtonOnClick, new Uri("/Assets/AppBar/Next.png", UriKind.Relative)));
			_nextButton.IsEnabled = false;
		}

		private void NextButtonOnClick(object sender, EventArgs e)
		{
			((FrameworkElement)FirstNameTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
			((FrameworkElement)LastNameTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
			CreateAccount();
		}

		private void CreateAccount()
		{
			WithViewModel((NewUserRegistrationViewModel vm) =>
			{
				vm.CreateAccountCommand.Execute(null);
			});
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateAppBar();
		}

		private void UpdateAppBar()
		{
			_nextButton.IsEnabled = !string.IsNullOrEmpty(FirstNameTextBox.Text) && !string.IsNullOrEmpty(LastNameTextBox.Text);
		}

		private void TextBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)e.Key == 3)
			{
				if (sender == FirstNameTextBox)
				{
					((Control)LastNameTextBox).Focus();
				}
				else if (sender == LastNameTextBox)
				{
					base.Focus();
					CreateAccount();
				}
			}
		}

	}
}

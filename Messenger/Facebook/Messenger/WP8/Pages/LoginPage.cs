using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.System;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class LoginPage : LoginViewModelPage
	{
		private bool _loginFormFocussed;

		private readonly Uri _nextUriIcon = new Uri("/Assets/AppBar/Next.png", UriKind.Relative);

		private ApplicationBarIconButton _nextButton;













		public LoginPage()
		{
			InitializeComponent();
			InitializeAppBar();
		}

		private void InitializeAppBar()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			_nextButton = new ApplicationBarIconButton(_nextUriIcon);
			_nextButton.Text = StringLibrary.Attachments_AppBar_Next;
			_nextButton.IsEnabled = false;
			_nextButton.Click += NextButton_OnClick;
			base.ApplicationBar.Buttons.Add(_nextButton);
		}

		private void NextButton_OnClick(object sender, EventArgs e)
		{
			AttemptLogin();
		}

		private void AttemptLogin()
		{
			if (!string.IsNullOrWhiteSpace(PasswordTextBox.Password) && !string.IsNullOrWhiteSpace(UsernameTextBox.Text))
			{
				BindingExpression bindingExpression = ((FrameworkElement)PasswordTextBox).GetBindingExpression(PasswordBox.PasswordProperty);
				if (bindingExpression != null)
				{
					bindingExpression.UpdateSource();
				}
				bindingExpression = ((FrameworkElement)UsernameTextBox).GetBindingExpression(TextBox.TextProperty);
				if (bindingExpression != null)
				{
					bindingExpression.UpdateSource();
				}
				_nextButton.IsEnabled = false;
				StateProgressBar.SetState(StringLibrary.Login_SigningIn);
				WithViewModel((LoginViewModel vm) =>
				{
					vm.LoginCommand.Execute(null);
				});
			}
		}

		private void FadeInCompleted(object sender, EventArgs e)
		{
			_loginFormFocussed = false;
		}

		private void FadeOutCompleted(object sender, EventArgs e)
		{
			_loginFormFocussed = true;
		}

		protected override async void OnErrorOccurred(object sender, Error error)
		{
			if (error == null)
			{
				return;
			}
			if (error.Code == 405)
			{
				MessageBoxResult response = MessageBox.Show(StringLibrary.MessageBox_Label_AuthenticateWithBrowser, string.Empty, (MessageBoxButton)0);
				if ((int)response == 1)
				{
					await Launcher.LaunchUriAsync(new Uri("http://www.facebook.com"));
				}
			}
			else
			{
				MessageBox.Show(error.Message, error.Title, (MessageBoxButton)0);
			}
			StateProgressBar.Reset();
		}

		private async void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (FadeOut != null)
			{
				((Timeline)FadeOut).Completed += FadeOutCompleted;
			}
			if (FadeIn != null)
			{
				((Timeline)FadeIn).Completed += FadeInCompleted;
			}
			await WithViewModel(async (LoginViewModel vm) =>
			{
				if (vm.SessionExpired)
				{
					vm.SessionExpired = false;
					await Task.Delay(1000);
					MessageBox.Show(StringLibrary.Session_Expired_Caption, StringLibrary.Session_Expired_Title, (MessageBoxButton)0);
				}
			});
		}

		private void OnUnLoaded(object sender, RoutedEventArgs e)
		{
			if (FadeOut != null)
			{
				((Timeline)FadeOut).Completed -= FadeOutCompleted;
			}
			if (FadeIn != null)
			{
				((Timeline)FadeIn).Completed -= FadeInCompleted;
			}
		}

		private void TextBoxKeyUp(object sender, KeyEventArgs keyEventArgs)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Invalid comparison between Unknown and I4
			_nextButton.IsEnabled = !string.IsNullOrWhiteSpace(PasswordTextBox.Password) && !string.IsNullOrWhiteSpace(UsernameTextBox.Text);
			if ((int)keyEventArgs.Key == 3)
			{
				if (sender == UsernameTextBox)
				{
					((Control)PasswordTextBox).Focus();
				}
				else if (sender == PasswordTextBox)
				{
					AttemptLogin();
				}
			}
		}

		private void LoginForm_OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (!VisualTreeHelperExtensions.IsChildElementOf(FocusManager.GetFocusedElement(), LoginFormContainer))
			{
				((UIElement)SignInTitle).Visibility = Visibility.Collapsed;
				((UIElement)WelcomeTitle).Visibility = Visibility.Visible;
				FadeIn.Begin();
				_loginFormFocussed = false;
			}
		}

		private void LoginForm_OnGotFocus(object sender, RoutedEventArgs e)
		{
			if (!_loginFormFocussed)
			{
				((UIElement)SignInTitle).Visibility = Visibility.Visible;
				((UIElement)WelcomeTitle).Visibility = Visibility.Collapsed;
				FadeOut.Begin();
				_loginFormFocussed = true;
			}
		}

	}
}

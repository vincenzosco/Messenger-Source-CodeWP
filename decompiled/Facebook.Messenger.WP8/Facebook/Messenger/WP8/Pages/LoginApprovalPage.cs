using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.System;

namespace Facebook.Messenger.WP8.Pages;

public class LoginApprovalPage : LoginViewModelPage
{
	private readonly Uri _okUriIcon = new Uri("/Assets/AppBar/Check.png", UriKind.Relative);

	private bool _formFocussed;

	private ApplicationBarIconButton _okButton;

	internal Storyboard FadeIn;

	internal Storyboard FadeOut;

	internal Grid LayoutRoot;

	internal Border TitleTopMargin;

	internal Image Logo;

	internal TextBlock SignInTitle;

	internal Grid LoginFormContainer;

	internal TextBlock ApprovalHintTextBlock;

	internal TextBox ApprovalTextBox;

	private bool _contentLoaded;

	public LoginApprovalPage()
	{
		InitializeComponent();
		InitializeAppBar();
	}

	private void InitializeAppBar()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected Obj, but got Unknown
		_okButton = new ApplicationBarIconButton(_okUriIcon);
		_okButton.Text = StringLibrary.AppBar_Ok;
		_okButton.Click += OkButtonOnClick;
		_okButton.IsEnabled = ApprovalTextBox.Text.Any();
		((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_okButton);
	}

	protected override async void OnErrorOccurred(object sender, Error error)
	{
		ApprovalTextBox.Text = string.Empty;
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
			MessageBox.Show(error.Message, string.Empty, (MessageBoxButton)0);
		}
	}

	private void OkButtonOnClick(object sender, EventArgs e)
	{
		((FrameworkElement)ApprovalTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
		WithViewModel((LoginViewModel vm) =>
		{
			vm.LoginCommand.Execute(null);
		});
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (FadeOut != null)
		{
			((Timeline)FadeOut).Completed += FadeOutCompleted;
		}
		if (FadeIn != null)
		{
			((Timeline)FadeIn).Completed += FadeInCompleted;
		}
		WithViewModel((LoginViewModel vm) =>
		{
			vm.PropertyChanged += OnPropertyChanged;
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
		WithViewModel((LoginViewModel vm) =>
		{
			vm.PropertyChanged -= OnPropertyChanged;
		});
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName.Equals("IsLoading"))
		{
			WithViewModel((LoginViewModel vm) =>
			{
				_okButton.IsEnabled = !vm.IsLoading && ApprovalTextBox.Text.Any();
			});
		}
	}

	private void FadeInCompleted(object sender, EventArgs e)
	{
		_formFocussed = false;
	}

	private void FadeOutCompleted(object sender, EventArgs e)
	{
		_formFocussed = true;
	}

	private void ApprovalHintTextBlock_OnTap(object sender, GestureEventArgs e)
	{
		((Control)ApprovalTextBox).Focus();
	}

	private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)e.Key == 3)
		{
			((FrameworkElement)ApprovalTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
			WithViewModel((LoginViewModel vm) =>
			{
				vm.LoginCommand.Execute(null);
			});
		}
	}

	private void ApprovalTextBox_OnGotFocus(object sender, RoutedEventArgs e)
	{
		if (!_formFocussed)
		{
			FadeOut.Begin();
			_formFocussed = true;
		}
	}

	private void ApprovalTextBox_OnLostFocus(object sender, RoutedEventArgs e)
	{
		FadeIn.Begin();
	}

	private void ApprovalTextboxOnTextChanged(object sender, TextChangedEventArgs e)
	{
		_okButton.IsEnabled = ApprovalTextBox.Text.Any();
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected Obj, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/LoginApprovalPage.xaml", UriKind.Relative));
			FadeIn = (Storyboard)((FrameworkElement)this).FindName("FadeIn");
			FadeOut = (Storyboard)((FrameworkElement)this).FindName("FadeOut");
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			TitleTopMargin = (Border)((FrameworkElement)this).FindName("TitleTopMargin");
			Logo = (Image)((FrameworkElement)this).FindName("Logo");
			SignInTitle = (TextBlock)((FrameworkElement)this).FindName("SignInTitle");
			LoginFormContainer = (Grid)((FrameworkElement)this).FindName("LoginFormContainer");
			ApprovalHintTextBlock = (TextBlock)((FrameworkElement)this).FindName("ApprovalHintTextBlock");
			ApprovalTextBox = (TextBox)((FrameworkElement)this).FindName("ApprovalTextBox");
		}
	}
}

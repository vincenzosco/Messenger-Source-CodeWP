using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Foundation;

namespace Facebook.Messenger.WP8.Pages;

public class VerifyPhoneNumberPage : VerifyPhoneNumberViewModelPage
{
	private readonly Uri _okUriIcon = new Uri("/Assets/AppBar/Check.png", UriKind.Relative);

	private readonly Uri _skipUriIcon = new Uri("/Assets/AppBar/Skip.png", UriKind.Relative);

	private ApplicationBarIconButton _okButton;

	private ApplicationBarIconButton _skipButton;

	private bool _registrationFlow;

	internal Grid LayoutRoot;

	internal Border TitleTopMargin;

	internal Image Logo;

	internal TextBlock SignInTitle;

	internal Grid ConfirmCodeFormContainer;

	internal TextBox ConfirmCodeTextBox;

	private bool _contentLoaded;

	public VerifyPhoneNumberPage()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		RegisterInputElements(new List<UIElement> { (UIElement)(object)ConfirmCodeTextBox });
	}

	private void InitializeAppBar()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected Obj, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected Obj, but got Unknown
		_okButton = new ApplicationBarIconButton(_okUriIcon);
		_okButton.Text = StringLibrary.AppBar_Ok;
		_okButton.Click += OkButtonOnClick;
		UpdateOkButton();
		_skipButton = new ApplicationBarIconButton(_skipUriIcon);
		_skipButton.Text = StringLibrary.Attachments_AppBar_Skip;
		_skipButton.Click += SkipButtonOnClick;
		((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_okButton);
		if (!_registrationFlow)
		{
			((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_skipButton);
		}
	}

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		WithViewModel((VerifyPhoneNumberViewModel vm) =>
		{
			SetSystemTrayVisibility(vm);
		});
		base.OnNavigatedTo(e);
	}

	private void SetSystemTrayVisibility(VerifyPhoneNumberViewModel vm)
	{
		_registrationFlow = vm.IsRegistrationFlow;
		SystemTrayManager.EnsurePagePreferencesStored((Page)(object)this, !vm.IsLoginFlow && !vm.IsRegistrationFlow);
	}

	private void SkipButtonOnClick(object sender, EventArgs e)
	{
		WithViewModel((VerifyPhoneNumberViewModel vm) =>
		{
			vm.SkipPhoneNumberCommand.Execute(null);
		});
	}

	private void OkButtonOnClick(object sender, EventArgs e)
	{
		((FrameworkElement)ConfirmCodeTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
		WithViewModel((VerifyPhoneNumberViewModel vm) =>
		{
			vm.ConfirmPhoneNumberCommand.Execute(null);
		});
	}

	private void ConfirmCodeTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)e.Key == 3)
		{
			((FrameworkElement)ConfirmCodeTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
			WithViewModel((VerifyPhoneNumberViewModel vm) =>
			{
				vm.ConfirmPhoneNumberCommand.Execute(null);
			});
		}
	}

	private void ConfirmCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		UpdateOkButton();
	}

	private void UpdateOkButton()
	{
		_okButton.IsEnabled = !string.IsNullOrWhiteSpace(ConfirmCodeTextBox.Text);
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		WithViewModel((VerifyPhoneNumberViewModel vm) =>
		{
			SetSystemTrayVisibility(vm);
			InitializeAppBar();
			vm.PropertyChanged += OnPropertyChanged;
		});
	}

	private void OnUnLoaded(object sender, RoutedEventArgs e)
	{
		WithViewModel((VerifyPhoneNumberViewModel vm) =>
		{
			vm.PropertyChanged -= OnPropertyChanged;
		});
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (!e.PropertyName.Equals("IsLoading"))
		{
			return;
		}
		WithViewModel((VerifyPhoneNumberViewModel vm) =>
		{
			if (vm.IsLoading)
			{
				_okButton.IsEnabled = false;
				if (!_registrationFlow)
				{
					_skipButton.IsEnabled = false;
				}
			}
			else
			{
				UpdateOkButton();
				if (!_registrationFlow)
				{
					_skipButton.IsEnabled = true;
				}
			}
		});
	}

	private void ConfirmCodeTextBox_OnGotFocus(object sender, RoutedEventArgs e)
	{
		SystemTrayManager.AddOperationToStack(((object)this).GetType());
	}

	private void ConfirmCodeTextBox_OnLostFocus(object sender, RoutedEventArgs e)
	{
		if (!VisualTreeHelperExtensions.IsChildElementOf(FocusManager.GetFocusedElement(), ConfirmCodeFormContainer))
		{
			SystemTrayManager.RemoveOperationFromStack(((object)this).GetType());
		}
	}

	protected override void OnKeyboardVisibilityChanged(bool showing, Rect occludedRect)
	{
		int num = GetKeyboardTransform((int)occludedRect.Height) + 72;
		int rootFrameTransform = (showing ? (-1 * num) : 0);
		SetRootFrameTransform(rootFrameTransform);
	}

	private void PhoneNumberOnTap(object sender, GestureEventArgs e)
	{
		WithViewModel((VerifyPhoneNumberViewModel vm) =>
		{
			vm.NavigateBackCommand.Execute(null);
		});
	}

	private int GetKeyboardTransform(int occludedHeight)
	{
		return InputHelper.GetOccludedOffset((FrameworkElement)(object)ConfirmCodeTextBox, (FrameworkElement)(object)LayoutRoot, occludedHeight);
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/VerifyPhoneNumberPage.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			TitleTopMargin = (Border)((FrameworkElement)this).FindName("TitleTopMargin");
			Logo = (Image)((FrameworkElement)this).FindName("Logo");
			SignInTitle = (TextBlock)((FrameworkElement)this).FindName("SignInTitle");
			ConfirmCodeFormContainer = (Grid)((FrameworkElement)this).FindName("ConfirmCodeFormContainer");
			ConfirmCodeTextBox = (TextBox)((FrameworkElement)this).FindName("ConfirmCodeTextBox");
		}
	}
}

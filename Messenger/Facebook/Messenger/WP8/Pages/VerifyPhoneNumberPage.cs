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
using Rect = System.Windows.Rect;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class VerifyPhoneNumberPage : VerifyPhoneNumberViewModelPage
	{
		private readonly Uri _okUriIcon = new Uri("/Assets/AppBar/Check.png", UriKind.Relative);

		private readonly Uri _skipUriIcon = new Uri("/Assets/AppBar/Skip.png", UriKind.Relative);

		private ApplicationBarIconButton _okButton;

		private ApplicationBarIconButton _skipButton;

		private bool _registrationFlow;








		public VerifyPhoneNumberPage()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
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
			base.ApplicationBar.Buttons.Add(_okButton);
			if (!_registrationFlow)
			{
				base.ApplicationBar.Buttons.Add(_skipButton);
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

	}
}

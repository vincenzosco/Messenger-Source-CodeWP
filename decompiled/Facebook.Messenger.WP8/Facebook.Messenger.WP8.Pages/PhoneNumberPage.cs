using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Common;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Foundation;

namespace Facebook.Messenger.WP8.Pages
{
	public class PhoneNumberPage : PhoneNumberViewModelPage
	{
		private readonly Uri _okUriIcon = new Uri("/Assets/AppBar/Check.png", UriKind.Relative);

		private readonly Uri _skipUriIcon = new Uri("/Assets/AppBar/Skip.png", UriKind.Relative);

		private ApplicationBarIconButton _okButton;

		private ApplicationBarIconButton _skipButton;

		private bool _prefixValidated;

		internal DataTemplate CountryCodeDataTemplate;

		internal DataTemplate GroupedCountryCodeHeaderTemplate;

		internal Grid LayoutRoot;

		internal Popup CountryCodeListPopup;

		internal Grid CountryCodeListGrid;

		internal FacebookListView CountryCodeList;

		internal FBMessengerProgressBar MessengerProgressBar;

		internal Border TitleTopMargin;

		internal Image Logo;

		internal TextBlock SignInTitle;

		internal Grid PhoneFormContainer;

		internal TextBox CountryCodeListPicker;

		internal Grid PhoneNumberGrid;

		internal TextBox PhonePrefixTextBox;

		internal TextBox PhoneNumberTextBox;

		internal FullscreenProgressIndicatorWithButton FullscreenProgressIndicator;

		private bool _contentLoaded;

		public PhoneNumberPage()
		{
			InitializeComponent();
			InitializeAppBar();
			RegisterInputElements(new List<UIElement> { (UIElement)(object)PhoneNumberGrid });
		}

		private void InitializeAppBar()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected Obj, but got Unknown
			_okButton = new ApplicationBarIconButton(_okUriIcon);
			_okButton.Text = StringLibrary.AppBar_Ok;
			_okButton.Click += OkButtonOnClick;
			UpdateOkButton();
			((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_okButton);
			_skipButton = new ApplicationBarIconButton(_skipUriIcon);
			_skipButton.Text = StringLibrary.Attachments_AppBar_Skip;
			_skipButton.Click += SkipButtonOnClick;
			((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_skipButton);
		}

		private void SkipButtonOnClick(object sender, EventArgs e)
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				vm.SkipPhoneNumberCommand.Execute(null);
			});
		}

		private void OkButtonOnClick(object sender, EventArgs e)
		{
			_prefixValidated = FocusManager.GetFocusedElement() == PhonePrefixTextBox;
			((Control)MessengerProgressBar).Focus();
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				if (string.IsNullOrEmpty(PhonePrefixTextBox.Text) || vm.UpdateValidatedCountryCode(PhonePrefixTextBox.Text))
				{
					vm.SavePhoneNumberCommand.Execute(null);
				}
			});
		}

		private void UpdateOkButton()
		{
			_okButton.IsEnabled = !string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text);
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)CountryCodeListGrid).Width = ((FrameworkElement)LayoutRoot).ActualWidth;
			((FrameworkElement)CountryCodeListGrid).Height = ((FrameworkElement)LayoutRoot).ActualHeight;
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				SystemTrayManager.EnsurePagePreferencesStored((Page)(object)this, !vm.IsLoginFlow);
				vm.PropertyChanged += OnPropertyChanged;
				_skipButton.IsEnabled = true;
			});
			UpdateOkButton();
			UpdateFullScreenInterstitialState();
			UpdateTitleAndRetryButton();
		}

		private void OnUnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				SystemTrayManager.EnsurePagePreferencesStored((Page)(object)this, !vm.IsLoginFlow);
			});
			base.OnNavigatedTo(e);
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				if (e.PropertyName.Equals("IsLoading"))
				{
					if (vm.IsLoading)
					{
						_okButton.IsEnabled = false;
						_skipButton.IsEnabled = false;
					}
					else
					{
						UpdateOkButton();
						_skipButton.IsEnabled = true;
					}
				}
				else if (e.PropertyName.Equals("MyPhoneNumber") && !vm.IsLoading)
				{
					UpdateOkButton();
				}
				else if (e.PropertyName.Equals("ShouldShowInterstitial"))
				{
					UpdateFullScreenInterstitialState();
				}
				else if (e.PropertyName.Equals("ShouldShowRetryButton"))
				{
					UpdateTitleAndRetryButton();
				}
			});
		}

		private void UpdateFullScreenInterstitialState()
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((UIElement)FullscreenProgressIndicator).Visibility = VisibilityUtil.FromBool(vm.ShouldShowInterstitial);
				((PhoneApplicationPage)this).ApplicationBar.IsVisible = !vm.ShouldShowInterstitial;
			});
		}

		private void UpdateTitleAndRetryButton()
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				if (vm.ShouldShowInterstitial)
				{
					if (vm.ShouldShowRetryButton)
					{
						FullscreenProgressIndicator.Text = StringLibrary.FetchingDataFromServerFailed;
						FullscreenProgressIndicator.IsIndeterminate = false;
						FullscreenProgressIndicator.ShowButton = true;
					}
					else
					{
						FullscreenProgressIndicator.Text = string.Empty;
						FullscreenProgressIndicator.IsIndeterminate = true;
						FullscreenProgressIndicator.ShowButton = false;
					}
				}
			});
		}

		private void PhoneNumberTextBox_KeyUp(object sender, RoutedEventArgs routedEventArgs)
		{
			int cursorPosition = PhoneNumberTextBox.SelectionStart;
			bool cursorAtEnd = cursorPosition >= PhoneNumberTextBox.Text.Length;
			string originalText = PhoneNumberTextBox.Text;
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				vm.MyPhoneNumber = PhoneNumberTextBox.Text;
				PhoneNumberTextBox.SelectionStart = (cursorAtEnd ? PhoneNumberTextBox.Text.Length : GetCursorPosition(cursorPosition, originalText, PhoneNumberTextBox.Text));
			});
			UpdateOkButton();
		}

		private static int GetCursorPosition(int originalPosition, string originalText, string newText)
		{
			if (originalPosition >= originalText.Length)
			{
				return newText.Length;
			}
			string text = originalText.Substring(0, originalPosition);
			int num = 0;
			string text2 = text;
			foreach (char value in text2)
			{
				if (num >= newText.Length)
				{
					break;
				}
				int num2 = newText.IndexOf(value, num);
				if (num2 >= 0)
				{
					num = num2 + 1;
				}
			}
			return num;
		}

		private void PhoneNumberForm_OnGotFocus(object sender, RoutedEventArgs e)
		{
			SystemTrayManager.AddOperationToStack(((object)this).GetType());
		}

		private void PhoneNumberForm_OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (!VisualTreeHelperExtensions.IsChildElementOf(FocusManager.GetFocusedElement(), PhoneFormContainer))
			{
				SystemTrayManager.RemoveOperationFromStack(((object)this).GetType());
			}
		}

		private void CountryCodeListPicker_OnTap(object sender, GestureEventArgs e)
		{
			CountryCodeListPopup.IsOpen = true;
			((Control)CountryCodeList).Focus();
		}

		private void CountryCodeList_OnLoaded(object sender, RoutedEventArgs eventArgs)
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				if (vm.CountryCodes != null && vm.ValidatedCountryCode != null)
				{
					try
					{
						((LongListSelector)CountryCodeList).ScrollTo((object)vm.ValidatedCountryCode);
					}
					catch (Exception arg)
					{
						Logger.Error(string.Format("Error while trying to scroll to country code ({0}): {1}", vm.ValidatedCountryCode.RegionCode, arg), "CountryCodeList_OnLoaded", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\PhoneNumberPage.xaml.cs", 236);
					}
				}
			});
		}

		private void CountryCodeListItem_OnTap(object sender, GestureEventArgs e)
		{
			TextBlock textBlock = (TextBlock)((sender is TextBlock) ? sender : null);
			if (textBlock != null)
			{
				WithViewModel((PhoneNumberViewModel vm) =>
				{
					vm.ValidatedCountryCode = ((FrameworkElement)textBlock).DataContext as CountryCode;
				});
				CountryCodeListPopup.IsOpen = false;
			}
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			if (CountryCodeListPopup.IsOpen)
			{
				CountryCodeListPopup.IsOpen = false;
				e.Cancel = true;
			}
		}

		private void PhonePrefixTextBoxOnLostFocus(object sender, RoutedEventArgs e)
		{
			if (_prefixValidated)
			{
				_prefixValidated = false;
				return;
			}
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				if (!string.IsNullOrEmpty(PhonePrefixTextBox.Text))
				{
					vm.UpdateValidatedCountryCode(PhonePrefixTextBox.Text);
				}
			});
		}

		protected override void OnKeyboardVisibilityChanged(bool showing, Rect occludedRect)
		{
			int num = GetKeyboardTransform((int)occludedRect.Height) + 72;
			int rootFrameTransform = (showing ? (-1 * num) : 0);
			SetRootFrameTransform(rootFrameTransform);
		}

		private int GetKeyboardTransform(int occludedHeight)
		{
			return InputHelper.GetOccludedOffset((FrameworkElement)(object)PhoneNumberGrid, (FrameworkElement)(object)LayoutRoot, occludedHeight);
		}

		private void RetryButtonOnTapped(object sender, EventArgs e)
		{
			WithViewModel((PhoneNumberViewModel vm) =>
			{
				vm.RefetchPrivacySettingsCommand.Execute(null);
			});
		}

		private void CountryCodeListPopup_OnOpened(object sender, EventArgs e)
		{
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
		}

		private void CountryCodeListPopup_OnClosed(object sender, EventArgs e)
		{
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
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
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected Obj, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected Obj, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected Obj, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected Obj, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected Obj, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected Obj, but got Unknown
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/PhoneNumberPage.xaml", UriKind.Relative));
				CountryCodeDataTemplate = (DataTemplate)((FrameworkElement)this).FindName("CountryCodeDataTemplate");
				GroupedCountryCodeHeaderTemplate = (DataTemplate)((FrameworkElement)this).FindName("GroupedCountryCodeHeaderTemplate");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				CountryCodeListPopup = (Popup)((FrameworkElement)this).FindName("CountryCodeListPopup");
				CountryCodeListGrid = (Grid)((FrameworkElement)this).FindName("CountryCodeListGrid");
				CountryCodeList = (FacebookListView)((FrameworkElement)this).FindName("CountryCodeList");
				MessengerProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("MessengerProgressBar");
				TitleTopMargin = (Border)((FrameworkElement)this).FindName("TitleTopMargin");
				Logo = (Image)((FrameworkElement)this).FindName("Logo");
				SignInTitle = (TextBlock)((FrameworkElement)this).FindName("SignInTitle");
				PhoneFormContainer = (Grid)((FrameworkElement)this).FindName("PhoneFormContainer");
				CountryCodeListPicker = (TextBox)((FrameworkElement)this).FindName("CountryCodeListPicker");
				PhoneNumberGrid = (Grid)((FrameworkElement)this).FindName("PhoneNumberGrid");
				PhonePrefixTextBox = (TextBox)((FrameworkElement)this).FindName("PhonePrefixTextBox");
				PhoneNumberTextBox = (TextBox)((FrameworkElement)this).FindName("PhoneNumberTextBox");
				FullscreenProgressIndicator = (FullscreenProgressIndicatorWithButton)((FrameworkElement)this).FindName("FullscreenProgressIndicator");
			}
		}
	}
}

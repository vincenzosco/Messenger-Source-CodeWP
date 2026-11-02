using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Windows.Foundation;
using Rect = System.Windows.Rect;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class PhoneNumberRegistrationPage : PhoneNumberRegistrationViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _okButton;

		public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(Error), typeof(PhoneNumberRegistrationPage), new PropertyMetadata((object)null, (PropertyChangedCallback)ErrorChangedCallback));


















		public Error Error
		{
			get
			{
				return (Error)base.GetValue(ErrorProperty);
			}
			set
			{
				base.SetValue(ErrorProperty, (object)value);
			}
		}

		public PhoneNumberRegistrationPage()
		{
			InitializeComponent();
			InitializeAppBar();
			RegisterInputElements(new List<UIElement> { (UIElement)(object)PhoneInputGrid });
		}

		private void InitializeAppBar()
		{
			_appBarHelper = new AppBarHelper(base.ApplicationBar);
			_okButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.AppBar_Ok, OkButtonOnClick, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
			_okButton.IsEnabled = false;
		}

		private void OkButtonOnClick(object sender, EventArgs e)
		{
			((Control)MessengerProgressBar).Focus();
			WithViewModel((PhoneNumberRegistrationViewModel vm) =>
			{
				if (string.IsNullOrEmpty(PhonePrefixTextBox.Text) || vm.UpdateValidatedCountryCode(PhonePrefixTextBox.Text))
				{
					vm.RegisterPhoneNumberCommand.Execute(null);
				}
			});
		}

		private async void OnLoaded(object sender, RoutedEventArgs e)
		{
			PhoneNumberRegistrationPage phoneNumberRegistrationPage = this;
			DependencyProperty errorProperty = ErrorProperty;
			Binding val = new Binding();
			val.Path = new PropertyPath("Error", new object[0]);
			((FrameworkElement)phoneNumberRegistrationPage).SetBinding(errorProperty, val);
			((FrameworkElement)CountryCodeListGrid).Width = ((FrameworkElement)LayoutRoot).ActualWidth;
			((FrameworkElement)CountryCodeListGrid).Height = ((FrameworkElement)LayoutRoot).ActualHeight;
			await WithViewModel(async (PhoneNumberRegistrationViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
				_okButton.IsEnabled = !string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text);
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
			WithViewModel((PhoneNumberRegistrationViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
			});
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("IsLoading"))
			{
				WithViewModel((PhoneNumberRegistrationViewModel vm) =>
				{
					_okButton.IsEnabled = !vm.IsLoading && !string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text);
				});
			}
		}

		private static void ErrorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			PhoneNumberRegistrationPage phoneNumberRegistrationPage = d as PhoneNumberRegistrationPage;
			if (phoneNumberRegistrationPage != null)
			{
				Error error = e.NewValue as Error;
				if (error != null && !string.IsNullOrEmpty(error.Message))
				{
					MessageBox.Show(error.Message, error.Title, (MessageBoxButton)0);
				}
			}
		}

		private void PhoneNumberTextBox_KeyUp(object sender, RoutedEventArgs routedEventArgs)
		{
			int cursorPosition = PhoneNumberTextBox.SelectionStart;
			bool cursorAtEnd = cursorPosition >= PhoneNumberTextBox.Text.Length;
			string originalText = PhoneNumberTextBox.Text;
			WithViewModel((PhoneNumberRegistrationViewModel vm) =>
			{
				vm.MyPhoneNumber = PhoneNumberTextBox.Text;
				PhoneNumberTextBox.SelectionStart = (cursorAtEnd ? PhoneNumberTextBox.Text.Length : GetCursorPosition(cursorPosition, originalText, PhoneNumberTextBox.Text));
			});
			_okButton.IsEnabled = !string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text);
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

		private void CountryCodeListPicker_OnTap(object sender, GestureEventArgs e)
		{
			CountryCodeListPopup.IsOpen = true;
			((Control)CountryCodeList).Focus();
		}

		private void CountryCodeList_OnLoaded(object sender, RoutedEventArgs eventArgs)
		{
			WithViewModel((PhoneNumberRegistrationViewModel vm) =>
			{
				if (vm.CountryCodes != null && vm.ValidatedCountryCode != null)
				{
					try
					{
						((LongListSelector)CountryCodeList).ScrollTo((object)vm.ValidatedCountryCode);
					}
					catch (Exception arg)
					{
						Logger.Error(string.Format("Error while trying to scroll to country code ({0}): {1}", vm.ValidatedCountryCode.RegionCode, arg), "CountryCodeList_OnLoaded", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\PhoneNumberRegistrationPage.xaml.cs", 175);
					}
				}
			});
		}

		private void CountryCodeListItem_OnTap(object sender, GestureEventArgs e)
		{
			TextBlock textBlock = (TextBlock)((sender is TextBlock) ? sender : null);
			if (textBlock != null)
			{
				WithViewModel((PhoneNumberRegistrationViewModel vm) =>
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
			WithViewModel((PhoneNumberRegistrationViewModel vm) =>
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
			return InputHelper.GetOccludedOffset((FrameworkElement)(object)LoginWithFBGrid, (FrameworkElement)(object)LayoutRoot, occludedHeight);
		}

		private void CountryCodeListPopup_OnOpened(object sender, EventArgs e)
		{
			base.ApplicationBar.IsVisible = false;
		}

		private void CountryCodeListPopup_OnClosed(object sender, EventArgs e)
		{
			base.ApplicationBar.IsVisible = true;
		}


		static PhoneNumberRegistrationPage()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
		}
	}
}

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

namespace Facebook.Messenger.WP8.Pages
{
	public class PhoneNumberRegistrationPage : PhoneNumberRegistrationViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _okButton;

		public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(Error), typeof(PhoneNumberRegistrationPage), new PropertyMetadata((object)null, (PropertyChangedCallback)ErrorChangedCallback));

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

		internal Grid PhoneInputGrid;

		internal TextBox PhonePrefixTextBox;

		internal TextBox PhoneNumberTextBox;

		internal Grid LoginWithFBGrid;

		private bool _contentLoaded;

		public Error Error
		{
			get
			{
				return (Error)((DependencyObject)this).GetValue(ErrorProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ErrorProperty, (object)value);
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
			_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
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
						Logger.Error($"Error while trying to scroll to country code ({vm.ValidatedCountryCode.RegionCode}): {arg}", "CountryCodeList_OnLoaded", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\PhoneNumberRegistrationPage.xaml.cs", 175);
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
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/PhoneNumberRegistrationPage.xaml", UriKind.Relative));
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
				PhoneInputGrid = (Grid)((FrameworkElement)this).FindName("PhoneInputGrid");
				PhonePrefixTextBox = (TextBox)((FrameworkElement)this).FindName("PhonePrefixTextBox");
				PhoneNumberTextBox = (TextBox)((FrameworkElement)this).FindName("PhoneNumberTextBox");
				LoginWithFBGrid = (Grid)((FrameworkElement)this).FindName("LoginWithFBGrid");
			}
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

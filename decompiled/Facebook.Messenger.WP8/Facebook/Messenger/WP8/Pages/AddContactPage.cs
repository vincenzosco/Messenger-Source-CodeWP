using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages;

public class AddContactPage : AddContactViewModelPage
{
	private enum CancelUndoButtonState
	{
		Cancel,
		Undo
	}

	private AppBarHelper _appBarHelper;

	private IconBarEntry<bool> _okButton;

	private IconBarEntry<CancelUndoButtonState> _cancelButton;

	internal DataTemplate CountryCodeDataTemplate;

	internal DataTemplate GroupedCountryCodeHeaderTemplate;

	internal Grid LayoutRoot;

	internal Grid AddContactGrid;

	internal Popup CountryCodeListPopup;

	internal Grid CountryCodeListGrid;

	internal FacebookListView CountryCodeList;

	internal FBMessengerProgressBar MessengerProgressBar;

	internal Grid PhoneFormContainer;

	internal TextBox CountryCodeListPicker;

	internal TextBox PhonePrefixTextBox;

	internal TextBox PhoneNumberTextBox;

	internal Grid MatchingUserFoundGrid;

	internal Image Logo;

	internal RichTextBox UserAddedRichTextBox;

	internal Button SendMessageButton;

	internal Grid InviteContactGrid;

	internal RichTextBox InviteUserRichTextBox;

	private bool _contentLoaded;

	public AddContactPage()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected Obj, but got Unknown
		InitializeComponent();
		InitializeAppBar();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnLoaded;
	}

	private void InitializeAppBar()
	{
		_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
		_okButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(state: false, StringLibrary.AppBar_Ok, OkButtonOnClick, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
		_cancelButton = _appBarHelper.CreateBarButtonEntry<CancelUndoButtonState>(new EntryState<CancelUndoButtonState>(CancelUndoButtonState.Undo, StringLibrary.AppBar_Undo, CancelButtonOnClick, new Uri("/Assets/AppBar/Stop.png", UriKind.Relative)), new EntryState<CancelUndoButtonState>(CancelUndoButtonState.Cancel, StringLibrary.AppBar_Cancel, CancelButtonOnClick, new Uri("/Assets/AppBar/Stop.png", UriKind.Relative)));
		_cancelButton.IsEnabled = false;
		_cancelButton.IsVisible = false;
		_okButton.IsEnabled = false;
	}

	private void CancelButtonOnClick(object sender, EventArgs eventArgs)
	{
		((Control)MessengerProgressBar).Focus();
		WithViewModel((AddContactViewModel vm) =>
		{
			switch (vm.ContactState)
			{
			case AddContactViewModel.FindContactState.MatchedExistingUser:
			case AddContactViewModel.FindContactState.MatchedNewUser:
				ShowRemoveContactMessageBox();
				break;
			case AddContactViewModel.FindContactState.InviteContactToMessenger:
				vm.NavigateBackCommand.Execute(null);
				break;
			}
		});
	}

	private void ShowRemoveContactMessageBox()
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			if (vm.MatchingUser != null)
			{
				string text = string.Format(StringLibrary.AddContact_Remove_Contact_Warning, vm.MatchingUser.Name);
				MessageBoxResult val = MessageBox.Show(text, StringLibrary.AddContact_Remove_MessageBox_Title, (MessageBoxButton)1);
				if ((int)val == 1)
				{
					vm.RemoveContactCommand.Execute(null);
				}
			}
		});
	}

	private void OkButtonOnClick(object sender, EventArgs e)
	{
		((Control)MessengerProgressBar).Focus();
		WithViewModel((AddContactViewModel vm) =>
		{
			switch (vm.ContactState)
			{
			case AddContactViewModel.FindContactState.AddContact:
				if (string.IsNullOrEmpty(PhonePrefixTextBox.Text) || vm.UpdateValidatedCountryCode(PhonePrefixTextBox.Text))
				{
					vm.FindContactCommand.Execute(null);
				}
				break;
			case AddContactViewModel.FindContactState.MatchedExistingUser:
			case AddContactViewModel.FindContactState.MatchedNewUser:
				vm.NavigateBackCommand.Execute(null);
				break;
			}
		});
	}

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		((FrameworkElement)CountryCodeListGrid).Width = ((FrameworkElement)LayoutRoot).ActualWidth;
		((FrameworkElement)CountryCodeListGrid).Height = ((FrameworkElement)LayoutRoot).ActualHeight;
		_okButton.IsEnabled = !string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text);
		WithViewModel((AddContactViewModel vm) =>
		{
			vm.PropertyChanged += OnPropertyChanged;
		});
	}

	private void OnUnLoaded(object sender, RoutedEventArgs e)
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			vm.PropertyChanged -= OnPropertyChanged;
		});
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName.Equals("IsLoading"))
		{
			WithViewModel((AddContactViewModel vm) =>
			{
				if (vm.IsLoading)
				{
					IconBarEntry<bool> okButton = _okButton;
					bool isEnabled = (_cancelButton.IsEnabled = false);
					okButton.IsEnabled = isEnabled;
				}
				else
				{
					_okButton.IsEnabled = !string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text);
					_cancelButton.IsEnabled = true;
				}
			});
		}
		else if (e.PropertyName.Equals("ContactState"))
		{
			UpdateView();
			UpdateAppBar();
		}
	}

	private void UpdateAppBar()
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			switch (vm.ContactState)
			{
			case AddContactViewModel.FindContactState.AddContact:
				_okButton.IsVisible = true;
				_cancelButton.IsVisible = false;
				break;
			case AddContactViewModel.FindContactState.MatchedExistingUser:
			case AddContactViewModel.FindContactState.MatchedNewUser:
				_okButton.IsVisible = true;
				_cancelButton.State = CancelUndoButtonState.Undo;
				_cancelButton.IsVisible = !vm.MatchingUserIsSelf;
				break;
			case AddContactViewModel.FindContactState.InviteContactToMessenger:
				_okButton.IsVisible = false;
				_cancelButton.State = CancelUndoButtonState.Cancel;
				_cancelButton.IsVisible = true;
				break;
			}
		});
	}

	private void UpdateMatchingUserView(bool isExistingUser)
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			if (vm.MatchingUser != null)
			{
				Paragraph paragraphForStringWithFormat = GetParagraphForStringWithFormat(vm.MatchingUser.Name, isExistingUser ? StringLibrary.AddContact_MatchedExistingUser_Label : StringLibrary.AddContact_MatchingUser_Label);
				((ContentControl)SendMessageButton).Content = string.Format(StringLibrary.AddContact_Message_Button, vm.MatchingUser.FirstName);
				((PresentationFrameworkCollection<Block>)(object)UserAddedRichTextBox.Blocks).Add((Block)(object)paragraphForStringWithFormat);
			}
		});
	}

	private void UpdateInviteContactView()
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			if (!string.IsNullOrEmpty(vm.MyPhoneNumber))
			{
				Paragraph paragraphForStringWithFormat = GetParagraphForStringWithFormat(vm.MyPhoneNumber, StringLibrary.Matching_Contact_NotFound_Message);
				((PresentationFrameworkCollection<Block>)(object)InviteUserRichTextBox.Blocks).Add((Block)(object)paragraphForStringWithFormat);
			}
		});
	}

	private Paragraph GetParagraphForStringWithFormat(string value, string format)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected Obj, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected Obj, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected Obj, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected Obj, but got Unknown
		int num = format.IndexOf("{0}", StringComparison.Ordinal);
		int num2 = num + 3;
		string text = format.Substring(0, num);
		string text2 = format.Substring(num2);
		Paragraph val = new Paragraph();
		if (num != 0)
		{
			InlineCollection inlines = val.Inlines;
			Run val2 = new Run();
			val2.Text = text;
			((PresentationFrameworkCollection<Inline>)(object)inlines).Add((Inline)(object)val2);
		}
		InlineCollection inlines2 = val.Inlines;
		Run val3 = new Run();
		val3.Text = value;
		((TextElement)val3).FontWeight = FontWeights.Bold;
		((PresentationFrameworkCollection<Inline>)(object)inlines2).Add((Inline)(object)val3);
		if (num2 < format.Length - 1)
		{
			InlineCollection inlines3 = val.Inlines;
			Run val4 = new Run();
			val4.Text = text2;
			((PresentationFrameworkCollection<Inline>)(object)inlines3).Add((Inline)(object)val4);
		}
		return val;
	}

	private void UpdateView()
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			bool visible = vm.ContactState == AddContactViewModel.FindContactState.AddContact;
			bool flag = vm.ContactState == AddContactViewModel.FindContactState.MatchedNewUser;
			bool flag2 = vm.ContactState == AddContactViewModel.FindContactState.MatchedExistingUser;
			bool flag3 = vm.ContactState == AddContactViewModel.FindContactState.InviteContactToMessenger;
			bool flag4 = flag || flag2;
			((UIElement)AddContactGrid).Visibility = VisibilityUtil.FromBool(visible);
			((UIElement)MatchingUserFoundGrid).Visibility = VisibilityUtil.FromBool(flag4);
			((UIElement)InviteContactGrid).Visibility = VisibilityUtil.FromBool(flag3);
			if (flag4)
			{
				UpdateMatchingUserView(flag2);
			}
			else if (flag3)
			{
				UpdateInviteContactView();
			}
		});
	}

	private void PhoneNumberTextBox_KeyUp(object sender, RoutedEventArgs routedEventArgs)
	{
		int cursorPosition = PhoneNumberTextBox.SelectionStart;
		bool cursorAtEnd = cursorPosition >= PhoneNumberTextBox.Text.Length;
		string originalText = PhoneNumberTextBox.Text;
		WithViewModel((AddContactViewModel vm) =>
		{
			if (vm.PhoneNumberUtil != null)
			{
				vm.MyPhoneNumber = PhoneNumberTextBox.Text;
				PhoneNumberTextBox.SelectionStart = (cursorAtEnd ? PhoneNumberTextBox.Text.Length : GetCursorPosition(cursorPosition, originalText, PhoneNumberTextBox.Text));
			}
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

	private void PhoneNumberForm_OnGotFocus(object sender, RoutedEventArgs e)
	{
		SystemTrayManager.AddOperationToStack(((object)this).GetType());
	}

	private void PhoneNumberForm_OnLostFocus(object sender, RoutedEventArgs e)
	{
		SystemTrayManager.RemoveOperationFromStack(((object)this).GetType());
	}

	private void CountryCodeListPicker_OnTap(object sender, GestureEventArgs e)
	{
		((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
		CountryCodeListPopup.IsOpen = true;
		((Control)CountryCodeList).Focus();
	}

	private void CountryCodeList_OnLoaded(object sender, RoutedEventArgs eventArgs)
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			if (vm.CountryCodes != null && vm.ValidatedCountryCode != null)
			{
				try
				{
					((LongListSelector)CountryCodeList).ScrollTo((object)vm.ValidatedCountryCode);
				}
				catch (Exception arg)
				{
					Logger.Error($"Error while trying to scroll to country code ({vm.ValidatedCountryCode.RegionCode}): {arg}", "CountryCodeList_OnLoaded", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\AddContactPage.xaml.cs", 358);
				}
			}
		});
	}

	private void CountryCodeListItem_OnTap(object sender, GestureEventArgs e)
	{
		TextBlock textBlock = (TextBlock)((sender is TextBlock) ? sender : null);
		if (textBlock != null)
		{
			WithViewModel((AddContactViewModel vm) =>
			{
				vm.ValidatedCountryCode = ((FrameworkElement)textBlock).DataContext as CountryCode;
			});
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
			CountryCodeListPopup.IsOpen = false;
		}
	}

	protected override void OnBackKeyPress(CancelEventArgs e)
	{
		if (CountryCodeListPopup.IsOpen)
		{
			CountryCodeListPopup.IsOpen = false;
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
			e.Cancel = true;
		}
	}

	private void PhonePrefixTextBoxOnLostFocus(object sender, RoutedEventArgs e)
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			if (!string.IsNullOrEmpty(PhonePrefixTextBox.Text))
			{
				vm.UpdateValidatedCountryCode(PhonePrefixTextBox.Text);
			}
		});
	}

	private void SendMessageOnClick(object sender, RoutedEventArgs e)
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			vm.SendMessageCommand.Execute(null);
		});
	}

	private void InviteOnClick(object sender, RoutedEventArgs e)
	{
		WithViewModel((AddContactViewModel vm) =>
		{
			vm.SendInviteCommand.Execute(null);
		});
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
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected Obj, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/AddContactPage.xaml", UriKind.Relative));
			CountryCodeDataTemplate = (DataTemplate)((FrameworkElement)this).FindName("CountryCodeDataTemplate");
			GroupedCountryCodeHeaderTemplate = (DataTemplate)((FrameworkElement)this).FindName("GroupedCountryCodeHeaderTemplate");
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			AddContactGrid = (Grid)((FrameworkElement)this).FindName("AddContactGrid");
			CountryCodeListPopup = (Popup)((FrameworkElement)this).FindName("CountryCodeListPopup");
			CountryCodeListGrid = (Grid)((FrameworkElement)this).FindName("CountryCodeListGrid");
			CountryCodeList = (FacebookListView)((FrameworkElement)this).FindName("CountryCodeList");
			MessengerProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("MessengerProgressBar");
			PhoneFormContainer = (Grid)((FrameworkElement)this).FindName("PhoneFormContainer");
			CountryCodeListPicker = (TextBox)((FrameworkElement)this).FindName("CountryCodeListPicker");
			PhonePrefixTextBox = (TextBox)((FrameworkElement)this).FindName("PhonePrefixTextBox");
			PhoneNumberTextBox = (TextBox)((FrameworkElement)this).FindName("PhoneNumberTextBox");
			MatchingUserFoundGrid = (Grid)((FrameworkElement)this).FindName("MatchingUserFoundGrid");
			Logo = (Image)((FrameworkElement)this).FindName("Logo");
			UserAddedRichTextBox = (RichTextBox)((FrameworkElement)this).FindName("UserAddedRichTextBox");
			SendMessageButton = (Button)((FrameworkElement)this).FindName("SendMessageButton");
			InviteContactGrid = (Grid)((FrameworkElement)this).FindName("InviteContactGrid");
			InviteUserRichTextBox = (RichTextBox)((FrameworkElement)this).FindName("InviteUserRichTextBox");
		}
	}
}

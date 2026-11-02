using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages;

public class NewUserRegistrationPage : NewUserRegistrationViewModelPage
{
	private AppBarHelper _appBarHelper;

	private IconBarEntry<bool> _nextButton;

	internal Grid RootGrid;

	internal Grid FormContainer;

	internal TextBox FirstNameTextBox;

	internal TextBox LastNameTextBox;

	private bool _contentLoaded;

	public NewUserRegistrationPage()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		InitializeComponent();
		InitializeAppBar();
		((FrameworkElement)this).Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		UpdateAppBar();
	}

	private void InitializeAppBar()
	{
		_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
		_nextButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(state: false, StringLibrary.Attachments_AppBar_Next, NextButtonOnClick, new Uri("/Assets/AppBar/Next.png", UriKind.Relative)));
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
				((Control)this).Focus();
				CreateAccount();
			}
		}
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/NewUserRegistrationPage.xaml", UriKind.Relative));
			RootGrid = (Grid)((FrameworkElement)this).FindName("RootGrid");
			FormContainer = (Grid)((FrameworkElement)this).FindName("FormContainer");
			FirstNameTextBox = (TextBox)((FrameworkElement)this).FindName("FirstNameTextBox");
			LastNameTextBox = (TextBox)((FrameworkElement)this).FindName("LastNameTextBox");
		}
	}
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages;

public class ProfilePicConfirmationPage : ProfilePicConfirmationViewModelPage
{
	private AppBarHelper _appBarHelper;

	private IconBarEntry<bool> _yesButton;

	private IconBarEntry<bool> _noButton;

	public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(Error), typeof(ProfilePicConfirmationPage), new PropertyMetadata((object)null, (PropertyChangedCallback)ErrorChangedCallback));

	public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(ProfilePicConfirmationPage), new PropertyMetadata((object)false, (PropertyChangedCallback)IsLoadingChangedCallback));

	internal Grid RootGrid;

	internal Border TitleTopMargin;

	internal Image ProfilePhoto;

	private bool _contentLoaded;

	public bool IsLoading
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsLoadingProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsLoadingProperty, (object)value);
		}
	}

	private static void ErrorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (d is ProfilePicConfirmationPage && e.NewValue is Error error && !string.IsNullOrEmpty(error.Message))
		{
			MessageBox.Show(error.Message, error.Title, (MessageBoxButton)0);
		}
	}

	private static void IsLoadingChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ProfilePicConfirmationPage profilePicConfirmationPage)
		{
			bool flag = e.NewValue is bool && (bool)e.NewValue;
			IconBarEntry<bool> yesButton = profilePicConfirmationPage._yesButton;
			bool isEnabled = (profilePicConfirmationPage._noButton.IsEnabled = !flag);
			yesButton.IsEnabled = isEnabled;
		}
	}

	public ProfilePicConfirmationPage()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		InitializeComponent();
		InitializeAppBar();
		((FrameworkElement)this).Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected Obj, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected Obj, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected Obj, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected Obj, but got Unknown
		DependencyProperty errorProperty = ErrorProperty;
		Binding val = new Binding();
		val.Path = new PropertyPath("Error", new object[0]);
		((FrameworkElement)this).SetBinding(errorProperty, val);
		DependencyProperty isLoadingProperty = IsLoadingProperty;
		Binding val2 = new Binding();
		val2.Path = new PropertyPath("IsLoading", new object[0]);
		((FrameworkElement)this).SetBinding(isLoadingProperty, val2);
	}

	private void InitializeAppBar()
	{
		_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
		_yesButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(state: false, StringLibrary.AppBar_Yes, YesButtonOnClick, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
		_noButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(state: false, StringLibrary.AppBar_No, NoButtonOnClick, new Uri("/Assets/AppBar/Skip.png", UriKind.Relative)));
	}

	private void NoButtonOnClick(object sender, EventArgs e)
	{
		WithViewModel((ProfilePicConfirmationViewModel vm) =>
		{
			vm.NavigateBackCommand.Execute(null);
		});
	}

	private void YesButtonOnClick(object sender, EventArgs e)
	{
		WithViewModel((ProfilePicConfirmationViewModel vm) =>
		{
			vm.ConfirmProfilePicCommand.Execute(null);
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/ProfilePicConfirmationPage.xaml", UriKind.Relative));
			RootGrid = (Grid)((FrameworkElement)this).FindName("RootGrid");
			TitleTopMargin = (Border)((FrameworkElement)this).FindName("TitleTopMargin");
			ProfilePhoto = (Image)((FrameworkElement)this).FindName("ProfilePhoto");
		}
	}

	static ProfilePicConfirmationPage()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected Obj, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected Obj, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected Obj, but got Unknown
	}
}

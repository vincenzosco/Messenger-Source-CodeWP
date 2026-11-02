using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Pages;

public class ExistingUserPage : ExistingUserViewModelPage
{
	internal Grid LayoutRoot;

	internal Border TitleTopMargin;

	internal Image ProfilePhoto;

	internal TextBlock TitleTextBlock;

	internal TextBlock DescriptionTextBlock;

	internal Button LoginAsUserButton;

	private bool _contentLoaded;

	public ExistingUserPage()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		WithViewModel((ExistingUserViewModel vm) =>
		{
			DescriptionTextBlock.Text = string.Format(StringLibrary.ExistingUser_Description, vm.FirstName, vm.LastName);
			((ContentControl)LoginAsUserButton).Content = string.Format(StringLibrary.ExistingUser_LoginAs_Button, vm.FirstName);
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/ExistingUserPage.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			TitleTopMargin = (Border)((FrameworkElement)this).FindName("TitleTopMargin");
			ProfilePhoto = (Image)((FrameworkElement)this).FindName("ProfilePhoto");
			TitleTextBlock = (TextBlock)((FrameworkElement)this).FindName("TitleTextBlock");
			DescriptionTextBlock = (TextBlock)((FrameworkElement)this).FindName("DescriptionTextBlock");
			LoginAsUserButton = (Button)((FrameworkElement)this).FindName("LoginAsUserButton");
		}
	}
}

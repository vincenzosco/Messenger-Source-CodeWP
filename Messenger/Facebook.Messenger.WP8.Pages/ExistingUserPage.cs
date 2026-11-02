using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class ExistingUserPage : ExistingUserViewModelPage
	{







		public ExistingUserPage()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			WithViewModel((ExistingUserViewModel vm) =>
			{
				DescriptionTextBlock.Text = string.Format(StringLibrary.ExistingUser_Description, vm.FirstName, vm.LastName);
				((ContentControl)LoginAsUserButton).Content = string.Format(StringLibrary.ExistingUser_LoginAs_Button, vm.FirstName);
			});
		}

	}
}

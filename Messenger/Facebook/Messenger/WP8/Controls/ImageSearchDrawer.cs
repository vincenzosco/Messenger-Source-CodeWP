using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Facebook.Services.Api.Methods.SearchImages;
using Microsoft.Phone.Controls;
using Image = System.Windows.Controls.Image;
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ImageSearchDrawer : ComposerDrawer
	{


		public event EventHandler<ImageResult> SendPhoto;

		public event EventHandler<ImageResult> EditPhoto;

		public void SearchForImages(string term)
		{
			SearchImageDrawer.IsLoading = true;
			(base.DataContext as MessagesViewModel)?.SearchImagesCommand.Execute(term);
		}

		public ImageSearchDrawer()
			: base(DrawerType.ImageSearch)
		{
			InitializeComponent();
		}

		private void RegisterForEvents()
		{
			MessagesViewModel messagesViewModel = base.DataContext as MessagesViewModel;
			if (messagesViewModel != null)
			{
				messagesViewModel.SearchImagesCompleted += OnSearchImagesCompleted;
			}
		}

		private void UnregisterForEvents()
		{
			MessagesViewModel messagesViewModel = base.DataContext as MessagesViewModel;
			if (messagesViewModel != null)
			{
				messagesViewModel.SearchImagesCompleted -= OnSearchImagesCompleted;
			}
		}

		private void OnSearchImagesCompleted(object sender, List<Facebook.Services.Api.Methods.SearchImages.Image> searchResults)
		{
			IEnumerable<Uri> source = searchResults.Select((Facebook.Services.Api.Methods.SearchImages.Image result) => new Uri(result.Thumbnail.Url));
			List<UriImageDrawerItemViewModel> itemsSource = source.Select((Uri uri) => new UriImageDrawerItemViewModel(uri)).ToList();
			SearchImageDrawer.ItemsSource = itemsSource;
			SearchImageDrawer.IsLoading = false;
		}

		public override void Show()
		{
			RegisterForEvents();
			base.Visibility = Visibility.Visible;
			base.Dispatcher.BeginInvoke((Action)PromptForSearch);
		}

		public override void Hide()
		{
			UnregisterForEvents();
			SearchImageDrawer.Reset();
			SearchImageDrawer.ItemsSource = null;
			base.Visibility = Visibility.Collapsed;
		}

		public override void Cleanup()
		{
			UnregisterForEvents();
		}

		private void SearchImageDrawer_OnSendClicked(object sender, PhotoData image)
		{
			if (SendPhoto != null)
			{
				ImageResult e = new ImageResult(MediaSource.ImageSearch, image);
				SendPhoto(this, e);
			}
		}

		private void SearchImageDrawer_OnEditClicked(object sender, PhotoData image)
		{
			if (EditPhoto != null)
			{
				ImageResult e = new ImageResult(MediaSource.ImageSearch, image);
				EditPhoto(this, e);
			}
		}

		private void SearchButton_Tap(object sender, GestureEventArgs e)
		{
			PromptForSearch();
		}

		private void PromptForSearch()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected Obj, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected Obj, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected Obj, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected Obj, but got Unknown
			((UIElement)SearchImageDrawer).Visibility = Visibility.Collapsed;
			TextBox textBox = new TextBox();
			InputScope val = new InputScope();
			IList names = val.Names;
			InputScopeName val2 = new InputScopeName();
			val2.NameValue = (InputScopeNameValue)49;
			names.Add(val2);
			textBox.InputScope = val;
			CustomMessageBox customMessageBox = new CustomMessageBox();
			customMessageBox.Caption = StringLibrary.Image_Search_Popup_Title;
			customMessageBox.Message = StringLibrary.Image_Search_Popup_Message;
			customMessageBox.LeftButtonContent = StringLibrary.Image_Search_Popup_Search;
			customMessageBox.RightButtonContent = StringLibrary.Image_Search_Popup_Cancel;
			((ContentControl)customMessageBox).Content = textBox;
			customMessageBox.IsFullScreen = false;
			CustomMessageBox customMessageBox2 = customMessageBox;
			((FrameworkElement)customMessageBox2).Loaded += (object sender, RoutedEventArgs args) =>
			{
				base.Dispatcher.BeginInvoke((Action)(() =>
				{
					((Control)textBox).Focus();
				}));
			};
			customMessageBox2.Dismissed += (object sender, DismissedEventArgs args) =>
			{
				((UIElement)SearchImageDrawer).Visibility = Visibility.Visible;
				if (args.Result == CustomMessageBoxResult.LeftButton)
				{
					SearchForImages(textBox.Text);
				}
			};
			customMessageBox2.Show();
		}

	}
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public class PhotoAttachmentsPage : PhotoAttachmentsViewModelPage
	{
		private readonly Uri _nextUriIcon = new Uri("/Assets/AppBar/Next.png", UriKind.Relative);

		private readonly Uri _saveUriIcon = new Uri("/Assets/AppBar/Save.png", UriKind.Relative);

		private readonly Uri _previousUriIcon = new Uri("/Assets/AppBar/Prev.png", UriKind.Relative);

		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _nextButton;

		private IconBarEntry<bool> _savePhotoButton;

		private IconBarEntry<bool> _previousButton;

		private MenuBarEntry<bool> _forwardMenuButton;

		internal Grid LayoutRoot;

		internal MediaViewer PhotoCarousel;

		internal StateIndicatingProgressBar StatusProgressBar;

		private bool _contentLoaded;

		public PhotoAttachmentsPage()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected Obj, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBar();
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
		}

		private void InitializeAppBar()
		{
			_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
			_previousButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(true, StringLibrary.Attachments_AppBar_Previous, PreviousButton_OnClick, _previousUriIcon));
			_savePhotoButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(true, StringLibrary.Photos_SavePhoto_MenuItem, SavePhoto_OnClick, _saveUriIcon));
			_nextButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(true, StringLibrary.Attachments_AppBar_Next, NextButton_OnClick, _nextUriIcon));
			_forwardMenuButton = _appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(true, StringLibrary.Messages_ContextMenu_Forward, ForwardButton_OnClick));
			_previousButton.IsEnabled = false;
			_nextButton.IsEnabled = false;
			_savePhotoButton.IsEnabled = false;
			_forwardMenuButton.IsVisible = false;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			WithViewModel((PhotoAttachmentsViewModel vm) =>
			{
				SetSupportedOrientation();
				vm.PropertyChanged += OnPropertyChanged;
				_previousButton.IsEnabled = vm.IsPreviousEnabled;
				_nextButton.IsEnabled = vm.IsNextEnabled;
				_savePhotoButton.IsEnabled = vm.IsSaveEnabled;
				_forwardMenuButton.IsVisible = vm.IsForwardingEnabled;
				vm.ImageSaveCompleted += ImageSaveCompleted;
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((PhotoAttachmentsViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
				vm.ImageSaveCompleted -= ImageSaveCompleted;
			});
		}

		private void PreviousButton_OnClick(object sender, EventArgs e)
		{
			PhotoCarousel.JumpToItem(PhotoCarousel.DisplayedItemIndex - 1);
		}

		private void NextButton_OnClick(object sender, EventArgs e)
		{
			PhotoCarousel.JumpToItem(PhotoCarousel.DisplayedItemIndex + 1);
		}

		private void SavePhoto_OnClick(object sender, EventArgs e)
		{
			WithViewModel((PhotoAttachmentsViewModel vm) =>
			{
				StatusProgressBar.SetState(StringLibrary.Photos_SavePhoto_Saving_StateIndicator);
				vm.SaveImageCommand.Execute(PhotoCarousel.DisplayedItemIndex);
			});
		}

		private void ForwardButton_OnClick(object sender, EventArgs e)
		{
			WithViewModel((PhotoAttachmentsViewModel vm) =>
			{
				vm.ForwardImageCommand.Execute(PhotoCarousel.DisplayedItemIndex);
			});
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
			case "IsPreviousEnabled":
			case "IsNextEnabled":
			case "IsSaveEnabled":
				UpdateAppBarMenuItems();
				break;
			case "DisableLandscapeOrientation":
				SetSupportedOrientation();
				break;
			}
		}

		private void SetSupportedOrientation()
		{
			WithViewModel((PhotoAttachmentsViewModel vm) =>
			{
				((PhoneApplicationPage)this).SupportedOrientations = (SupportedPageOrientation)(vm.DisableLandscapeOrientation ? 1 : 3);
			});
		}

		private void UpdateAppBarMenuItems()
		{
			WithViewModel((PhotoAttachmentsViewModel vm) =>
			{
				_previousButton.IsEnabled = vm.IsPreviousEnabled;
				_nextButton.IsEnabled = vm.IsNextEnabled;
				_savePhotoButton.IsEnabled = vm.IsSaveEnabled;
			});
		}

		private void ImageSaveCompleted(object sender, ImageSaveEventArgs imageSaveEventArgs)
		{
			if (imageSaveEventArgs.State == ActionState.Succeeded)
			{
				StatusProgressBar.NextState(StringLibrary.Photos_SavePhoto_Success_StateIndicator);
			}
			else if (imageSaveEventArgs.State == ActionState.Failed)
			{
				StatusProgressBar.NextState(StringLibrary.Photos_SavePhoto_Error_StateIndicator);
			}
		}

		private void MediaViewer_ItemDisplayed(object sender, ItemDisplayedEventArgs e)
		{
			WithViewModel((PhotoAttachmentsViewModel vm) =>
			{
				vm.SetButtonStates(e.ItemIndex);
				vm.UpdateIndicatorString(e.ItemIndex);
			});
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/PhotoAttachmentsPage.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				PhotoCarousel = (MediaViewer)((FrameworkElement)this).FindName("PhotoCarousel");
				StatusProgressBar = (StateIndicatingProgressBar)((FrameworkElement)this).FindName("StatusProgressBar");
			}
		}
	}
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Common;
using Facebook.Messenger.WP8.Controls.Photos;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.Pages
{
	public class GroupComposerPage : GroupComposerViewModelPage
	{
		private const string GroupComposerPhoto = "GroupComposerPhoto.jpg";

		private AppBarHelper _appBarHelper;

		private IconBarEntry<bool> _nextButton;

		private PhotoAttachmentPopup _photoPopup;

		private PhotoCroppingPopup _croppingPopup;

		private Uri _currentImageUri;

		internal Grid LayoutRoot;

		internal TextBlock CreateGroupTitle;

		internal RoundImageControl GroupImageControl;

		internal TextBox GroupNameTextBox;

		internal FullscreenProgressIndicator FullscreenProgressIndicator;

		private bool _contentLoaded;

		public GroupComposerPage()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBarItems();
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			if (!AttemptToDismissPopup(e))
			{
				base.OnBackKeyPress(e);
			}
		}

		protected bool AttemptToDismissPopup(CancelEventArgs e)
		{
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
			if (_photoPopup != null && _photoPopup.IsOpen)
			{
				SetLastGroupPhoto();
				e.Cancel = true;
				_photoPopup.IsOpen = false;
				return true;
			}
			if (_croppingPopup != null && _croppingPopup.IsOpen)
			{
				SetLastGroupPhoto();
				e.Cancel = true;
				_croppingPopup.IsOpen = false;
				return true;
			}
			return false;
		}

		private void SetLastGroupPhoto()
		{
			if (_currentImageUri != null)
			{
				WithViewModel((GroupComposerViewModel vm) =>
				{
					vm.ThreadImageUri = _currentImageUri;
				});
			}
		}

		private void InitializeAppBarItems()
		{
			_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
			_nextButton = _appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.Groups_AppBar_Next, NextButton_Clicked, new Uri("/Assets/AppBar/Next.png", UriKind.Relative)), new EntryState<bool>(true, StringLibrary.AppBar_Ok, OnPin_Clicked, new Uri("/Assets/AppBar/Check.png", UriKind.Relative)));
			_nextButton.IsEnabled = false;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			((Control)GroupNameTextBox).Focus();
			GroupNameTextBox.MaxLength = 100;
			WithViewModel((GroupComposerViewModel vm) =>
			{
				_nextButton.State = vm.HasExistingThread;
				CreateGroupTitle.Text = (vm.HasExistingThread ? StringLibrary.SuggestedPins_Title_PinGroup : StringLibrary.CreateGroup_Title);
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			FacebookPopup.SafeCleanUp(_photoPopup);
			FacebookPopup.SafeCleanUp(_croppingPopup);
		}

		private async void OnPin_Clicked(object sender, EventArgs e)
		{
			await WithViewModel(async (GroupComposerViewModel vm) =>
			{
				if (vm.HasExistingThread)
				{
					((Control)this).Focus();
					((UIElement)FullscreenProgressIndicator).Visibility = (Visibility)0;
				}
				((Control)GroupImageControl).IsEnabled = false;
				((Control)GroupNameTextBox).IsEnabled = false;
				_nextButton.IsEnabled = false;
				bool succeeded = await vm.ProceedToNextState();
				((Control)GroupImageControl).IsEnabled = true;
				((Control)GroupNameTextBox).IsEnabled = true;
				_nextButton.IsEnabled = true;
				if (!succeeded)
				{
					((UIElement)FullscreenProgressIndicator).Visibility = (Visibility)1;
				}
			});
		}

		private void NextButton_Clicked(object sender, EventArgs e)
		{
			WithViewModel((GroupComposerViewModel vm) =>
			{
				vm.ProceedToNextState().DoNotWait();
			});
		}

		private void GroupPhoto_OnTap(object sender, GestureEventArgs e)
		{
			if (_photoPopup == null)
			{
				_photoPopup = new PhotoAttachmentPopup();
				PhotoAttachmentPopup photoPopup = _photoPopup;
				object obj = Application.Current.Resources[(object)"MessengerBackgroundGreyBrush"];
				((Control)photoPopup).Background = (Brush)((obj is SolidColorBrush) ? obj : null);
				PhotoAttachmentPopup photoPopup2 = _photoPopup;
				object obj2 = Application.Current.Resources[(object)"MessengerBlackBrush"];
				((Control)photoPopup2).Foreground = (Brush)((obj2 is SolidColorBrush) ? obj2 : null);
				_photoPopup.Completed += OnPhotoPopupCompleted;
				_photoPopup.PopupTitle = StringLibrary.AddPhotos_Title_SetGroupPhoto;
				_photoPopup.IsLightTheme = true;
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)_photoPopup);
			}
			_photoPopup.IsOpen = true;
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
			WithViewModel((GroupComposerViewModel vm) =>
			{
				vm.ThreadImageUri = null;
			});
		}

		protected void ShowCropPopup(PhotoData image)
		{
			if (_croppingPopup == null)
			{
				_croppingPopup = new PhotoCroppingPopup();
				_croppingPopup.CropCompleted += OnCropCompleted;
				_croppingPopup.PopupDismissed += CroppingPopupOnPopupDismissed;
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)_croppingPopup);
			}
			_croppingPopup.SetPhoto(image);
			_croppingPopup.IsOpen = true;
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
		}

		private void CroppingPopupOnPopupDismissed(object sender, EventArgs eventArgs)
		{
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
		}

		private async void OnCropCompleted(object sender, BitmapSource image)
		{
			_croppingPopup.IsOpen = false;
			if (image == null)
			{
				MessageBox.Show(StringLibrary.Error_Cannot_Attach_File);
				return;
			}
			await WithViewModel(async (GroupComposerViewModel vm) =>
			{
				Uri uri = await vm.PrepareImageUriAsync(PhotoData.FromImage(image), "GroupComposerPhoto.jpg");
				if (!(uri == null))
				{
					_currentImageUri = uri;
					vm.ThreadImageUri = null;
					vm.ThreadImageUri = uri;
					_photoPopup.ShowRemovePhotoOption = true;
				}
			});
		}

		private void OnPhotoPopupCompleted(object sender, PhotoResult e)
		{
			WithViewModel((GroupComposerViewModel vm) =>
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Invalid comparison between Unknown and I4
				_photoPopup.IsOpen = false;
				((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
				if (e == null)
				{
					RemovePhoto();
				}
				else if (vm == null || (int)((TaskEventArgs)e).TaskResult != 1)
				{
					SetLastGroupPhoto();
				}
				else
				{
					ShowCropPopup(vm.PhotoDataFromPhotoResult(e));
				}
			});
		}

		private void RemovePhoto()
		{
			WithViewModel((GroupComposerViewModel vm) =>
			{
				vm.ThreadImageUri = null;
			});
			_currentImageUri = null;
			_photoPopup.ShowRemovePhotoOption = false;
		}

		private void GroupNameChanged(object sender, TextChangedEventArgs e)
		{
			if (GroupNameTextBox.Text.Trim().Length > 0)
			{
				_nextButton.IsEnabled = true;
				((FrameworkElement)GroupNameTextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
			}
			else
			{
				_nextButton.IsEnabled = false;
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/GroupComposerPage.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				CreateGroupTitle = (TextBlock)((FrameworkElement)this).FindName("CreateGroupTitle");
				GroupImageControl = (RoundImageControl)((FrameworkElement)this).FindName("GroupImageControl");
				GroupNameTextBox = (TextBox)((FrameworkElement)this).FindName("GroupNameTextBox");
				FullscreenProgressIndicator = (FullscreenProgressIndicator)((FrameworkElement)this).FindName("FullscreenProgressIndicator");
			}
		}
	}
}

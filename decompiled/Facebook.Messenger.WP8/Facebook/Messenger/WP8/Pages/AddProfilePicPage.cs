using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Photos;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.Pages;

public class AddProfilePicPage : AddProfilePicViewModelPage
{
	private const string NewUserProfilePicFileName = "NewUserProfilePic.jpg";

	private PhotoCroppingPopup _croppingPopup;

	private CameraCaptureTask _cameraCaptureTask;

	private PhotoChooserTask _photoChooserTask;

	private bool _showingPhotoChooser;

	private AppBarHelper _appBarHelper;

	internal Grid LayoutRoot;

	internal Grid ContentGrid;

	internal Border TitleTopMargin;

	internal Image Logo;

	internal TextBlock SignInTitle;

	internal Button TakePhotoButton;

	internal Image TakePhotoIconBlack;

	internal TextBlock TakePhotoText;

	internal Image AttachImageIconBlack;

	internal TextBlock AttachImageText;

	private bool _contentLoaded;

	public AddProfilePicPage()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
		InitializeAppBar();
	}

	private void InitializeAppBar()
	{
		_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
		_appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(state: false, StringLibrary.Attachments_AppBar_Skip, SkipButtonOnClick, new Uri("/Assets/AppBar/Skip.png", UriKind.Relative)));
	}

	private void SkipButtonOnClick(object sender, EventArgs e)
	{
		WithViewModel((AddProfilePicViewModel vm) =>
		{
			vm.SkipProfilePicCommand.Execute(null);
		});
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected Obj, but got Unknown
		_cameraCaptureTask = new CameraCaptureTask();
		((ChooserBase<PhotoResult>)(object)_cameraCaptureTask).Completed += PhotoTask_Completed;
		_photoChooserTask = new PhotoChooserTask();
		((ChooserBase<PhotoResult>)(object)_photoChooserTask).Completed += PhotoTask_Completed;
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		FacebookPopup.SafeCleanUp(_croppingPopup);
	}

	private void PhotoTask_Completed(object sender, PhotoResult e)
	{
		_showingPhotoChooser = false;
		if (e != null && e.ChosenPhoto != null)
		{
			WithViewModel((AddProfilePicViewModel vm) =>
			{
				ShowCropPopup(vm.PhotoDataFromPhotoResult(e));
			});
		}
	}

	private void ChoosePhoto_OnTap(object sender, GestureEventArgs e)
	{
		ShowChooser((ChooserBase<PhotoResult>)(object)_photoChooserTask);
	}

	private void TakePhoto_OnTap(object sender, GestureEventArgs e)
	{
		ShowChooser((ChooserBase<PhotoResult>)(object)_cameraCaptureTask);
	}

	public void ShowChooser(ChooserBase<PhotoResult> chooser)
	{
		if (!_showingPhotoChooser)
		{
			_showingPhotoChooser = true;
			chooser.Show();
		}
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

	private void CroppingPopupOnPopupDismissed(object sender, EventArgs e)
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
		await WithViewModel(async (AddProfilePicViewModel vm) =>
		{
			Uri uri = await vm.PrepareImageUriAsync(PhotoData.FromImage(image), "NewUserProfilePic.jpg");
			if (!(uri == null))
			{
				vm.ConfirmProfilePicCommand.Execute(uri.LocalPath);
			}
		});
	}

	private void OnBackKeyPress(object sender, CancelEventArgs e)
	{
		if (_croppingPopup != null && _croppingPopup.IsOpen)
		{
			e.Cancel = true;
			_croppingPopup.IsOpen = false;
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected Obj, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected Obj, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/AddProfilePicPage.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			ContentGrid = (Grid)((FrameworkElement)this).FindName("ContentGrid");
			TitleTopMargin = (Border)((FrameworkElement)this).FindName("TitleTopMargin");
			Logo = (Image)((FrameworkElement)this).FindName("Logo");
			SignInTitle = (TextBlock)((FrameworkElement)this).FindName("SignInTitle");
			TakePhotoButton = (Button)((FrameworkElement)this).FindName("TakePhotoButton");
			TakePhotoIconBlack = (Image)((FrameworkElement)this).FindName("TakePhotoIconBlack");
			TakePhotoText = (TextBlock)((FrameworkElement)this).FindName("TakePhotoText");
			AttachImageIconBlack = (Image)((FrameworkElement)this).FindName("AttachImageIconBlack");
			AttachImageText = (TextBlock)((FrameworkElement)this).FindName("AttachImageText");
		}
	}
}

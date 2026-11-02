using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Assets;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Messages;
using Facebook.Messenger.WP8.Controls.Photos;
using Facebook.Messenger.WP8.Controls.Stickers;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Windows.Foundation;

namespace Facebook.Messenger.WP8.Pages;

public abstract class MessageSendingPage<TViewModel> : FacebookBasePage<TViewModel> where TViewModel : MessageBaseViewModel
{
	protected enum SendButtonState
	{
		Like,
		Send,
		FocusToTextBox
	}

	protected enum DrawerSelectedState
	{
		Default,
		Selected
	}

	protected IconBarEntry<SendButtonState> _sendOrLikeButton;

	protected IconBarEntry<DrawerSelectedState> _cameraButton;

	protected IconBarEntry<DrawerSelectedState> _galleryButton;

	protected IconBarEntry<DrawerSelectedState> _stickerButton;

	protected MenuBarEntry<DrawerSelectedState> _recordVoiceMenuItem;

	protected MenuBarEntry<DrawerSelectedState> _imageSearchMenuItem;

	protected MenuBarEntry<DrawerSelectedState> _locationMenuItem;

	private bool _showingTask;

	private PhotoDrawingPopup _photoDrawingPopup;

	private readonly List<ComposerDrawer> _drawers = new List<ComposerDrawer>();

	private ComposerDrawer _viewfinderDrawer;

	private ComposerDrawer _galleryDrawer;

	private ComposerDrawer _stickerDrawer;

	private ComposerDrawer _voiceClipDrawer;

	private ComposerDrawer _imageSearchDrawer;

	private ComposerDrawer _locationDrawer;

	protected abstract Grid PhotoPopupParent { get; }

	protected abstract ImageUploadAttachmentControl ImageUploadControl { get; }

	protected MessageSendingPage()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected Obj, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected Obj, but got Unknown
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
	}

	private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
	{
		UIElement layoutRoot = GetLayoutRoot();
		if (layoutRoot != null)
		{
			layoutRoot.RemoveHandler(UIElement.TapEvent, (Delegate)new EventHandler<GestureEventArgs>(DetectDrawerDeselect));
		}
		FacebookPopup.SafeCleanUp(_photoDrawingPopup);
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		UIElement layoutRoot = GetLayoutRoot();
		if (layoutRoot != null)
		{
			layoutRoot.AddHandler(UIElement.TapEvent, (Delegate)new EventHandler<GestureEventArgs>(DetectDrawerDeselect), true);
		}
	}

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		if (_galleryDrawer is GalleryDrawer galleryDrawer)
		{
			galleryDrawer.RefreshGalleryDrawer = true;
		}
		ComposerDrawer composerDrawer = _drawers.FirstOrDefault((ComposerDrawer d) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)((UIElement)d).Visibility == 0;
		});
		if (composerDrawer != null)
		{
			ShowDrawer(composerDrawer);
		}
	}

	protected override void OnNavigatedFrom(NavigationEventArgs e)
	{
		WithViewModel((TViewModel vm) =>
		{
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				_drawers.ForEach((ComposerDrawer d) =>
				{
					CleanupDrawer(d, vm);
				});
			}));
		});
		base.OnNavigatedFrom(e);
	}

	protected void InitializeAppBar()
	{
		AppBarHelper appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
		_sendOrLikeButton = appBarHelper.CreateDebounceBarButtonEntry<SendButtonState>(new EntryState<SendButtonState>(SendButtonState.Like, StringLibrary.Conversation_AppBar_Like, OnLikeClick, AssetConstants.LikeIcon), new EntryState<SendButtonState>(SendButtonState.Send, StringLibrary.Conversation_AppBar_Send, OnSendMessageClick, AssetConstants.SendMessageIcon), new EntryState<SendButtonState>(SendButtonState.FocusToTextBox, StringLibrary.Conversation_AppBar_Text, OnTransferFocusToComposerClick, AssetConstants.TextIcon));
		_cameraButton = appBarHelper.CreateBarButtonEntry<DrawerSelectedState>(new EntryState<DrawerSelectedState>(DrawerSelectedState.Default, StringLibrary.Conversation_AppBar_Camera, (object e, EventArgs s) =>
		{
			SetViewfinderDrawerVisibility(visible: true);
		}, AssetConstants.CameraUriIcon), new EntryState<DrawerSelectedState>(DrawerSelectedState.Selected, StringLibrary.Conversation_AppBar_Camera, null, AssetConstants.CameraSelectedUriIcon));
		_galleryButton = appBarHelper.CreateBarButtonEntry<DrawerSelectedState>(new EntryState<DrawerSelectedState>(DrawerSelectedState.Default, StringLibrary.Conversation_AppBar_AttachPhoto, (object e, EventArgs s) =>
		{
			SetGalleryDrawerVisibility(visible: true);
		}, AssetConstants.GalleryUriIcon), new EntryState<DrawerSelectedState>(DrawerSelectedState.Selected, StringLibrary.Conversation_AppBar_AttachPhoto, (object s, EventArgs e) =>
		{
			ScrollGalleryToStart();
		}, AssetConstants.GallerySelectedUriIcon));
		_stickerButton = appBarHelper.CreateBarButtonEntry<DrawerSelectedState>(new EntryState<DrawerSelectedState>(DrawerSelectedState.Default, StringLibrary.Messages_AppBar_Send_Sticker, (object e, EventArgs s) =>
		{
			SetStickerDrawerVisibility(visible: true);
		}, AssetConstants.StickerUriIcon), new EntryState<DrawerSelectedState>(DrawerSelectedState.Selected, StringLibrary.Messages_AppBar_Send_Sticker, null, AssetConstants.StickerSelectedUriIcon));
		_recordVoiceMenuItem = appBarHelper.CreateMenuEntry<DrawerSelectedState>(new EntryState<DrawerSelectedState>(DrawerSelectedState.Default, StringLibrary.Voice_Clip_Menu_Item, (object s, EventArgs e) =>
		{
			SetVoiceClipDrawerVisibility(visible: true);
		}), new EntryState<DrawerSelectedState>(DrawerSelectedState.Selected, StringLibrary.Voice_Clip_Menu_Item, null, null, enabled: false));
		_imageSearchMenuItem = appBarHelper.CreateMenuEntry<DrawerSelectedState>(new EntryState<DrawerSelectedState>(DrawerSelectedState.Default, StringLibrary.Image_Search_Menu_Item, (object s, EventArgs e) =>
		{
			SetImageSearchDrawerVisibility(visible: true);
		}), new EntryState<DrawerSelectedState>(DrawerSelectedState.Selected, StringLibrary.Image_Search_Menu_Item, null, null, enabled: false));
		_imageSearchMenuItem.IsVisible = DebuggingUtility.IsTestBuildOrEmployee;
		_locationMenuItem = appBarHelper.CreateMenuEntry<DrawerSelectedState>(new EntryState<DrawerSelectedState>(DrawerSelectedState.Default, StringLibrary.Location_Menu_Item, (object s, EventArgs e) =>
		{
			SetLocationDrawerVisibility(visible: true);
		}), new EntryState<DrawerSelectedState>(DrawerSelectedState.Selected, StringLibrary.Location_Menu_Item, null, null, enabled: false));
		_locationMenuItem.IsVisible = DebuggingUtility.IsTestBuildOrEmployee;
		InitializeCustomApplicationBarItems(appBarHelper);
	}

	protected abstract void InitializeCustomApplicationBarItems(AppBarHelper appBarHelper);

	protected abstract void OnSendMessageClick(object sender, EventArgs e);

	protected abstract void OnSendMessageEnter(object sender, EventArgs e);

	protected abstract void OnLikeClick(object sender, EventArgs e);

	protected abstract void OnTransferFocusToComposerClick(object sender, EventArgs e);

	protected abstract DrawerContainer GetDrawerContainer();

	protected abstract FrameworkElement GetDrawerPaddingElement();

	protected bool IsAnyDrawerVisible()
	{
		return GetVisibleDrawerType() != DrawerType.None;
	}

	protected DrawerType GetVisibleDrawerType()
	{
		return _drawers.FirstOrDefault((ComposerDrawer d) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)((UIElement)d).Visibility == 0;
		})?.DrawerType ?? DrawerType.None;
	}

	protected void HideAllDrawers()
	{
		_drawers.ForEach(HideDrawer);
		DrawerStateChanged();
	}

	protected virtual void DrawerStateChanged()
	{
		if (IsAnyDrawerVisible())
		{
			SystemTrayManager.AddOperationToStack(((object)this).GetType());
		}
		else
		{
			SystemTrayManager.RemoveOperationFromStack(((object)this).GetType());
		}
	}

	private void DetectDrawerDeselect(object sender, GestureEventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!IsAnyDrawerVisible())
		{
			return;
		}
		DrawerContainer drawerContainer = GetDrawerContainer();
		Point relativePoint = drawerContainer.GetRelativePoint(e);
		Rect val = new Rect(0.0, 0.0, drawerContainer.DrawerWidth, drawerContainer.DrawerHeight);
		if (!val.Contains(relativePoint))
		{
			object focusedElement = FocusManager.GetFocusedElement();
			if (!(focusedElement is CheckBox))
			{
				HideAllDrawers();
			}
		}
	}

	private void SetDrawerVisibility(ref ComposerDrawer drawer, Func<ComposerDrawer> createDrawer, bool visible)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (drawer == null)
		{
			if (!visible)
			{
				return;
			}
			drawer = createDrawer();
			HideDrawer(drawer);
			LoadPreferences(drawer);
			FrameworkElement val = (FrameworkElement)(object)drawer;
			val.Height = InputHelper.GetKeyboardHeight();
			GetDrawerContainer().AddDrawer(val);
			_drawers.Add(drawer);
		}
		if (VisibilityUtil.FromBool(visible) == ((UIElement)drawer).Visibility)
		{
			return;
		}
		if (visible)
		{
			HideAllDrawers();
			ShowDrawer(drawer);
			((Control)drawer).Focus();
			WithViewModel((TViewModel vm) =>
			{
				vm.PlayDrawerSelectedSound();
			});
		}
		else
		{
			HideDrawer(drawer);
			WithViewModel((TViewModel vm) =>
			{
				vm.PlayTextDrawerSelectedSound();
			});
		}
		DrawerStateChanged();
	}

	private void ShowDrawer(ComposerDrawer drawer)
	{
		drawer.Show();
		GetDrawerPaddingElement().Height = Math.Min(((FrameworkElement)drawer).Height, InputHelper.GetKeyboardHeight());
	}

	private void HideDrawer(ComposerDrawer drawer)
	{
		drawer.Hide();
		GetDrawerPaddingElement().Height = 0.0;
	}

	private void CleanupDrawer(ComposerDrawer drawer, MessageBaseViewModel vm)
	{
		SavePreferences(drawer, vm);
		drawer.Cleanup();
	}

	private void LoadPreferences(IPreferences item)
	{
		WithViewModel((TViewModel vm) =>
		{
			IDictionary<string, string> preferences = vm.LoadPreferences(item.GetPreferenceKeys());
			item.RestorePreferences(preferences);
		});
	}

	private void SavePreferences(IPreferences item, MessageBaseViewModel vm)
	{
		IDictionary<string, string> preferences = item.GetPreferences();
		vm.SavePreferences(preferences);
	}

	private ComposerDrawer CreateViewfinderDrawer()
	{
		ViewfinderDrawer viewfinderDrawer = new ViewfinderDrawer();
		viewfinderDrawer.CameraButtonClicked += ViewfinderCameraButtonClicked;
		viewfinderDrawer.SendClicked += ViewfinderSendClicked;
		viewfinderDrawer.VideoRecorded += ViewfinderVideoRecorded;
		viewfinderDrawer.UserGestured += ViewfinderOnUserGestured;
		return viewfinderDrawer;
	}

	private void ViewfinderOnUserGestured(object sender, UserGesture userGesture)
	{
		switch (userGesture)
		{
		case UserGesture.CameraButtonDown:
			WithViewModel((TViewModel vm) =>
			{
				vm.PlayCameraDownSound();
			});
			break;
		case UserGesture.CameraButtonUp:
			WithViewModel((TViewModel vm) =>
			{
				vm.PlayCameraUpSound();
			});
			break;
		case UserGesture.CancelRecord:
			WithViewModel((TViewModel vm) =>
			{
				vm.PlayCancelRecordSound();
			});
			break;
		}
	}

	private void ViewfinderCameraButtonClicked(object sender, EventArgs eventArgs)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected Obj, but got Unknown
		HideAllDrawers();
		if (!_showingTask)
		{
			_showingTask = true;
			CameraCaptureTask val = new CameraCaptureTask();
			((ChooserBase<PhotoResult>)(object)val).Completed += (object o, PhotoResult result) =>
			{
				AttachPhoto(result);
			};
			((ChooserBase<PhotoResult>)(object)val).Show();
		}
	}

	protected virtual async void AttachPhoto(PhotoResult e)
	{
		_showingTask = false;
		await WithViewModel(async (TViewModel vm) =>
		{
			await vm.SetAttachment(e);
			ScrollViewer viewer = ImageUploadControl.ScrollViewer;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				viewer.ScrollToHorizontalOffset(viewer.ScrollableWidth);
			}));
		});
	}

	private void ViewfinderSendClicked(object sender, ImageResult imageResult)
	{
		SendImage(imageResult);
	}

	private void SendImage(ImageResult imageResult)
	{
		WithViewModel((TViewModel vm) =>
		{
			vm.SendImageCommand.Execute(imageResult);
		});
	}

	private void ViewfinderVideoRecorded(object sender, VideoResult e)
	{
		WithViewModel((TViewModel vm) =>
		{
			vm.SendVideoCommand.Execute(e);
		});
	}

	protected void ToggleViewfinderDrawerVisibility()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		bool flag = _viewfinderDrawer != null && (int)((UIElement)_viewfinderDrawer).Visibility == 0;
		SetViewfinderDrawerVisibility(!flag);
	}

	protected void SetViewfinderDrawerVisibility(bool visible)
	{
		SetDrawerVisibility(ref _viewfinderDrawer, CreateViewfinderDrawer, visible);
	}

	private ComposerDrawer CreateGalleryDrawer()
	{
		GalleryDrawer galleryDrawer = new GalleryDrawer();
		galleryDrawer.SendPhoto += DrawerOnSendPhoto;
		galleryDrawer.EditPhoto += DrawerOnEditPhoto;
		galleryDrawer.GalleryButtonClicked += DrawerGalleryButtonClicked;
		galleryDrawer.GetPhotos().DoNotWait();
		return galleryDrawer;
	}

	private void DrawerOnEditPhoto(object sender, ImageResult imageResult)
	{
		if (_photoDrawingPopup == null)
		{
			_photoDrawingPopup = new PhotoDrawingPopup(((PhoneApplicationPage)this).ApplicationBar);
			_photoDrawingPopup.PhotoEditCompleted += OnPhotoDrawingComplete;
			_photoDrawingPopup.PopupDismissed += OnPhotoDrawingDismissed;
			UIElement layoutRoot = GetLayoutRoot();
			Grid val = (Grid)(object)((layoutRoot is Grid) ? layoutRoot : null);
			if (val == null)
			{
				return;
			}
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)val).Children).Insert(0, (UIElement)(object)_photoDrawingPopup);
		}
		LoadPreferences(_photoDrawingPopup);
		_photoDrawingPopup.SetPhoto(imageResult.Image);
		_photoDrawingPopup.IsOpen = true;
		((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
	}

	private void OnPhotoDrawingComplete(object sender, BitmapSource image)
	{
		_photoDrawingPopup.IsOpen = false;
		ImageResult imageResult = new ImageResult(MediaSource.Gallery, PhotoData.FromImage(image));
		SendImage(imageResult);
	}

	private void OnPhotoDrawingDismissed(object sender, EventArgs e)
	{
		WithViewModel((TViewModel vm) =>
		{
			SavePreferences(_photoDrawingPopup, vm);
		});
		((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
	}

	private void DrawerGalleryButtonClicked(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected Obj, but got Unknown
		HideAllDrawers();
		if (!_showingTask)
		{
			_showingTask = true;
			PhotoChooserTask val = new PhotoChooserTask();
			((ChooserBase<PhotoResult>)(object)val).Completed += (object o, PhotoResult result) =>
			{
				AttachPhoto(result);
			};
			((ChooserBase<PhotoResult>)(object)val).Show();
		}
	}

	private void DrawerOnSendPhoto(object sender, ImageResult imageResult)
	{
		SendImage(imageResult);
	}

	protected void ScrollGalleryToStart()
	{
		if (_galleryDrawer is GalleryDrawer galleryDrawer)
		{
			galleryDrawer.ScrollToStart();
		}
	}

	protected void ToggleGalleryDrawerVisibility()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		bool flag = _galleryDrawer != null && (int)((UIElement)_galleryDrawer).Visibility == 0;
		SetGalleryDrawerVisibility(!flag);
	}

	protected void SetGalleryDrawerVisibility(bool visible)
	{
		SetDrawerVisibility(ref _galleryDrawer, CreateGalleryDrawer, visible);
	}

	private ComposerDrawer CreateStickerDrawer()
	{
		StickerDrawer stickerDrawer = new StickerDrawer();
		((FrameworkElement)stickerDrawer).DataContext = ViewModel.StickerDrawerViewModel;
		return stickerDrawer;
	}

	protected void ToggleStickerDrawerVisibility()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		bool flag = _stickerDrawer != null && (int)((UIElement)_stickerDrawer).Visibility == 0;
		SetStickerDrawerVisibility(!flag);
	}

	protected void SetStickerDrawerVisibility(bool visible)
	{
		SetDrawerVisibility(ref _stickerDrawer, CreateStickerDrawer, visible);
	}

	private ComposerDrawer CreateVoiceClipDrawer()
	{
		VoiceClipDrawer voiceClipDrawer = new VoiceClipDrawer();
		voiceClipDrawer.OnVoiceClipSend = (Action<VoiceRecordingHelper.VoiceRecordingResult>)Delegate.Combine(voiceClipDrawer.OnVoiceClipSend, new Action<VoiceRecordingHelper.VoiceRecordingResult>(SendVoiceClip));
		voiceClipDrawer.OnVoiceClipCancel = (Action)Delegate.Combine(voiceClipDrawer.OnVoiceClipCancel, new Action(PlayVoiceClipCancelSound));
		return voiceClipDrawer;
	}

	private async void SendVoiceClip(VoiceRecordingHelper.VoiceRecordingResult result)
	{
		if (result != null && result.File != null)
		{
			long unixTimeMs = result.Time.ToUnixTimeMilliseconds();
			string formattedName = MailAttachment.GenerateVoiceClipFileName(unixTimeMs, result.DurationMs);
			await result.File.RenameAsync(formattedName);
			WithViewModel((TViewModel vm) =>
			{
				vm.PlayVoiceClipSentSound();
				vm.SendVoiceClip.Execute(result);
			});
		}
	}

	private void PlayVoiceClipCancelSound()
	{
		WithViewModel((TViewModel vm) =>
		{
			vm.PlayVoiceClipCancelledSound();
		});
	}

	protected void SetVoiceClipDrawerVisibility(bool visible)
	{
		SetDrawerVisibility(ref _voiceClipDrawer, CreateVoiceClipDrawer, visible);
	}

	private ComposerDrawer CreateImageSearchDrawer()
	{
		ImageSearchDrawer imageSearchDrawer = new ImageSearchDrawer();
		imageSearchDrawer.SendPhoto += DrawerOnSendPhoto;
		imageSearchDrawer.EditPhoto += DrawerOnEditPhoto;
		return imageSearchDrawer;
	}

	protected void SetImageSearchDrawerVisibility(bool visible)
	{
		SetDrawerVisibility(ref _imageSearchDrawer, CreateImageSearchDrawer, visible);
	}

	private ComposerDrawer CreateLocationDrawer()
	{
		LocationDrawer locationDrawer = new LocationDrawer();
		locationDrawer.SendLocation += DrawerOnSendPhoto;
		return locationDrawer;
	}

	protected void SetLocationDrawerVisibility(bool visible)
	{
		SetDrawerVisibility(ref _locationDrawer, CreateLocationDrawer, visible);
	}

	protected abstract UIElement GetLayoutRoot();

	protected override void OnKeyboardVisibilityChanged(bool showing, Rect occludedRect)
	{
		if (_photoDrawingPopup == null || !_photoDrawingPopup.IsOpen)
		{
			int rootFrameTransform = (showing ? (-1 * (int)occludedRect.Height) : 0);
			SetRootFrameTransform(rootFrameTransform);
		}
	}

	protected override void OnBackKeyPress(CancelEventArgs e)
	{
		if (!AttemptToDismissPopups(e))
		{
			base.OnBackKeyPress(e);
		}
	}

	protected virtual bool AttemptToDismissPopups(CancelEventArgs e)
	{
		if (_photoDrawingPopup != null && _photoDrawingPopup.IsOpen)
		{
			e.Cancel = true;
			_photoDrawingPopup.IsOpen = false;
			return true;
		}
		if (IsAnyDrawerVisible())
		{
			e.Cancel = true;
			HideAllDrawers();
			return true;
		}
		return false;
	}
}

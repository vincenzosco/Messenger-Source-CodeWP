using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Details;
using Facebook.Messenger.WP8.Controls.Photos;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.Pages
{
	public class GroupDetailsPage : GroupDetailsViewModelPage
	{
		private const string NewThreadPicFileName = "NewThreadPic.jpg";

		private readonly Lazy<PhotoCroppingPopup> _croppingPopup;

		private readonly Lazy<PhotoAttachmentPopup> _photoAttachmentPopup;

		private IconBarEntry<MuteButtonState> _muteButton;

		private MenuBarEntry<bool> _clearGroupNameMenuItem;

		private MenuBarEntry<bool> _clearGroupPhotoMenuItem;

		private IconBarEntry<bool> _addPersonButton;

		private IconBarEntry<bool> _renameButton;

		private IconBarEntry<bool> _changePhotoButton;

		private MenuBarEntry<bool> _leaveGroupMenuItem;

		internal Grid LayoutRoot;

		internal FacebookListView ContactList;

		internal StackPanel RootPanel;

		internal Button FaceMashButton;

		internal FaceMash FaceMash;

		internal Button ThreadNameButton;

		internal FBMessengerProgressBar BusyIndicator;

		private bool _contentLoaded;

		public GroupDetailsPage()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
			_croppingPopup = new Lazy<PhotoCroppingPopup>(CreateCroppingPopup);
			_photoAttachmentPopup = new Lazy<PhotoAttachmentPopup>(CreatePhotoAttachmentPopup);
			InitializeAppBar();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
				if (vm.Thread == null)
				{
					vm.FetchThreadCommand.Execute(null);
				}
				UpdateView(vm);
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
			});
			if (_croppingPopup.IsValueCreated)
			{
				FacebookPopup.SafeCleanUp(_croppingPopup.Value);
			}
			if (_photoAttachmentPopup.IsValueCreated)
			{
				FacebookPopup.SafeCleanUp(_photoAttachmentPopup.Value);
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				switch (e.PropertyName)
				{
				case "Thread":
					UpdateView(vm);
					break;
				case "IsMuteInProgress":
					_muteButton.IsEnabled = !vm.IsMuteInProgress;
					break;
				}
			});
		}

		private void InitializeAppBar()
		{
			AppBarHelper appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
			_addPersonButton = appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.People_AppBar_AddContact, OnAddPeopleAppBarButtonClick, new Uri("/Assets/AppBar/AddPersonToGroup.png", UriKind.Relative)));
			_muteButton = appBarHelper.CreateBarButtonEntry<MuteButtonState>(new EntryState<MuteButtonState>(MuteButtonState.Mute, StringLibrary.Mute_Conversation_Menu_Item, OnMuteButtonClicked, new Uri("/Assets/AppBar/Mute.png", UriKind.Relative)), new EntryState<MuteButtonState>(MuteButtonState.Unmute, StringLibrary.UnMute_Conversation_Menu_Item, OnUnmuteButtonClicked, new Uri("/Assets/AppBar/Unmute.png", UriKind.Relative)));
			_renameButton = appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.Conversation_MenuItem_Rename, ChangeGroupNameAppBarButtonOnClick, new Uri("/Assets/AppBar/Edit.png", UriKind.Relative)));
			_changePhotoButton = appBarHelper.CreateBarButtonEntry<bool>(new EntryState<bool>(false, StringLibrary.Conversation_AppBar_ChangePhoto, ChangeGroupPhotoApBarButtonOnClick, new Uri("/Assets/AppBar/SetPhoto.png", UriKind.Relative)));
			_clearGroupNameMenuItem = appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(false, StringLibrary.GroupDetailsPage_AppBar_RemoveGroupName, OnClearGroupNameAppBarMenutItemClicked));
			_clearGroupPhotoMenuItem = appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(false, StringLibrary.GroupDetailsPage_AppBar_RemoveGroupPhoto, OnRemoveGroupPhotoMenuItemClicked));
			_leaveGroupMenuItem = appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(false, StringLibrary.GroupDetailsPage_AppBar_LeaveGroup, OnLeaveGroupMenuItemClicked));
			_clearGroupNameMenuItem.IsVisible = false;
			_clearGroupPhotoMenuItem.IsVisible = false;
		}

		private void ChangeGroupPhotoApBarButtonOnClick(object sender, EventArgs e)
		{
			ChangeGroupPhoto();
		}

		private void ChangeGroupNameAppBarButtonOnClick(object sender, EventArgs e)
		{
			ChangeGroupName();
		}

		private void OnClearGroupNameAppBarMenutItemClicked(object sender, EventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				vm.ClearGroupNameCommand.Execute(null);
			});
		}

		private void OnRemoveGroupPhotoMenuItemClicked(object sender, EventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				vm.ClearThreadPictureCommand.Execute(null);
			});
		}

		private void OnLeaveGroupMenuItemClicked(object sender, EventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			MessageBoxResult val = MessageBox.Show(StringLibrary.MessageBox_Description_LeaveThread, StringLibrary.MessageBox_Label_LeaveThread, (MessageBoxButton)1);
			if ((int)val == 1)
			{
				WithViewModel((GroupDetailsViewModel vm) =>
				{
					vm.LeaveThreadCommand.Execute(null);
				});
			}
		}

		private void UpdateView(GroupDetailsViewModel vm)
		{
			FaceMash.SetFaceMash();
			((ContentControl)ThreadNameButton).Content = ThreadNameUtils.GetThreadNameOrPlaceholder(vm.Thread, vm.Context);
			if (vm.Thread != null)
			{
				_muteButton.State = (vm.Thread.IsMuted ? MuteButtonState.Unmute : MuteButtonState.Mute);
				_muteButton.ResetCurrentState();
				_photoAttachmentPopup.Value.ShowRemovePhotoOption = vm.Thread.HasPicture();
				_clearGroupNameMenuItem.IsVisible = vm.Thread.HasName();
				_clearGroupPhotoMenuItem.IsVisible = vm.Thread.HasPicture();
			}
			SetMenuItemsEnabled(vm);
		}

		private void SetMenuItemsEnabled(GroupDetailsViewModel vm)
		{
			bool flag = vm.Thread != null;
			_muteButton.IsEnabled = flag && !vm.IsMuteInProgress;
			_addPersonButton.IsEnabled = flag;
			_renameButton.IsEnabled = flag;
			_changePhotoButton.IsEnabled = flag;
			_leaveGroupMenuItem.IsEnabled = flag;
		}

		private void ThreadNameButton_OnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			ChangeGroupName();
		}

		private void ThreadImage_OnTapped(object sender, GestureEventArgs e)
		{
			ChangeGroupPhoto();
		}

		private void ChangeGroupName()
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				vm.NameConversationPageCommand.Execute(null);
			});
		}

		private void ChangeGroupPhoto()
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				if (!vm.IsLoading)
				{
					((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
					_photoAttachmentPopup.Value.IsOpen = true;
				}
			});
		}

		private void OnAddPeopleAppBarButtonClick(object sender, EventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				vm.AddPeopleCommand.Execute(null);
			});
		}

		private void OnPhotoPopupCompleted(object sender, PhotoResult e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Invalid comparison between Unknown and I4
				_photoAttachmentPopup.Value.IsOpen = false;
				((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
				if (e == null)
				{
					vm.ClearThreadPictureCommand.Execute(null);
				}
				else if ((int)((TaskEventArgs)e).TaskResult == 1)
				{
					ShowCropPopup(vm.PhotoDataFromPhotoResult(e));
				}
			});
		}

		private PhotoCroppingPopup CreateCroppingPopup()
		{
			PhotoCroppingPopup photoCroppingPopup = new PhotoCroppingPopup();
			photoCroppingPopup.CropCompleted += OnCropCompleted;
			photoCroppingPopup.PopupDismissed += CroppingPopupOnPopupDismissed;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)photoCroppingPopup);
			return photoCroppingPopup;
		}

		private PhotoAttachmentPopup CreatePhotoAttachmentPopup()
		{
			PhotoAttachmentPopup photoAttachmentPopup = new PhotoAttachmentPopup();
			photoAttachmentPopup.Completed += OnPhotoPopupCompleted;
			photoAttachmentPopup.PopupDismissed += PhotoAttachmentPopupOnDismissed;
			photoAttachmentPopup.PopupTitle = StringLibrary.AddPhotos_Title_SetGroupPhoto;
			photoAttachmentPopup.IsLightTheme = true;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)photoAttachmentPopup);
			return photoAttachmentPopup;
		}

		protected void ShowCropPopup(PhotoData image)
		{
			_croppingPopup.Value.SetPhoto(image);
			_croppingPopup.Value.IsOpen = true;
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
		}

		private void PhotoAttachmentPopupOnDismissed(object sender, EventArgs e)
		{
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
		}

		private void CroppingPopupOnPopupDismissed(object sender, EventArgs e)
		{
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
		}

		private async void OnCropCompleted(object sender, BitmapSource image)
		{
			_croppingPopup.Value.IsOpen = false;
			if (image == null)
			{
				MessageBox.Show(StringLibrary.Error_Cannot_Attach_File);
				return;
			}
			await WithViewModel(async (GroupDetailsViewModel vm) =>
			{
				Uri uri = await vm.PrepareImageUriAsync(PhotoData.FromImage(image), "NewThreadPic.jpg");
				if (!(uri == null))
				{
					vm.SetThreadPictureCommand.Execute(uri.LocalPath);
				}
			});
		}

		private void OnBackKeyPress(object sender, CancelEventArgs e)
		{
			if (_croppingPopup.IsValueCreated && _croppingPopup.Value.IsOpen)
			{
				e.Cancel = true;
				_croppingPopup.Value.IsOpen = false;
			}
			else if (_photoAttachmentPopup.IsValueCreated && _photoAttachmentPopup.Value.IsOpen)
			{
				e.Cancel = true;
				_photoAttachmentPopup.Value.IsOpen = false;
			}
		}

		private void OnMuteButtonClicked(object sender, EventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				if (vm.Thread != null)
				{
					vm.MuteCommand.Execute(null);
				}
			});
		}

		private void OnUnmuteButtonClicked(object sender, EventArgs e)
		{
			WithViewModel((GroupDetailsViewModel vm) =>
			{
				if (vm.Thread != null)
				{
					vm.UnmuteCommand.Execute(null);
				}
			});
		}

		private void ContextMenu_OnUnloaded(object sender, RoutedEventArgs e)
		{
			ContextMenu contextMenu = sender as ContextMenu;
			if (contextMenu != null)
			{
				((DependencyObject)contextMenu).ClearValue(FrameworkElement.DataContextProperty);
			}
		}

		private void OnRemoveMemberClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			FacebookUser user = ((FrameworkElement)menuItem).DataContext as FacebookUser;
			if (user != null)
			{
				WithViewModel((GroupDetailsViewModel vm) =>
				{
					vm.RemoveUserCommand.Execute(user);
				});
			}
		}

		private void OnPersonTapped(object sender, GestureEventArgs e)
		{
			ContactUserControl contactUserControl = sender as ContactUserControl;
			if (contactUserControl == null)
			{
				return;
			}
			FacebookUser user = ((FrameworkElement)contactUserControl).DataContext as FacebookUser;
			if (user != null)
			{
				WithViewModel((GroupDetailsViewModel vm) =>
				{
					vm.NavigateToCanonicalThreadCommand.Execute(user);
				});
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/GroupDetailsPage.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				ContactList = (FacebookListView)((FrameworkElement)this).FindName("ContactList");
				RootPanel = (StackPanel)((FrameworkElement)this).FindName("RootPanel");
				FaceMashButton = (Button)((FrameworkElement)this).FindName("FaceMashButton");
				FaceMash = (FaceMash)((FrameworkElement)this).FindName("FaceMash");
				ThreadNameButton = (Button)((FrameworkElement)this).FindName("ThreadNameButton");
				BusyIndicator = (FBMessengerProgressBar)((FrameworkElement)this).FindName("BusyIndicator");
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Contacts;
using Facebook.Messenger.WP8.Controls.MainPanorama;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Facebook.Services.MessageSender;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Pages;

public class MessengerMainPage : MainViewModelPage
{
	private enum PageState
	{
		Normal,
		SearchingContacts
	}

	private PageState _mainPageState;

	private ApplicationBarIconButton _refreshThreadsBarButton;

	private ApplicationBarIconButton _composeNewThreadBarButton;

	private ApplicationBarIconButton _addGroupBarButton;

	private ApplicationBarIconButton _createGroupBarButton;

	private ApplicationBarIconButton _searchFriendsBarButton;

	private ApplicationBarIconButton _chatSwitchBarButton;

	private ApplicationBarIconButton _addContactButton;

	private bool _isDisplayingPromptToLeaveWithQueuedMessages;

	private int? _lastVisiblePanoramaIndex;

	private SwitchChatPopup _chatTogglePopup;

	private ContactCardPopup _contactCardPopup;

	private FoundContactsPopup _foundContactPopup;

	private SearchOverlay _searchOverlay;

	private PinnedThreadsControl _pinnedThreadsControl;

	private ContactListUserControl _friendList;

	private bool _didFirstLoadOccur;

	private Stack<FacebookPopup> _popupStack;

	internal Grid LayoutRoot;

	internal Panorama MainPanorama;

	internal PanoramaItem RecentPanorama;

	internal PanoramaItem GroupsPanorama;

	internal PanoramaItem FriendsPanorama;

	internal PanoramaItem ActivePanorama;

	private bool _contentLoaded;

	public MessengerMainPage()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected Obj, but got Unknown
		_mainPageState = PageState.Normal;
		InitializeComponent();
		_popupStack = new Stack<FacebookPopup>();
		SetupApplicationBarItems();
		SetupApplicationBarMenuItems();
		((FrameworkElement)this).Unloaded += OnUnloaded;
	}

	private void SetupApplicationBarMenuItems()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected Obj, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected Obj, but got Unknown
		ApplicationBarMenuItem val = new ApplicationBarMenuItem(StringLibrary.MainPage_AppMenu_Settings);
		val.Click += (object sender, EventArgs args) =>
		{
			Navigator.Navigate(FacebookUri.SettingsUri);
		};
		((PhoneApplicationPage)this).ApplicationBar.MenuItems.Add(val);
		ApplicationBarMenuItem val2 = new ApplicationBarMenuItem(StringLibrary.Sticker_Store_Menu_Item);
		val2.Click += (object sender, EventArgs args) =>
		{
			Navigator.Navigate(FacebookUri.StickerStoreUri);
		};
		((PhoneApplicationPage)this).ApplicationBar.MenuItems.Add(val2);
		ApplicationBarMenuItem val3 = new ApplicationBarMenuItem(StringLibrary.FlyTrap_MenuItem_ReportProblem);
		val3.Click += ReportProblemBarButtonOnClick;
		((PhoneApplicationPage)this).ApplicationBar.MenuItems.Add(val3);
		if (DebuggingUtility.IsTestBuildOrEmployee)
		{
			SetupInternalOnlyApplicationBarMenuItems();
		}
	}

	private void SetupInternalOnlyApplicationBarMenuItems()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected Obj, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected Obj, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected Obj, but got Unknown
		ApplicationBarMenuItem val = new ApplicationBarMenuItem(InternalStringsLibrary.Settings_Logout);
		val.Click += OnLogoutClick;
		((PhoneApplicationPage)this).ApplicationBar.MenuItems.Add(val);
		ApplicationBarMenuItem val2 = new ApplicationBarMenuItem(InternalStringsLibrary.Settings_Crash);
		val2.Click += (object sender, EventArgs args) =>
		{
			throw new ImARebelThatLikesToCauseCrashesException();
		};
		((PhoneApplicationPage)this).ApplicationBar.MenuItems.Add(val2);
		ApplicationBarMenuItem val3 = new ApplicationBarMenuItem(InternalStringsLibrary.Settings_Internal_Settings);
		val3.Click += (object sender, EventArgs args) =>
		{
			Navigator.Navigate(FacebookUri.GetAppUri(UriId.InternalSettings));
		};
		((PhoneApplicationPage)this).ApplicationBar.MenuItems.Add(val3);
	}

	private void OnLogoutClick(object sender, EventArgs eventArgs)
	{
		FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
		instance.Logout();
	}

	private void SetupApplicationBarItems()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected Obj, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected Obj, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected Obj, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected Obj, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected Obj, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected Obj, but got Unknown
		ApplicationBarIconButton val = new ApplicationBarIconButton();
		val.IconUri = new Uri("/Assets/AppBar/Refresh.png", UriKind.Relative);
		val.Text = StringLibrary.Threads_AppBar_Refresh;
		_refreshThreadsBarButton = val;
		ApplicationBarIconButton val2 = new ApplicationBarIconButton();
		val2.IconUri = new Uri("/Assets/AppBar/Add.png", UriKind.Relative);
		val2.Text = StringLibrary.Threads_AppBar_Compose;
		_composeNewThreadBarButton = val2;
		ApplicationBarIconButton val3 = new ApplicationBarIconButton();
		val3.IconUri = new Uri("/Assets/AppBar/Pin.png", UriKind.Relative);
		val3.Text = StringLibrary.Groups_AppBar_Add;
		_addGroupBarButton = val3;
		ApplicationBarIconButton val4 = new ApplicationBarIconButton();
		val4.IconUri = new Uri("/Assets/AppBar/CreateGroup.png", UriKind.Relative);
		val4.Text = StringLibrary.Groups_AppBar_Create;
		_createGroupBarButton = val4;
		ApplicationBarIconButton val5 = new ApplicationBarIconButton();
		val5.IconUri = new Uri("/Assets/AppBar/Search.png", UriKind.Relative);
		val5.Text = StringLibrary.People_AppBar_Search;
		_searchFriendsBarButton = val5;
		ApplicationBarIconButton val6 = new ApplicationBarIconButton();
		val6.IconUri = new Uri("/Assets/AppBar/ChatSwitch.png", UriKind.Relative);
		val6.Text = StringLibrary.Active_AppBar_Chat;
		_chatSwitchBarButton = val6;
		ApplicationBarIconButton val7 = new ApplicationBarIconButton();
		val7.IconUri = new Uri("/Assets/AppBar/AddContact.png", UriKind.Relative);
		val7.Text = StringLibrary.Friends_AppBar_AddContact;
		_addContactButton = val7;
		_refreshThreadsBarButton.Click += RefreshThreadsOnClick;
		_composeNewThreadBarButton.Click += ComposeNewThreadButtonOnClick;
		_addGroupBarButton.Click += AddGroupButtonClick;
		_createGroupBarButton.Click += CreateGroupButtonOnClick;
		_searchFriendsBarButton.Click += SearchFriendsOnClick;
		_chatSwitchBarButton.Click += ChatSwitchOnClick;
		_addContactButton.Click += AddContactsOnClick;
	}

	private void MainPanorama_OnLoaded(object sender, RoutedEventArgs e)
	{
		UpdatePanoramaSelection();
	}

	private void MainPanorama_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		UpdatePanoramaSelection();
	}

	private void MakeCurrentPanoramaAndNeighboursVisible()
	{
		int num = _lastVisiblePanoramaIndex ?? 0;
		if (num < 0)
		{
			num = 0;
		}
		MakePanoramaItemVisible(num).DoNotWait();
		int nextIndex = (num + 1) % ((PresentationFrameworkCollection<object>)(object)((ItemsControl)MainPanorama).Items).Count;
		int previousIndex = (num + ((PresentationFrameworkCollection<object>)(object)((ItemsControl)MainPanorama).Items).Count - 1) % ((PresentationFrameworkCollection<object>)(object)((ItemsControl)MainPanorama).Items).Count;
		int nextDelay = (_didFirstLoadOccur ? 1500 : 750);
		_didFirstLoadOccur = true;
		((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
		{
			MakePanoramaItemVisible(nextIndex, nextDelay).DoNotWait();
		}));
		((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
		{
			MakePanoramaItemVisible(previousIndex, 3000).DoNotWait();
		}));
	}

	private void UpdatePanoramaSelection()
	{
		if (_pinnedThreadsControl != null)
		{
			_pinnedThreadsControl.CancelRearranging();
		}
		int selectedIndex = MainPanorama.SelectedIndex;
		if (_lastVisiblePanoramaIndex != MainPanorama.SelectedIndex)
		{
			_lastVisiblePanoramaIndex = selectedIndex;
			MakeCurrentPanoramaAndNeighboursVisible();
			object selectedItem = MainPanorama.SelectedItem ?? RecentPanorama;
			((PhoneApplicationPage)this).ApplicationBar.Buttons.Clear();
			if (selectedItem == RecentPanorama)
			{
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_composeNewThreadBarButton);
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_refreshThreadsBarButton);
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_searchFriendsBarButton);
			}
			else if (selectedItem == GroupsPanorama)
			{
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_addGroupBarButton);
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_createGroupBarButton);
			}
			else if (selectedItem == FriendsPanorama)
			{
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_addContactButton);
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_searchFriendsBarButton);
			}
			else if (selectedItem == ActivePanorama)
			{
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_searchFriendsBarButton);
				((PhoneApplicationPage)this).ApplicationBar.Buttons.Add(_chatSwitchBarButton);
			}
			if (MainPanorama.DefaultItem != null)
			{
				MainPanorama.DefaultItem = null;
			}
			WithViewModel((MainViewModel vm) =>
			{
				vm.ViewingThreads = selectedItem == RecentPanorama;
			});
		}
	}

	public bool IsAtTopOfThreads()
	{
		if (((ContentControl)RecentPanorama).Content is RecentThreadsUserControl recentThreadsUserControl)
		{
			return recentThreadsUserControl.ThreadList.IsAtTop();
		}
		return false;
	}

	public void ScrollToTopOfThreads()
	{
		if (((ContentControl)RecentPanorama).Content is RecentThreadsUserControl recentThreadsUserControl)
		{
			recentThreadsUserControl.ThreadList.ScrollToTop(animate: true);
		}
	}

	private async Task MakePanoramaItemVisible(int index, int delay = 0)
	{
		if (delay > 0)
		{
			await Task.Delay(delay);
		}
		object obj = ((PresentationFrameworkCollection<object>)(object)((ItemsControl)MainPanorama).Items)[index];
		PanoramaItem item = (PanoramaItem)((obj is PanoramaItem) ? obj : null);
		if (item == null)
		{
			return;
		}
		object content = ((ContentControl)item).Content;
		UIElement nextElement = (UIElement)((content is UIElement) ? content : null);
		if (nextElement == null)
		{
			WithViewModel((MainViewModel vm) =>
			{
				FrameworkElement val = null;
				if (item == GroupsPanorama)
				{
					val = (FrameworkElement)(object)(_pinnedThreadsControl = new PinnedThreadsControl());
					val.DataContext = vm.PinnedThreads;
					((ContentControl)item).Content = val;
				}
				else if (item == ActivePanorama)
				{
					if (vm.IsPartialUser)
					{
						return;
					}
					val = (FrameworkElement)(object)new ActiveUsersListControl();
					val.DataContext = vm.ActiveUsers;
					((ContentControl)item).Content = val;
				}
				else if (item == FriendsPanorama)
				{
					val = (FrameworkElement)(object)(_friendList = new ContactListUserControl());
					_friendList.ContactTapEvent += FriendsList_OnContactTapEvent;
					_friendList.FoundContactsTapEvent += FriendsList_OnFoundContactsTapEvent;
					val.DataContext = vm.Contacts;
				}
				((ContentControl)item).Content = val;
			});
		}
		if (nextElement != null && (int)nextElement.Visibility != 0)
		{
			nextElement.Visibility = (Visibility)0;
		}
	}

	private void ShowFacebookPopup(FacebookPopup popup)
	{
		_popupStack.Push(popup);
		popup.IsOpen = true;
		((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
	}

	private void FriendsList_OnContactTapEvent(object sender, FacebookUser user)
	{
		if (_contactCardPopup == null)
		{
			ContactCardPopup contactCardPopup = new ContactCardPopup();
			((FrameworkElement)contactCardPopup).DataContext = user;
			_contactCardPopup = contactCardPopup;
			_contactCardPopup.SendMessageClicked += ContactPopup_SendMessageClicked;
			_contactCardPopup.CreateGroupClicked += ContactPopup_CreateGroupClicked;
			_contactCardPopup.RemoveContactClicked += ContactPopup_RemoveContactClicked;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)_contactCardPopup);
		}
		else
		{
			((FrameworkElement)_contactCardPopup).DataContext = user;
		}
		ShowFacebookPopup(_contactCardPopup);
	}

	private void FriendsList_OnFoundContactsTapEvent(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			if (_foundContactPopup == null)
			{
				FoundContactsPopup foundContactsPopup = new FoundContactsPopup();
				((FrameworkElement)foundContactsPopup).DataContext = vm.Contacts.NewSyncedContacts;
				_foundContactPopup = foundContactsPopup;
				_foundContactPopup.ContactTapped += ContactPopup_SendMessageClicked;
				_foundContactPopup.ContactCardTapped += FoundContactPopup_OnContactCardTapped;
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)_foundContactPopup);
			}
			else
			{
				((FrameworkElement)_foundContactPopup).DataContext = vm.Contacts.NewSyncedContacts;
			}
			ShowFacebookPopup(_foundContactPopup);
		});
	}

	private void FoundContactPopup_OnContactCardTapped(object sender, FacebookUser facebookUser)
	{
		if (_contactCardPopup == null)
		{
			ContactCardPopup contactCardPopup = new ContactCardPopup();
			((FrameworkElement)contactCardPopup).DataContext = facebookUser;
			_contactCardPopup = contactCardPopup;
			_contactCardPopup.SendMessageClicked += ContactPopup_SendMessageClicked;
			_contactCardPopup.CreateGroupClicked += ContactPopup_CreateGroupClicked;
			_contactCardPopup.RemoveContactClicked += ContactPopup_RemoveContactClicked;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Insert(0, (UIElement)(object)_contactCardPopup);
		}
		else
		{
			((FrameworkElement)_contactCardPopup).DataContext = facebookUser;
		}
		ShowFacebookPopup(_contactCardPopup);
	}

	private void ContactPopup_CreateGroupClicked(object sender, FacebookUser profile)
	{
		DismissAllPopups();
		Navigator.Navigate(FacebookUri.GroupComposerUri, profile);
	}

	private void ContactPopup_SendMessageClicked(object sender, FacebookUser profile)
	{
		DismissAllPopups();
		if (profile != null)
		{
			FacebookUri uri = FacebookUri.GetConversationUri(profile.Id, profile.Name);
			WithViewModel((MainViewModel vm) =>
			{
				vm.NavigateCommand.Execute(new NavigationEventArgs(uri));
			});
		}
	}

	private void ContactPopup_RemoveContactClicked(object sender, FacebookUser e)
	{
		DismissAllPopups();
		WithViewModel((MainViewModel vm) =>
		{
			vm.DeleteContact(e);
		});
	}

	protected override void OnNavigatedFrom(NavigationEventArgs e)
	{
		if (((ItemsControl)MainPanorama).Items != null && ((IEnumerable<object>)((ItemsControl)MainPanorama).Items).Any())
		{
			foreach (object item in (PresentationFrameworkCollection<object>)(object)((ItemsControl)MainPanorama).Items)
			{
				PanoramaItem val = (PanoramaItem)((item is PanoramaItem) ? item : null);
				if (val != null)
				{
					object content = ((ContentControl)val).Content;
					UIElement val2 = (UIElement)((content is UIElement) ? content : null);
					if (val2 != null)
					{
						val2.Visibility = (Visibility)1;
					}
				}
			}
		}
		((UIElement)LayoutRoot).Visibility = (Visibility)1;
		if (_pinnedThreadsControl != null)
		{
			_pinnedThreadsControl.OnNavigatedFrom();
		}
		base.OnNavigatedFrom(e);
	}

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		base.OnNavigatedTo(e);
		DismissAllPopups();
		if (LayoutRoot != null)
		{
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				((UIElement)LayoutRoot).Visibility = (Visibility)0;
				((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
				{
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					MakeCurrentPanoramaAndNeighboursVisible();
					OnLastPanoramaVisible(e.NavigationMode);
				}));
			}));
		}
		else
		{
			OnLastPanoramaVisible(e.NavigationMode);
		}
	}

	private void OnLastPanoramaVisible(NavigationMode mode)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
		{
			WithViewModel((MainViewModel vm) =>
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				SetUpdateBannerVisibility(vm.UpgradePromptViewModel);
				if ((int)mode == 0)
				{
					vm.CheckIfPushIsDisabled();
				}
				else
				{
					((DependencyObject)this).Dispatcher.BeginInvoke((Action)vm.OnNavigatedBack);
				}
			});
		}));
	}

	private void RefreshThreadsOnClick(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			if (vm.Threads != null)
			{
				vm.Threads.ForcedRefreshCommand.Execute(null);
			}
		});
		ScrollToTopOfThreads();
	}

	private void SearchFriendsOnClick(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			if (_friendList != null)
			{
				((FrameworkElement)_friendList).Height = double.NaN;
			}
			_mainPageState = PageState.SearchingContacts;
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = false;
			if (_searchOverlay == null)
			{
				_searchOverlay = new SearchOverlay(SystemTrayManager);
				((FrameworkElement)_searchOverlay).DataContext = vm.SearchViewModel;
				_searchOverlay.OnContactTapped += OnContactTapped;
				_searchOverlay.OnGroupThreadTapped += OnGroupThreadTapped;
				_searchOverlay.OnClosed += ResetPageState;
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Add((UIElement)(object)_searchOverlay);
			}
			_searchOverlay.Show();
		});
	}

	private void ReportProblemBarButtonOnClick(object sender, EventArgs eventArgs)
	{
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.ReportProblem));
	}

	public void ComposeNewThreadButtonOnClick(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			vm.LaunchComposerCommand.Execute(null);
		});
	}

	private void AddGroupButtonClick(object sender, EventArgs e)
	{
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.SuggestedPins));
	}

	public void CreateGroupButtonOnClick(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			vm.LaunchGroupComposerCommand.Execute(null);
		});
	}

	private void ChatSwitchOnClick(object sender, EventArgs e)
	{
		if (_chatTogglePopup == null)
		{
			_chatTogglePopup = new SwitchChatPopup();
			_chatTogglePopup.ChatEnabled += EnableChatTapped;
			_chatTogglePopup.ChatDisabled += DisableChatTapped;
			((FrameworkElement)_chatTogglePopup).DataContext = ((FrameworkElement)this).DataContext;
			((FrameworkElement)_chatTogglePopup.SwitchChatPopupGrid).Width = ((FrameworkElement)this).ActualWidth;
			((FrameworkElement)_chatTogglePopup.SwitchChatPopupGrid).Height = ((FrameworkElement)this).ActualHeight + ((PhoneApplicationPage)this).ApplicationBar.DefaultSize;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)LayoutRoot).Children).Add((UIElement)(object)_chatTogglePopup);
		}
		ShowFacebookPopup(_chatTogglePopup);
	}

	private void AddContactsOnClick(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			vm.AddContactCommand.Execute(null);
		});
	}

	protected void DismissAllPopups()
	{
		while (AttemptToDismissCurrentPopup())
		{
		}
	}

	protected bool AttemptToDismissCurrentPopup()
	{
		if (_popupStack.Any())
		{
			FacebookPopup facebookPopup = _popupStack.Pop();
			facebookPopup.IsOpen = false;
			if (!_popupStack.Any())
			{
				((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
			}
			return true;
		}
		return false;
	}

	private void ResetPageState(object sender, EventArgs eventArgs)
	{
		if (_mainPageState != PageState.Normal)
		{
			((PhoneApplicationPage)this).ApplicationBar.IsVisible = true;
			_mainPageState = PageState.Normal;
		}
	}

	protected override void OnBackKeyPress(CancelEventArgs e)
	{
		if (AttemptToDismissCurrentPopup())
		{
			e.Cancel = true;
			return;
		}
		if (_mainPageState == PageState.SearchingContacts)
		{
			_searchOverlay.Reset();
			e.Cancel = true;
		}
		else if (_pinnedThreadsControl != null && _pinnedThreadsControl.IsRearranging)
		{
			_pinnedThreadsControl.CancelRearranging();
			e.Cancel = true;
		}
		else if (CheckForQueuedMessages())
		{
			e.Cancel = true;
			if (_isDisplayingPromptToLeaveWithQueuedMessages)
			{
				return;
			}
			_isDisplayingPromptToLeaveWithQueuedMessages = true;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)PromptToLeaveWithQueuedMessages);
		}
		base.OnBackKeyPress(e);
	}

	private bool CheckForQueuedMessages()
	{
		Service instance = ServiceLocator.Current.GetInstance<Service>();
		if (!instance.HasQueuedRequests())
		{
			return false;
		}
		return true;
	}

	private void PromptToLeaveWithQueuedMessages()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		MessageBoxResult val = MessageBox.Show(StringLibrary.Unsent_Message_Warning, StringLibrary.App_Quit_Caption, (MessageBoxButton)1);
		_isDisplayingPromptToLeaveWithQueuedMessages = false;
		if ((int)val == 1)
		{
			((App)(object)Application.Current).Shutdown();
		}
	}

	private void OnContactTapped(object sender, FacebookProfile profile)
	{
		FacebookUri uri = FacebookUri.GetConversationUri(profile.Id, profile.Name);
		WithViewModel((MainViewModel vm) =>
		{
			vm.NavigateCommand.Execute(new NavigationEventArgs(uri));
		});
	}

	private void OnGroupThreadTapped(object sender, ThreadViewModel thread)
	{
		WithViewModel((MainViewModel vm) =>
		{
			FacebookUri threadUri = FacebookUri.GetThreadUri(thread.Id, ThreadNameUtils.GetThreadNameOrPlaceholder(thread, vm.Context));
			vm.NavigateCommand.Execute(new NavigationEventArgs(threadUri));
		});
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		FacebookPopup.SafeCleanUp(_chatTogglePopup);
		FacebookPopup.SafeCleanUp(_contactCardPopup);
		FacebookPopup.SafeCleanUp(_foundContactPopup);
	}

	private static void SetUpdateBannerVisibility(UpgradePromptViewModel vm)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected Obj, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected Obj, but got Unknown
		if (!vm.ShouldShowUpgradePrompt())
		{
			return;
		}
		CustomMessageBox customMessageBox = new CustomMessageBox();
		customMessageBox.Caption = StringLibrary.AppUpdate_Title;
		customMessageBox.Message = StringLibrary.AppUpdate_Description;
		TextBlock val = new TextBlock();
		val.Text = StringLibrary.AppUpdate_UpdateAppButton;
		val.TextWrapping = (TextWrapping)2;
		customMessageBox.LeftButtonContent = val;
		TextBlock val2 = new TextBlock();
		val2.Text = StringLibrary.AppUpdate_NotNowButton;
		val2.TextWrapping = (TextWrapping)2;
		customMessageBox.RightButtonContent = val2;
		CustomMessageBox customMessageBox2 = customMessageBox;
		customMessageBox2.Dismissed += (object sender, DismissedEventArgs args) =>
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected Obj, but got Unknown
			switch (args.Result)
			{
			case CustomMessageBoxResult.LeftButton:
			{
				WebBrowserTask val3 = new WebBrowserTask();
				val3.Uri = vm.StoreUri;
				val3.Show();
				break;
			}
			case CustomMessageBoxResult.RightButton:
				vm.SetUpgradeNagShown();
				break;
			}
		};
		customMessageBox2.Show();
	}

	private void EnableChatTapped(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			if (vm.EnableChatCommand.CanExecute(e))
			{
				vm.EnableChatCommand.Execute(e);
				AttemptToDismissCurrentPopup();
			}
		});
	}

	private void DisableChatTapped(object sender, EventArgs e)
	{
		WithViewModel((MainViewModel vm) =>
		{
			if (vm.DisableChatCommand.CanExecute(e))
			{
				vm.DisableChatCommand.Execute(e);
				AttemptToDismissCurrentPopup();
			}
		});
	}

	public void ResetPanorama()
	{
		_lastVisiblePanoramaIndex = null;
		if (MainPanorama != null)
		{
			MainPanorama.DefaultItem = ((PresentationFrameworkCollection<object>)(object)((ItemsControl)MainPanorama).Items)[0];
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/MessengerMainPage.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			MainPanorama = (Panorama)((FrameworkElement)this).FindName("MainPanorama");
			RecentPanorama = (PanoramaItem)((FrameworkElement)this).FindName("RecentPanorama");
			GroupsPanorama = (PanoramaItem)((FrameworkElement)this).FindName("GroupsPanorama");
			FriendsPanorama = (PanoramaItem)((FrameworkElement)this).FindName("FriendsPanorama");
			ActivePanorama = (PanoramaItem)((FrameworkElement)this).FindName("ActivePanorama");
		}
	}
}

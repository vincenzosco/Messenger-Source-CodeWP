using System;
using System.Windows;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.DeleteContact;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Windows.Phone.System.Power;

namespace Facebook.Messenger.WP8.ViewModels;

public class MainViewModel : FacebookMessengerViewModel
{
	private ThreadsViewModel _threads;

	private ContactsViewModel _contacts;

	private ActiveUsersViewModel _activeUsers;

	private PinnedThreadsViewModel _pinnedThreads;

	private UpgradePromptViewModel _upgradePromptViewModel;

	private ContactSearchViewModel _search;

	private RelayCommand _launchComposerCommand;

	private RelayCommand _launchGroupComposerCommand;

	private RelayCommand _addContactCommand;

	private RelayCommand _enableChatCommand;

	private RelayCommand _disableChatCommand;

	public ThreadsViewModel Threads => _threads ?? (_threads = ServiceLocator.Current.GetInstance<ThreadsViewModel>());

	public ContactsViewModel Contacts
	{
		get
		{
			if (_contacts != null)
			{
				return _contacts;
			}
			_contacts = ServiceLocator.Current.GetInstance<ContactsViewModel>();
			_contacts.FetchContacts();
			return _contacts;
		}
	}

	public ActiveUsersViewModel ActiveUsers
	{
		get
		{
			if (_activeUsers != null)
			{
				return _activeUsers;
			}
			_activeUsers = ServiceLocator.Current.GetInstance<ActiveUsersViewModel>();
			_activeUsers.UpdatePresence().DoNotWait();
			return _activeUsers;
		}
	}

	public PinnedThreadsViewModel PinnedThreads
	{
		get
		{
			if (_pinnedThreads != null)
			{
				return _pinnedThreads;
			}
			_pinnedThreads = ServiceLocator.Current.GetInstance<PinnedThreadsViewModel>();
			_pinnedThreads.FetchPinnedThreads().DoNotWait();
			return _pinnedThreads;
		}
	}

	public UpgradePromptViewModel UpgradePromptViewModel
	{
		get
		{
			if (_upgradePromptViewModel == null)
			{
				_upgradePromptViewModel = ServiceLocator.Current.GetInstance<UpgradePromptViewModel>();
			}
			return _upgradePromptViewModel;
		}
	}

	public ContactSearchViewModel SearchViewModel => _search ?? (_search = ServiceLocator.Current.GetInstance<ContactSearchViewModel>());

	public bool ViewingThreads { get; set; }

	public bool IsMuted => MuteHelper.IsMuted(Context);

	public RelayCommand LaunchComposerCommand => _launchComposerCommand ?? (_launchComposerCommand = new RelayCommand(ExecuteLaunchComposerCommand));

	public RelayCommand LaunchGroupComposerCommand => _launchGroupComposerCommand ?? (_launchGroupComposerCommand = new RelayCommand(ExecuteLaunchGroupComposerCommand));

	public RelayCommand AddContactCommand => _addContactCommand ?? (_addContactCommand = new RelayCommand(ExecuteAddContactCommand));

	public RelayCommand EnableChatCommand => _enableChatCommand ?? (_enableChatCommand = new RelayCommand(() =>
	{
		ActiveUsers.IsChatEnabled = true;
	}));

	public RelayCommand DisableChatCommand => _disableChatCommand ?? (_disableChatCommand = new RelayCommand(() =>
	{
		ActiveUsers.IsChatEnabled = false;
	}));

	public MainViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, INetworkStatus networkStatus, IDispatcher dispatcher)
		: base(apiService, context, navigator, dispatcher, networkStatus)
	{
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<GlobalMuteSignal>(this, OnGlobalMuteChanged);
	}

	public override void Cleanup()
	{
		base.Cleanup();
		if (_search != null)
		{
			_search.Cleanup();
		}
	}

	private void OnGlobalMuteChanged(GlobalMuteSignal obj)
	{
		((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
		{
			RaisePropertyChanged("IsMuted");
		}));
	}

	private void ExecuteLaunchComposerCommand()
	{
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.Composer));
	}

	private void ExecuteLaunchGroupComposerCommand()
	{
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.GroupComposer));
	}

	private void ExecuteAddContactCommand()
	{
		Navigator.Navigate(FacebookUri.GetAppUri(UriId.AddContact));
	}

	public override void OnApplicationResumed()
	{
		base.OnApplicationResumed();
		Threads.OnApplicationResumed();
		CheckIfPushIsDisabled();
	}

	public void CheckIfPushIsDisabled()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)PowerManager.PowerSavingMode == 1)
			{
				MessageBox.Show(StringLibrary.Power_Saver_Mode_Warning);
			}
		}
		catch
		{
		}
	}

	public void OnNavigatedBack()
	{
		Threads.UpdateVisibleThreads();
	}

	public override void LogNavigatedTo()
	{
		base.LogNavigatedTo();
		if (Threads.ThreadsFetched)
		{
			LogPageReady();
			return;
		}
		bool logNavigation = true;
		Threads.FirstPageFetched += (ThreadCollection collection, bool fromWeb, Error error) =>
		{
			if (logNavigation)
			{
				LogPageReady();
				logNavigation = false;
			}
		};
	}

	public async void DeleteContact(FacebookUser user)
	{
		Contacts.RemoveContactLocally(user);
		Facebook.Services.Api.Methods.DeleteContact.Request deleteContactRequest = new Facebook.Services.Api.Methods.DeleteContact.Request
		{
			AccessToken = Context.Session.Credentials.AccessToken,
			CancelToken = CancelTokenSource.Token,
			ContactWriteId = user.GraphApiWriteId,
			ContactId = user.Id
		};
		await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.DeleteContact.Request, Facebook.Services.Api.Methods.DeleteContact.Response>(deleteContactRequest);
	}
}

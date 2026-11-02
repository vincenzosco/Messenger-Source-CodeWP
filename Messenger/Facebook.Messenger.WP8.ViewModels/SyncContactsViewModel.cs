using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Models;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class SyncContactsViewModel : FacebookMessengerViewModel
	{
		private readonly PhoneContactsManager PhoneContactsManager;

		private RelayCommand _syncContactsCommand;

		private RelayCommand _skipSyncCommand;

		public RelayCommand SyncContactsCommand
		{
			get
			{
				return _syncContactsCommand ?? (_syncContactsCommand = new RelayCommand(ExecuteSyncContactsCommand));
			}
		}

		public RelayCommand SkipSyncCommand
		{
			get
			{
				return _skipSyncCommand ?? (_skipSyncCommand = new RelayCommand(ExecuteSkipSyncCommand));
			}
		}

		public SyncContactsViewModel(PhoneContactsManager phoneContactsManager, Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			PhoneContactsManager = phoneContactsManager;
		}

		private void ExecuteSyncContactsCommand()
		{
			PhoneContactsManager.StartContactSync();
			Navigator.Reset(FacebookUri.NuxOnMessengerUri);
		}

		private void ExecuteSkipSyncCommand()
		{
			Navigator.Reset(FacebookUri.NuxOnMessengerUri);
		}
	}
}

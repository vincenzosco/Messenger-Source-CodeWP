using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ContactsViewModel : FacebookMessengerViewModel
	{
		private const int TopContactsCount = 18;

		private readonly PhoneContactsManager _phoneContactsManager;

		private readonly ContactSyncFeatureEnabledProvider _contactSyncFeatureEnabledProvider;

		protected bool _friendListIsStale;

		private FacebookUserCollection _contacts;

		private bool _shouldGroupContacts;

		private string _emptyText;

		private string _emptyActionButtonLabel;

		private ObservableCollection<AlphaKeyGroup<FacebookUser>> _alphaGroupedContactList;

		private ObservableCollection<FacebookUser> _topContactsCollection;

		protected RelayCommand<UserSelectedEventArgs> _userSelectedCommand;

		private RelayCommand _addContactCommand;

		private RelayCommand _findContactsCommand;

		private RelayCommand _contactSyncLearnMoreCommand;

		private ContactSyncStatus _contactSyncStatus;

		public FacebookUserCollection Contacts
		{
			get
			{
				return _contacts;
			}
			set
			{
				Set(ref _contacts, value, false, "Contacts");
			}
		}

		public ObservableCollection<FacebookUser> ContactsWithMessenger
		{
			get
			{
				return new ObservableCollection<FacebookUser>(_contacts.AllUsers.Where((FacebookUser x) => x.IsMessengerUser));
			}
		}

		public ObservableCollection<FacebookUser> NewSyncedContacts
		{
			get
			{
				return new ObservableCollection<FacebookUser>(ContactSyncStatus.SyncedContactsList);
			}
		}

		public bool ShouldGroupContacts
		{
			get
			{
				if (_shouldGroupContacts)
				{
					if (Contacts != null)
					{
						return Contacts.AllUsers.Count > 20;
					}
					return false;
				}
				return false;
			}
			set
			{
				Set(ref _shouldGroupContacts, value, false, "ShouldGroupContacts");
			}
		}

		public string EmptyText
		{
			get
			{
				return _emptyText;
			}
			set
			{
				Set(ref _emptyText, value, false, "EmptyText");
			}
		}

		public string EmptyActionButtonLabel
		{
			get
			{
				return _emptyActionButtonLabel;
			}
			set
			{
				Set(ref _emptyActionButtonLabel, value, false, "EmptyActionButtonLabel");
			}
		}

		public ObservableCollection<AlphaKeyGroup<FacebookUser>> AlphaGroupedContactList
		{
			get
			{
				if (_alphaGroupedContactList == null)
				{
					_alphaGroupedContactList = AlphaKeyGroup<FacebookUser>.CreateGroups(_contacts.AllUsers.Where((FacebookUser x) => x.IsMessengerUser), Thread.CurrentThread.CurrentUICulture, (FacebookUser s) => s.Name, true, TopContacts);
				}
				return _alphaGroupedContactList;
			}
		}

		public ObservableCollection<FacebookUser> TopContacts
		{
			get
			{
				if (_topContactsCollection == null)
				{
					_topContactsCollection = new ObservableCollection<FacebookUser>(ContactsWithMessenger.OrderByDescending((FacebookUser a) => a.CommunicationRank).Take(18));
				}
				return _topContactsCollection;
			}
		}

		public RelayCommand<UserSelectedEventArgs> UserSelectedCommand
		{
			get
			{
				return _userSelectedCommand ?? (_userSelectedCommand = new RelayCommand<UserSelectedEventArgs>(OnUserSelected));
			}
		}

		public RelayCommand AddContactCommand
		{
			get
			{
				return _addContactCommand ?? (_addContactCommand = new RelayCommand(ExecuteAddContactCommand));
			}
		}

		public RelayCommand FindContactsCommand
		{
			get
			{
				return _findContactsCommand ?? (_findContactsCommand = new RelayCommand(ExecuteFindContactsCommand));
			}
		}

		public RelayCommand ContactSyncLearnMoreCommand
		{
			get
			{
				return _contactSyncLearnMoreCommand ?? (_contactSyncLearnMoreCommand = new RelayCommand(ExecuteContactSyncLearnMoreCommand));
			}
		}

		public string ContactSyncStatusText
		{
			get
			{
				if (ContactSyncStatus == null)
				{
					return string.Empty;
				}
				switch (ContactSyncStatus.Status)
				{
				case SyncState.NotStarted:
					return StringLibrary.ContactList_Header_FindContacts;
				case SyncState.InProgress:
					return StringLibrary.ContactList_Header_SyncingContacts;
				case SyncState.Complete:
				{
					int count = ContactSyncStatus.SyncedContactsList.Count;
					if (count != 0)
					{
						return string.Format(StringLibrary.ContactList_Header_ContactsFound, count);
					}
					return string.Empty;
				}
				case SyncState.Failed:
					return StringLibrary.ContactList_Header_Failed;
				default:
					return string.Empty;
				}
			}
		}

		public ContactSyncStatus ContactSyncStatus
		{
			get
			{
				return _contactSyncStatus;
			}
			set
			{
				Set(ref _contactSyncStatus, value, false, "ContactSyncStatus");
			}
		}

		public bool ShowAllNewContactsEnabled
		{
			get
			{
				if (ContactSyncStatus == null)
				{
					return false;
				}
				if (ContactSyncStatus.Status == SyncState.Complete)
				{
					return ContactSyncStatus.SyncedContactsList.Count > 0;
				}
				return false;
			}
		}

		public bool ContactSyncFeatureEnabled
		{
			get
			{
				return _contactSyncFeatureEnabledProvider.Enabled;
			}
		}

		public virtual void OnUserSelected(UserSelectedEventArgs obj)
		{
		}

		private void ExecuteAddContactCommand()
		{
			Navigator.Navigate(FacebookUri.GetAppUri(UriId.AddContact));
		}

		private void ExecuteFindContactsCommand()
		{
			Task.Run(() =>
			{
				_phoneContactsManager.StartContactSync();
			});
			EmptyActionButtonLabel = string.Empty;
		}

		public void ExecuteContactSyncLearnMoreCommand()
		{
			FacebookUri webViewDeepLinkUri = FacebookAppDeepLinkUtil.GetWebViewDeepLinkUri(FacebookAppDeepLinkUtil.Page.ContactSyncLearnMore);
			Navigator.Navigate(webViewDeepLinkUri);
		}

		public ContactsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, ContactSyncFeatureEnabledProvider contactSyncFeatureEnabledProvider, PhoneContactsManager phoneContactsManager)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_contactSyncFeatureEnabledProvider = contactSyncFeatureEnabledProvider;
			_phoneContactsManager = phoneContactsManager;
			_friendListIsStale = true;
			ShouldGroupContacts = true;
			ContactSyncStatus = phoneContactsManager.ContactSyncStatus;
			Contacts = new FacebookUserCollection(ApiService, Context.Session.Credentials.AccessToken);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<FriendsUpdatedSignal>(this, OnFriendsUpdated);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<ContactSyncToggled>(this, OnContactSyncToggled);
			Contacts.LoadFinished += ContactsOnLoadFinished;
			_phoneContactsManager.SyncStatusUpdate += PhoneContactsManagerOnSyncStatusUpdate;
		}

		private void PhoneContactsManagerOnSyncStatusUpdate(object sender, ContactSyncStatus contactSyncStatus)
		{
			Dispatcher.RunAsync(() =>
			{
				RaisePropertyChanged(() => ContactSyncStatusText);
				RaisePropertyChanged(() => ShowAllNewContactsEnabled);
			});
		}

		public override void Cleanup()
		{
			base.Cleanup();
			_phoneContactsManager.SyncStatusUpdate -= PhoneContactsManagerOnSyncStatusUpdate;
		}

		private void ContactsOnLoadFinished(object sender, Error error)
		{
			if (_friendListIsStale && error == null)
			{
				_friendListIsStale = false;
			}
			_alphaGroupedContactList = null;
			_topContactsCollection = null;
			RaisePropertyChanged("AlphaGroupedContactList");
			RaisePropertyChanged("TopContacts");
			RaisePropertyChanged("ShouldGroupContacts");
			UpdateEmptyState();
		}

		private void UpdateEmptyState()
		{
			if (!Contacts.AllUsers.Any() && Context.IsPartialUser())
			{
				EmptyText = (Context.IsContactSyncEnabled() ? StringLibrary.ContactList_Nux_Label_NoMatchingContacts : StringLibrary.ContactList_Nux_Label_EmptyList);
				EmptyActionButtonLabel = (Context.IsContactSyncEnabled() ? StringLibrary.Contacts_Invite_Button_Label : StringLibrary.Contacts_Find_Button_Label);
			}
			else
			{
				EmptyText = string.Empty;
				EmptyActionButtonLabel = string.Empty;
			}
		}

		private void OnFriendsUpdated(FriendsUpdatedSignal signal)
		{
			FetchContacts(true);
		}

		private void OnContactSyncToggled(ContactSyncToggled obj)
		{
			Dispatcher.RunAsync(UpdateEmptyState);
		}

		public void FetchContacts(bool force = false)
		{
			CancelOldRequests();
			if (_friendListIsStale || force)
			{
				LoadContacts();
			}
		}

		protected virtual void LoadContacts()
		{
			Contacts.FetchContacts(CancelTokenSource.Token, "", true, (ICollection<string>)null, false).DoNotWait();
		}

		private void CancelOldRequests()
		{
			CancelTokenSource.Cancel();
			CancelTokenSource = new CancellationTokenSource();
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			FetchContacts();
		}

		public void RemoveContactLocally(FacebookUser user)
		{
			foreach (AlphaKeyGroup<FacebookUser> alphaGroupedContact in AlphaGroupedContactList)
			{
				RemoveUserFromList(user, alphaGroupedContact);
			}
			RemoveUserFromList(user, TopContacts);
			RemoveUserFromList(user, Contacts.AllUsers);
		}

		private void RemoveUserFromList(FacebookUser user, IList<FacebookUser> list)
		{
			FacebookUser facebookUser = list.FirstOrDefault((FacebookUser u) => u.Id == user.Id);
			if (facebookUser != null)
			{
				list.Remove(facebookUser);
			}
		}
	}
}

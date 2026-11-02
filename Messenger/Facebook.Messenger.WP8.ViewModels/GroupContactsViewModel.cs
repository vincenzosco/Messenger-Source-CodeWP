using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities.PhoneContacts;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.AddThreadParticipants;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class GroupContactsViewModel : ContactsViewModel
	{
		private string _threadId;

		protected ISet<string> _contactsIdsToExclude;

		private bool _isSearching;

		private CancellationTokenSource _searchTokenSource;

		private FacebookUserCollection _searchResults;

		private string _filter;

		private RelayCommand _addGroupParticipantsCommand;

		public bool IsSearching
		{
			get
			{
				return _isSearching;
			}
			set
			{
				Set(ref _isSearching, value, false, "IsSearching");
			}
		}

		public FacebookUserCollection SearchResults
		{
			get
			{
				return _searchResults;
			}
			set
			{
				Set(ref _searchResults, value, false, "SearchResults");
			}
		}

		public string Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					value = string.Empty;
				}
				Set(ref _filter, value, false, "Filter");
				SearchContacts();
			}
		}

		public RelayCommand AddGroupParticipantsCommand
		{
			get
			{
				return _addGroupParticipantsCommand ?? (_addGroupParticipantsCommand = new RelayCommand(ExecuteAddGroupParticipants));
			}
		}

		public bool AnyUserSelected
		{
			get
			{
				return Contacts.AllUsers.Any((FacebookUser c) => c.IsSelected);
			}
		}

		private CancellationTokenSource LinkedTokenSource { get; set; }

		public GroupContactsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, ContactSyncFeatureEnabledProvider contactSyncFeatureEnabledProvider, PhoneContactsManager phoneContactsManager)
			: base(apiService, context, navigator, dispatcher, networkStatus, contactSyncFeatureEnabledProvider, phoneContactsManager)
		{
			SearchResults = new FacebookUserCollection(ApiService, Context.Session.Credentials.AccessToken);
			SearchResults.LoadStarted += SearchResultsOnLoadStarted;
			SearchResults.LoadFinished += SearchResultsOnLoadFinished;
			SearchResults.FetchWithEmptyFilter = false;
		}

		public override void Cleanup()
		{
			base.Cleanup();
			SearchResults.LoadStarted -= SearchResultsOnLoadStarted;
			SearchResults.LoadFinished -= SearchResultsOnLoadFinished;
		}

		private void SearchResultsOnLoadStarted(object sender, EventArgs eventArgs)
		{
			IsLoading = true;
		}

		private void SearchResultsOnLoadFinished(object sender, Error error)
		{
			IsLoading = false;
		}

		public override void SetPath(Uri uri)
		{
			FacebookUri facebookUri = (uri as FacebookUri) ?? FacebookUri.UnknownUri;
			if (facebookUri.Params.ContainsKey("threadId"))
			{
				_threadId = facebookUri.Params["threadId"];
			}
			if (facebookUri.Params.ContainsKey("contactsIdsToExclude"))
			{
				string text = facebookUri.Params["contactsIdsToExclude"];
				if (!string.IsNullOrWhiteSpace(text))
				{
					_contactsIdsToExclude = new HashSet<string>(text.Split(','));
				}
			}
			base.SetPath(uri);
		}

		protected override void LoadContacts()
		{
			Contacts.FetchContacts(CancelTokenSource.Token, "", false, _contactsIdsToExclude).DoNotWait();
		}

		private void SearchContacts()
		{
			CancelOldSearchRequests();
			if (IsSearching)
			{
				SearchResults.FetchContacts(LinkedTokenSource.Token, Filter, false, _contactsIdsToExclude, true).DoNotWait();
			}
		}

		private void CancelOldSearchRequests()
		{
			if (_searchTokenSource != null)
			{
				_searchTokenSource.Cancel();
			}
			_searchTokenSource = new CancellationTokenSource();
			LinkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancelTokenSource.Token, _searchTokenSource.Token);
		}

		public override async void OnUserSelected(UserSelectedEventArgs args)
		{
			if (args != null && args.SelectedFacebookUsers != null && args.SelectedFacebookUsers.Any())
			{
				await AddGroupParticipants(args.SelectedFacebookUsers);
			}
		}

		private async void ExecuteAddGroupParticipants()
		{
			await AddGroupParticipants(Contacts.AllUsers.Where((FacebookUser c) => c.IsSelected));
		}

		private async Task AddGroupParticipants(IEnumerable<FacebookUser> users)
		{
			if (!IsLoading)
			{
				IsLoading = true;
				IList<FacebookUser> addedParticipants = (users as IList<FacebookUser>) ?? users.ToList();
				List<string> userIds = addedParticipants.Select((FacebookUser c) => c.Id).ToList();
				Facebook.Services.Api.Methods.AddThreadParticipants.Request addParticipantsRequest = new Facebook.Services.Api.Methods.AddThreadParticipants.Request
				{
					AccessToken = Context.Session.Credentials.AccessToken,
					ThreadId = _threadId,
					UserIds = userIds
				};
				Facebook.Services.Api.Methods.AddThreadParticipants.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.AddThreadParticipants.Request, Facebook.Services.Api.Methods.AddThreadParticipants.Response>(addParticipantsRequest);
				if (response.HasError() || !response.Succeeded)
				{
					MessageBox.Show(StringLibrary.Group_AddingParticipants_Failed_Message, StringLibrary.Group_AddingParticipants_Failed_Title, (MessageBoxButton)0);
				}
				else
				{
					Navigator.GoBack();
				}
				IsLoading = false;
			}
		}
	}
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchGroupSearchResults;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ContactSearchViewModel : FacebookMessengerViewModel
	{
		public enum SearchState
		{
			None,
			Querying,
			NoResults,
			HasResults
		}

		private const string GroupAndContactSearchProperty = "GroupAndContactSearch";

		private int _localSearchResultCount;

		private readonly SearchTermHelper _searchHelper;

		private FacebookUserCollection _searchResults;

		private ObservableCollection<AlphaKeyGroup<object>> _groupAndContactSearchResults;

		private List<string> _selectedThreadIds;

		private SearchState _contactSearchState;

		private bool _isSearching;

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

		public ObservableCollection<AlphaKeyGroup<object>> GroupAndContactSearchResults
		{
			get
			{
				return _groupAndContactSearchResults;
			}
			set
			{
				Set(ref _groupAndContactSearchResults, value, false, "GroupAndContactSearchResults");
			}
		}

		public List<string> SelectedThreadIds
		{
			get
			{
				return _selectedThreadIds;
			}
			set
			{
				Set(ref _selectedThreadIds, value, false, "SelectedThreadIds");
			}
		}

		public string SearchFilter
		{
			set
			{
				ContactSearchState = SearchState.None;
				_searchHelper.TriggerSearch(value);
			}
		}

		public SearchState ContactSearchState
		{
			get
			{
				return _contactSearchState;
			}
			set
			{
				Set(ref _contactSearchState, value, false, "ContactSearchState");
			}
		}

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

		public ContactSearchViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			SearchResults = new FacebookUserCollection(ApiService, Context.Session.Credentials.AccessToken);
			SearchResults.FetchWithEmptyFilter = false;
			GroupAndContactSearchResults = new ObservableCollection<AlphaKeyGroup<object>>();
			SelectedThreadIds = new List<string>();
			ResetSearchResults();
			_searchHelper = new SearchTermHelper(GetLocalSearchResults, GetRemoteSearchResults);
		}

		private async void GetRemoteSearchResults(string term, CancellationToken token)
		{
			ResetWebSearchResults();
			IsLoading = true;
			await SearchWeb(term, token).RunAndDetermineIfCancelled();
			IsLoading = false;
		}

		private void ResetWebSearchResults()
		{
			_groupAndContactSearchResults[1].Clear();
			_groupAndContactSearchResults[2].Clear();
			RaisePropertyChanged("GroupAndContactSearch");
		}

		private void ResetSearchResults()
		{
			_groupAndContactSearchResults.Clear();
			_groupAndContactSearchResults.Add(new AlphaKeyGroup<object>(string.Empty, new List<object>()));
			_groupAndContactSearchResults.Add(new AlphaKeyGroup<object>(StringLibrary.Composer_Groups_Search_Header, new List<object>()));
			_groupAndContactSearchResults.Add(new AlphaKeyGroup<object>(StringLibrary.Composer_More_People_Search_Header, new List<object>()));
			RaisePropertyChanged("GroupAndContactSearch");
		}

		private void GetLocalSearchResults(string term, CancellationToken token)
		{
			SearchResults.FetchFriendsAsync(term, token, FilterFriendsDelegate).ContinueWith((Task<int> task) => Dispatcher.RunAsync(() =>
			{
				_localSearchResultCount = task.Result;
			}));
		}

		private async Task SearchWeb(string term, CancellationToken token)
		{
			Task<int> fetchFriendsOfFriendsTask = SearchResults.FetchFriendsAsync(term, token, FilterFriendsOfFriendsDelegate, FacebookUserCollection.SearchRequestType.FriendsOfFriends);
			Facebook.Services.Api.Methods.FetchGroupSearchResults.Request groupSearchrequest = new Facebook.Services.Api.Methods.FetchGroupSearchResults.Request(term)
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = token
			};
			FilterGroupSearch(groupSearchrequest);
			int groupCount = 0;
			Task fetchGroupsTask = Task.Run(async () =>
			{
				Facebook.Services.Api.Methods.FetchGroupSearchResults.Response groupSearchResponse = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchGroupSearchResults.Request, Facebook.Services.Api.Methods.FetchGroupSearchResults.Response>(groupSearchrequest);
				if (!groupSearchResponse.HasError() && groupSearchResponse.Threads != null && !token.IsCancellationRequested)
				{
					groupCount = groupSearchResponse.Threads.Count;
					Dispatcher.Schedule(() =>
					{
						if (!token.IsCancellationRequested)
						{
							List<ThreadViewModel> list = groupSearchResponse.Threads.Select((MailThread t) => new ThreadViewModel(t)
							{
								IsSelected = SelectedThreadIds.Contains(t.Id)
							}).ToList();
							_groupAndContactSearchResults[1].AddRange(list);
							RaisePropertyChanged("GroupAndContactSearch");
						}
					});
				}
			}, token);
			await Task.WhenAll(fetchFriendsOfFriendsTask, fetchGroupsTask);
			if (!token.IsCancellationRequested && !string.IsNullOrEmpty(term))
			{
				int num = fetchFriendsOfFriendsTask.Result + groupCount + _localSearchResultCount;
				ContactSearchState = ((num > 0) ? SearchState.HasResults : SearchState.NoResults);
			}
		}

		protected virtual void FilterGroupSearch(Facebook.Services.Api.Methods.FetchGroupSearchResults.Request request)
		{
		}

		protected virtual void FilterFriendsDelegate(AlphaKeyGroup<FacebookUser> collection)
		{
			GroupAndContactSearchResults[0] = new AlphaKeyGroup<object>(collection.Key, collection);
		}

		protected virtual void FilterFriendsOfFriendsDelegate(AlphaKeyGroup<FacebookUser> collection)
		{
			_groupAndContactSearchResults[2].AddRange(collection);
			RaisePropertyChanged("GroupAndContactSearch");
		}

		public void Reset()
		{
			IsLoading = false;
			IsSearching = false;
			_contactSearchState = SearchState.None;
			_searchHelper.Reset();
			SearchResults.Reset();
			ResetSearchResults();
		}
	}
}

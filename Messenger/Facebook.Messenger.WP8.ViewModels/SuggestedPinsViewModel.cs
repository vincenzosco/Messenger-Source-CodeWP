using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.AddPinThread;
using Facebook.Services.Api.Methods.FetchGroupSearchResults;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class SuggestedPinsViewModel : FacebookMessengerViewModel
	{
		private List<string> _pinnedThreadIds;

		private List<ThreadViewModel> _defaultSuggestion;

		private ObservableCollection<ThreadViewModel> _searchResults;

		private readonly SearchTermHelper _searchHelper;

		private ContactSearchViewModel.SearchState _contactSearchState;

		public ObservableCollection<ThreadViewModel> SearchResults
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

		public string SearchFilter
		{
			set
			{
				_searchHelper.TriggerSearch(value);
			}
		}

		public ContactSearchViewModel.SearchState ContactSearchState
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

		public SuggestedPinsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_pinnedThreadIds = new List<string>();
			_searchResults = new ObservableCollection<ThreadViewModel>();
			_searchHelper = new SearchTermHelper(SearchGroupThreads);
			_searchHelper.TriggerSearch(string.Empty);
		}

		public async void SearchGroupThreads(string term, CancellationToken token)
		{
			SearchResults.Clear();
			IsLoading = true;
			ContactSearchState = ContactSearchViewModel.SearchState.None;
			if (string.IsNullOrWhiteSpace(term) && _defaultSuggestion != null)
			{
				SearchResults = new ObservableCollection<ThreadViewModel>(_defaultSuggestion);
				IsLoading = false;
				return;
			}
			Facebook.Services.Api.Methods.FetchGroupSearchResults.Request request = new Facebook.Services.Api.Methods.FetchGroupSearchResults.Request(term, 100)
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = token,
				IncludePinnedIds = (_pinnedThreadIds.Count == 0),
				ExcludeUnsubscribedThreads = true
			};
			Facebook.Services.Api.Methods.FetchGroupSearchResults.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchGroupSearchResults.Request, Facebook.Services.Api.Methods.FetchGroupSearchResults.Response>(request);
			if (_pinnedThreadIds.Count == 0 && response.PinnedIds != null)
			{
				_pinnedThreadIds = response.PinnedIds;
			}
			if (!response.HasError() && !request.CancelToken.IsCancellationRequested)
			{
				List<ThreadViewModel> threads = response.Threads.Select((MailThread t) => new ThreadViewModel(t)).ToList();
				List<ThreadViewModel> list = FilterPinnedThreads(threads);
				SearchResults = new ObservableCollection<ThreadViewModel>(list);
			}
			if (_defaultSuggestion == null && string.IsNullOrWhiteSpace(term) && !response.HasError())
			{
				List<ThreadViewModel> threads2 = response.Threads.Select((MailThread t) => new ThreadViewModel(t)).ToList();
				_defaultSuggestion = FilterPinnedThreads(threads2);
			}
			if (!request.CancelToken.IsCancellationRequested)
			{
				IsLoading = false;
				if (!string.IsNullOrEmpty(term))
				{
					ContactSearchState = ((SearchResults.Count > 0) ? ContactSearchViewModel.SearchState.HasResults : ContactSearchViewModel.SearchState.NoResults);
				}
			}
		}

		private List<ThreadViewModel> FilterPinnedThreads(List<ThreadViewModel> threads)
		{
			if (_pinnedThreadIds != null)
			{
				return threads.Where((ThreadViewModel t) => !_pinnedThreadIds.Contains(t.Id)).ToList();
			}
			return threads;
		}

		public async Task PinThread(ThreadViewModel thread)
		{
			if (thread == null)
			{
				return;
			}
			if (!thread.HasName())
			{
				Navigator.Navigate(FacebookUri.GroupComposerUri, thread);
				return;
			}
			Facebook.Services.Api.Methods.AddPinThread.Request request = new Facebook.Services.Api.Methods.AddPinThread.Request(thread.Id)
			{
				AccessToken = Context.Session.Credentials.AccessToken
			};
			if ((await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.AddPinThread.Request, Facebook.Services.Api.Methods.AddPinThread.Response>(request)).HasError())
			{
				OnError(new Error
				{
					Message = StringLibrary.MessageBox_Label_PinGroupFailed
				});
			}
			else
			{
				Navigator.GoBackToRoot();
				GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new GroupPinnedSignal(thread.Id));
			}
		}
	}
}

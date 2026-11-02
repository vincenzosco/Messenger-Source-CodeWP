using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchFriends;
using Facebook.Services.Api.Methods.FetchFriendsOfFriends;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class FacebookUserCollection : ObservableCollection<AlphaKeyGroup<FacebookUser>>
	{
		public delegate void FilterFriendsDelegate(AlphaKeyGroup<FacebookUser> collection);

		public enum SearchRequestType
		{
			Friends,
			FriendsOfFriends
		}

		private readonly Facebook.Services.Api.Service _apiService;

		private string _previousFilter;

		public string AccessToken { get; set; }

		public bool FetchWithEmptyFilter { get; set; }

		public ObservableCollection<FacebookUser> AllUsers
		{
			get
			{
				if (Count > 0)
				{
					return this.Aggregate((AlphaKeyGroup<FacebookUser> i, AlphaKeyGroup<FacebookUser> j) =>
					{
						if (i == null)
						{
							i = new AlphaKeyGroup<FacebookUser>(string.Empty);
						}
						if (j != null)
						{
							foreach (FacebookUser item in j)
							{
								i.Add(item);
							}
						}
						return i;
					});
				}
				return new ObservableCollection<FacebookUser>();
			}
		}

		public List<string> SelectedContactIds { get; set; }

		public event EventHandler<Error> LoadFinished;

		public event EventHandler LoadStarted;

		public event EventHandler FirstElementLoaded;

		public FacebookUserCollection(Facebook.Services.Api.Service service, string accessToken = null)
		{
			_apiService = service;
			AccessToken = accessToken;
			FetchWithEmptyFilter = true;
			SelectedContactIds = new List<string>();
		}

		public void Reset()
		{
			Clear();
			_previousFilter = null;
		}

		public async Task FetchContacts(CancellationToken token, string filter, bool force = false, FilterFriendsDelegate filterFriendsDelegate = null, bool fetchFriendsOfFriends = false, FilterFriendsDelegate filterFriendsOfFriendsDelegate = null)
		{
			Error responseError = null;
			filter = filter ?? string.Empty;
			if ((force || string.IsNullOrEmpty(_previousFilter) || !_previousFilter.Trim().Equals(filter.Trim())) && !token.IsCancellationRequested)
			{
				if (LoadStarted != null)
				{
					LoadStarted(this, EventArgs.Empty);
				}
				try
				{
					await GetSearchResults(token, filter, filterFriendsDelegate, fetchFriendsOfFriends, filterFriendsOfFriendsDelegate);
				}
				catch (ApiErrorException ex)
				{
					responseError = ex.Error;
				}
				if (LoadFinished != null && !token.IsCancellationRequested)
				{
					LoadFinished(this, responseError);
				}
				_previousFilter = filter;
			}
		}

		public async Task FetchContacts(CancellationToken token, string filter = "", bool force = false, ICollection<string> contactsToIgnore = null, bool fetchFriendsOfFriends = false)
		{
			await FetchContacts(token, filter, force, RemoveContactsToIgnore(contactsToIgnore), fetchFriendsOfFriends, RemoveContactsToIgnore(contactsToIgnore));
		}

		private static FilterFriendsDelegate RemoveContactsToIgnore(ICollection<string> contactsToIgnore)
		{
			return (AlphaKeyGroup<FacebookUser> collection) =>
			{
				if (collection != null && collection.Count > 0 && contactsToIgnore != null)
				{
					List<FacebookUser> list = collection.Where((FacebookUser friend) => friend == null || friend.Id == null || contactsToIgnore.Contains(friend.Id)).ToList();
					foreach (FacebookUser item in list)
					{
						collection.Remove(item);
					}
				}
			};
		}

		public async Task GetSearchResults(CancellationToken token, string filter, FilterFriendsDelegate filterFriendsDelegate = null, bool fetchFriendsOfFriends = false, FilterFriendsDelegate filterFriendsOfFriendsDelegate = null)
		{
			Clear();
			if (string.IsNullOrWhiteSpace(filter) && !FetchWithEmptyFilter)
			{
				return;
			}
			AlphaKeyGroup<FacebookUser> result = await GetFriends(filter, token);
			if (token.IsCancellationRequested)
			{
				return;
			}
			if (result != null)
			{
				if (filterFriendsDelegate != null)
				{
					filterFriendsDelegate(result);
					if (token.IsCancellationRequested)
					{
						return;
					}
				}
				if (result.Count > 0)
				{
					Add(result);
					if (FirstElementLoaded != null)
					{
						FirstElementLoaded(this, null);
					}
				}
			}
			if (token.IsCancellationRequested || !fetchFriendsOfFriends)
			{
				return;
			}
			result = await GetFriendsOfFriends(filter, token);
			if (token.IsCancellationRequested || result == null)
			{
				return;
			}
			if (filterFriendsOfFriendsDelegate != null)
			{
				filterFriendsOfFriendsDelegate(result);
				if (token.IsCancellationRequested)
				{
					return;
				}
			}
			if (result.Count > 0)
			{
				Add(result);
				if (Count == 1 && FirstElementLoaded != null)
				{
					FirstElementLoaded(this, null);
				}
			}
		}

		public async Task<int> FetchFriendsAsync(string filter, CancellationToken token, FilterFriendsDelegate filterDelegate = null, SearchRequestType searchRequestType = SearchRequestType.Friends)
		{
			AlphaKeyGroup<FacebookUser> result;
			switch (searchRequestType)
			{
			case SearchRequestType.FriendsOfFriends:
				result = await GetFriendsOfFriends(filter, token);
				break;
			default:
				result = await GetFriends(filter, token);
				break;
			}
			if (token.IsCancellationRequested)
			{
				return 0;
			}
			if (result != null)
			{
				filterDelegate?.Invoke(result);
				if (result.Count > 0)
				{
					Add(result);
					if (FirstElementLoaded != null)
					{
						FirstElementLoaded(this, null);
					}
				}
			}
			return result?.Count ?? 0;
		}

		public async Task<AlphaKeyGroup<FacebookUser>> GetFriends(string filter, CancellationToken token)
		{
			Facebook.Services.Api.Methods.FetchFriends.Request request = new Facebook.Services.Api.Methods.FetchFriends.Request
			{
				Filter = filter,
				CancelToken = token
			};
			if (token.IsCancellationRequested)
			{
				return new AlphaKeyGroup<FacebookUser>(string.Empty);
			}
			Facebook.Services.Api.Methods.FetchFriends.Response result = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchFriends.Request, Facebook.Services.Api.Methods.FetchFriends.Response>(request);
			if (result.HasError())
			{
				throw new ApiErrorException(result.Error);
			}
			foreach (string id in SelectedContactIds)
			{
				List<FacebookUser> friends = result.Friends;
				Predicate<FacebookUser> match = (FacebookUser f) => f.Id == id;
				FacebookUser facebookUser = friends.Find(match);
				if (facebookUser != null)
				{
					facebookUser.IsSelected = true;
				}
			}
			return new AlphaKeyGroup<FacebookUser>(string.Empty, result.Friends);
		}

		public async Task<AlphaKeyGroup<FacebookUser>> GetFriendsOfFriends(string filter, CancellationToken token)
		{
			Facebook.Services.Api.Methods.FetchFriendsOfFriends.Request request = new Facebook.Services.Api.Methods.FetchFriendsOfFriends.Request(filter, "40")
			{
				AccessToken = AccessToken,
				CancelToken = token
			};
			if (token.IsCancellationRequested)
			{
				return new AlphaKeyGroup<FacebookUser>(string.Empty);
			}
			Facebook.Services.Api.Methods.FetchFriendsOfFriends.Response result = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchFriendsOfFriends.Request, Facebook.Services.Api.Methods.FetchFriendsOfFriends.Response>(request);
			if (result.HasError())
			{
				return new AlphaKeyGroup<FacebookUser>(string.Empty);
			}
			foreach (string id in SelectedContactIds)
			{
				List<FacebookUser> users = result.Users;
				Predicate<FacebookUser> match = (FacebookUser f) => f.Id == id;
				FacebookUser facebookUser = users.Find(match);
				if (facebookUser != null)
				{
					result.MarkAsSelected(facebookUser);
				}
			}
			return new AlphaKeyGroup<FacebookUser>(StringLibrary.Composer_More_People_Search_Header, result.Users);
		}
	}
}

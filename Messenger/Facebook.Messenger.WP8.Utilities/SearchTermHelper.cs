using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Extensions;

namespace Facebook.Messenger.WP8.Utilities
{
	public class SearchTermHelper
	{
		public delegate void SearchTermExecutor(string term, CancellationToken token);

		private const int SearchKeyPressDelayMs = 450;

		private readonly SearchTermExecutor _immediateSearchCallback;

		private readonly SearchTermExecutor _delayedSearchCallback;

		private CancellationTokenSource _cancellationToken;

		private string _searchFilter;

		public SearchTermHelper(SearchTermExecutor delayedSearchCallback)
			: this(null, delayedSearchCallback)
		{
		}

		public SearchTermHelper(SearchTermExecutor immediateSearchCallback, SearchTermExecutor delayedSearchCallback)
		{
			_immediateSearchCallback = immediateSearchCallback;
			_delayedSearchCallback = delayedSearchCallback;
		}

		public async void TriggerSearch(string inputFilter)
		{
			string newFilter = SanitizeInput(inputFilter);
			if (!string.Equals(newFilter, _searchFilter))
			{
				_searchFilter = newFilter;
				ResetCancellationToken();
				if (_immediateSearchCallback != null)
				{
					_immediateSearchCallback(newFilter, _cancellationToken.Token);
				}
				bool taskCancelled = await Task.Delay(450, _cancellationToken.Token).RunAndDetermineIfCancelled();
				bool termChangedSinceDelay = _searchFilter != newFilter;
				if (!taskCancelled && !termChangedSinceDelay && _delayedSearchCallback != null)
				{
					_delayedSearchCallback(newFilter, _cancellationToken.Token);
				}
			}
		}

		public void Reset()
		{
			ResetCancellationToken();
			_searchFilter = string.Empty;
		}

		private void ResetCancellationToken()
		{
			if (_cancellationToken != null)
			{
				_cancellationToken.Cancel();
			}
			_cancellationToken = new CancellationTokenSource();
		}

		private static string SanitizeInput(string filter)
		{
			return (filter ?? string.Empty).Trim();
		}
	}
}

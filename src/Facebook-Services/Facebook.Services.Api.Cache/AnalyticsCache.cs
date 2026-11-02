using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Services.Analytics;

namespace Facebook.Services.Api.Cache
{
	public class AnalyticsCache : ICache
	{
		private const string CacheSaveKey = "analytics_cache";

		private readonly IUserDataStorage _storage;

		private readonly DisposableMutexInit _lock;

		private List<AnalyticsEvent> _events;

		private int _nextAnalyticsEventId;

		private bool _dirty;

		public AnalyticsCache(IUserDataStorage storage)
		{
			_storage = storage;
			_lock = new DisposableMutexInit(Init);
		}

		private async Task Init()
		{
			_events = (await SaveCacheUtil.Load<List<AnalyticsEvent>>(_storage, "analytics_cache")) ?? new List<AnalyticsEvent>();
			foreach (AnalyticsEvent @event in _events)
			{
				@event.Id = _nextAnalyticsEventId;
				_nextAnalyticsEventId++;
			}
		}

		public async Task EnsureSave()
		{
			if (!(await _lock.IsInitialized()))
			{
				Logger.Info(this, "Did not save cache: Cache was not initialized.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\AnalyticsCache.cs", 45);
				return;
			}
			Logger.Info(this, "Acquiring Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\AnalyticsCache.cs", 49);
			using (await _lock.WaitAsync())
			{
				Logger.Info(this, "Acquired Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\AnalyticsCache.cs", 52);
				if (_dirty)
				{
					await UnsafeSaveState();
				}
				else
				{
					Logger.Info(this, "Did not save cache: Cache did not change.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\AnalyticsCache.cs", 59);
				}
			}
		}

		private async Task UnsafeSaveState()
		{
			await SaveCacheUtil.Save(_storage, "analytics_cache", _events);
			_dirty = false;
		}

		public async Task Purge()
		{
			using (await _lock.WaitAsync())
			{
				_events.Clear();
				_nextAnalyticsEventId = 0;
				_dirty = true;
			}
		}

		public async Task AddEvent(AnalyticsEvent e)
		{
			using (await _lock.WaitAsync())
			{
				e.Id = _nextAnalyticsEventId;
				_nextAnalyticsEventId++;
				_events.Add(e);
				_dirty = true;
			}
		}

		public async Task<IEnumerable<AnalyticsEvent>> GetEvents(int limit)
		{
			using (await _lock.WaitAsync())
			{
				return _events.Take(limit).ToList();
			}
		}

		public async Task DeleteEvents(long maxLogIdToDelete)
		{
			using (await _lock.WaitAsync())
			{
				_events.RemoveAll((AnalyticsEvent e) => e.Id <= maxLogIdToDelete);
				_dirty = true;
			}
		}
	}
}

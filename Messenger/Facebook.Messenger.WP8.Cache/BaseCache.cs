using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Utilities;
using Facebook.Models;
using Facebook.Services.Api.Cache;
using Microsoft.Practices.ServiceLocation;
using Windows.Storage;

namespace Facebook.Messenger.WP8.Cache
{
	public abstract class BaseCache<TElement, TFetchingEntry> : ICache where TElement : class where TFetchingEntry : FetchingEntry<TElement>
	{
		protected enum FailedRequestRetryPolicy
		{
			Never,
			OnNetworkGained
		}

		private enum FileWriteResult
		{
			Success,
			Failure
		}

		protected enum FetchStatus
		{
			NeedsRequest,
			AlreadyInFlight
		}

		private readonly long _maximumCacheSizeInBytes;

		private readonly FailedRequestRetryPolicy _failedRequestsRetryPolicy;

		private readonly object _requestLock = new object();

		private readonly string _folderName;

		private string _cacheFolderPath;

		private StorageFolder _cacheFolder;

		private Dictionary<int, TFetchingEntry> _activeRequests;

		private DiskCache _diskCache;

		private bool _dirty;

		private CancellationTokenSource _sessionTokenSource;

		protected CancellationTokenSource _networkTokenSource;

		protected readonly HttpClient _httpClient;

		private readonly SemaphoreSlim _writeToDiskSemaphore;

		private readonly FacebookContext _context;

		private readonly IUserDataStorage _userDataStorage;

		protected readonly INetworkStatus _networkStatus;

		private string CacheSettingsKey
		{
			get
			{
				return _folderName + "-state";
			}
		}

		protected BaseCache(string cacheFolder, long maximumCacheSize, FailedRequestRetryPolicy failedRequestRetryPolicy)
		{
			_folderName = cacheFolder;
			_maximumCacheSizeInBytes = maximumCacheSize;
			_failedRequestsRetryPolicy = failedRequestRetryPolicy;
			_writeToDiskSemaphore = new SemaphoreSlim(1);
			_sessionTokenSource = new CancellationTokenSource();
			_networkTokenSource = new CancellationTokenSource();
			_activeRequests = new Dictionary<int, TFetchingEntry>();
			_httpClient = new HttpClient();
			_context = ServiceLocator.Current.GetInstance<FacebookContext>();
			_userDataStorage = ServiceLocator.Current.GetInstance<IUserDataStorage>();
			Task<LruCache<int, string>.Entry[]> savedCacheStateTask = SaveCacheUtil.Load<LruCache<int, string>.Entry[]>(_userDataStorage, CacheSettingsKey);
			_diskCache = new DiskCache(_maximumCacheSizeInBytes, GetCacheFolder(), savedCacheStateTask);
			_networkStatus = ServiceLocator.Current.GetInstance<INetworkStatus>();
			_networkStatus.AddHandler(OnNetworkStatusChanged);
		}

		protected abstract Task LoadMediaInner(TFetchingEntry entry);

		protected abstract TFetchingEntry CreateEntryFor(Uri uri, CancellationToken token);

		protected abstract void TagElementWithEntry(TElement element, TFetchingEntry entry);

		protected abstract WeakReference<TFetchingEntry> GetFetchEntry(TElement element);

		protected abstract void ClearFetchEntry(TElement element);

		protected virtual void OnFileSaved(TFetchingEntry entry, string filepath)
		{
		}

		public virtual Task Purge()
		{
			_sessionTokenSource.Cancel();
			_sessionTokenSource = new CancellationTokenSource();
			lock (_requestLock)
			{
				_activeRequests = new Dictionary<int, TFetchingEntry>();
			}
			_diskCache = new DiskCache(_maximumCacheSizeInBytes, GetCacheFolder(), Task.FromResult<LruCache<int, string>.Entry[]>(null));
			_dirty = true;
			_cacheFolderPath = null;
			_cacheFolder = null;
			return Task.FromResult(string.Empty);
		}

		private async Task SaveState()
		{
			Logger.Info(this, "Getting Serialized State", "SaveState", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\BaseCache.cs", 133);
			LruCache<int, string>.Entry[] serializableState = await _diskCache.GetSerializedState();
			Logger.Info(this, string.Format("Got Serialized State {0}", serializableState), "SaveState", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\BaseCache.cs", 135);
			await SaveCacheUtil.Save(_userDataStorage, CacheSettingsKey, serializableState);
			_dirty = false;
		}

		public async Task EnsureSave()
		{
			if (_dirty)
			{
				await SaveState();
			}
			else
			{
				Logger.Info(this, "Did not save cache: Cache did not change.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\BaseCache.cs", 148);
			}
		}

		private void OnNetworkStatusChanged(INetworkStatus sender, bool connected)
		{
			if (!connected)
			{
				_networkTokenSource.Cancel();
				_networkTokenSource = new CancellationTokenSource();
			}
			else if (_failedRequestsRetryPolicy == FailedRequestRetryPolicy.OnNetworkGained)
			{
				RetryFailedRequests();
			}
		}

		private FetchStatus EnsureEntryFor(Uri uri, out TFetchingEntry entry)
		{
			int hashCode = uri.GetHashCode();
			FetchStatus result = FetchStatus.NeedsRequest;
			lock (_requestLock)
			{
				Dictionary<int, TFetchingEntry> activeRequests = _activeRequests;
				if (activeRequests.ContainsKey(hashCode))
				{
					entry = activeRequests[hashCode];
					result = ((!entry.FailedToRetrieve || _failedRequestsRetryPolicy == FailedRequestRetryPolicy.Never) ? FetchStatus.AlreadyInFlight : FetchStatus.NeedsRequest);
				}
				else
				{
					entry = CreateEntryFor(uri, _sessionTokenSource.Token);
					activeRequests[hashCode] = entry;
				}
			}
			return result;
		}

		public void LoadMedia(Uri uri, TElement element)
		{
			CancelExistingRequest(element);
			TFetchingEntry entry;
			FetchStatus fetchStatus = EnsureEntryFor(uri, out entry);
			entry.AddTargetElement(element);
			if (element != null)
			{
				TagElementWithEntry(element, entry);
			}
			if (fetchStatus != FetchStatus.AlreadyInFlight)
			{
				Task.Run(() => LoadMediaInner(entry));
			}
		}

		protected void MarkRequestAsInactive(TFetchingEntry entry)
		{
			lock (_requestLock)
			{
				_activeRequests.Remove(entry.Hash);
			}
		}

		internal void CancelExistingRequest(TElement element)
		{
			if (element != null)
			{
				TFetchingEntry target = null;
				WeakReference<TFetchingEntry> fetchEntry = GetFetchEntry(element);
				if (fetchEntry != null && fetchEntry.TryGetTarget(out target) && target.RemoveTargetElement(element) == 0)
				{
					MarkRequestAsInactive(target);
				}
				ClearFetchEntry(element);
			}
		}

		private void RetryFailedRequests()
		{
			List<TFetchingEntry> list;
			lock (_requestLock)
			{
				list = (from p in _activeRequests.Where((KeyValuePair<int, TFetchingEntry> p) =>
					{
						TFetchingEntry value = p.Value;
						return value.FailedToRetrieve;
					})
					select p.Value).ToList();
			}
			List<TFetchingEntry> list2 = list.Where((TFetchingEntry val) => val.AreTargetControlsStillAlive).ToList();
			foreach (TFetchingEntry item in list2)
			{
				list.Remove(item);
			}
			foreach (TFetchingEntry entry in list2)
			{
				Func<Task> function = () => LoadMediaInner(entry);
				Task.Run(function);
			}
			lock (_requestLock)
			{
				foreach (TFetchingEntry item2 in list)
				{
					TFetchingEntry current2 = item2;
					_activeRequests.Remove(current2.Hash);
				}
			}
		}

		public async Task<StorageFolder> GetCacheFolder()
		{
			if (!_context.IsAuthenticated())
			{
				return null;
			}
			if (_cacheFolderPath == null)
			{
				await _context.Settings.GetUserFolderPath();
				_cacheFolderPath = Path.Combine(_context.Session.UserId, _folderName);
			}
			if (_cacheFolder == null)
			{
				try
				{
					StorageFolder local = ApplicationData.Current.LocalFolder;
					_cacheFolder = await local.CreateFolderAsync(_cacheFolderPath, (CreationCollisionOption)3);
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to open cached folder", "GetCacheFolder", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\BaseCache.cs", 298);
					Logger.Error(this, ex.ToString(), "GetCacheFolder", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\BaseCache.cs", 299);
				}
			}
			return _cacheFolder;
		}

		private static string FilenameFor(TFetchingEntry entry)
		{
			return entry.Hash.ToString();
		}

		protected async Task<StorageFile> OpenEntryOnDisk(TFetchingEntry entry)
		{
			if (entry.IsCancelled)
			{
				return null;
			}
			StorageFolder folder = await GetCacheFolder();
			if (folder == null)
			{
				return null;
			}
			string fileName = FilenameFor(entry);
			IStorageItem val = await ErrorIgnoringStorageUtils.TryGetItemAsync(folder, fileName);
			StorageFile val2 = (StorageFile)(object)((val is StorageFile) ? val : null);
			StorageFile file = val2;
			if (file == null || entry.IsCancelled)
			{
				return null;
			}
			await _diskCache.DidOpenFile(file, entry.Hash, fileName);
			return file;
		}

		protected async Task SaveToDisk(TFetchingEntry entry, byte[] bytes)
		{
			if (entry.IsCancelled)
			{
				return;
			}
			StorageFolder folder = await GetCacheFolder();
			if (folder == null)
			{
				return;
			}
			_dirty = true;
			string fileName = FilenameFor(entry);
			if (await _diskCache.WillSaveFile(entry.Hash, fileName, bytes) == DiskCache.WillSaveResponse.ShouldWriteToDisk)
			{
				switch (await WriteBytesToFile(folder, entry, bytes))
				{
				case FileWriteResult.Success:
					await _diskCache.DidSaveFile(fileName);
					break;
				case FileWriteResult.Failure:
					_diskCache.DidNotSaveFile(fileName);
					break;
				}
			}
		}

		private async Task<FileWriteResult> WriteBytesToFile(StorageFolder folder, TFetchingEntry entry, byte[] bytes)
		{
			string fileName = FilenameFor(entry);
			if (bytes != null && bytes.Length != 0)
			{
				try
				{
					await _writeToDiskSemaphore.WaitAsync(entry.CancelToken);
					if (entry.IsCancelled)
					{
						return FileWriteResult.Failure;
					}
					StorageFile file = await folder.CreateFileAsync(fileName, (CreationCollisionOption)3);
					if (entry.IsCancelled)
					{
						return FileWriteResult.Failure;
					}
					await StorageUtils.WriteBytesAsync(file, bytes);
					OnFileSaved(entry, file.Path);
					return FileWriteResult.Success;
				}
				catch (IOException ex)
				{
					Logger.Error(this, ex.ToString(), "WriteBytesToFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\BaseCache.cs", 388);
					return FileWriteResult.Failure;
				}
				finally
				{
					_writeToDiskSemaphore.Release();
				}
			}
			return FileWriteResult.Failure;
		}
	}
}

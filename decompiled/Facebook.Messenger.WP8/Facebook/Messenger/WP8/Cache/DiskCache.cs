using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Messenger.Utilities;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;

namespace Facebook.Messenger.WP8.Cache
{
	internal class DiskCache
	{
		public enum WillSaveResponse
		{
			ShouldWriteToDisk,
			AvoidDiskWrite
		}

		private enum FileStatus
		{
			NeedsToBeWritten,
			Writing,
			Written,
			DeleteRequested,
			Deleting,
			WaitingForDelete
		}

		private class FileState
		{
			public readonly SemaphoreSlim DeleteFinished;

			public FileStatus Status { get; set; }

			public FileState()
			{
				DeleteFinished = new SemaphoreSlim(0, 1);
			}
		}

		private readonly Task<StorageFolder> _cacheFolderCreationTask;

		private readonly object _cacheLock = new object();

		private LruCache<int, string> _lruFilenames;

		private readonly Dictionary<string, FileState> _diskCache = new Dictionary<string, FileState>();

		private readonly Task _initializeTask;

		public DiskCache(long maximumSize, Task<StorageFolder> cacheFolderCreationTask, Task<LruCache<int, string>.Entry[]> savedCacheStateTask)
		{
			DiskCache diskCache = this;
			_cacheFolderCreationTask = cacheFolderCreationTask;
			Func<Task> function = () => diskCache.Initialize(maximumSize, savedCacheStateTask);
			_initializeTask = Task.Run(function);
		}

		public async Task<LruCache<int, string>.Entry[]> GetSerializedState()
		{
			await _initializeTask;
			lock (_cacheLock)
			{
				return _lruFilenames.SerializedState;
			}
		}

		private async Task Initialize(long maximumSize, Task<LruCache<int, string>.Entry[]> savedCacheStateTask)
		{
			LruCache<int, string>.Entry[] savedCacheState = await savedCacheStateTask;
			_lruFilenames = new LruCache<int, string>(maximumSize, savedCacheState);
			_lruFilenames.ItemEvicted += FileWasEvicted;
			if (savedCacheState != null)
			{
				string[] expectedFileNamesOnDisk = savedCacheState.Select((LruCache<int, string>.Entry e) => e.Obj).ToArray();
				await CleanupExtraneousFiles(expectedFileNamesOnDisk);
			}
		}

		private async Task CleanupExtraneousFiles(IEnumerable<string> expectedFileNamesOnDisk)
		{
			try
			{
				StorageFolder folder = await _cacheFolderCreationTask;
				if (folder != null)
				{
					IReadOnlyList<StorageFile> actualFilesOnDisk = await folder.GetFilesAsync((CommonFileQuery)0);
					IEnumerable<string> actualFileNamesOnDisk = actualFilesOnDisk.Select((StorageFile file) => file.Name);
					ILookup<string, string> fileNamesToDelete = actualFileNamesOnDisk.Except(expectedFileNamesOnDisk).ToLookup((string name) => name);
					IEnumerable<StorageFile> filesToDelete = actualFilesOnDisk.Where((StorageFile f) => fileNamesToDelete.Contains(f.Name));
					IEnumerable<Task> deleteTasks = filesToDelete.Select((StorageFile file) => file.DeleteAsync((StorageDeleteOption)1).AsTask());
					await Task.WhenAll(deleteTasks);
				}
			}
			catch (IOException ex)
			{
				Logger.Error(this, "Unable to delete extra cached files", "CleanupExtraneousFiles", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\DiskCache.cs", 103);
				Logger.Error(this, ex.ToString(), "CleanupExtraneousFiles", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Cache\\DiskCache.cs", 104);
			}
		}

		private async void FileWasEvicted(string fileName)
		{
			bool shouldDelete = false;
			lock (_cacheLock)
			{
				FileState value;
				if (!_diskCache.TryGetValue(fileName, out value))
				{
					FileState fileState = (_diskCache[fileName] = new FileState
					{
						Status = FileStatus.Written
					});
					value = fileState;
				}
				switch (value.Status)
				{
				case FileStatus.Written:
				case FileStatus.WaitingForDelete:
					value.Status = FileStatus.Deleting;
					shouldDelete = true;
					break;
				case FileStatus.Writing:
					value.Status = FileStatus.DeleteRequested;
					break;
				}
			}
			if (shouldDelete)
			{
				await DeleteFile(fileName);
			}
		}

		private async Task DeleteFile(string fileName)
		{
			StorageFolder folder = await _cacheFolderCreationTask;
			if (folder == null)
			{
				return;
			}
			await ErrorIgnoringStorageUtils.DeleteIfExists(folder, fileName, (StorageDeleteOption)1);
			lock (_cacheLock)
			{
				FileState fileState = _diskCache[fileName];
				if (fileState.Status == FileStatus.WaitingForDelete)
				{
					fileState.DeleteFinished.Release();
				}
				else
				{
					_diskCache.Remove(fileName);
				}
			}
		}

		public async Task DidOpenFile(StorageFile file, int hash, string fileName)
		{
			await _initializeTask;
			BasicProperties props = await file.GetBasicPropertiesAsync();
			lock (_cacheLock)
			{
				string obj;
				if (!_lruFilenames.Lookup(hash, out obj))
				{
					_lruFilenames.Add(hash, fileName, (long)props.Size);
					FileState value;
					if (!_diskCache.TryGetValue(fileName, out value))
					{
						FileState fileState = (_diskCache[fileName] = new FileState());
						value = fileState;
						value.Status = FileStatus.Written;
					}
				}
			}
		}

		public async Task<WillSaveResponse> WillSaveFile(int hash, string fileName, byte[] bytes)
		{
			await _initializeTask;
			SemaphoreSlim deleteSemaphore = null;
			lock (_cacheLock)
			{
				string obj;
				if (!_lruFilenames.Lookup(hash, out obj))
				{
					_lruFilenames.Add(hash, fileName, bytes.Length);
				}
				FileState value;
				if (!_diskCache.TryGetValue(fileName, out value))
				{
					FileState fileState = (_diskCache[fileName] = new FileState());
					value = fileState;
					value.Status = FileStatus.NeedsToBeWritten;
				}
				switch (value.Status)
				{
				case FileStatus.Writing:
				case FileStatus.Written:
				case FileStatus.WaitingForDelete:
					return WillSaveResponse.AvoidDiskWrite;
				case FileStatus.DeleteRequested:
					value.Status = FileStatus.Writing;
					return WillSaveResponse.ShouldWriteToDisk;
				case FileStatus.NeedsToBeWritten:
					value.Status = FileStatus.Writing;
					break;
				case FileStatus.Deleting:
					deleteSemaphore = value.DeleteFinished;
					value.Status = FileStatus.WaitingForDelete;
					break;
				}
			}
			if (deleteSemaphore != null)
			{
				await deleteSemaphore.WaitAsync();
				lock (_cacheLock)
				{
					FileState fileState3 = _diskCache[fileName];
					fileState3.Status = FileStatus.Writing;
				}
			}
			return WillSaveResponse.ShouldWriteToDisk;
		}

		public async Task DidSaveFile(string fileName)
		{
			bool shouldDeleteFile = false;
			lock (_cacheLock)
			{
				FileState fileState = _diskCache[fileName];
				switch (fileState.Status)
				{
				case FileStatus.Writing:
					fileState.Status = FileStatus.Written;
					break;
				case FileStatus.DeleteRequested:
					shouldDeleteFile = true;
					fileState.Status = FileStatus.Deleting;
					break;
				}
			}
			if (shouldDeleteFile)
			{
				await DeleteFile(fileName);
			}
		}

		public void DidNotSaveFile(string fileName)
		{
			lock (_cacheLock)
			{
				FileState fileState = _diskCache[fileName];
				switch (fileState.Status)
				{
				case FileStatus.Writing:
				case FileStatus.DeleteRequested:
					_diskCache.Remove(fileName);
					break;
				case FileStatus.Written:
					break;
				}
			}
		}
	}
}

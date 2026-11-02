using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Stickers
{
	public class StickerPackProgressHelper
	{
		private const int MaxDownloadCount = 5;

		private readonly IDispatcher _uiDispatcher;

		private readonly IStickerStorage _stickerStorage;

		private readonly StickerPackCache _stickerCache;

		private readonly SemaphoreSlim _downloadLock = new SemaphoreSlim(5);

		private readonly List<string> _downloadingStickerPackIds = new List<string>();

		private WeakReference<IStickerPackProgressHandler> _stickerPackProgressHandler;

		public StickerPackProgressHelper(IDispatcher uiDispatcher, IStickerStorage stickerStorage, StickerPackCache stickerCache)
		{
			_uiDispatcher = uiDispatcher;
			_stickerStorage = stickerStorage;
			_stickerCache = stickerCache;
		}

		public void AddProgressHandler(IStickerPackProgressHandler handler)
		{
			if (_stickerPackProgressHandler != null)
			{
				Logger.Warning(this, "Replacing existing progress handler.", "AddProgressHandler", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\StickerPackProgressHelper.cs", 45);
			}
			_stickerPackProgressHandler = new WeakReference<IStickerPackProgressHandler>(handler);
		}

		public void RemoveProgressHandler(IStickerPackProgressHandler handler)
		{
			if (_stickerPackProgressHandler == null)
			{
				Logger.Warning(this, "Attempted to remove progress handler before a handler was added.", "RemoveProgressHandler", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\StickerPackProgressHelper.cs", 55);
				return;
			}
			ExecuteOnProgressHandlerAndPruneIfNotAlive((IStickerPackProgressHandler t) =>
			{
				if (t == handler)
				{
					_stickerPackProgressHandler = null;
				}
				else
				{
					Logger.Warning(this, "Attempted to remove handler after a new handler was added.", "RemoveProgressHandler", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\StickerPackProgressHelper.cs", 67);
				}
			});
		}

		public async Task<bool> DownloadStickerPack(StickerPack stickerPack, CancellationToken cancelToken)
		{
			bool downloadSucceeded = false;
			try
			{
				int num = default(int);
				int num2 = num;
				int num3 = 0;
				try
				{
					lock (_downloadingStickerPackIds)
					{
						_downloadingStickerPackIds.Add(stickerPack.Id);
					}
					ExecuteOnProgressHandlerAndPruneIfNotAlive((IStickerPackProgressHandler t) =>
					{
						t.DownloadStarted(stickerPack.Id);
					});
					return await Task.Run(async () =>
					{
						await _downloadLock.WaitAsync(cancelToken);
						downloadSucceeded = await _stickerStorage.DownloadStickerPack(stickerPack, cancelToken, (double p) =>
						{
							UpdateStickerPackDownloadProgress(stickerPack.Id, p);
						});
						if (downloadSucceeded)
						{
							await _stickerCache.SaveStickerPackDownloaded(stickerPack.Id);
						}
						else if (!cancelToken.IsCancellationRequested)
						{
							Logger.Warning(this, string.Format("Failed to download sticker pack: {0}, Id: {1}", new object[2] { stickerPack.Name, stickerPack.Id }), "DownloadStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\StickerPackProgressHelper.cs", 99);
						}
						return downloadSucceeded;
					}, cancelToken);
				}
				catch (OperationCanceledException)
				{
					return false;
				}
			}
			finally
			{
				lock (_downloadingStickerPackIds)
				{
					_downloadingStickerPackIds.Remove(stickerPack.Id);
				}
				if (!downloadSucceeded)
				{
					ExecuteOnProgressHandlerAndPruneIfNotAlive((IStickerPackProgressHandler t) =>
					{
						t.DownloadFailed(stickerPack.Id);
					});
				}
				_downloadLock.Release();
			}
		}

		public List<string> GetDownloadingStickerPackIds()
		{
			lock (_downloadingStickerPackIds)
			{
				return new List<string>(_downloadingStickerPackIds);
			}
		}

		private void UpdateStickerPackDownloadProgress(string stickerPackId, double progress)
		{
			ExecuteOnProgressHandlerAndPruneIfNotAlive((IStickerPackProgressHandler t) =>
			{
				t.ProgressUpdated(stickerPackId, progress);
			});
		}

		private void ExecuteOnProgressHandlerAndPruneIfNotAlive(Action<IStickerPackProgressHandler> action)
		{
			if (_stickerPackProgressHandler == null)
			{
				return;
			}
			IStickerPackProgressHandler target;
			if (_stickerPackProgressHandler.TryGetTarget(out target))
			{
				_uiDispatcher.RunAsync(() =>
				{
					action(target);
				});
			}
			else
			{
				_stickerPackProgressHandler = null;
			}
		}
	}
}

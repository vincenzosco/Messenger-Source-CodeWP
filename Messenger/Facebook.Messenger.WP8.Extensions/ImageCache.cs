using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Strategy;
using Facebook.Messenger.Utilities;
using Facebook.Messenger.WP8.Cache;
using Facebook.Services.Analytics;
using Facebook.Services.Analytics.Logging;
using Microsoft.Practices.ServiceLocation;
using Windows.Storage;

namespace Facebook.Messenger.WP8.Extensions
{
	internal class ImageCache : BaseCache<Image, ImageFetchingEntry>
	{
		private const long MaxCacheSizeInBytes = 1048576L;

		private const int MaxNumberofFetchAttempts = 5;

		private const string ImageCacheFolder = ".local_cache";

		private readonly object _cacheLock = new object();

		private LruCache<int, byte[]> _lruCache;

		private readonly ExponentialFallback<ImageFetchingEntry, NullResponse> _imageFetchFallback;

		private readonly Service _analyticsService;

		public ImageCache()
			: base(".local_cache", 5242880L, FailedRequestRetryPolicy.OnNetworkGained)
		{
			_imageFetchFallback = new ExponentialFallback<ImageFetchingEntry, NullResponse>(LoadImageWithExponentialFallback);
			_lruCache = new LruCache<int, byte[]>(1048576L);
			_analyticsService = ServiceLocator.Current.GetInstance<Service>();
		}

		protected override async Task LoadMediaInner(ImageFetchingEntry entry)
		{
			CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(entry.CancelToken, _networkTokenSource.Token);
			await _imageFetchFallback.Execute(source.Token, entry, 5000, 60000);
		}

		protected override ImageFetchingEntry CreateEntryFor(Uri uri, CancellationToken token)
		{
			return new ImageFetchingEntry(uri, token);
		}

		protected override void TagElementWithEntry(Image element, ImageFetchingEntry entry)
		{
			ImageCacheExtensions.SetFetchEntry((FrameworkElement)(object)element, entry);
		}

		protected override WeakReference<ImageFetchingEntry> GetFetchEntry(Image element)
		{
			return ImageCacheExtensions.GetFetchEntry((FrameworkElement)(object)element);
		}

		protected override void ClearFetchEntry(Image element)
		{
			((DependencyObject)element).ClearValue(ImageCacheExtensions.FetchingEntryProperty);
		}

		private async Task<StrategyResponse<NullResponse>> LoadImageWithExponentialFallback(int attempt, CancellationToken token, ImageFetchingEntry entry)
		{
			try
			{
				if (LoadFromMemory(entry) || await LoadFromDisk(entry) || await LoadFromWeb(entry))
				{
					MarkRequestAsInactive(entry);
					List<Image> activeTargets = entry.GetActiveTargets();
					foreach (Image item in activeTargets)
					{
						Image targetControl = item;
						App.Dispatcher.Schedule(() =>
						{
							NotifyTargetControl(entry, targetControl);
						});
					}
					return StrategyResponse.Create(ExecutionResult.Success);
				}
				ExecutionResult executionResult = OnImageLoadFailed(entry, attempt);
				return StrategyResponse.Create(executionResult);
			}
			catch
			{
				MarkRequestAsInactive(entry);
				throw;
			}
		}

		private ExecutionResult OnImageLoadFailed(ImageFetchingEntry entry, int attempt)
		{
			if (entry.IsCancelled || attempt == 5)
			{
				MarkRequestAsInactive(entry);
				return ExecutionResult.Cancelled;
			}
			if (_networkStatus.IsConnected())
			{
				return ExecutionResult.Failed;
			}
			entry.FailedToRetrieve = true;
			return ExecutionResult.Cancelled;
		}

		private void NotifyTargetControl(ImageFetchingEntry entry, Image targetControl)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected Obj, but got Unknown
			WeakReference<ImageFetchingEntry> fetchEntry = ImageCacheExtensions.GetFetchEntry((FrameworkElement)(object)targetControl);
			ImageFetchingEntry target;
			if (fetchEntry != null && fetchEntry.TryGetTarget(out target) && target == entry)
			{
				((DependencyObject)targetControl).ClearValue(ImageCacheExtensions.FetchingEntryProperty);
				BitmapImage val = new BitmapImage();
				val.CreateOptions = (BitmapCreateOptions)0;
				BitmapImage val2 = val;
				bool flag = true;
				try
				{
					((BitmapSource)val2).SetSource((Stream)new MemoryStream(entry.ImageData, 0, entry.ImageData.Length));
					targetControl.Source = (ImageSource)(object)val2;
				}
				catch (Exception)
				{
					flag = false;
				}
				Action<BitmapImage> imageLoadedHandler = ImageCacheExtensions.GetImageLoadedHandler((FrameworkElement)(object)targetControl);
				if (imageLoadedHandler != null && flag)
				{
					imageLoadedHandler(val2);
				}
			}
		}

		private async Task<bool> LoadFromWeb(ImageFetchingEntry entry)
		{
			try
			{
				if (entry.IsCancelled)
				{
					return false;
				}
				CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(entry.CancelToken, _networkTokenSource.Token);
				using (HttpResponseMessage response = await _httpClient.GetAsync(entry.SourceUri, source.Token))
				{
					if (response != null && response.IsSuccessStatusCode)
					{
						entry.ImageData = await response.Content.ReadAsByteArrayAsync();
						DataConsumptionLogger.LogDataEvent(downloadedBytes: (entry.ImageData != null) ? entry.ImageData.Length : 0, service: _analyticsService, type: DataType.Image, uploadedBytes: 0L);
					}
				}
			}
			catch (TaskCanceledException ex)
			{
				Logger.Warning(this, "Image request from web was cancelled", "LoadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 414);
				Logger.Warning(this, ex.ToString(), "LoadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 415);
				return false;
			}
			catch (HttpRequestException ex2)
			{
				Logger.Warning(this, "Failed to load image from web", "LoadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 420);
				Logger.Warning(this, ex2.ToString(), "LoadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 421);
				return false;
			}
			if (entry.IsCancelled || entry.ImageData == null || entry.ImageData.Length == 0)
			{
				return false;
			}
			AddToMemoryCache(entry);
			if (entry.IsCancelled)
			{
				return false;
			}
			SaveToDisk(entry, entry.ImageData).DoNotWait();
			return true;
		}

		private async Task<bool> LoadFromDisk(ImageFetchingEntry entry)
		{
			try
			{
				StorageFile file = await OpenEntryOnDisk(entry);
				if (file == null)
				{
					return false;
				}
				entry.ImageData = await StorageUtils.ReadBytesAsync(file);
			}
			catch (IOException ex)
			{
				Logger.Warning(this, "Failed to load image from disk", "LoadFromDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 453);
				Logger.Warning(this, ex.ToString(), "LoadFromDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 454);
				return false;
			}
			if (entry.IsCancelled || entry.ImageData == null || entry.ImageData.Length == 0)
			{
				return false;
			}
			AddToMemoryCache(entry);
			return true;
		}

		private bool LoadFromMemory(ImageFetchingEntry entry)
		{
			lock (_cacheLock)
			{
				byte[] obj;
				if (_lruCache.Lookup(entry.Hash, out obj))
				{
					entry.ImageData = obj;
					return true;
				}
				return false;
			}
		}

		private void AddToMemoryCache(ImageFetchingEntry entry)
		{
			lock (_cacheLock)
			{
				byte[] obj;
				if (!_lruCache.Lookup(entry.Hash, out obj))
				{
					_lruCache.Add(entry.Hash, entry.ImageData, entry.ImageData.Length);
				}
				else if (obj.Length != entry.ImageData.Length)
				{
					Logger.Warning(this, string.Format("New Entry - Uri: {0}, ImageData: {1}", entry.SourceUri, entry.ImageData.Length), "AddToMemoryCache", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 492);
					Logger.Warning(this, string.Format("Old Entry - ImageData: {0}", obj.Length), "AddToMemoryCache", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\ImagePlaceholderExtension.cs", 493);
					_lruCache.Remove(entry.Hash);
					if (entry.ImageData.Length > 0)
					{
						_lruCache.Add(entry.Hash, entry.ImageData, entry.ImageData.Length);
					}
				}
			}
		}

		public override async Task Purge()
		{
			await base.Purge();
			lock (_cacheLock)
			{
				_lruCache = new LruCache<int, byte[]>(1048576L);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.WP8.Cache;
using Windows.Storage;

namespace Facebook.Messenger.WP8.Extensions;

internal class VideoCache : BaseCache<MediaElement, VideoFetchingEntry>
{
	private const string VideoCacheFolder = ".temp_cache";

	public VideoCache()
		: base(".temp_cache", 10485760L, FailedRequestRetryPolicy.Never)
	{
	}

	protected override async Task LoadMediaInner(VideoFetchingEntry entry)
	{
		if (await LoadFromDisk(entry) || await DownloadFromWeb(entry))
		{
			List<MediaElement> targets = entry.GetActiveTargets();
			await App.Dispatcher.RunAsync(() =>
			{
				foreach (MediaElement item in targets)
				{
					WeakReference<VideoFetchingEntry> fetchEntry = MediaElementCacheExtensions.GetFetchEntry((FrameworkElement)(object)item);
					if (fetchEntry != null && fetchEntry.TryGetTarget(out var target) && target == entry)
					{
						((DependencyObject)item).ClearValue(MediaElementCacheExtensions.FetchingEntryProperty);
						Uri obj = (item.Source = entry.LocalUri ?? entry.SourceUri);
						MediaElementCacheExtensions.GetVideoDownloadedHandler((FrameworkElement)(object)item)?.Invoke(obj);
					}
				}
			});
		}
		MarkRequestAsInactive(entry);
	}

	private async Task<bool> LoadFromDisk(VideoFetchingEntry entry)
	{
		int num = default;
		_ = num;
		_ = 0;
		try
		{
			StorageFile file = await OpenEntryOnDisk(entry);
			if (file == null)
			{
				return false;
			}
			Uri.TryCreate(FileUtil.ConvertPathToUrl(file.Path), UriKind.Absolute, out var uri);
			entry.LocalUri = uri;
			if (entry.IsCancelled || entry.LocalUri == null)
			{
				return false;
			}
			return true;
		}
		catch (IOException ex)
		{
			Logger.Warning(this, "Failed to load video from disk", "LoadFromDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 185);
			Logger.Warning(this, ex.ToString(), "LoadFromDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 186);
			return false;
		}
	}

	private async Task<bool> DownloadFromWeb(VideoFetchingEntry entry)
	{
		try
		{
			if (entry.IsCancelled)
			{
				return false;
			}
			CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(entry.CancelToken, _networkTokenSource.Token);
			using HttpResponseMessage response = await _httpClient.GetAsync(entry.SourceUri, source.Token);
			if (response != null && response.IsSuccessStatusCode)
			{
				await SaveToDisk(entry, await response.Content.ReadAsByteArrayAsync());
			}
			else
			{
				Logger.Warning(this, "Failed to download video from web.", "DownloadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 208);
				if (response != null)
				{
					Logger.Warning(this, $"HTTP Status code: {response.StatusCode}", "DownloadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 211);
				}
			}
		}
		catch (TaskCanceledException ex)
		{
			Logger.Warning(this, "Video download from web was cancelled", "DownloadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 218);
			Logger.Warning(this, ex.ToString(), "DownloadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 219);
			return false;
		}
		catch (HttpRequestException ex2)
		{
			Logger.Warning(this, "Failed to download video from web", "DownloadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 224);
			Logger.Warning(this, ex2.ToString(), "DownloadFromWeb", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\MediaElementCacheExtensions.cs", 225);
			return false;
		}
		if (entry.IsCancelled || entry.LocalUri == null)
		{
			return false;
		}
		return true;
	}

	protected override VideoFetchingEntry CreateEntryFor(Uri uri, CancellationToken token)
	{
		return new VideoFetchingEntry(uri, token);
	}

	protected override void TagElementWithEntry(MediaElement element, VideoFetchingEntry entry)
	{
		MediaElementCacheExtensions.SetFetchEntry((FrameworkElement)(object)element, entry);
	}

	protected override WeakReference<VideoFetchingEntry> GetFetchEntry(MediaElement element)
	{
		return MediaElementCacheExtensions.GetFetchEntry((FrameworkElement)(object)element);
	}

	protected override void ClearFetchEntry(MediaElement element)
	{
		((DependencyObject)element).ClearValue(MediaElementCacheExtensions.FetchingEntryProperty);
	}

	protected override void OnFileSaved(VideoFetchingEntry entry, string filepath)
	{
		Uri.TryCreate(FileUtil.ConvertPathToUrl(filepath), UriKind.Absolute, out var result);
		entry.LocalUri = result;
	}
}

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Messenger.Utilities;
using Facebook.Models;
using Facebook.Services.Analytics;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Stickers;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger.WP8.Utilities.Stickers
{
	public class StickerStorage : IStickerStorage
	{
		private readonly FacebookContext _context;

		private readonly HttpClient _httpClient;

		private readonly Facebook.Services.Analytics.Service _analyticService;

		private string _stickerFolderPath;

		private StorageFolder _stickerFolder;

		public StickerStorage(FacebookContext context, Facebook.Services.Analytics.Service analyticService)
		{
			_context = context;
			_analyticService = analyticService;
			_httpClient = new HttpClient();
		}

		private async Task<StorageFolder> GetStickersFolder()
		{
			if (!_context.IsAuthenticated())
			{
				return null;
			}
			if (_stickerFolderPath == null)
			{
				await _context.Settings.GetUserFolderPath();
				_stickerFolderPath = Path.Combine(_context.Session.UserId, "Stickers");
			}
			if (_stickerFolder == null)
			{
				try
				{
					StorageFolder local = ApplicationData.Current.LocalFolder;
					_stickerFolder = await local.CreateFolderAsync(_stickerFolderPath, (CreationCollisionOption)3);
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to open sticker folder", "GetStickersFolder", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\Stickers\\StickerStorage.cs", 59);
					Logger.Error(this, ex.ToString(), "GetStickersFolder", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\Stickers\\StickerStorage.cs", 60);
				}
			}
			return _stickerFolder;
		}

		public async Task<Uri> GenerateStickerUri(string stickerPackId, string stickerId, bool animatedSticker)
		{
			string path = Path.Combine(path2: GetStickerFileName(stickerId, animatedSticker), path1: (await GetStickersFolder()).Path);
			return new Uri(path, UriKind.RelativeOrAbsolute);
		}

		public async Task<bool> DownloadStickerPack(StickerPack sp, CancellationToken cancellationToken, Action<double> progressUpdate = null)
		{
			for (int i = 0; i < sp.Stickers.Count; i++)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}
				Sticker sticker = sp.Stickers[i];
				if (!(await SaveStickerToDisk(sticker.Id, sticker.ThreadImage.Uri, false, cancellationToken)))
				{
					return false;
				}
				if (sp.Stickers[i].AnimatedImage != null && !(await SaveStickerToDisk(sticker.Id, sticker.AnimatedImage.Uri, true, cancellationToken)))
				{
					return false;
				}
				progressUpdate?.Invoke(100.0 * ((double)(i + 1) / (double)sp.Stickers.Count));
			}
			return true;
		}

		private async Task<bool> SaveStickerToDisk(string stickerId, Uri stickerUrl, bool animatedSticker, CancellationToken cancellationToken)
		{
			try
			{
				string fileName = GetStickerFileName(stickerId, animatedSticker);
				StorageFolder stickerFolder = await GetStickersFolder();
				if (stickerFolder == null)
				{
					return false;
				}
				StorageFile file = await stickerFolder.CreateFileAsync(fileName, (CreationCollisionOption)3);
				IRandomAccessStreamWithContentType stream = await file.OpenReadAsync();
				try
				{
					if (((IRandomAccessStream)stream).Size != 0)
					{
						Logger.Info($"Sticker {stickerId} already downloaded. Skipping write to disk.", "SaveStickerToDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\Stickers\\StickerStorage.cs", 126);
						return true;
					}
				}
				finally
				{
					((IDisposable)stream)?.Dispose();
				}
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}
				using (HttpResponseMessage response = await _httpClient.GetAsync(stickerUrl, cancellationToken))
				{
					if (response != null && response.IsSuccessStatusCode)
					{
						byte[] bytes = await response.Content.ReadAsByteArrayAsync();
						DataConsumptionLogger.LogDataEvent(downloadedBytes: (bytes != null) ? bytes.Length : 0, service: _analyticService, type: DataType.Sticker, uploadedBytes: 0L);
						await StorageUtils.WriteBytesAsync(file, bytes);
						return true;
					}
					return false;
				}
			}
			catch (OperationCanceledException)
			{
				Logger.Info(this, "Sticker download cancelled.", "SaveStickerToDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\Stickers\\StickerStorage.cs", 151);
				return false;
			}
			catch (Exception ex2)
			{
				Logger.Warning(this, "Failed to download sticker from web", "SaveStickerToDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\Stickers\\StickerStorage.cs", 156);
				Logger.Warning(this, ex2.ToString(), "SaveStickerToDisk", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\Stickers\\StickerStorage.cs", 157);
				return false;
			}
		}

		private static string GetStickerFileName(string stickerId, bool animated)
		{
			string text = stickerId;
			if (animated)
			{
				text += "_ani";
			}
			return text;
		}

		public async Task<Uri> GetStickerUri(string stickerId, bool animated)
		{
			StorageFile sticker = await GetStickerFile(GetStickerFileName(stickerId, animated));
			if (sticker == null)
			{
				return null;
			}
			return new Uri(sticker.Path, UriKind.RelativeOrAbsolute);
		}

		private async Task<StorageFile> GetStickerFile(string filename)
		{
			IStorageItem val = await ErrorIgnoringStorageUtils.TryGetItemAsync(await GetStickersFolder(), filename);
			StorageFile val2 = (StorageFile)(object)((val is StorageFile) ? val : null);
			StorageFile item = val2;
			if (item == null)
			{
				return null;
			}
			return item;
		}

		public async Task DeleteStickerPack(StickerPack sp)
		{
			foreach (Sticker sticker in sp.Stickers)
			{
				await DeleteSticker(GetStickerFileName(sticker.Id, false));
				if (sticker.AnimatedImage != null)
				{
					await DeleteSticker(GetStickerFileName(sticker.Id, true));
				}
			}
		}

		private async Task DeleteSticker(string id)
		{
			StorageFile sticker = await GetStickerFile(id);
			if (sticker != null)
			{
				await sticker.DeleteAsync();
			}
		}

		public void Reset()
		{
			_stickerFolder = null;
			_stickerFolderPath = null;
		}
	}
}

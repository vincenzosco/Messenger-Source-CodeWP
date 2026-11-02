using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger
{
	public class UserDataStorage : IUserDataStorage
	{
		private readonly FacebookContext _context;

		public UserDataStorage(FacebookContext context)
		{
			_context = context;
		}

		public async Task<bool> DataExists(string key)
		{
			string folder = await GetFolder();
			if (folder == null)
			{
				return false;
			}
			string path = Path.Combine(folder, key);
			return File.Exists(path);
		}

		public async Task<string> GetData(string key)
		{
			StorageFile file = await StorageFile.GetFileFromPathAsync(await GetFile(key));
			byte[] bytes = null;
			IRandomAccessStream stream = await file.OpenAsync((FileAccessMode)0);
			try
			{
				using (Stream readStream = ((IInputStream)(object)stream).AsStreamForRead())
				{
					bytes = new byte[readStream.Length];
					await readStream.ReadAsync(bytes, 0, bytes.Length);
				}
			}
			finally
			{
				((IDisposable)stream)?.Dispose();
			}
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		public async Task SetData(string key, string data)
		{
			string filePath = await GetFile(key);
			File.Delete(filePath);
			using (StreamWriter stream = File.CreateText(filePath))
			{
				await stream.WriteAsync(data);
			}
		}

		private async Task<string> GetFile(string key)
		{
			return Path.Combine(await GetFolder(), key);
		}

		private async Task<string> GetFolder()
		{
			string rootPath = await _context.Settings.GetUserFolderPath();
			if (rootPath == "")
			{
				return null;
			}
			return (await (await StorageFolder.GetFolderFromPathAsync(rootPath)).CreateFolderAsync("DataStorage", (CreationCollisionOption)3)).Path;
		}
	}
}

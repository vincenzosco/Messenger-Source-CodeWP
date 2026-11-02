using System;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Settings;
using Facebook.Messenger.Base;
using Windows.Storage;

namespace Facebook.Messenger.Settings
{
	public class UserSettings : BaseSettings, IUserSettings
	{
		private static object _lock = new object();

		private readonly string _userId;

		public UserSettings(string userId)
		{
			_userId = userId;
		}

		public async Task<string> GetUserFolderPath()
		{
			await InitializeUserFolder();
			return Path.Combine(BaseSettings.LocalFolder.Path, _userId);
		}

		public async Task DeleteUserFolder()
		{
			try
			{
				string path = Path.Combine(BaseSettings.LocalFolder.Path, _userId);
				StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
				if (folder != null)
				{
					await folder.DeleteRecursivelyAsync((StorageDeleteOption)0);
				}
			}
			catch (Exception)
			{
			}
		}

		public async Task InitializeUserFolder()
		{
			await BaseSettings.LocalFolder.CreateFolderAsync(_userId, (CreationCollisionOption)3);
		}

		public T GetSetting<T>(string name)
		{
			return GetSetting(name, default(T));
		}

		public T GetSetting<T>(string name, T defaultValue)
		{
			string key = GetKey(name);
			lock (_lock)
			{
				if (BaseSettings.Settings.Contains(key))
				{
					return Serialization.Get<T>(BaseSettings.Settings[key]);
				}
				return defaultValue;
			}
		}

		public bool ContainsSetting(string name)
		{
			string key = GetKey(name);
			lock (_lock)
			{
				return BaseSettings.Settings.Contains(key);
			}
		}

		public void SaveSetting(string name, object obj)
		{
			string key = GetKey(name);
			lock (_lock)
			{
				BaseSettings.Settings[key] = obj;
				BaseSettings.Settings.Save();
			}
		}

		public void RemoveSetting(string name)
		{
			string key = GetKey(name);
			lock (_lock)
			{
				if (BaseSettings.Settings.Contains(key))
				{
					BaseSettings.Settings.Remove(key);
				}
				BaseSettings.Settings.Save();
			}
		}

		private string GetKey(string name)
		{
			return BaseSettings.GetUserKey(_userId, name);
		}
	}
}

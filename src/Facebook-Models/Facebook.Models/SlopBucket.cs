using System.Threading.Tasks;
using Facebook.Base.Settings;

namespace Facebook.Models
{
	internal class SlopBucket : IUserSettings
	{
		public Task<string> GetUserFolderPath()
		{
			return Task.FromResult(string.Empty);
		}

		public Task DeleteUserFolder()
		{
			return Task.FromResult(false);
		}

		public Task InitializeUserFolder()
		{
			return Task.FromResult(string.Empty);
		}

		public T GetSetting<T>(string key)
		{
			return GetSetting(key, default(T));
		}

		public T GetSetting<T>(string key, T defaultValue)
		{
			return default(T);
		}

		public void SaveSetting(string key, object obj)
		{
		}

		public void RemoveSetting(string key)
		{
		}

		public bool ContainsSetting(string key)
		{
			return false;
		}
	}
}

using System.Threading.Tasks;

namespace Facebook.Base.Settings
{
	public interface IUserSettings
	{
		Task<string> GetUserFolderPath();

		Task DeleteUserFolder();

		Task InitializeUserFolder();

		T GetSetting<T>(string key);

		T GetSetting<T>(string key, T defaultValue);

		void SaveSetting(string key, object obj);

		void RemoveSetting(string key);

		bool ContainsSetting(string key);
	}
}

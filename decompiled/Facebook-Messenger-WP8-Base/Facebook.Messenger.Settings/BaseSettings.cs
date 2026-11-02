using Windows.Storage;

namespace Facebook.Messenger.Settings
{
	public abstract class BaseSettings
	{
		public static AppSettingsManager Settings
		{
			get
			{
				return AppSettingsManager.Settings;
			}
		}

		public static StorageFolder LocalFolder
		{
			get
			{
				return ApplicationData.Current.LocalFolder;
			}
		}

		public static string GetUserKey(string userId, string name)
		{
			return string.Format("{0}_{1}", userId, name);
		}

		public static bool IsUserKey(string userId, string key)
		{
			string value = string.Format("{0}_", userId);
			return key.StartsWith(value);
		}
	}
}

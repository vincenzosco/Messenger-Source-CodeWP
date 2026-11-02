using Windows.Storage;

namespace Facebook.Messenger.Settings;

public abstract class BaseSettings
{
	public static AppSettingsManager Settings => AppSettingsManager.Settings;

	public static StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;

	public static string GetUserKey(string userId, string name)
	{
		return $"{userId}_{name}";
	}

	public static bool IsUserKey(string userId, string key)
	{
		string value = $"{userId}_";
		return key.StartsWith(value);
	}
}

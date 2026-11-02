using System.IO.IsolatedStorage;

namespace Microsoft.Phone.Applications.Common
{
	public static class ApplicationSettingHelper
	{
		public static TValue TryGetValueWithDefault<TValue>(string key, TValue defaultValue)
		{
			TValue result = defaultValue;
			if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
			{
				object obj = IsolatedStorageSettings.ApplicationSettings[key];
				if (obj is TValue)
				{
					result = (TValue)obj;
				}
			}
			return result;
		}

		public static bool AddOrUpdateValue(string key, object value)
		{
			bool result = false;
			if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
			{
				if (IsolatedStorageSettings.ApplicationSettings[key] != value)
				{
					IsolatedStorageSettings.ApplicationSettings[key] = value;
					result = true;
				}
			}
			else
			{
				IsolatedStorageSettings.ApplicationSettings.Add(key, value);
				result = true;
			}
			return result;
		}

		public static void Save()
		{
			IsolatedStorageSettings.ApplicationSettings.Save();
		}
	}
}

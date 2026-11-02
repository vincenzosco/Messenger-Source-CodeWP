using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Base.Settings;

namespace Facebook.Messenger.Settings
{
	public class UserSettingsProvider : BaseSettings, IUserSettingsProvider
	{
		public IUserSettings GetOrCreate(string id)
		{
			return new UserSettings(id);
		}

		public void Clear(string id)
		{
			try
			{
				List<string> list = (from string key in BaseSettings.Settings.Keys
					where BaseSettings.IsUserKey(id, key)
					select key).ToList();
				foreach (string item in list)
				{
					BaseSettings.Settings.Remove(item);
				}
				BaseSettings.Settings.Save();
			}
			catch (Exception)
			{
			}
		}
	}
}

using System.Collections.Generic;
using System.IO.IsolatedStorage;

namespace Facebook.Messenger.Settings
{
	public class AppSettingsManager
	{
		private readonly object _lockObject;

		public static AppSettingsManager Settings { get; private set; }

		public object this[string key]
		{
			get
			{
				lock (_lockObject)
				{
					return IsolatedStorageSettings.ApplicationSettings[key];
				}
			}
			set
			{
				lock (_lockObject)
				{
					IsolatedStorageSettings.ApplicationSettings[key] = value;
				}
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return (ICollection<string>)IsolatedStorageSettings.ApplicationSettings.Keys;
			}
		}

		static AppSettingsManager()
		{
			Settings = new AppSettingsManager();
		}

		private AppSettingsManager()
		{
			_lockObject = new object();
		}

		public void Save()
		{
			lock (_lockObject)
			{
				try
				{
					IsolatedStorageSettings.ApplicationSettings.Save();
				}
				catch (IsolatedStorageException)
				{
				}
			}
		}

		public void Add(string key, object value)
		{
			lock (_lockObject)
			{
				IsolatedStorageSettings.ApplicationSettings.Add(key, value);
			}
		}

		public bool Remove(string key)
		{
			lock (_lockObject)
			{
				return IsolatedStorageSettings.ApplicationSettings.Remove(key);
			}
		}

		public bool Contains(string key)
		{
			lock (_lockObject)
			{
				return IsolatedStorageSettings.ApplicationSettings.Contains(key);
			}
		}

		public bool TryGetValue<T>(string key, out T value)
		{
			lock (_lockObject)
			{
				object value2;
				bool result = TryGetValue(key, out value2);
				value = (T)value2;
				return result;
			}
		}

		public bool TryGetValue(string key, out object value)
		{
			lock (_lockObject)
			{
				return IsolatedStorageSettings.ApplicationSettings.TryGetValue<object>(key, out value);
			}
		}
	}
}

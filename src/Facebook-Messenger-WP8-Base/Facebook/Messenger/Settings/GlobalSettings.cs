using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Settings;
using Facebook.Messenger.Base;
using Facebook.Models;
using Windows.Phone.System.Analytics;
using Windows.Storage;

namespace Facebook.Messenger.Settings
{
	public class GlobalSettings : BaseSettings, IGlobalSettings
	{
		private const string CurrentUserKey = "CurrentUser";

		private const string FacebookMachineIdKey = "FBMachineId";

		private const string UserIdKey = "UserId";

		private const string NameKey = "Name";

		private const string TypeKey = "Type";

		private const string EmployeeKey = "IsEmployee";

		public string DeviceIdentifier { get; private set; }

		public string FacebookMachineId
		{
			get
			{
				if (BaseSettings.Settings.Contains("FBMachineId"))
				{
					return BaseSettings.Settings["FBMachineId"] as string;
				}
				return null;
			}
			set
			{
				BaseSettings.Settings["FBMachineId"] = value;
				BaseSettings.Settings.Save();
			}
		}

		public GlobalSettings()
		{
			DeviceIdentifier = NamedBasedUuidGenerator.Generate(HostInformation.PublisherHostId, NamedBasedUuidGenerator.Namespace.ObjectId, NamedBasedUuidGenerator.Version.SHA1).ToString("D");
		}

		public void ClearSession(string id)
		{
			BaseSettings.Settings.Remove("CurrentUser");
			BaseSettings.Settings.Remove(id);
			BaseSettings.Settings.Save();
		}

		public bool SaveSession(FacebookSession session)
		{
			try
			{
				string userId = session.UserId;
				BaseSettings.Settings["CurrentUser"] = userId;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("UserId", session.UserId);
				dictionary.Add("Name", session.Profile.Name);
				dictionary.Add("Type", session.Profile.Type);
				dictionary.Add("IsEmployee", session.Profile.IsEmployee.ToString());
				Dictionary<string, string> value = dictionary;
				BaseSettings.Settings[userId] = value;
				BaseSettings.Settings.Save();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public FacebookSession LoadSession()
		{
			string value = null;
			if (!BaseSettings.Settings.TryGetValue("CurrentUser", out value))
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			Dictionary<string, string> value2;
			if (!BaseSettings.Settings.TryGetValue(value, out value2))
			{
				ClearSession(value);
				return null;
			}
			string text = (value2.ContainsKey("UserId") ? value2["UserId"] : string.Empty);
			string name = (value2.ContainsKey("Name") ? value2["Name"] : string.Empty);
			string type = (value2.ContainsKey("Type") ? value2["Type"] : string.Empty);
			string value3 = (value2.ContainsKey("IsEmployee") ? value2["IsEmployee"] : false.ToString());
			if (string.IsNullOrEmpty(text))
			{
				ClearSession(value);
				return null;
			}
			FacebookSession facebookSession = new FacebookSession();
			facebookSession.UserId = text;
			facebookSession.Profile = new FacebookProfile
			{
				Id = text,
				Name = name,
				Type = type,
				IsEmployee = bool.Parse(value3)
			};
			return facebookSession;
		}

		public async Task PurgeUserFolders(params string[] excludes)
		{
			try
			{
				IReadOnlyList<StorageFolder> folders = await BaseSettings.LocalFolder.GetFoldersAsync();
				if (folders == null)
				{
					return;
				}
				foreach (StorageFolder folder in folders)
				{
					Func<string, bool> predicate = (string e) => e == folder.Name;
					if (excludes.FirstOrDefault(predicate) == null)
					{
						await folder.DeleteRecursivelyAsync((StorageDeleteOption)0);
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}

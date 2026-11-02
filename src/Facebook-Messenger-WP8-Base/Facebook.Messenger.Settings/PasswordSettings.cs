using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Facebook.Base.Settings;
using Facebook.Models;

namespace Facebook.Messenger.Settings
{
	public class PasswordSettings : BaseSettings, IPasswordSettings
	{
		private const string SessionKey = "SessionKey";

		private const string AccessToken = "AccessToken";

		private const string SecretName = "Secret";

		private const string UserId = "UserId";

		private const string LoginCredentialsKey = "Credentials";

		private Dictionary<string, string> _vault;

		public PasswordSettings()
		{
			object value = null;
			if (BaseSettings.Settings.TryGetValue("Credentials", out value))
			{
				_vault = value as Dictionary<string, string>;
				return;
			}
			_vault = new Dictionary<string, string>();
			BaseSettings.Settings["Credentials"] = _vault;
		}

		public FacebookCredentials RetrieveCredentials()
		{
			try
			{
				if (_vault == null || !_vault.Any())
				{
					return null;
				}
				FacebookCredentials facebookCredentials = new FacebookCredentials();
				if (_vault.ContainsKey("SessionKey"))
				{
					facebookCredentials.SessionKey = GetUnprotectedDataString(_vault["SessionKey"]);
				}
				if (_vault.ContainsKey("AccessToken"))
				{
					facebookCredentials.AccessToken = GetUnprotectedDataString(_vault["AccessToken"]);
				}
				if (_vault.ContainsKey("Secret"))
				{
					facebookCredentials.Secret = GetUnprotectedDataString(_vault["Secret"]);
				}
				if (_vault.ContainsKey("UserId"))
				{
					facebookCredentials.Id = GetUnprotectedDataString(_vault["UserId"]);
				}
				return facebookCredentials.IsValid() ? facebookCredentials : null;
			}
			catch (Exception)
			{
			}
			return null;
		}

		public bool RemoveCredentials()
		{
			bool result = BaseSettings.Settings.Remove("Credentials");
			BaseSettings.Settings.Save();
			return result;
		}

		private bool SaveCredentials(string sessionKey, string accessToken, string secret, string id)
		{
			if (SaveCredential(sessionKey, "SessionKey") && SaveCredential(accessToken, "AccessToken") && SaveCredential(secret, "Secret"))
			{
				return SaveCredential(id, "UserId");
			}
			return false;
		}

		public bool SaveCredentials(FacebookCredentials credentials)
		{
			bool flag = SaveCredentials(credentials.SessionKey, credentials.AccessToken, credentials.Secret, credentials.Id);
			if (!flag)
			{
				RemoveCredentials();
			}
			else
			{
				BaseSettings.Settings["Credentials"] = _vault;
			}
			return flag;
		}

		private bool SaveCredential(string credential, string name)
		{
			try
			{
				_vault[name] = GetProtectedDataString(credential);
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		private string GetProtectedDataString(string unprotectedData)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(unprotectedData);
			byte[] inArray = ProtectedData.Protect(bytes, (byte[])null);
			return Convert.ToBase64String(inArray);
		}

		private string GetUnprotectedDataString(string protectedData)
		{
			byte[] array = Convert.FromBase64String(protectedData);
			byte[] array2 = ProtectedData.Unprotect(array, (byte[])null);
			return Encoding.UTF8.GetString(array2, 0, array2.Length);
		}
	}
}

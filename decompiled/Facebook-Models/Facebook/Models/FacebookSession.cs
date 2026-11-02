namespace Facebook.Models;

public class FacebookSession
{
	public string UserId { get; set; }

	public string UserName { get; set; }

	public FacebookCredentials Credentials { get; set; }

	public FacebookProfile Profile { get; set; }

	internal bool IsValid()
	{
		if (Credentials != null && !string.IsNullOrWhiteSpace(Credentials.AccessToken) && !string.IsNullOrWhiteSpace(Credentials.SessionKey))
		{
			return !string.IsNullOrWhiteSpace(Credentials.Secret);
		}
		return false;
	}

	public static FacebookSession CreateInvalidSession()
	{
		FacebookProfile facebookProfile = new FacebookProfile();
		facebookProfile.Id = string.Empty;
		facebookProfile.Name = string.Empty;
		facebookProfile.Type = string.Empty;
		FacebookProfile profile = facebookProfile;
		FacebookCredentials facebookCredentials = new FacebookCredentials();
		facebookCredentials.AccessToken = string.Empty;
		facebookCredentials.Id = string.Empty;
		facebookCredentials.Secret = string.Empty;
		facebookCredentials.SessionKey = string.Empty;
		FacebookCredentials credentials = facebookCredentials;
		FacebookSession facebookSession = new FacebookSession();
		facebookSession.UserId = string.Empty;
		facebookSession.UserName = string.Empty;
		facebookSession.Credentials = credentials;
		facebookSession.Profile = profile;
		return facebookSession;
	}
}

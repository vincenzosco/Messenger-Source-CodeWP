using Facebook.Models;

namespace Facebook.Base.Settings
{
	public interface IPasswordSettings
	{
		FacebookCredentials RetrieveCredentials();

		bool RemoveCredentials();

		bool SaveCredentials(FacebookCredentials credentials);
	}
}

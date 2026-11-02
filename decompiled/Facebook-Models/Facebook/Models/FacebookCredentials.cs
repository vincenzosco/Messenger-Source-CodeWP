namespace Facebook.Models;

public class FacebookCredentials
{
	public string AccessToken { get; set; }

	public string SessionKey { get; set; }

	public string Secret { get; set; }

	public string Id { get; set; }

	public bool IsValid()
	{
		if (!string.IsNullOrWhiteSpace(AccessToken) && !string.IsNullOrWhiteSpace(SessionKey) && !string.IsNullOrWhiteSpace(Secret))
		{
			return !string.IsNullOrWhiteSpace(Id);
		}
		return false;
	}
}

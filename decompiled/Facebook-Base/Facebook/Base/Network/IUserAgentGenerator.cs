namespace Facebook.Base.Network;

public interface IUserAgentGenerator
{
	string GetUserAgent();

	string GetSystemInformation();
}

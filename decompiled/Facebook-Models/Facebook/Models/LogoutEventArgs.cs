namespace Facebook.Models;

public class LogoutEventArgs
{
	public FacebookSession Session { get; private set; }

	public bool SessionWasExpired { get; private set; }

	public LogoutEventArgs(FacebookSession session, bool expired)
	{
		Session = session;
		SessionWasExpired = expired;
	}
}

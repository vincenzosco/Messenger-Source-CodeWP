namespace Facebook.Models.Signals;

public class LoginSignal
{
	public FacebookSession Session { get; private set; }

	public LoginSignal(FacebookSession session)
	{
		Session = session;
	}
}

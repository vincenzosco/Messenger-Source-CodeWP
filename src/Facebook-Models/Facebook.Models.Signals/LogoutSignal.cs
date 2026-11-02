namespace Facebook.Models.Signals
{
	public class LogoutSignal
	{
		public FacebookSession Session { get; private set; }

		public LogoutSignal(FacebookSession session)
		{
			Session = session;
		}
	}
}

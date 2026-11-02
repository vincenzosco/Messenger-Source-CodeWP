namespace Facebook.Models.Signals
{
	public class CacheThreadUpdatedSignal
	{
		public readonly MailThread Thread;

		public CacheThreadUpdatedSignal(MailThread thread)
		{
			Thread = thread;
		}
	}
}

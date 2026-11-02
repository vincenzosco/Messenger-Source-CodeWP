namespace Facebook.Base
{
	public class MailMessageUtil
	{
		public static long GetTimestamp(long sendTimestamp, long serverTimestamp)
		{
			if (serverTimestamp == 0)
			{
				return sendTimestamp;
			}
			return serverTimestamp;
		}
	}
}

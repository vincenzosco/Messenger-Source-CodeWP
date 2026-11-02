using System.Collections.Generic;

namespace Facebook.Models.Signals
{
	public class CacheMessagesUpdatedSignal
	{
		public readonly MailThread Thread;

		public readonly List<MailMessage> Messages;

		public readonly bool ShouldNotify;

		public CacheMessagesUpdatedSignal(MailThread thread, List<MailMessage> messages, bool shouldNotify)
		{
			Thread = thread;
			Messages = messages;
			ShouldNotify = shouldNotify;
		}
	}
}

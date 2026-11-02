using Facebook.Models;

namespace Facebook.Services.Api.Methods.MarkThreadState
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }

		public MailThread.Tag Tag { get; set; }

		public bool Enabled { get; set; }

		public bool ActionRemovesThread()
		{
			if (Enabled)
			{
				if (Tag != MailThread.Tag.Spam && Tag != MailThread.Tag.Archived)
				{
					return Tag == MailThread.Tag.Other;
				}
				return true;
			}
			return false;
		}
	}
}

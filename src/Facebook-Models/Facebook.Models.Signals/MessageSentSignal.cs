namespace Facebook.Models.Signals
{
	public class MessageSentSignal
	{
		public readonly MailMessage Message;

		public string DeviceSourceId { get; set; }

		public MessageSentSignal(MailMessage message)
		{
			Message = message;
		}
	}
}

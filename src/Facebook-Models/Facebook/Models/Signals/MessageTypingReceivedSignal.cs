namespace Facebook.Models.Signals
{
	public class MessageTypingReceivedSignal
	{
		public string SenderId { get; set; }

		public bool State { get; set; }
	}
}

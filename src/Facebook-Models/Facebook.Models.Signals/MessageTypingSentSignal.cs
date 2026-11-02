namespace Facebook.Models.Signals
{
	public class MessageTypingSentSignal
	{
		public string ToId { get; set; }

		public bool State { get; set; }
	}
}

namespace Facebook.Models.Signals
{
	public class GroupPinnedSignal
	{
		public readonly string ThreadId;

		public GroupPinnedSignal(string threadId)
		{
			ThreadId = threadId;
		}
	}
}

namespace Facebook.Models.Signals;

public class CachePinnedThreadUpdatedSignal
{
	public readonly PinnedThread Thread;

	public CachePinnedThreadUpdatedSignal(PinnedThread thread)
	{
		Thread = thread;
	}
}

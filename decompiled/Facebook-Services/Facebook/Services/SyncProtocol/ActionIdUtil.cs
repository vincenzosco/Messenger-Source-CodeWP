using System.Threading;

namespace Facebook.Services.SyncProtocol;

public class ActionIdUtil
{
	private static long _nextSubsecond;

	public static long CreateActionIdFromTimestamp(long timestamp)
	{
		long num = Interlocked.Increment(ref _nextSubsecond);
		return timestamp * 1000000 + num;
	}
}

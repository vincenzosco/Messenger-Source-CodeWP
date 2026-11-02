using Facebook.Models.Utilities;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol
{
	public class DeltaHandlerContext
	{
		public readonly ThreadCache ThreadCache;

		public readonly LoggedInUserUtils LoggedInUserUtils;

		public DeltaHandlerContext(ThreadCache threadCache, LoggedInUserUtils loggedInUserUtils)
		{
			ThreadCache = threadCache;
			LoggedInUserUtils = loggedInUserUtils;
		}
	}
}

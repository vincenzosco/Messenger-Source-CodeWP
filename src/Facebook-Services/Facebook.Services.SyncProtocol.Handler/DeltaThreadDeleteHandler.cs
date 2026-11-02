using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaThreadDeleteHandler : DeltaHandler
	{
		public DeltaThreadDeleteHandler()
			: base(DeltaType.THREAD_DELETE)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			return FetchRequired.NoFetchRequiredTaskResult;
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaThreadDelete delta = wrapper.DeltaThreadDelete;
			IEnumerable<Task> tasks = delta.ThreadKeys.Select((ThreadKey key) => DeleteThread(context.ThreadCache, key));
			await Task.WhenAll(tasks);
		}

		private static async Task DeleteThread(ThreadCache cache, ThreadKey threadKey)
		{
			string threadId = await ThreadDeltaUtil.GetThreadId(cache, threadKey);
			if (threadId != null)
			{
				await cache.RemoveThreadAndPinnedThreadByThreadId(threadId);
			}
		}
	}
}

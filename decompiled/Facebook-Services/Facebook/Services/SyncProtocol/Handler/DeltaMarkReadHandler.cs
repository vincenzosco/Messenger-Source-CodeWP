using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaMarkReadHandler : DeltaHandler
{
	public DeltaMarkReadHandler()
		: base(DeltaType.MARK_READ)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		return FetchRequired.NoFetchRequiredTaskResult;
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		DeltaMarkRead delta = wrapper.DeltaMarkRead;
		if (delta.__isset.threadKeys)
		{
			IEnumerable<Task> tasks = delta.ThreadKeys.Select((ThreadKey key) => MarkThreadRead(context.ThreadCache, key));
			await Task.WhenAll(tasks);
		}
		if (delta.__isset.folders && delta.Folders.Contains(ThreadFolder.FOLDER_INBOX))
		{
			await context.ThreadCache.MarkAllThreadsRead();
		}
	}

	private static async Task MarkThreadRead(ThreadCache cache, ThreadKey key)
	{
		string threadId = await ThreadDeltaUtil.GetThreadId(cache, key);
		if (threadId != null)
		{
			await cache.MarkThreadAsRead(threadId);
		}
	}
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaForcedFetchHandler : DeltaHandler
	{
		public DeltaForcedFetchHandler()
			: base(DeltaType.FORCED_FETCH)
		{
		}

		public override async Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			DeltaForcedFetch delta = wrapper.DeltaForcedFetch;
			ThreadKey threadKey = delta.ThreadKey;
			FetchRequired fetchRequired;
			if (delta.__isset.messageId)
			{
				string messageId = ThreadDeltaUtil.PrepareMessageId(delta.MessageId);
				string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, threadKey);
				MailThread mailThread = ((threadId != null) ? (await context.ThreadCache.GetThreadByThreadId(threadId)) : null);
				MailThread thread = mailThread;
				fetchRequired = ((thread != null) ? FetchRequired.FetchMessage(messageId, threadKey) : FetchRequired.FetchMessageAndThread(messageId, threadKey));
			}
			else
			{
				fetchRequired = FetchRequired.FetchThread(threadKey);
			}
			return fetchRequired.ToList();
		}

		public override Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			return Task.FromResult<object>(null);
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaMessageDeleteHandler : DeltaHandler
	{
		public DeltaMessageDeleteHandler()
			: base(DeltaType.MESSAGE_DELETE)
		{
		}

		public override async Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			DeltaMessageDelete delta = wrapper.DeltaMessageDelete;
			return (await WouldDeleteAllMessages(context.ThreadCache, delta)) ? (await Task.FromResult(FetchRequired.FetchThread(delta.ThreadKey).ToList())) : (await FetchRequired.NoFetchRequiredTaskResult);
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaMessageDelete delta = wrapper.DeltaMessageDelete;
			string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, delta.ThreadKey);
			if (threadId == null)
			{
				return;
			}
			foreach (string messageId in delta.MessageIds.Select((string m) => ThreadDeltaUtil.PrepareMessageId(m)))
			{
				await context.ThreadCache.RemoveMessage(threadId, messageId);
			}
		}

		private static async Task<bool> WouldDeleteAllMessages(ThreadCache cache, DeltaMessageDelete delta)
		{
			string threadId = await ThreadDeltaUtil.GetThreadId(cache, delta.ThreadKey);
			if (threadId == null)
			{
				return false;
			}
			int numMessagesToBeDeleted = 0;
			foreach (string messageId in delta.MessageIds.Select((string m) => ThreadDeltaUtil.PrepareMessageId(m)))
			{
				if (await cache.GetMessageByIdAndThreadId(threadId, messageId, null) != null)
				{
					numMessagesToBeDeleted++;
				}
			}
			return await cache.GetMessageCountByThreadId(threadId) == numMessagesToBeDeleted;
		}
	}
}

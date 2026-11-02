using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaThreadNameHandler : DeltaHandler
	{
		public DeltaThreadNameHandler()
			: base(DeltaType.THREAD_NAME)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			ThreadKey threadKey = wrapper.DeltaThreadName.MessageMetadata.ThreadKey;
			return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaThreadName delta = wrapper.DeltaThreadName;
			MailMessage message = await MessageDeltaUtil.CreateMessageForThread(context.ThreadCache, delta.MessageMetadata, GetMessageType(delta));
			await context.ThreadCache.SetThreadName(message.ThreadId, delta.Name);
			await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify);
		}

		private static MailMessage.Type GetMessageType(DeltaThreadName delta)
		{
			if (!string.IsNullOrWhiteSpace(delta.Name))
			{
				return MailMessage.Type.ThreadNameSet;
			}
			return MailMessage.Type.ThreadNameRemoved;
		}
	}
}

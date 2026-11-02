using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaSentMessageHandler : DeltaHandler
	{
		public DeltaSentMessageHandler()
			: base(DeltaType.SENT_MESSAGE)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			ThreadKey threadKey = wrapper.DeltaSentMessage.MessageMetadata.ThreadKey;
			return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			MailMessage message = await CreateMessageFromDelta(delta: wrapper.DeltaSentMessage, cache: context.ThreadCache);
			await context.ThreadCache.PatchSentMessageAndUpdateThread(message);
		}

		private static async Task<MailMessage> CreateMessageFromDelta(ThreadCache cache, DeltaSentMessage delta)
		{
			MailMessage message = await MessageDeltaUtil.CreateMessageForThread(cache, delta.MessageMetadata, MailMessage.Type.Regular);
			message.Attachments = MessageDeltaUtil.CreateAttachments(delta.Attachments);
			return message;
		}
	}
}

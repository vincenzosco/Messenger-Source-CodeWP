using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaAdminTextMessageHandler : DeltaHandler
{
	public DeltaAdminTextMessageHandler()
		: base(DeltaType.ADMIN_TEXT)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		ThreadKey threadKey = wrapper.DeltaAdminTextMessage.MessageMetadata.ThreadKey;
		return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		MailMessage message = await MessageDeltaUtil.CreateMessageForThread(metadata: wrapper.DeltaAdminTextMessage.MessageMetadata, cache: context.ThreadCache, messageType: MailMessage.Type.Unknown);
		await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify);
	}
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaVideoCallHandler : DeltaHandler
{
	public DeltaVideoCallHandler()
		: base(DeltaType.VIDEO_CALL)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		ThreadKey threadKey = wrapper.DeltaVideoCall.MessageMetadata.ThreadKey;
		return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		MailMessage message = await MessageDeltaUtil.CreateMessageForThread(metadata: wrapper.DeltaVideoCall.MessageMetadata, cache: context.ThreadCache, messageType: MailMessage.Type.VideoCall);
		await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify);
	}
}

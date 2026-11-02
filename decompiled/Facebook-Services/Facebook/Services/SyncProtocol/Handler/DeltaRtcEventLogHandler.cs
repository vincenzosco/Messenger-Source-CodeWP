using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaRtcEventLogHandler : DeltaHandler
{
	public DeltaRtcEventLogHandler()
		: base(DeltaType.RTC_EVENT_LOG)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		ThreadKey threadKey = wrapper.DeltaRTCEventLog.MessageMetadata.ThreadKey;
		return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		DeltaRTCEventLog delta = wrapper.DeltaRTCEventLog;
		MailMessage message = await MessageDeltaUtil.CreateMessageForThread(context.ThreadCache, delta.MessageMetadata, GetMessageType(delta));
		await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify);
	}

	private static MailMessage.Type GetMessageType(DeltaRTCEventLog delta)
	{
		if (!delta.Answered)
		{
			return MailMessage.Type.VoiceCallMissed;
		}
		return MailMessage.Type.VoiceCall;
	}
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaReadReceiptHandler : DeltaHandler
{
	public DeltaReadReceiptHandler()
		: base(DeltaType.READ_RECEIPT)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		return FetchRequired.NoFetchRequiredTaskResult;
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		DeltaReadReceipt delta = wrapper.DeltaReadReceipt;
		string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, delta.ThreadKey);
		if (threadId != null)
		{
			ReadReceipt readReceipt = CreateReadReceipt(delta);
			if (readReceipt.Timestamp > 0)
			{
				await context.ThreadCache.AddReadReceiptByThreadId(threadId, readReceipt, shouldNotify: true);
			}
		}
	}

	private static ReadReceipt CreateReadReceipt(DeltaReadReceipt delta)
	{
		long num = (delta.ThreadKey.__isset.otherUserFbId ? delta.ThreadKey.OtherUserFbId : delta.ActorFbId);
		ReadReceipt readReceipt = new ReadReceipt();
		readReceipt.Id = num.ToString();
		readReceipt.Timestamp = delta.TimestampMs;
		return readReceipt;
	}
}

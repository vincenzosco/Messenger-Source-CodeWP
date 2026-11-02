using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaThreadActionHandler : DeltaHandler
{
	public DeltaThreadActionHandler()
		: base(DeltaType.THREAD_ACTION)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		DeltaThreadAction deltaThreadAction = wrapper.DeltaThreadAction;
		switch (deltaThreadAction.Action)
		{
		case ThreadAction.ACTION_UNARCHIVED:
			return Task.FromResult(FetchRequired.FetchThread(deltaThreadAction.ThreadKey).ToList());
		case ThreadAction.ACTION_INVALID:
		case ThreadAction.ACTION_ARCHIVED:
			return FetchRequired.NoFetchRequiredTaskResult;
		default:
			throw new NotImplementedException("New action type added but no handler");
		}
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		DeltaThreadAction delta = wrapper.DeltaThreadAction;
		switch (delta.Action)
		{
		case ThreadAction.ACTION_ARCHIVED:
		{
			string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, delta.ThreadKey);
			if (threadId == null)
			{
				Logger.Info(this, "Attempting to archive thread but no thread is found", "Handle", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Handler\\DeltaThreadActionHandler.cs", 45);
			}
			else
			{
				await context.ThreadCache.RemoveThreadAndPinnedThreadByThreadId(threadId);
			}
			break;
		}
		default:
			throw new NotImplementedException("New action type added but no handler");
		case ThreadAction.ACTION_INVALID:
		case ThreadAction.ACTION_UNARCHIVED:
			break;
		}
	}
}

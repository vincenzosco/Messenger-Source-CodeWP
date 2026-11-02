using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaThreadMuteSettingsHandler : DeltaHandler
	{
		public DeltaThreadMuteSettingsHandler()
			: base(DeltaType.THREAD_MUTE_SETTINGS)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			return FetchRequired.NoFetchRequiredTaskResult;
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaThreadMuteSettings delta = wrapper.DeltaThreadMuteSettings;
			string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, delta.ThreadKey);
			if (threadId != null)
			{
				await context.ThreadCache.MarkThreadMuteState(threadId, delta.ExpireTime);
			}
		}
	}
}

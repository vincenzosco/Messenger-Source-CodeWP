using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaNewGroupThreadHandler : DeltaHandler
	{
		public DeltaNewGroupThreadHandler()
			: base(DeltaType.NEW_GROUP_THREAD)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			ThreadKey threadKey = wrapper.DeltaNewGroupThread.ThreadKey;
			return Task.FromResult(FetchRequired.FetchThread(threadKey).ToList());
		}

		public override Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			return Task.FromResult<object>(null);
		}
	}
}

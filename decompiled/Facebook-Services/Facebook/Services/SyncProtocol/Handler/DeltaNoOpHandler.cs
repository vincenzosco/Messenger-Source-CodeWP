using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaNoOpHandler : DeltaHandler
	{
		public DeltaNoOpHandler()
			: base(DeltaType.NOOP)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			return FetchRequired.NoFetchRequiredTaskResult;
		}

		public override Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			return Task.FromResult<object>(null);
		}
	}
}

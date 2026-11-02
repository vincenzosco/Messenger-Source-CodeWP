using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol
{
	public abstract class DeltaHandler
	{
		public readonly DeltaType HandlerType;

		protected DeltaHandler(DeltaType handlerType)
		{
			HandlerType = handlerType;
		}

		public abstract Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper);

		public abstract Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify);
	}
}

using Facebook.Base;

namespace Facebook.Services.Api.Methods.MarkMessageFailedById
{
	[Queue(QueueType.Local)]
	public class Request : Facebook.Base.Request
	{
		public string ThreadId;

		public string OfflineThreadingId;
	}
}

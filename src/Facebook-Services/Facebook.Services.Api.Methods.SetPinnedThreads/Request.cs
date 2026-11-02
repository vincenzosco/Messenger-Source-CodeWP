using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.SetPinnedThreads
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public List<string> OrderedThreadIds;

		public List<string> RemovedThreadIds;
	}
}

using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.RemoveThreadParticipants
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }

		public IList<string> UserIds { get; set; }

		public Request(string threadId, IList<string> userIds)
		{
			ThreadId = threadId;
			UserIds = userIds;
		}
	}
}

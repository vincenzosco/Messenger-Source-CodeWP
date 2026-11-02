using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.AddThreadParticipants
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }

		public List<string> UserIds { get; set; }
	}
}

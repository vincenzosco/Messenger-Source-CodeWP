using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.FetchLocalThreadData
{
	[Queue(QueueType.Local)]
	public class Request : FacebookServiceRequest
	{
		public List<string> ThreadIds { get; set; }

		public Request(List<string> ids)
		{
			ThreadIds = ids;
		}
	}
}

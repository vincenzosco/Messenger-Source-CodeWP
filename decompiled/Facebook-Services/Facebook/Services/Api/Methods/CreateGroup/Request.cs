using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.CreateGroup;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string ThreadName { get; set; }

	public List<string> ThreadParticipants { get; set; }

	public string ImagePath { get; set; }

	public Request(string threadName, List<string> threadParticipants)
	{
		ThreadName = threadName;
		ThreadParticipants = threadParticipants;
	}
}

using System.Collections.Generic;
using System.Linq;

namespace Facebook.Services.Api.Methods.FetchThreadsAndMessages;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public readonly IEnumerable<long> ThreadFbIds;

	public readonly IEnumerable<long> ThreadRecipientIds;

	public readonly IEnumerable<string> MessageIds;

	public bool HasThreads
	{
		get
		{
			if (ThreadFbIds == null || !ThreadFbIds.Any())
			{
				if (ThreadRecipientIds != null)
				{
					return ThreadRecipientIds.Any();
				}
				return false;
			}
			return true;
		}
	}

	public bool HasMessages
	{
		get
		{
			if (MessageIds != null)
			{
				return MessageIds.Any();
			}
			return false;
		}
	}

	public Request(IEnumerable<long> threadFbIds, IEnumerable<long> threadRecipientIds, IEnumerable<string> messageIds)
	{
		ThreadFbIds = threadFbIds;
		ThreadRecipientIds = threadRecipientIds;
		MessageIds = messageIds;
	}
}

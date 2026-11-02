namespace Facebook.Services.Api.Methods.DeleteMessage
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; private set; }

		public string MessageId { get; private set; }

		public bool IsLocalMessage { get; private set; }

		public Request(string messageId, string threadId, bool isLocalMessage)
		{
			MessageId = messageId;
			ThreadId = threadId;
			IsLocalMessage = isLocalMessage;
		}

		public bool HasValidMessageId()
		{
			return !string.IsNullOrWhiteSpace(MessageId);
		}
	}
}

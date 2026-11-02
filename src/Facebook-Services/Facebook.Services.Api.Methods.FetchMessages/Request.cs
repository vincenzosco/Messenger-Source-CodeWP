namespace Facebook.Services.Api.Methods.FetchMessages
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string ThreadId { get; set; }

		public long MaxTimestamp { get; set; }

		public long ActionId { get; set; }

		public bool ForceToServer { get; set; }

		public long RecentPushActionId { get; set; }

		public bool FetchAllThreadInfo { get; set; }

		public bool IsIncremental()
		{
			return ActionId > 0;
		}
	}
}

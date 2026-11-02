namespace Facebook.Services.Api.Methods.FetchThreads
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string Folder { get; set; }

		public long MaxTimestamp { get; set; }

		public long ActionId { get; set; }

		public bool ForceToServer { get; set; }

		public bool ForceFullSync { get; set; }

		public long RecentPushActionId { get; set; }

		public int ThreadLimit { get; set; }

		public bool RequestFromSyncProtocolService { get; set; }

		public Request()
		{
			Folder = "inbox";
			ThreadLimit = 10;
		}

		public bool IsIncremental()
		{
			if (!ForceFullSync)
			{
				return ActionId > 0;
			}
			return false;
		}
	}
}

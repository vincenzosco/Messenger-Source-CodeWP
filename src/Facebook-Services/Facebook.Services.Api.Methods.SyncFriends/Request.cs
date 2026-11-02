namespace Facebook.Services.Api.Methods.SyncFriends
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public static int FetchUserSize = 200;

		public int Limit { get; set; }

		public int Offset { get; set; }

		public long LastUpdated { get; set; }

		public bool ShouldPurgeBeforeSaving()
		{
			bool flag = LastUpdated == 0;
			bool result = Offset == 0;
			if (flag)
			{
				return result;
			}
			return false;
		}
	}
}

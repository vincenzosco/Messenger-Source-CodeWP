namespace Facebook.Services.Api.Methods.SendAnalyticsEvents
{
	public class Response : FacebookApiResponse
	{
		public int LastUploadedEventLogId { get; set; }

		public Response()
		{
			LastUploadedEventLogId = -1;
		}
	}
}

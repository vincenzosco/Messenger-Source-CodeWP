using Facebook.Base;

namespace Facebook.Services.Api.Methods.FetchFriends
{
	[Queue(QueueType.Local)]
	public class Request : Facebook.Base.Request
	{
		public string Uid { get; set; }

		public string Filter { get; set; }
	}
}

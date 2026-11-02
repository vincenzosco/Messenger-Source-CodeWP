using System.Collections.Generic;
using Facebook.Services.Friends;

namespace Facebook.Services.Api.Methods.FetchContactsById
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public List<string> ContactIds { get; set; }

		public Facebook.Services.Friends.Service.SyncContactTypes RequestType { get; set; }
	}
}

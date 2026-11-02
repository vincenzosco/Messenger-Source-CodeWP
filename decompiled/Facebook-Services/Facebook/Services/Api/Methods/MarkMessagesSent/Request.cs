using System.Collections.Generic;
using Facebook.Base;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Services.Api.Methods.MarkMessagesSent
{
	[Queue(QueueType.Local)]
	public class Request : Facebook.Base.Request
	{
		public List<Facebook.Services.Api.Methods.SendMessage.Response> SentMessages { get; set; }
	}
}

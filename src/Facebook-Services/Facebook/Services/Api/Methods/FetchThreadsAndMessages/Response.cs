using System.Collections.Generic;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchThreadsAndMessages
{
	public class Response : FacebookApiResponse
	{
		public List<MailThread> Threads { get; set; }

		public List<MailMessage> Messages { get; set; }
	}
}

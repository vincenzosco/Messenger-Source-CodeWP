using System.Collections.Generic;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.ThreadQuery
{
	[JsonConverter(typeof(ThreadQueryConverter))]
	public class Response : FacebookApiResponse
	{
		public MailThread Thread { get; set; }

		public List<MailMessage> Messages { get; set; }
	}
}

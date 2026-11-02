using System.Collections.Generic;
using Facebook.Models;
using Facebook.Services.Api.Methods.SendMessage;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchMessages
{
	public class Response : FacebookApiResponse
	{
		[JsonProperty("data")]
		public List<MailMessage> Messages { get; set; }

		public List<ReadReceipt> Receipts { get; set; }

		public List<Facebook.Services.Api.Methods.SendMessage.Request> OutgoingRequests { get; set; }

		public MailThread Thread { get; set; }

		public bool ShouldPurgeMessageCache { get; set; }

		public bool ReceivedMessagesFromWeb { get; set; }
	}
}

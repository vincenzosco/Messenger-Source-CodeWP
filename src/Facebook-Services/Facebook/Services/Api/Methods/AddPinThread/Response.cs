using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.AddPinThread
{
	public class Response : FacebookApiResponse
	{
		[JsonProperty("id")]
		[DataMember]
		public string Id { get; set; }

		[JsonProperty("thread_id")]
		[DataMember]
		public string ThreadId { get; set; }

		[JsonProperty("update_time")]
		[DataMember]
		public long UpdateTime { get; set; }

		public MailThread Thread { get; set; }
	}
}

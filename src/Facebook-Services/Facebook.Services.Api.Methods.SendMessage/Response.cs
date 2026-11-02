using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SendMessage
{
	public class Response : FacebookApiResponse
	{
		[JsonProperty("id")]
		[DataMember]
		public string MessageId { get; set; }

		[JsonProperty("tid")]
		[DataMember]
		public string ThreadId { get; set; }

		[JsonProperty("offline_id")]
		[DataMember]
		public string OfflineId { get; set; }
	}
}

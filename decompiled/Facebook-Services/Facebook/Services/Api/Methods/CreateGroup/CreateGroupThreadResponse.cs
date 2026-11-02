using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.CreateGroup
{
	internal class CreateGroupThreadResponse : FacebookApiResponse
	{
		[JsonProperty("id")]
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty("thread_id")]
		public string ThreadId { get; set; }
	}
}

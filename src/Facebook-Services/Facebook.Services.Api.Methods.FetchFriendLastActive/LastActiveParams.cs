using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchFriendLastActive
{
	public class LastActiveParams
	{
		[DataMember]
		[JsonProperty(PropertyName = "last_active")]
		public LastActive LastActive { get; set; }

		[JsonProperty(PropertyName = "is_pushable")]
		[DataMember]
		public bool Pushable { get; set; }

		[JsonProperty(PropertyName = "is_messenger_user")]
		[DataMember]
		public bool IsMessengerUser { get; set; }
	}
}

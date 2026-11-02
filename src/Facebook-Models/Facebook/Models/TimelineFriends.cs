using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class TimelineFriends
	{
		[JsonProperty("first_name")]
		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		[JsonProperty("friends")]
		public TimelineFriendsContainer Friends { get; set; }

		[DataMember]
		[JsonProperty("mutual_friends")]
		public TimelineFriendsContainer MutualFriends { get; set; }
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class JewelCounts
	{
		[DataMember]
		[JsonProperty(PropertyName = "notification_counts")]
		public JewelCount Notifications { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "friend_requests_counts")]
		public JewelCount FriendRequests { get; set; }

		public JewelCount Messages { get; set; }
	}
}

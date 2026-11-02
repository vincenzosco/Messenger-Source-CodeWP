using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class TimelineFriendsContainer
	{
		[JsonProperty("nodes")]
		[DataMember]
		public List<FacebookUser> Users { get; set; }
	}
}

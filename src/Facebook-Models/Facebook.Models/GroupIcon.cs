using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class GroupIcon
	{
		[JsonProperty(PropertyName = "light_icon")]
		[DataMember]
		public FacebookImage LightIcon { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "dark_icon")]
		public FacebookImage DarkIcon { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
	}
}

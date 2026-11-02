using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FacebookSettings : Notifiable
	{
		[DataMember]
		[JsonProperty(PropertyName = "setting")]
		public string Setting;

		[DataMember]
		[JsonProperty(PropertyName = "value")]
		public string Value;
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FacebookType
	{
		[DataMember]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
	}
}

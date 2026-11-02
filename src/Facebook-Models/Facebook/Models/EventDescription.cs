using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class EventDescription
	{
		[DataMember]
		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }
	}
}

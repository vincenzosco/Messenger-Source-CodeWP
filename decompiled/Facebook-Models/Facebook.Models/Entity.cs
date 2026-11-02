using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Entity
	{
		[JsonProperty(PropertyName = "__type__")]
		[DataMember]
		[JsonConverter(typeof(FacebookSubTypeConverter))]
		public FacebookSubType Type { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "canvas_url")]
		public string CanvasUrl { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }
	}
}

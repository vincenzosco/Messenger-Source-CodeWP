using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class FacebookProfileGraphQl
	{
		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "__type__")]
		[JsonConverter(typeof(FacebookSubTypeConverter))]
		public FacebookSubType Type { get; set; }
	}
}

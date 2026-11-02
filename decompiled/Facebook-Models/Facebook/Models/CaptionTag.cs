using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class CaptionTag
	{
		[DataMember]
		[JsonProperty("id")]
		public string ObjectId { get; set; }

		[DataMember]
		[JsonProperty("name")]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty("offset")]
		public int Offset { get; set; }

		[DataMember]
		[JsonProperty("length")]
		public int Length { get; set; }
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class PhotoTagObject
	{
		[JsonProperty("location")]
		[DataMember]
		public PhotoTagLocation PhotoTagLocation { get; set; }

		[DataMember]
		[JsonProperty("tagger")]
		public Actor Tagger { get; set; }
	}
}

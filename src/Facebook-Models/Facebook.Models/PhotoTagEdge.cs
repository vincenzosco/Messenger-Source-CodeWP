using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class PhotoTagEdge
	{
		[DataMember]
		[JsonProperty("tag")]
		public PhotoTagObject TagObject { get; set; }

		[JsonProperty("node")]
		[DataMember]
		public FacebookProfileGraphQl Profile { get; set; }

		public bool NewlyAdded { get; set; }
	}
}

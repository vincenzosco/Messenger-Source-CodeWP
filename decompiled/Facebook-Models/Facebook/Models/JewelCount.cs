using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class JewelCount
	{
		[DataMember]
		[JsonProperty(PropertyName = "unread")]
		public int Unread { get; set; }

		[JsonProperty(PropertyName = "unseen")]
		[DataMember]
		public int Unseen { get; set; }
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class MessageRange : BaseRange
	{
		[DataMember]
		[JsonProperty(PropertyName = "entity")]
		public Entity Entity { get; set; }
	}
}

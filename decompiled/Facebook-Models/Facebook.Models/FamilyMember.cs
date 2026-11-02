using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class FamilyMember : IAboutType
	{
		[DataMember]
		[JsonProperty(PropertyName = "relation")]
		public string Relation { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "node")]
		public FacebookProfile FacebookProfile { get; set; }

		public AboutType Type
		{
			get
			{
				return AboutType.Family;
			}
		}
	}
}

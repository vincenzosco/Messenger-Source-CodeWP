using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FeedSection
	{
		[DataMember]
		[JsonProperty(PropertyName = "feed_section_type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "icon_image")]
		public FacebookImage Image { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "section_feed")]
		public SectionInfo Info { get; set; }

		public bool HasInfoCount
		{
			get
			{
				if (Info != null)
				{
					return !string.IsNullOrEmpty(Info.DisplayCount);
				}
				return false;
			}
		}
	}
}

using System;
using System.Runtime.Serialization;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FeedPhoto
	{
		[DataMember]
		[JsonProperty(PropertyName = "album")]
		public FeedAlbum Album { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_add_tags")]
		public bool? CanViewerAddTags { get; set; }

		[JsonConverter(typeof(UnixDateTimeConverter))]
		[JsonProperty(PropertyName = "created_time")]
		[DataMember]
		public DateTime CreatedTime { get; set; }

		[JsonProperty(PropertyName = "focus")]
		[DataMember]
		public Focus Focus { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "image")]
		public FacebookImage Image { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "image_small")]
		public FacebookImage ImageSmall { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "image_medium")]
		public FacebookImage ImageMedium { get; set; }
	}
}

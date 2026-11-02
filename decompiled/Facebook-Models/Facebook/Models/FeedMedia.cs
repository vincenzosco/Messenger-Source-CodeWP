using System;
using System.Runtime.Serialization;
using System.Text;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FeedMedia
	{
		private string _displayDuration;

		[JsonConverter(typeof(FacebookSubTypeConverter))]
		[JsonProperty(PropertyName = "__type__")]
		[DataMember]
		public FacebookSubType Type { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "album")]
		public FeedAlbum Album { get; set; }

		[JsonProperty(PropertyName = "can_viewer_add_tags")]
		[DataMember]
		public bool? CanViewerAddTags { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "captions_url")]
		public string CaptionUrl { get; set; }

		[JsonConverter(typeof(UnixDateTimeConverter))]
		[JsonProperty(PropertyName = "created_time")]
		[DataMember]
		public DateTime CreatedTime { get; set; }

		[JsonProperty(PropertyName = "feedback")]
		[DataMember]
		public Feedback Feedback { get; set; }

		[JsonProperty(PropertyName = "focus")]
		[DataMember]
		public Focus Focus { get; set; }

		[JsonProperty(PropertyName = "height")]
		[DataMember]
		public int? Height { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "image")]
		public FacebookImage Image { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "imageSmall")]
		public FacebookImage ImageSmall { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "imageMedium")]
		public FacebookImage ImageMedium { get; set; }

		[JsonProperty(PropertyName = "imageLarge")]
		[DataMember]
		public FacebookImage ImageLarge { get; set; }

		[JsonProperty(PropertyName = "is_playable")]
		[DataMember]
		public bool? IsPlayable { get; set; }

		[JsonProperty(PropertyName = "playable_duration")]
		[DataMember]
		public int? PlayableDuration { get; set; }

		[DataMember]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		[JsonProperty(PropertyName = "modified_time")]
		public DateTime ModifiedTime { get; set; }

		[JsonProperty(PropertyName = "playable_url")]
		[DataMember]
		public string PlayableUrl { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "width")]
		public int? Width { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "title")]
		public StringEntity Title { get; set; }

		public string DisplayDuration
		{
			get
			{
				if (_displayDuration == null && PlayableDuration.HasValue)
				{
					int? playableDuration = PlayableDuration;
					int? num = playableDuration / 3600;
					playableDuration -= num * 3600;
					int? num2 = playableDuration / 60;
					playableDuration -= num2 * 60;
					StringBuilder stringBuilder = new StringBuilder();
					if (num > 0)
					{
						stringBuilder.AppendFormat("{0:D2}:", new object[1] { num });
					}
					stringBuilder.AppendFormat("{0:D2}:{1:D2}", new object[2] { num2, playableDuration });
					_displayDuration = stringBuilder.ToString();
				}
				return _displayDuration;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class MailShare
	{
		[DataContract]
		[JsonObject(MemberSerialization.OptIn)]
		public class Property
		{
			[DataMember]
			[JsonProperty("name")]
			public string Name { get; set; }

			[DataMember]
			[JsonProperty("text")]
			public string Text { get; set; }

			[JsonProperty("href")]
			[DataMember]
			public string Href { get; set; }
		}

		[DataContract]
		[JsonObject(MemberSerialization.OptIn)]
		public class Media
		{
			[DataContract]
			[JsonObject(MemberSerialization.OptIn)]
			public class Photo
			{
				[JsonProperty("aid")]
				[DataMember]
				public string AlbumId { get; set; }

				[DataMember]
				[JsonProperty("pid")]
				public string PhotoId { get; set; }

				[DataMember]
				[JsonProperty("fbid")]
				public string FbId { get; set; }

				[JsonProperty("owner")]
				[DataMember]
				public string Owner { get; set; }

				[JsonProperty("index")]
				[DataMember]
				public long Index { get; set; }

				[JsonProperty("width")]
				[DataMember]
				public long Height { get; set; }

				[DataMember]
				[JsonProperty("height")]
				public long Width { get; set; }
			}

			[JsonObject(MemberSerialization.OptIn)]
			[DataContract]
			public class Video
			{
				[DataMember]
				[JsonProperty("display_url")]
				public string DisplayUrl { get; set; }

				[DataMember]
				[JsonProperty("source_url")]
				public string SourceUrl { get; set; }

				[DataMember]
				[JsonProperty("owner")]
				public string Owner { get; set; }

				[DataMember]
				[JsonProperty("source_type")]
				public string SourceType { get; set; }
			}

			[JsonObject(MemberSerialization.OptIn)]
			[DataContract]
			public class Swf
			{
				[JsonProperty("preview_img")]
				[DataMember]
				public string PreviewImage { get; set; }

				[JsonProperty("source_url")]
				[DataMember]
				public string SourceUrl { get; set; }
			}

			[DataMember]
			[JsonProperty("href")]
			public string Href { get; set; }

			[DataMember]
			[JsonProperty("alt")]
			public string Alt { get; set; }

			[JsonProperty("type")]
			[DataMember]
			public string Type { get; set; }

			[DataMember]
			[JsonProperty("src")]
			public string Src { get; set; }

			[DataMember]
			[JsonProperty("photo")]
			public Photo MediaPhoto { get; set; }

			[DataMember]
			[JsonProperty("video")]
			public Video MediaVideo { get; set; }

			[DataMember]
			[JsonProperty("swf")]
			public Swf MediaSwf { get; set; }
		}

		public const string RegularLikeStickerId = "227878347358915";

		public const string HotLikeSmallStickerId = "369239263222822";

		public const string HotLikeMediumStickerId = "369239343222814";

		public const string HotLikeLargeStickerId = "369239383222810";

		public static readonly string[] LikeStickerIds = new string[4] { "227878347358915", "369239263222822", "369239343222814", "369239383222810" };

		[DataMember]
		[JsonProperty("media")]
		public List<Media> MediaList;

		[JsonProperty("properties")]
		[DataMember]
		public List<Property> PropertyList;

		[DataMember]
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("fb_object_type")]
		[DataMember]
		public string FBObjectType { get; set; }

		[DataMember]
		[JsonProperty("caption")]
		public string Caption { get; set; }

		[DataMember]
		[JsonProperty("description")]
		public string Description { get; set; }

		[DataMember]
		[JsonProperty("href")]
		public string Href { get; set; }

		[JsonProperty("sticker_id")]
		[DataMember]
		public string StickerId { get; set; }

		[DataMember]
		[JsonProperty("icon")]
		public string Icon { get; set; }

		public bool HasImage()
		{
			if (MediaList != null && MediaList.Any())
			{
				return Uri.IsWellFormedUriString(MediaList[0].Src, UriKind.Absolute);
			}
			return false;
		}
	}
}

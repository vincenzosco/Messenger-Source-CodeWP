using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Base.Mqtt.Serialization.Payloads
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Attachment
	{
		[JsonObject(MemberSerialization.OptIn)]
		[DataContract]
		public class ImageData
		{
			[JsonProperty("width")]
			[DataMember]
			public int Width { get; set; }

			[JsonProperty("height")]
			[DataMember]
			public int Height { get; set; }

			[DataMember]
			[JsonProperty("url")]
			public string Url { get; set; }

			[DataMember]
			[JsonProperty("preview_url")]
			public string PreviewUrl { get; set; }
		}

		[JsonObject(MemberSerialization.OptIn)]
		[DataContract]
		public class VideoData
		{
			public enum VideoSource
			{
				NonQuickCam = 1,
				QuickCam
			}

			[JsonProperty("width")]
			[DataMember]
			public int Width { get; set; }

			[DataMember]
			[JsonProperty("height")]
			public int Height { get; set; }

			[DataMember]
			[JsonProperty("length")]
			public int Length { get; set; }

			[JsonProperty("video_type")]
			[DataMember]
			public VideoSource Source { get; set; }

			[DataMember]
			[JsonProperty("url")]
			public string Url { get; set; }

			[DataMember]
			[JsonProperty("preview_url")]
			public string PreviewUrl { get; set; }

			[DataMember]
			[JsonProperty("rotation")]
			public int Rotation { get; set; }
		}

		[DataMember]
		[JsonProperty("id")]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty("fbid")]
		public long Fbid { get; set; }

		[JsonProperty("type")]
		[DataMember]
		public int Type { get; set; }

		[DataMember]
		[JsonProperty("mime_type")]
		public string MimeType { get; set; }

		[JsonProperty("filename")]
		[DataMember]
		public string FileName { get; set; }

		[DataMember]
		[JsonProperty("image_data")]
		public ImageData Image { get; set; }

		[DataMember]
		[JsonProperty("video_data")]
		public VideoData Video { get; set; }

		[DataMember]
		[JsonProperty("file_size")]
		public long FileSize { get; set; }
	}
}

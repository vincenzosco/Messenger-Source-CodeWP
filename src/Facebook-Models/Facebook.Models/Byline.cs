using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Byline
	{
		private Uri _iconUri;

		[DataMember]
		[JsonProperty(PropertyName = "concise_text")]
		public StringEntity ConciseText { get; set; }

		[JsonProperty(PropertyName = "icon_image")]
		[DataMember]
		public FacebookImage IconImage { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "is_prompt")]
		public bool? IsPrompt { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "text")]
		public StringEntity Text { get; set; }

		public Uri IconUri
		{
			get
			{
				if (_iconUri != null || IconImage == null || string.IsNullOrWhiteSpace(IconImage.Name))
				{
					return _iconUri;
				}
				switch (IconImage.Name)
				{
				case "birthday":
				case "bloodtype":
				case "currentcity":
				case "date":
				case "education":
				case "hometown":
				case "languages":
				case "nearby":
				case "place":
				case "relationship":
				case "work":
					return _iconUri = new Uri(string.Format("ms-appx:///Assets/Timeline/Byline/timeline_byline_{0}.png", new object[1] { IconImage.Name }), UriKind.Absolute);
				default:
					return _iconUri = new Uri("ms-appx:///Assets/Timeline/Byline/timeline_byline_languages.png", UriKind.Absolute);
				}
			}
		}
	}
}

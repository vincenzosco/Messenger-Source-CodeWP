using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class PrivacyScope
	{
		private Uri _iconUri;

		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_edit")]
		public bool? CanViewerEdit { get; set; }

		[JsonProperty(PropertyName = "label")]
		[DataMember]
		public string Label { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "icon_image")]
		public FacebookImage IconImage { get; set; }

		public Uri IconUri
		{
			get
			{
				if (_iconUri == null && IconImage != null && IconImage.Name != null)
				{
					switch (IconImage.Name)
					{
					case "acquaintances":
					case "close_friends":
					case "custom":
					case "event":
					case "everyone":
					case "facebook":
					case "family":
					case "friends":
					case "friends_except_acquaintances":
					case "friends_of_friends":
					case "group":
					case "list":
					case "location":
					case "only_me":
					case "school":
					case "work":
						_iconUri = new Uri(string.Format("ms-appx:///Assets/Feed/PrivacyScope/privacy_scope_{0}.png", new object[1] { IconImage.Name }), UriKind.Absolute);
						break;
					default:
						_iconUri = new Uri("ms-appx:///Assets/Feed/PrivacyScope/privacy_scope_custom.png", UriKind.Absolute);
						break;
					}
				}
				return _iconUri;
			}
		}

		public Uri IconUriPhotoCaption
		{
			get
			{
				if (_iconUri == null && IconImage != null && IconImage.Name != null)
				{
					switch (IconImage.Name)
					{
					case "custom":
					case "everyone":
					case "friends":
					case "friends_except_acquaintances":
					case "group":
					case "list":
					case "generic_list":
					case "only_me":
						_iconUri = new Uri(string.Format("ms-appx:///Assets/Gallery/Privacy/privacy_scope_{0}.png", new object[1] { IconImage.Name }), UriKind.Absolute);
						break;
					default:
						_iconUri = new Uri("ms-appx:///Assets/Gallery/Privacy/privacy_scope_custom.png", UriKind.Absolute);
						break;
					}
				}
				return _iconUri;
			}
		}
	}
}

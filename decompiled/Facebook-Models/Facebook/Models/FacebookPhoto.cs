using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class FacebookPhoto
{
	public enum IdTypes
	{
		None,
		AlbumId,
		PhotoObjectId,
		PhotosOfSubject,
		UploadPhotoSet,
		PhotoUrlMessages,
		PhotoUrlLocalMessages,
		PhotoUriComposer,
		PhotoIdList
	}

	[DataMember]
	[JsonProperty("type")]
	public FacebookType Type { get; set; }

	[DataMember]
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty(PropertyName = "album")]
	[DataMember]
	public FacebookAlbum Album { get; set; }

	[JsonProperty("can_viewer_delete")]
	[DataMember]
	public bool CanViewerDelete { get; set; }

	[JsonProperty("can_viewer_add_tags")]
	[DataMember]
	public bool CanViewerAddTags { get; set; }

	[JsonProperty("message")]
	[DataMember]
	public StringEntity Caption { get; set; }

	[DataMember]
	[JsonProperty("feedback")]
	public Feedback Feedback { get; set; }

	[DataMember]
	[JsonProperty("created_time")]
	public long CreatedTime { get; set; }

	[DataMember]
	[JsonProperty("image_max_size")]
	public FacebookImage ImageHD { get; set; }

	[JsonProperty("image_thumbnail")]
	[DataMember]
	public FacebookImage ImageThumbnail { get; set; }

	[JsonProperty("owner")]
	[DataMember]
	public Actor Owner { get; set; }

	[DataMember]
	[JsonProperty("privacy_scope")]
	public PrivacyScope Privacy { get; set; }

	[DataMember]
	[JsonProperty("tags")]
	public PhotoTags PhotoTags { get; set; }

	public IdTypes IdType { get; set; }
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class FacebookAlbum
{
	private long _createdTime;

	[DataMember]
	[JsonProperty("type")]
	public FacebookType Type { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "media")]
	public FacebookMedia Media { get; set; }

	[JsonProperty("privacy_scope")]
	[DataMember]
	public PrivacyScope Privacy { get; set; }

	[JsonProperty(PropertyName = "id")]
	[DataMember]
	public string Id { get; set; }

	[JsonProperty("created_time")]
	[DataMember]
	public long CreatedTime
	{
		get
		{
			return _createdTime * 1000;
		}
		set
		{
			_createdTime = value;
		}
	}

	[JsonProperty(PropertyName = "message")]
	[DataMember]
	public StringEntity Message { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "title")]
	public StringEntity Title { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "album_type")]
	public string AlbumType { get; set; }

	[JsonProperty(PropertyName = "can_upload")]
	[DataMember]
	public bool CanUpload { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "album_cover_photo")]
	public FacebookPhoto AlbumCoverPhoto { get; set; }

	public string SubjectId { get; set; }

	public FacebookPhoto.IdTypes IdType { get; set; }
}

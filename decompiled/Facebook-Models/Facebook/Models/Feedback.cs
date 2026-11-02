using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class Feedback : Notifiable
{
	private bool? _doesViewerLike;

	[DataMember]
	[JsonProperty(PropertyName = "can_viewer_comment")]
	public bool? CanViewerComment { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "can_viewer_comment_with_photo")]
	public bool? CanViewerCommentWithPhoto { get; set; }

	[JsonProperty(PropertyName = "can_viewer_like")]
	[DataMember]
	public bool? CanViewerLike { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "does_viewer_like")]
	public bool? DoesViewerLike
	{
		get
		{
			return _doesViewerLike;
		}
		set
		{
			_doesViewerLike = value;
			OnPropertyChanged("DoesViewerLike");
		}
	}

	[DataMember]
	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }

	[JsonProperty(PropertyName = "legacy_api_post_id")]
	[DataMember]
	public string LegacyApiPostId { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "comments")]
	public ActionCount Comments { get; set; }

	[JsonProperty(PropertyName = "likers")]
	[DataMember]
	public ActionCount Likers { get; set; }

	[JsonProperty(PropertyName = "reshares")]
	[DataMember]
	public ActionCount Reshares { get; set; }
}

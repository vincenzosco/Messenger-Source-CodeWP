using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class CommentNode
	{
		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_comment")]
		public bool? CanViewerComment;

		[JsonProperty(PropertyName = "can_viewer_comment_with_photo")]
		[DataMember]
		public bool? CanViewerCommentWithPhoto;

		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_like")]
		public bool? CanViewerLike;

		[DataMember]
		[JsonProperty(PropertyName = "does_viewer_like")]
		public bool? DoesViewerLike;

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id;

		[DataMember]
		[JsonProperty(PropertyName = "legacy_api_post_id")]
		public string LegacyApiPostId;

		[DataMember]
		[JsonProperty(PropertyName = "likers")]
		public ActionCount Likers;

		[DataMember]
		[JsonProperty(PropertyName = "viewer_acts_as_page")]
		public ViewerActsAsPage ViewerActsAsPage;

		[JsonProperty(PropertyName = "viewer_does_not_like_sentence")]
		[DataMember]
		public StringEntity ViewerDoesNotLikeSentence;

		[JsonProperty(PropertyName = "viewer_likes_sentence")]
		[DataMember]
		public StringEntity ViewerLikesSentence;

		[JsonProperty(PropertyName = "comments")]
		[DataMember]
		public ObjectComments ObjectComments { get; set; }
	}
}

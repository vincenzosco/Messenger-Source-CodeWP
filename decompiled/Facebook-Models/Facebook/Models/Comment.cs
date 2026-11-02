using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Comment
	{
		[JsonProperty(PropertyName = "author")]
		[DataMember]
		public Actor Author;

		[JsonProperty(PropertyName = "body")]
		[DataMember]
		public StringEntity Body;

		[JsonProperty(PropertyName = "can_viewer_delete")]
		[DataMember]
		public bool? CanViewerDelete;

		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_edit")]
		public bool? CanViewerEdit;

		[JsonConverter(typeof(UnixDateTimeConverter))]
		[DataMember]
		[JsonProperty(PropertyName = "created_time")]
		public DateTime CreatedTime;

		[JsonProperty(PropertyName = "edit_history")]
		[DataMember]
		public ActionCount EditHistory;

		[JsonProperty(PropertyName = "feedback")]
		[DataMember]
		public Feedback Feedback;

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id;

		private bool? _hasPhoto;

		private FeedMedia _photoAttachment;

		[JsonProperty(PropertyName = "attachments")]
		[DataMember]
		public List<CommentAttachment> CommentAttachments { get; set; }

		public bool HasPhoto
		{
			get
			{
				if (_hasPhoto.HasValue)
				{
					return _hasPhoto.Value;
				}
				if (CommentAttachments == null || !CommentAttachments.Any())
				{
					bool? flag = (_hasPhoto = false);
					return flag.Value;
				}
				bool? flag2 = (_hasPhoto = CommentAttachments.Any((CommentAttachment a) => a.Media != null && a.Media.Type == FacebookSubType.Photo));
				return flag2.Value;
			}
		}

		public FeedMedia PhotoAttachment
		{
			get
			{
				if (_photoAttachment != null && !HasPhoto)
				{
					return _photoAttachment;
				}
				return _photoAttachment = CommentAttachments.FirstOrDefault((CommentAttachment a) => a.Media != null && a.Media.Type == FacebookSubType.Photo).Media;
			}
		}
	}
}

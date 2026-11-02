using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Story
	{
		private bool? _isSponsored;

		private Actor _primaryActor;

		private DateTime? _localCreationTime;

		private DateTime? _creationDateTime;

		private bool? _hasPhotoAttachment;

		private bool? _hasVideoAttachment;

		private bool? _hasMediaAttachment;

		private bool? _hasOpenGraphAttachment;

		private bool? _hasAttachmentTarget;

		private bool? _hasCheckin;

		private bool? _hasCommonAttachedStory;

		private bool? _hasCommonCheckin;

		private Place _commonCheckinPlace;

		private Story _commonAttachedStory;

		private bool? _hasCommonMediaAttachment;

		private Attachment _commonMediaAttachment;

		private bool? _hasCommonOpenGraphAttachment;

		private Attachment _commonOpenGraphAttachment;

		private bool? _hasFileAttachment;

		private Attachment _fileAttachment;

		private bool? _hasExternalUrl;

		private Attachment _externalUrlAttachment;

		private FeedMedia _videoMedia;

		private List<FeedMedia> _photoMedia;

		private Attachment _genericMediaAttachment;

		private Attachment _openGraphAttachment;

		private bool _truncateContent = true;

		public StoryType StoryType { get; set; }

		public Story Parent { get; set; }

		[JsonConverter(typeof(FacebookSubTypeConverter))]
		[DataMember]
		[JsonProperty(PropertyName = "__type__")]
		public FacebookSubType Type { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "actors")]
		public List<Actor> Actors { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "attached_story")]
		public Story AttachedStory { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "display_explanation")]
		public StringEntity DisplayExplanation { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "sponsored_data")]
		public SponsoredData SponsoredData { get; set; }

		[JsonProperty(PropertyName = "substories")]
		[DataMember]
		public List<Story> Substories { get; set; }

		[JsonProperty(PropertyName = "attachments")]
		[DataMember]
		public List<Attachment> Attachments { get; set; }

		[JsonProperty(PropertyName = "feedback")]
		[DataMember]
		public Feedback Feedback { get; set; }

		[JsonProperty(PropertyName = "cache_id")]
		[DataMember]
		public string CacheId { get; set; }

		[JsonProperty(PropertyName = "can_viewer_delete")]
		[DataMember]
		public bool? CanViewerDelete { get; set; }

		[JsonProperty(PropertyName = "can_viewer_edit")]
		[DataMember]
		public bool? CanViewerEdit { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "creation_time")]
		public long? CreationTime { get; set; }

		[JsonProperty(PropertyName = "edit_history")]
		[DataMember]
		public ActionCount EditHistory { get; set; }

		[JsonProperty(PropertyName = "has_comprehensive_title")]
		[DataMember]
		public bool? HasComprehensiveTitle { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "hideable_token")]
		public string HideableToken { get; set; }

		[JsonProperty(PropertyName = "tracking")]
		[DataMember]
		public string Tracking { get; set; }

		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "to")]
		public Actor To { get; set; }

		[JsonProperty(PropertyName = "via")]
		[DataMember]
		public Actor Via { get; set; }

		[JsonProperty(PropertyName = "with")]
		[DataMember]
		public List<Actor> With { get; set; }

		[JsonProperty(PropertyName = "legacy_api_story_id")]
		[DataMember]
		public string LegacyApiStoryId { get; set; }

		[JsonProperty(PropertyName = "legacy_story_hideable_id")]
		[DataMember]
		public string LegacyStoryHideableId { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "message")]
		public StringEntity Message { get; set; }

		[JsonProperty(PropertyName = "explicit_place")]
		[DataMember]
		public Place ExplicitPlace { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "implicit_place")]
		public Place ImplicitPlace { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "privacy_scope")]
		public PrivacyScope PrivacyScope { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "report_info")]
		public ReportInfo ReportInfo { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "shareable")]
		public Shareable Shareable { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "title")]
		public StringEntity Title { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "subtitle")]
		public StringEntity Subtitle { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "unit_photos")]
		public List<FeedMedia> UnitPhotos { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "icon_image")]
		public FacebookImage IconImage { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "digital_goods")]
		public List<DigitalGoodsUnit> DigitalGoods { get; set; }

		public List<string> TrackingCodes { get; set; }

		public bool CanReport
		{
			get
			{
				if (ReportInfo != null)
				{
					return ReportInfo.ActionUrl != null;
				}
				return false;
			}
		}

		public bool IsSponsored
		{
			get
			{
				if (_isSponsored.HasValue)
				{
					return _isSponsored.Value;
				}
				if (SponsoredData != null)
				{
					bool? flag = (_isSponsored = true);
					return flag.Value;
				}
				for (Story parent = Parent; parent != null; parent = parent.Parent)
				{
					if (parent.IsSponsored)
					{
						bool? flag2 = (_isSponsored = true);
						return flag2.Value;
					}
				}
				bool? flag3 = (_isSponsored = false);
				return flag3.Value;
			}
		}

		public Actor PrimaryActor
		{
			get
			{
				if (_primaryActor == null && Actors != null && Actors.Any())
				{
					if (Actors.Count > 1 && Title != null && Title.Ranges != null && Title.Ranges.Any() && Title.Ranges[0].Entity != null)
					{
						_primaryActor = Actors.FirstOrDefault((Actor a) => a.Id != null && a.Id.Equals(Title.Ranges[0].Entity.Id));
					}
					_primaryActor = _primaryActor ?? Actors[0];
				}
				return _primaryActor;
			}
		}

		public DateTime LocalCreationTime
		{
			get
			{
				if (!_localCreationTime.HasValue)
				{
					_localCreationTime = CreationDateTime.ToLocalTime();
				}
				return _localCreationTime.Value;
			}
		}

		public DateTime CreationDateTime
		{
			get
			{
				if (_creationDateTime.HasValue)
				{
					return _creationDateTime.Value;
				}
				DateTime? dateTime = (_creationDateTime = DateExtensions.FromUnixTimeSeconds(CreationTime ?? 0));
				return dateTime.Value;
			}
		}

		public bool HasNestedStories
		{
			get
			{
				if (AttachedStory == null)
				{
					return HasSubstories;
				}
				return true;
			}
		}

		public bool HasSubstories
		{
			get
			{
				if (Substories != null && !Substories.All((Story s) => s.Attachments != null && s.Attachments.Any() && s.Attachments.All((Attachment a) => a.Target != null && a.Target.Type == FacebookSubType.User)))
				{
					return !Substories.All((Story s) => s.Attachments != null && s.Attachments.Any() && s.Attachments.All((Attachment a) => a.Target != null && a.Target.Type == FacebookSubType.Page));
				}
				return false;
			}
		}

		public bool HasDigitalGoods
		{
			get
			{
				if (DigitalGoods != null)
				{
					return DigitalGoods.Count > 0;
				}
				return false;
			}
		}

		public bool HasPhotoAttachment
		{
			get
			{
				if (!_hasPhotoAttachment.HasValue)
				{
					if (UnitPhotos != null && UnitPhotos.Any())
					{
						bool? flag = (_hasPhotoAttachment = true);
						return flag.Value;
					}
					if (Attachments != null && Attachments.Any())
					{
						foreach (Attachment attachment in Attachments)
						{
							if (attachment.SubAttachments != null && attachment.SubAttachments.Any((Attachment subattachment) => subattachment.HasPhotoAttachment))
							{
								bool? flag2 = (_hasPhotoAttachment = true);
								return flag2.Value;
							}
							if (attachment.HasPhotoAttachment)
							{
								bool? flag3 = (_hasPhotoAttachment = true);
								return flag3.Value;
							}
						}
					}
					_hasPhotoAttachment = false;
				}
				return _hasPhotoAttachment.Value;
			}
		}

		public bool HasVideoAttachment
		{
			get
			{
				if (!_hasVideoAttachment.HasValue)
				{
					if (Attachments != null && Attachments.Any())
					{
						foreach (Attachment attachment in Attachments)
						{
							if (attachment.SubAttachments != null && attachment.SubAttachments.Any((Attachment subattachment) => subattachment.HasVideoAttachment))
							{
								bool? flag = (_hasVideoAttachment = true);
								return flag.Value;
							}
							if (attachment.HasVideoAttachment)
							{
								bool? flag2 = (_hasVideoAttachment = true);
								return flag2.Value;
							}
						}
					}
					_hasVideoAttachment = false;
				}
				return _hasVideoAttachment.Value;
			}
		}

		public bool HasMediaAttachment
		{
			get
			{
				if (!_hasMediaAttachment.HasValue)
				{
					if (Attachments != null && Attachments.Any())
					{
						foreach (Attachment attachment in Attachments)
						{
							if (attachment.HasMediaAttachment)
							{
								bool? flag = (_hasMediaAttachment = true);
								return flag.Value;
							}
							if (attachment.SubAttachments != null && attachment.SubAttachments.Any((Attachment subattachment) => subattachment.HasMediaAttachment))
							{
								bool? flag2 = (_hasMediaAttachment = true);
								return flag2.Value;
							}
						}
					}
					_hasMediaAttachment = false;
				}
				return _hasMediaAttachment.Value;
			}
		}

		public bool HasOpenGraphAttachment
		{
			get
			{
				if (!_hasOpenGraphAttachment.HasValue)
				{
					if (Attachments != null && Attachments.Any())
					{
						foreach (Attachment attachment in Attachments)
						{
							if (attachment.HasOpenGraphAttachment)
							{
								bool? flag = (_hasOpenGraphAttachment = true);
								return flag.Value;
							}
							if (attachment.SubAttachments != null && attachment.SubAttachments.Any((Attachment subattachment) => subattachment.HasOpenGraphAttachment))
							{
								bool? flag2 = (_hasOpenGraphAttachment = true);
								return flag2.Value;
							}
						}
					}
					_hasOpenGraphAttachment = false;
				}
				return _hasOpenGraphAttachment.Value;
			}
		}

		public bool HasAttachmentTarget
		{
			get
			{
				if (_hasAttachmentTarget.HasValue)
				{
					return _hasAttachmentTarget.Value;
				}
				if (Attachments == null || !Attachments.Any() || HasCheckin || HasExplicitPlace || HasMediaAttachment || HasPhotoAttachment || HasVideoAttachment)
				{
					bool? flag = (_hasAttachmentTarget = false);
					return flag.Value;
				}
				Attachment attachment = Attachments[0];
				bool? flag2 = (_hasAttachmentTarget = attachment.Target != null && (attachment.Target.Type == FacebookSubType.User || attachment.Target.Type == FacebookSubType.Page));
				return flag2.Value;
			}
		}

		public bool HasExplicitPlace
		{
			get
			{
				if (ExplicitPlace != null && ExplicitPlace.PlaceType != null)
				{
					return ExplicitPlace.Type != FacebookSubType.FreeformPlace;
				}
				return false;
			}
		}

		public bool HasEvent
		{
			get
			{
				if (Attachments == null || !Attachments.Any())
				{
					return false;
				}
				Attachment attachment = Attachments[0];
				if (attachment.Target != null)
				{
					return attachment.Target.Type == FacebookSubType.Event;
				}
				return false;
			}
		}

		public bool HasCheckin
		{
			get
			{
				if (_hasCheckin.HasValue)
				{
					return _hasCheckin.Value;
				}
				bool? flag = (_hasCheckin = HasExplicitPlace && ExplicitPlace.Type == FacebookSubType.Page && !HasPhotoAttachment && !HasVideoAttachment && !HasMediaAttachment && !HasOpenGraphAttachment);
				return flag.Value;
			}
		}

		public bool HasCommonAttachedStory
		{
			get
			{
				if (_hasCommonAttachedStory.HasValue)
				{
					return _hasCommonAttachedStory.Value;
				}
				if (Substories == null || Substories.Count < 2 || Substories.Any((Story s) => s.AttachedStory == null))
				{
					bool? flag = (_hasCommonAttachedStory = false);
					return flag.Value;
				}
				Story attachedStory = Substories[0].AttachedStory;
				bool? flag2 = (_hasCommonAttachedStory = Substories.All((Story s) => s.AttachedStory != null && s.AttachedStory.Id != null && s.AttachedStory.Id.Equals(attachedStory.Id)));
				return flag2.Value;
			}
		}

		public bool HasCommonCheckin
		{
			get
			{
				if (_hasCommonCheckin.HasValue)
				{
					return _hasCommonCheckin.Value;
				}
				if (HasCheckin || Substories == null || Substories.Count < 2 || Substories.Any((Story s) => !s.HasCheckin))
				{
					bool? flag = (_hasCommonCheckin = false);
					return flag.Value;
				}
				Place checkin = Substories[0].ExplicitPlace;
				bool? flag2 = (_hasCommonCheckin = Substories.All((Story s) => s.ExplicitPlace != null && s.ExplicitPlace.Id != null && s.ExplicitPlace.Id.Equals(checkin.Id)));
				return flag2.Value;
			}
		}

		public Place CommonCheckinPlace
		{
			get
			{
				if (_commonCheckinPlace != null || !HasCommonCheckin)
				{
					return _commonCheckinPlace;
				}
				return _commonCheckinPlace = Substories[0].ExplicitPlace;
			}
		}

		public Story CommonAttachedStory
		{
			get
			{
				if (_commonAttachedStory != null || !HasCommonAttachedStory)
				{
					return _commonAttachedStory;
				}
				return _commonAttachedStory = Substories[0].AttachedStory;
			}
		}

		public bool HasCommonMediaAttachment
		{
			get
			{
				if (_hasCommonMediaAttachment.HasValue)
				{
					return _hasCommonMediaAttachment.Value;
				}
				if (HasMediaAttachment || Substories == null || Substories.Count < 2 || Substories.Any((Story s) => !s.HasMediaAttachment))
				{
					bool? flag = (_hasCommonMediaAttachment = false);
					return flag.Value;
				}
				Attachment attachment = Substories[0].MediaAttachment;
				bool? flag2 = (_hasCommonMediaAttachment = Substories.All((Story s) => s.MediaAttachment != null && s.MediaAttachment.MatchesAttachment(attachment)));
				return flag2.Value;
			}
		}

		public Attachment CommonMediaAttachment
		{
			get
			{
				if (_commonMediaAttachment != null || !HasCommonMediaAttachment)
				{
					return _commonMediaAttachment;
				}
				return _commonMediaAttachment = Substories[0].MediaAttachment;
			}
		}

		public bool HasCommonOpenGraphAttachment
		{
			get
			{
				if (_hasCommonOpenGraphAttachment.HasValue)
				{
					return _hasCommonOpenGraphAttachment.Value;
				}
				if (HasMediaAttachment || Substories == null || Substories.Count < 2 || Substories.Any((Story s) => !s.HasOpenGraphAttachment))
				{
					bool? flag = (_hasCommonOpenGraphAttachment = false);
					return flag.Value;
				}
				Attachment attachment = Substories[0].OpenGraphAttachment;
				bool? flag2 = (_hasCommonOpenGraphAttachment = Substories.All((Story s) => s.OpenGraphAttachment != null && s.OpenGraphAttachment.MatchesAttachment(attachment)));
				return flag2.Value;
			}
		}

		public Attachment CommonOpenGraphAttachment
		{
			get
			{
				if (_commonOpenGraphAttachment != null || !HasCommonOpenGraphAttachment)
				{
					return _commonOpenGraphAttachment;
				}
				return _commonOpenGraphAttachment = Substories[0].OpenGraphAttachment;
			}
		}

		public bool HasFileAttachment
		{
			get
			{
				if (_hasFileAttachment.HasValue)
				{
					return _hasFileAttachment.Value;
				}
				bool? flag = (_hasFileAttachment = Attachments != null && Attachments.Any((Attachment a) => a.HasFileAttachment));
				return flag.Value;
			}
		}

		public Attachment FileAttachment
		{
			get
			{
				if (_fileAttachment != null || !HasFileAttachment)
				{
					return _fileAttachment;
				}
				return _fileAttachment = Attachments.FirstOrDefault((Attachment a) => a.HasFileAttachment);
			}
		}

		public bool HasExternalUrl
		{
			get
			{
				if (_hasExternalUrl.HasValue)
				{
					return _hasExternalUrl.Value;
				}
				bool? flag = (_hasExternalUrl = HasMediaAttachment || HasOpenGraphAttachment || HasCommonMediaAttachment || HasCommonOpenGraphAttachment || HasFileAttachment);
				return flag.Value;
			}
		}

		public Attachment ExternalUrlAttachment
		{
			get
			{
				if (_externalUrlAttachment != null || !HasExternalUrl)
				{
					return _externalUrlAttachment;
				}
				if (HasMediaAttachment)
				{
					return _externalUrlAttachment = MediaAttachment;
				}
				if (HasOpenGraphAttachment)
				{
					return _externalUrlAttachment = OpenGraphAttachment;
				}
				if (HasCommonMediaAttachment)
				{
					return _externalUrlAttachment = CommonMediaAttachment;
				}
				if (HasCommonOpenGraphAttachment)
				{
					return _externalUrlAttachment = CommonOpenGraphAttachment;
				}
				if (HasFileAttachment)
				{
					return _externalUrlAttachment = FileAttachment;
				}
				return null;
			}
		}

		public bool HasFeedback
		{
			get
			{
				if (!HasLikes)
				{
					return HasComments;
				}
				return true;
			}
		}

		public bool HasLikes
		{
			get
			{
				if (Feedback != null)
				{
					if (Feedback.Likers != null && Feedback.Likers.Count.HasValue)
					{
						return Feedback.Likers.Count.Value > 0;
					}
					return false;
				}
				return false;
			}
		}

		public bool HasComments
		{
			get
			{
				if (Feedback != null)
				{
					if (Feedback.Comments != null && Feedback.Comments.Count.HasValue)
					{
						return Feedback.Comments.Count.Value > 0;
					}
					return false;
				}
				return false;
			}
		}

		public bool CanProvideFeedback
		{
			get
			{
				if (!CanLike)
				{
					return CanComment;
				}
				return true;
			}
		}

		public bool CanShare
		{
			get
			{
				if (Feedback != null)
				{
					return Shareable != null;
				}
				return false;
			}
		}

		public bool CanLike
		{
			get
			{
				if (Feedback != null)
				{
					return Feedback.CanViewerLike ?? false;
				}
				return false;
			}
		}

		public bool CanComment
		{
			get
			{
				if (Feedback != null)
				{
					return Feedback.CanViewerComment ?? false;
				}
				return false;
			}
		}

		public bool AtSpecificPlace
		{
			get
			{
				if (ExplicitPlace != null)
				{
					if (ImplicitPlace != null)
					{
						if (ExplicitPlace.Id != null)
						{
							return !ExplicitPlace.Id.Equals(ImplicitPlace.Id);
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		public FeedMedia VideoMedia
		{
			get
			{
				if (_videoMedia == null && Attachments != null && Attachments.Any())
				{
					Attachment attachment = Attachments[0];
					if (attachment != null && attachment.SubAttachments != null && attachment.SubAttachments.Any())
					{
						Attachment attachment2 = attachment.SubAttachments.FirstOrDefault((Attachment a) => a.Media != null && a.Media.Type == FacebookSubType.Video);
						if (attachment2 != null)
						{
							_videoMedia = attachment2.Media;
						}
					}
					else
					{
						attachment = Attachments.FirstOrDefault((Attachment a) => a.Media != null && a.Media.Type == FacebookSubType.Video);
						if (attachment != null)
						{
							_videoMedia = attachment.Media;
						}
					}
				}
				return _videoMedia;
			}
		}

		public List<FeedMedia> PhotoMedia
		{
			get
			{
				if (_photoMedia == null)
				{
					if (UnitPhotos != null && UnitPhotos.Any())
					{
						return _photoMedia = UnitPhotos;
					}
					if (Attachments != null && Attachments.Any())
					{
						Attachment attachment = Attachments[0];
						if (attachment != null && attachment.SubAttachments != null && attachment.SubAttachments.Any())
						{
							IEnumerable<Attachment> enumerable = attachment.SubAttachments.Where((Attachment a) => a.Media != null && a.Media.Type == FacebookSubType.Photo);
							IList<Attachment> source = (enumerable as IList<Attachment>) ?? enumerable.ToList();
							if (source.Any())
							{
								_photoMedia = source.Select((Attachment a) => a.Media).ToList();
							}
						}
						else
						{
							IEnumerable<Attachment> enumerable2 = Attachments.Where((Attachment a) => a.Media != null && a.Media.Type == FacebookSubType.Photo);
							IList<Attachment> source2 = (enumerable2 as IList<Attachment>) ?? enumerable2.ToList();
							if (source2.Any())
							{
								_photoMedia = source2.Select((Attachment a) => a.Media).ToList();
							}
						}
					}
				}
				return _photoMedia;
			}
		}

		public Attachment MediaAttachment
		{
			get
			{
				if (_genericMediaAttachment == null && Attachments != null && Attachments.Any())
				{
					_genericMediaAttachment = Attachments.FirstOrDefault((Attachment a) => a.HasMediaAttachment);
				}
				return _genericMediaAttachment;
			}
		}

		public Attachment OpenGraphAttachment
		{
			get
			{
				if (_openGraphAttachment == null && Attachments != null && Attachments.Any())
				{
					_openGraphAttachment = Attachments.FirstOrDefault((Attachment a) => a.HasOpenGraphAttachment);
					if (_openGraphAttachment == null)
					{
						foreach (Attachment attachment in Attachments)
						{
							if (attachment.SubAttachments != null && attachment.SubAttachments.Any())
							{
								_openGraphAttachment = attachment.SubAttachments.FirstOrDefault((Attachment s) => s.HasOpenGraphAttachment);
							}
						}
					}
				}
				return _openGraphAttachment;
			}
		}

		public bool TruncateContent
		{
			get
			{
				return _truncateContent;
			}
			set
			{
				if (_truncateContent == value)
				{
					return;
				}
				_truncateContent = value;
				if (AttachedStory != null)
				{
					AttachedStory.TruncateContent = _truncateContent;
				}
				if (Substories == null || !Substories.Any())
				{
					return;
				}
				foreach (Story substory in Substories)
				{
					substory.TruncateContent = _truncateContent;
				}
			}
		}

		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			List<string> list = new List<string>();
			if (Tracking != null)
			{
				list.Add(Tracking);
			}
			if (AttachedStory != null)
			{
				AttachedStory.Parent = this;
				if (AttachedStory.TrackingCodes != null)
				{
					list.AddRange(AttachedStory.TrackingCodes);
				}
			}
			if (Substories != null && Substories.Any())
			{
				foreach (Story substory in Substories)
				{
					substory.Parent = this;
					if (substory.TrackingCodes != null)
					{
						list.AddRange(substory.TrackingCodes);
					}
				}
			}
			if (list.Any())
			{
				TrackingCodes = list;
			}
			if (HasDigitalGoods)
			{
				DigitalGoodsUnit digitalGoodsUnit = DigitalGoods.FirstOrDefault();
				if (digitalGoodsUnit != null)
				{
					digitalGoodsUnit.Parent = this;
					DisplayExplanation = new StringEntity
					{
						Text = digitalGoodsUnit.Explanation
					};
					Message = digitalGoodsUnit.Body;
					Title = new StringEntity
					{
						Text = digitalGoodsUnit.Name
					};
					SponsoredData = digitalGoodsUnit.SponsoredData;
				}
			}
		}
	}
}

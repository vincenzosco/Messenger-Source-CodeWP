using System.Runtime.Serialization;

namespace Facebook.Models;

public enum FacebookSubType
{
	Unknown,
	[EnumMember(Value = "NotificationActor")]
	NotificationActor,
	[EnumMember(Value = "Page")]
	Page,
	[EnumMember(Value = "ThirdPartyUser")]
	ThirdPartyUser,
	[EnumMember(Value = "User")]
	User,
	[EnumMember(Value = "AdAccount")]
	AdAccount,
	[EnumMember(Value = "Album")]
	Album,
	[EnumMember(Value = "Application")]
	Application,
	[EnumMember(Value = "Bookmark")]
	Bookmark,
	[EnumMember(Value = "Comment")]
	Comment,
	[EnumMember(Value = "ComputerVisionInfo")]
	ComputerVisionInfo,
	[EnumMember(Value = "Contact")]
	Contact,
	[EnumMember(Value = "ContactEmail")]
	ContactEmail,
	[EnumMember(Value = "ContactEntry")]
	ContactEntry,
	[EnumMember(Value = "ContactPhone")]
	ContactPhone,
	[EnumMember(Value = "ContactRecommendationField")]
	ContactRecommendationField,
	[EnumMember(Value = "Coupon")]
	Coupon,
	[EnumMember(Value = "EditAction")]
	EditAction,
	[EnumMember(Value = "EducationExperience")]
	EducationExperience,
	[EnumMember(Value = "Event")]
	Event,
	[EnumMember(Value = "ExternalUrl")]
	ExternalUrl,
	[EnumMember(Value = "FaceBox")]
	FaceBox,
	[EnumMember(Value = "FreeformPlace")]
	FreeformPlace,
	[EnumMember(Value = "FreeformTag")]
	FreeformTag,
	[EnumMember(Value = "FriendList")]
	FriendList,
	[EnumMember(Value = "GiftCardCategory")]
	GiftCardCategory,
	[EnumMember(Value = "GiftCoupon")]
	GiftCoupon,
	[EnumMember(Value = "GiftProduct")]
	GiftProduct,
	[EnumMember(Value = "GiftProductSku")]
	GiftProductSku,
	[EnumMember(Value = "Group")]
	Group,
	[EnumMember(Value = "GroupFactory")]
	GroupFactory,
	[EnumMember(Value = "Hashtag")]
	Hashtag,
	[EnumMember(Value = "LifeEventUnit")]
	LifeEventUnit,
	[EnumMember(Value = "MultiBackgroundIcon")]
	MultiBackgroundIcon,
	[EnumMember(Value = "Note")]
	Note,
	[EnumMember(Value = "OpenGraphAction")]
	OpenGraphAction,
	[EnumMember(Value = "OpenGraphObject")]
	OpenGraphObject,
	[EnumMember(Value = "Photo")]
	Photo,
	[EnumMember(Value = "PremiumVideosFeedUnit")]
	PremiumVideosFeedUnit,
	[EnumMember(Value = "PrivacyOption")]
	PrivacyOption,
	[EnumMember(Value = "Question")]
	Question,
	[EnumMember(Value = "QuestionOption")]
	QuestionOption,
	[EnumMember(Value = "SchoolClassExperience")]
	SchoolClassExperience,
	[EnumMember(Value = "Story")]
	Story,
	[EnumMember(Value = "StoryAttachment")]
	StoryAttachment,
	[EnumMember(Value = "Survey")]
	Survey,
	[EnumMember(Value = "SurveyQuestion")]
	SurveyQuestion,
	[EnumMember(Value = "TVAiring")]
	TVAiring,
	[EnumMember(Value = "TVSource")]
	TVSource,
	[EnumMember(Value = "TaggableActivity")]
	TaggableActivity,
	[EnumMember(Value = "TaggableActivityIcon")]
	TaggableActivityIcon,
	[EnumMember(Value = "TaggableActivityObject")]
	TaggableActivityObject,
	[EnumMember(Value = "TaggedMediaOfUserMediaSet")]
	TaggedMediaOfUserMediaSet,
	[EnumMember(Value = "Task")]
	Task,
	[EnumMember(Value = "TimelineAppCollection")]
	TimelineAppCollection,
	[EnumMember(Value = "TimelineAppCollectionItem")]
	TimelineAppCollectionItem,
	[EnumMember(Value = "TimelineAppSection")]
	TimelineAppSection,
	[EnumMember(Value = "TimelineContactItemResult")]
	TimelineContactItemResult,
	[EnumMember(Value = "TimelineSection")]
	TimelineSection,
	[EnumMember(Value = "Video")]
	Video,
	[EnumMember(Value = "WorkExperience")]
	WorkExperience,
	[EnumMember(Value = "WorkProjectExperience")]
	WorkProjectExperience,
	[EnumMember(Value = "CouponBookletUnit")]
	CouponBookletUnit,
	[EnumMember(Value = "DigitalGoodsFeedUnit")]
	DigitalGoodsFeedUnit,
	[EnumMember(Value = "HoldoutAdFeedUnit")]
	HoldoutAdFeedUnit,
	[EnumMember(Value = "ItemListFeedUnit")]
	ItemListFeedUnit,
	[EnumMember(Value = "GraphSearchQueryFilterCustomNumericValue")]
	GraphSearchQueryFilterCustomNumericValue,
	[EnumMember(Value = "GraphSearchQueryFilterCustomPageValue")]
	GraphSearchQueryFilterCustomPageValue,
	[EnumMember(Value = "GraphSearchQueryFilterCustomTextValue")]
	GraphSearchQueryFilterCustomTextValue,
	[EnumMember(Value = "CelebrationsFeedUnit")]
	CelebrationsFeedUnit,
	[EnumMember(Value = "CreativePagesYouMayLikeFeedUnit")]
	CreativePagesYouMayLikeFeedUnit,
	[EnumMember(Value = "EventsYouMayLikeFeedUnit")]
	EventsYouMayLikeFeedUnit,
	[EnumMember(Value = "PagesYouMayLikeFeedUnit")]
	PagesYouMayLikeFeedUnit,
	[EnumMember(Value = "PeopleYouMayKnowFeedUnit")]
	PeopleYouMayKnowFeedUnit,
	[EnumMember(Value = "PopularObjectsFeedUnit")]
	PopularObjectsFeedUnit,
	[EnumMember(Value = "PresenceFeedUnit")]
	PresenceFeedUnit,
	[EnumMember(Value = "RecommendedApplicationsFeedUnit")]
	RecommendedApplicationsFeedUnit,
	[EnumMember(Value = "TrendingEntitiesFeedUnit")]
	TrendingEntitiesFeedUnit,
	[EnumMember(Value = "TrendingGamesSummaryFeedUnit")]
	TrendingGamesSummaryFeedUnit,
	[EnumMember(Value = "CelebrationsFeedUnitItem")]
	CelebrationsFeedUnitItem,
	[EnumMember(Value = "CreativePagesYouMayLikeFeedUnitItem")]
	CreativePagesYouMayLikeFeedUnitItem,
	[EnumMember(Value = "EventsYouMayLikeFeedUnitItem")]
	EventsYouMayLikeFeedUnitItem,
	[EnumMember(Value = "PagesYouMayLikeFeedUnitItem")]
	PagesYouMayLikeFeedUnitItem,
	[EnumMember(Value = "PeopleYouMayKnowFeedUnitItem")]
	PeopleYouMayKnowFeedUnitItem,
	[EnumMember(Value = "PopularObjectsFeedUnitItem")]
	PopularObjectsFeedUnitItem,
	[EnumMember(Value = "RecommendedApplicationsFeedUnitItem")]
	RecommendedApplicationsFeedUnitItem,
	[EnumMember(Value = "TrendingEntitiesFeedUnitItem")]
	TrendingEntitiesFeedUnitItem,
	[EnumMember(Value = "TrendingGamesSummaryFeedUnitItem")]
	TrendingGamesSummaryFeedUnitItem,
	[EnumMember(Value = "GenericAttachmentMedia")]
	GenericAttachmentMedia,
	[EnumMember(Value = "ProfilePicAttachmentMedia")]
	ProfilePicAttachmentMedia,
	[EnumMember(Value = "BirthdayMomentUnit")]
	BirthdayMomentUnit,
	[EnumMember(Value = "RelationshipMomentUnit")]
	RelationshipMomentUnit,
	[EnumMember(Value = "Feedback")]
	Feedback,
	[EnumMember(Value = "GraphSearchQuery")]
	GraphSearchQuery,
	[EnumMember(Value = "AddFriendActionLink")]
	AddFriendActionLink,
	[EnumMember(Value = "AddToTimelineAppCollectionActionLink")]
	AddToTimelineAppCollectionActionLink,
	[EnumMember(Value = "GetCouponActionLink")]
	GetCouponActionLink,
	[EnumMember(Value = "InstallAppActionLink")]
	InstallAppActionLink,
	[EnumMember(Value = "LikePageActionLink")]
	LikePageActionLink,
	[EnumMember(Value = "RemoveFromTimelineAppCollectionActionLink")]
	RemoveFromTimelineAppCollectionActionLink,
	[EnumMember(Value = "ReportActionLink")]
	ReportActionLink
}

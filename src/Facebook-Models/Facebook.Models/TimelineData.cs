using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class TimelineData
	{
		[JsonProperty(PropertyName = "timeline_sections")]
		[DataMember]
		public TimelineSections TimelineSections { get; set; }

		[JsonProperty(PropertyName = "recent_photo")]
		[DataMember]
		public RecentPhoto RecentPhoto { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "profile_picture_is_silhouette")]
		public bool? ProfilePicIsSilhouette { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "profile_photo")]
		public FeedPhoto ProfilePhoto { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "is_verified")]
		[DataMember]
		public bool? IsVerified { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "gender")]
		[JsonConverter(typeof(StringEnumConverter))]
		[DataMember]
		public Gender? Gender { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(PropertyName = "subscribe_status")]
		[DataMember]
		public SubscribeStatus? SubscribedStatus { get; set; }

		[JsonProperty(PropertyName = "friendship_status")]
		[JsonConverter(typeof(StringEnumConverter))]
		[DataMember]
		public FriendRequestStatus? FriendshipStatus { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "featured_about_profiles")]
		public FeaturedContent FeaturedAboutProfiles { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "featured_friends")]
		public FeaturedContent FeaturedFriends { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "featuredLikedProfiles")]
		public FeaturedContent FeaturedLikedProfiles { get; set; }

		[JsonProperty(PropertyName = "cover_photo")]
		[DataMember]
		public CoverPhoto CoverPhoto { get; set; }

		[JsonProperty(PropertyName = "can_viewer_send_gift")]
		[DataMember]
		public bool? CanViewerSendGift { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_post")]
		public bool? CanViewerPost { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_poke")]
		public bool? CanViewerPoke { get; set; }

		[JsonProperty(PropertyName = "can_viewer_subscribe")]
		[DataMember]
		public bool? CanViewerSubscribe { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "is_viewer_subscribed")]
		public bool? IsViewerSubscribed { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "can_viewer_message")]
		public bool? CanViewerMessage { get; set; }

		[JsonProperty(PropertyName = "bylines")]
		[DataMember]
		public List<Byline> Bylines { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "alternate_name")]
		public string AlternateName { get; set; }

		[JsonProperty(PropertyName = "all_phones")]
		[DataMember]
		public List<Phone> AllPhones { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "timeline_collection_app_sections")]
		public TimelineCollectionAppSections AppSections { get; set; }

		public bool CanFollow
		{
			get
			{
				if (CanViewerSubscribe.HasValue)
				{
					return CanViewerSubscribe.Value;
				}
				if (FriendshipStatus != FriendRequestStatus.AreFriends)
				{
					if (SubscribedStatus != SubscribeStatus.CanSubscribe)
					{
						return SubscribedStatus == SubscribeStatus.IsSubscribed;
					}
					return true;
				}
				return false;
			}
		}

		public bool IsFollowing
		{
			get
			{
				if (IsViewerSubscribed.HasValue)
				{
					return IsViewerSubscribed.Value;
				}
				return SubscribedStatus == SubscribeStatus.IsSubscribed;
			}
		}
	}
}

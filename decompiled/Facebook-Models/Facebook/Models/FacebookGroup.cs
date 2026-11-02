using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class FacebookGroup
{
	[DataMember]
	[JsonProperty("cover_photo")]
	public CoverPhoto CoverPhoto { get; set; }

	[DataMember]
	[JsonProperty("description")]
	public string Description { get; set; }

	[DataMember]
	[JsonProperty("id")]
	public string Id { get; set; }

	[DataMember]
	[JsonProperty("is_viewer_admin")]
	public bool? Admin { get; set; }

	[JsonProperty("name")]
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	[JsonProperty("viewer_join_state")]
	public string JoinState { get; set; }

	[JsonProperty("viewer_post_status")]
	[DataMember]
	public string PostStatus { get; set; }

	[JsonProperty("visibility_sentence")]
	[DataMember]
	public GroupVisibility GroupVisibility { get; set; }

	[JsonProperty("group_icon")]
	[DataMember]
	public GroupIcon GroupIcons { get; set; }

	[JsonProperty("group_members")]
	[DataMember]
	public ActionCount GroupMembers { get; set; }

	[JsonProperty("child_groups")]
	[DataMember]
	public Group ChildGroups { get; set; }

	[JsonProperty("group_pinned_stories")]
	[DataMember]
	public GroupStoryEdges PinnedStories { get; set; }
}

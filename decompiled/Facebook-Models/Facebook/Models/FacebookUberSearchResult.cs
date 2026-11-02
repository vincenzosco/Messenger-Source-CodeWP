using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Models;

public class FacebookUberSearchResult
{
	private bool _viewerLikes;

	private string _displaySubtext;

	[JsonProperty(PropertyName = "category")]
	[DataMember]
	public string Category { get; set; }

	[JsonProperty(PropertyName = "friendship_status")]
	[JsonConverter(typeof(StringEnumConverter))]
	[DataMember]
	public FriendRequestStatus FriendshipStatus { get; set; }

	[JsonProperty(PropertyName = "path")]
	[DataMember]
	public string Path { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "paths")]
	public List<string> Paths { get; set; }

	[JsonProperty(PropertyName = "photo")]
	[DataMember]
	public string Photo { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "subtext")]
	public string Subtext { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "text")]
	public string Text { get; set; }

	[JsonProperty(PropertyName = "type")]
	[DataMember]
	[JsonConverter(typeof(StringEnumConverter))]
	public SearchResultType Type { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "uid")]
	public string Uid { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "is_verified")]
	public bool IsVerified { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "native_app_url")]
	public string NativeAppUrl { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "title_html")]
	public string TitleHtml { get; set; }

	[JsonProperty(PropertyName = "does_viewer_like")]
	[DataMember]
	public bool ViewerLikes
	{
		get
		{
			return _viewerLikes;
		}
		set
		{
			_viewerLikes = value;
			ShowSelected = ShowSelected || value;
		}
	}

	[DataMember]
	[JsonProperty(PropertyName = "is_owned")]
	public bool IsOwned { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "expressed_as_place")]
	public bool ExpressedAsPlace { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "super_category")]
	public string SuperCategory { get; set; }

	public string DisplaySubtext
	{
		get
		{
			if (_displaySubtext != null)
			{
				return _displaySubtext;
			}
			switch (Type)
			{
			case SearchResultType.User:
				_displaySubtext = ((!string.IsNullOrEmpty(Subtext)) ? Subtext : Category);
				break;
			case SearchResultType.Page:
			case SearchResultType.App:
			case SearchResultType.Group:
				_displaySubtext = Type.ToString();
				if (!string.IsNullOrEmpty(Subtext))
				{
					_displaySubtext = _displaySubtext + " · " + Subtext;
				}
				break;
			}
			return _displaySubtext;
		}
	}

	public bool ShowSelected { get; set; }

	public bool IsOperable
	{
		get
		{
			switch (Type)
			{
			case SearchResultType.Page:
				return true;
			case SearchResultType.App:
				return false;
			case SearchResultType.User:
				if (FriendshipStatus != FriendRequestStatus.CanRequest)
				{
					return FriendshipStatus == FriendRequestStatus.OutgoingRequest;
				}
				return true;
			case SearchResultType.Group:
				return false;
			case SearchResultType.HashtagExact:
				return false;
			default:
				return false;
			}
		}
	}

	[DataMember]
	[JsonProperty(PropertyName = "is_messenger_user")]
	public bool IsMessengerUser { get; set; }

	public bool IsSelected { get; set; }
}

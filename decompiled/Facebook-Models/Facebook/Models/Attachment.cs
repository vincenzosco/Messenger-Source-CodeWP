using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Facebook.Models;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class Attachment
{
	private string _subline;

	[JsonProperty(PropertyName = "__type__")]
	[JsonConverter(typeof(FacebookSubTypeConverter))]
	[DataMember]
	public FacebookSubType Type { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "subattachments")]
	public List<Attachment> SubAttachments { get; set; }

	[JsonProperty(PropertyName = "target")]
	[DataMember]
	public Target Target { get; set; }

	[JsonProperty(PropertyName = "media")]
	[DataMember]
	public FeedMedia Media { get; set; }

	[JsonProperty(PropertyName = "source")]
	[DataMember]
	public StringEntity Source { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "description")]
	public StringEntity Description { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "title")]
	public string Title { get; set; }

	[JsonProperty(PropertyName = "url")]
	[DataMember]
	public string Url { get; set; }

	public string Subline
	{
		get
		{
			if (_subline == null)
			{
				if (Target.Description != null)
				{
					_subline = Target.Description;
				}
				else if (Target.Type == FacebookSubType.Page)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (Target.CategoryNames != null && Target.CategoryNames.Any())
					{
						stringBuilder.Append(Target.CategoryNames[0]);
					}
					if (Target.PageLikers != null)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(" · ");
						}
						stringBuilder.AppendFormat("{0:N0} like", new object[1] { Target.PageLikers.Count });
						if (Target.PageLikers.Count > 0)
						{
							stringBuilder.Append("s");
						}
					}
					_subline = stringBuilder.ToString();
				}
				else if (Description != null)
				{
					_subline = Description.Text;
				}
				else
				{
					_subline = string.Empty;
				}
			}
			return _subline;
		}
	}

	public bool HasFileAttachment
	{
		get
		{
			if (Type == FacebookSubType.StoryAttachment && Media == null && (SubAttachments == null || !SubAttachments.Any()) && Target == null && Url != null)
			{
				return Title != null;
			}
			return false;
		}
	}

	public bool HasPhotoAttachment
	{
		get
		{
			if (Media != null)
			{
				return Media.Type == FacebookSubType.Photo;
			}
			return false;
		}
	}

	public bool HasVideoAttachment
	{
		get
		{
			if (Media != null)
			{
				return Media.Type == FacebookSubType.Video;
			}
			return false;
		}
	}

	public bool HasMediaAttachment
	{
		get
		{
			bool flag = Target != null && Target.Type == FacebookSubType.ExternalUrl;
			if (Media != null)
			{
				flag &= Media.Type == FacebookSubType.GenericAttachmentMedia || Media.Type == FacebookSubType.ProfilePicAttachmentMedia;
			}
			return flag;
		}
	}

	public bool HasOpenGraphAttachment
	{
		get
		{
			if (Target != null)
			{
				if (Target.Type != FacebookSubType.OpenGraphObject)
				{
					return Target.Type == FacebookSubType.OpenGraphAction;
				}
				return true;
			}
			return false;
		}
	}

	public bool MatchesAttachment(Attachment attachment)
	{
		bool flag = true;
		if (Title != null && attachment.Title != null)
		{
			flag &= Title.Equals(attachment.Title);
		}
		else if (Title != null || attachment.Title != null)
		{
			flag = false;
		}
		if (Description != null && attachment.Description != null)
		{
			flag &= Description.Text.Equals(attachment.Description.Text);
		}
		else if (Description != null || attachment.Description != null)
		{
			flag = false;
		}
		if (Source != null && attachment.Source != null)
		{
			flag &= Source.Text.Equals(attachment.Source.Text);
		}
		else if (Source != null || attachment.Source != null)
		{
			flag = false;
		}
		return flag;
	}
}

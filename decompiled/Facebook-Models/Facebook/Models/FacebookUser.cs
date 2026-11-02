using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class FacebookUser : FacebookProfile, IGroupHeader, IEventType
{
	private bool _isSelected;

	private int? _clientType = null;

	private bool _isOnline;

	public bool IsGroupHeader { get; set; }

	public string GroupName { get; set; }

	public EventType EventType { get; set; }

	public bool IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			Set(ref _isSelected, value, "IsSelected");
		}
	}

	public int? ClientType
	{
		get
		{
			return _clientType;
		}
		set
		{
			if (value != _clientType)
			{
				_clientType = value;
				OnPropertyChanged("ClientType");
			}
		}
	}

	[IgnoreDataMember]
	[JsonProperty("uid")]
	internal string UId
	{
		get
		{
			return Id;
		}
		set
		{
			Id = value;
		}
	}

	[DataMember]
	[JsonProperty(PropertyName = "first_name")]
	public string FirstName { get; set; }

	[JsonProperty(PropertyName = "middle_name")]
	[DataMember]
	public string MiddleName { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "last_name")]
	public string LastName { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "is_pushable")]
	public bool Pushable { get; set; }

	[JsonProperty(PropertyName = "is_messenger_user")]
	[DataMember]
	public bool IsMessengerUser { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "is_partial")]
	public bool IsPartial { get; set; }

	public double CommunicationRank { get; set; }

	[DataMember]
	[JsonProperty("last_active", NullValueHandling = NullValueHandling.Ignore)]
	public LastActive SerializedLastActive
	{
		set
		{
			LastActive = value;
		}
	}

	public LastActive LastActive { get; set; }

	[JsonProperty(PropertyName = "email")]
	[DataMember]
	public string Email { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "contact_email")]
	public string ContactEmail { get; set; }

	[JsonProperty(PropertyName = "phones")]
	[DataMember]
	public List<PhoneInfo> Phones { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "username")]
	public string Username { get; set; }

	[JsonProperty(PropertyName = "affiliations")]
	[DataMember]
	public List<AffiliationsInfo> Affiliations { get; set; }

	public bool IsFriendOfFriend { get; set; }

	public bool IsOnline
	{
		get
		{
			return _isOnline;
		}
		set
		{
			Set(ref _isOnline, value, "IsOnline");
		}
	}

	[DataMember]
	[JsonProperty(PropertyName = "is_phone_only_contact")]
	public bool IsPhoneOnlyContact { get; set; }

	[JsonProperty(PropertyName = "graph_api_write_id")]
	[DataMember]
	public string GraphApiWriteId { get; set; }

	public FacebookUser()
	{
		Type = "user";
		IsPhoneOnlyContact = false;
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext context)
	{
		if (Affiliations != null)
		{
			IsEmployee = Affiliations.Any((AffiliationsInfo a) => a.Id == AffiliationsInfo.FacebookAffiliationId);
		}
	}
}

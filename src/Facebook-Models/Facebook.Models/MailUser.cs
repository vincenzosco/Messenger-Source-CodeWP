using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class MailUser : FacebookUser, IEquatable<MailUser>
	{
		[DataMember]
		[JsonProperty("user_id")]
		internal string UserId
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

		public MailUser()
		{
		}

		public MailUser(FacebookUser user)
		{
			Affiliations = user.Affiliations;
			ClientType = user.ClientType;
			CommunicationRank = user.CommunicationRank;
			ContactEmail = user.ContactEmail;
			Email = user.Email;
			EventType = user.EventType;
			FirstName = user.FirstName;
			GraphApiWriteId = user.GraphApiWriteId;
			Id = user.Id;
			IsGroupHeader = user.IsGroupHeader;
			IsMessengerUser = user.IsMessengerUser;
			IsOnline = user.IsOnline;
			IsPartial = user.IsPartial;
			IsPhoneOnlyContact = user.IsPhoneOnlyContact;
			IsSelected = user.IsSelected;
			LastName = user.LastName;
			MiddleName = user.MiddleName;
			Name = user.Name;
			Phones = user.Phones;
			Pushable = user.Pushable;
			Type = user.Type;
			UId = user.UId;
			UserName = user.UserName;
		}

		public bool Equals(MailUser other)
		{
			return Equals((FacebookProfile)other);
		}
	}
}

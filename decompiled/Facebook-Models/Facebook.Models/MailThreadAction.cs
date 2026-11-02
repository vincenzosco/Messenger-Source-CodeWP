using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class MailThreadAction : MailMessage
	{
		[DataMember]
		[JsonProperty("users")]
		internal IList<MailUser> Users
		{
			set
			{
				if (value != null)
				{
					TargetUsersIds = value.Select((MailUser user) => user.Id).ToList();
				}
			}
		}

		[JsonProperty("internal_message_id")]
		[DataMember]
		internal string InternalMessageId
		{
			get
			{
				return Id;
			}
			set
			{
				Id = "a_" + value;
			}
		}

		[JsonProperty("actor")]
		[DataMember]
		internal MailUser Actor
		{
			get
			{
				return Sender;
			}
			set
			{
				Sender = value;
			}
		}

		[DataMember]
		[JsonProperty("type")]
		internal int ActionType
		{
			get
			{
				return (int)MessageType;
			}
			set
			{
				switch (value)
				{
				case 1:
					MessageType = Type.ParticipantAdded;
					break;
				case 2:
					MessageType = Type.ParticipantLeft;
					break;
				default:
					MessageType = Type.Unknown;
					break;
				}
			}
		}
	}
}

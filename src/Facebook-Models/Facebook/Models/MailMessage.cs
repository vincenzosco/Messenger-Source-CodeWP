using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class MailMessage
	{
		public enum Type
		{
			Regular,
			ParticipantAdded,
			ParticipantLeft,
			VideoCall,
			VideoCallMissed,
			VoiceCall,
			VoiceCallMissed,
			ThreadImageSet,
			ThreadImageRemoved,
			ThreadNameSet,
			ThreadNameRemoved,
			Unknown
		}

		public enum MessageState
		{
			Sent,
			Queued,
			Failed,
			Incomplete,
			Delivered,
			Seen
		}

		public enum MessageSource
		{
			Unknown,
			Mqtt,
			Sync,
			Fetch,
			MicrosoftPush,
			Local
		}

		public const string SyncChangeTypeDeleted = "deleted";

		private List<string> _mostRecentMessageReadByUserIds;

		private HashSet<string> _readByUserIds;

		[JsonProperty("message_id")]
		[DataMember]
		public string Id { get; set; }

		[JsonProperty("sender")]
		[DataMember]
		public MailUser Sender { get; set; }

		[JsonProperty("body")]
		[DataMember]
		public string Body { get; set; }

		[JsonProperty("message_state")]
		[DataMember]
		public MessageState State { get; set; }

		[DataMember]
		[JsonProperty("most_recent_message_read_by_user_ids")]
		public List<string> MostRecentMessageReadByUserIds
		{
			get
			{
				return _mostRecentMessageReadByUserIds ?? (_mostRecentMessageReadByUserIds = new List<string>());
			}
			set
			{
				_mostRecentMessageReadByUserIds = value;
			}
		}

		[DataMember]
		[JsonProperty("read_by_users_ids")]
		public HashSet<string> ReadByUserIds
		{
			get
			{
				return _readByUserIds ?? (_readByUserIds = new HashSet<string>());
			}
			set
			{
				_readByUserIds = value;
			}
		}

		[JsonProperty("timestamp")]
		[DataMember]
		public long ServerTimestamp { get; set; }

		[JsonProperty("send_timestamp")]
		[DataMember]
		public long SendTimestamp { get; set; }

		[JsonProperty("action_id")]
		[DataMember]
		public long ActionId { get; set; }

		[DataMember]
		[JsonProperty("tags")]
		public List<string> Tags { get; set; }

		[JsonConverter(typeof(ListJsonConverter<MailAttachment>))]
		[DataMember]
		[JsonProperty("attachment_map")]
		public List<MailAttachment> Attachments { get; set; }

		[JsonProperty("share_map")]
		[JsonConverter(typeof(ListJsonConverter<MailShare>))]
		[DataMember]
		public List<MailShare> Shares { get; set; }

		[DataMember]
		[JsonProperty("shares")]
		public List<string> ShareIds { get; set; }

		[JsonProperty("coordinates")]
		[DataMember]
		public MailCoordinates Coordinates { get; set; }

		[DataMember]
		[JsonProperty("offline_threading_id")]
		public string OfflineThreadingId { get; set; }

		[DataMember]
		[JsonProperty("recipients")]
		public List<MailUser> Recipients { get; set; }

		[DataMember]
		[JsonProperty("thread_id")]
		public string ThreadId { get; set; }

		[JsonProperty("log_message")]
		[JsonConverter(typeof(LogConverter))]
		[DataMember]
		public Type MessageType { get; set; }

		[JsonProperty("sync_change_type")]
		[DataMember]
		public string SyncChangeType { get; set; }

		public IList<string> TargetUsersIds { get; set; }

		public long Timestamp => MailMessageUtil.GetTimestamp(SendTimestamp, ServerTimestamp);

		public bool Deleted => string.Equals("deleted", SyncChangeType, StringComparison.Ordinal);

		[DataMember]
		[JsonProperty("message_source")]
		public MessageSource Source { get; set; }

		public bool SourceIsPush
		{
			get
			{
				if (Source != MessageSource.MicrosoftPush && Source != MessageSource.Mqtt)
				{
					return Source == MessageSource.Sync;
				}
				return true;
			}
		}

		public bool HasSendTimestamp()
		{
			return SendTimestamp != 0;
		}

		public bool HasShares()
		{
			if (Shares != null)
			{
				return Shares.Count > 0;
			}
			return false;
		}

		public bool HasSticker()
		{
			if (HasShares())
			{
				return !string.IsNullOrWhiteSpace(Shares[0].StickerId);
			}
			return false;
		}

		public string StickerId()
		{
			if (!HasShares())
			{
				return null;
			}
			return Shares[0].StickerId;
		}

		public bool HasStaleData()
		{
			return State == MessageState.Incomplete;
		}

		public bool IsMessageSent()
		{
			if (State != MessageState.Sent && State != MessageState.Delivered)
			{
				return State == MessageState.Seen;
			}
			return true;
		}

		public static MailMessage CreateCopy(MailMessage message)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.CopyFrom(message);
			return mailMessage;
		}

		public void CopyFrom(MailMessage m)
		{
			Id = m.Id;
			Sender = m.Sender;
			Body = m.Body;
			State = m.State;
			MostRecentMessageReadByUserIds = m.MostRecentMessageReadByUserIds;
			ReadByUserIds = m.ReadByUserIds;
			SendTimestamp = ((m.SendTimestamp > 0) ? m.SendTimestamp : SendTimestamp);
			ServerTimestamp = m.ServerTimestamp;
			ActionId = m.ActionId;
			Tags = m.Tags;
			Attachments = m.Attachments;
			Shares = m.Shares;
			ShareIds = m.ShareIds;
			Coordinates = m.Coordinates;
			OfflineThreadingId = m.OfflineThreadingId;
			Recipients = m.Recipients;
			ThreadId = m.ThreadId;
			MessageType = m.MessageType;
			SyncChangeType = m.SyncChangeType;
			TargetUsersIds = m.TargetUsersIds;
			Source = m.Source;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class MailThread
{
	public enum Tag
	{
		Read,
		Archived,
		Other,
		Spam,
		Inbox,
		Unknown
	}

	public const string SyncChangeTypeDeleted = "deleted";

	[DataMember]
	[JsonProperty("message_state")]
	public MailMessage.MessageState LastMessageState;

	[JsonProperty("last_message_read_user_ids")]
	[DataMember]
	public List<string> LastMessageReadUserIds;

	[JsonProperty("participants")]
	[DataMember]
	public List<MailUser> Participants;

	[DataMember]
	[JsonProperty("former_participants")]
	public List<MailUser> FormerParticipants;

	[JsonProperty("thread_id")]
	[DataMember]
	public string Id { get; set; }

	[JsonProperty("thread_fbid")]
	[JsonConverter(typeof(ThreadFbidConverter))]
	[DataMember]
	public ThreadFbid Fbid { get; set; }

	[JsonProperty("sync_sequence_id")]
	[DataMember]
	public long SyncSequenceId { get; set; }

	[JsonProperty("device_generated_id")]
	[DataMember]
	public string DeviceGeneratedId { get; set; }

	[JsonProperty("name")]
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	[JsonProperty("folder")]
	public string Folder { get; set; }

	[JsonProperty("pic_hash")]
	[DataMember]
	public string PictureHash { get; set; }

	[JsonProperty("pic_url")]
	[DataMember]
	public string PictureUrl { get; set; }

	[JsonProperty("mute")]
	[DataMember]
	[JsonConverter(typeof(MuteJsonConvertor))]
	public Dictionary<string, long> MuteMap { get; set; }

	[JsonProperty("mute_until")]
	[DataMember]
	public long MuteUntil { get; set; }

	[DataMember]
	[JsonProperty("snippet")]
	public string Snippet { get; set; }

	[DataMember]
	[JsonProperty("snippet_sender")]
	public MailUser SnippetSender { get; set; }

	[DataMember]
	[JsonProperty("snippet_from_me")]
	public bool SnippetFromMe { get; set; }

	[DataMember]
	[JsonProperty("snippet_message_id")]
	public string SnippetMessageId { get; set; }

	[JsonProperty("admin_snippet")]
	[DataMember]
	public string AdminSnippet { get; set; }

	[DataMember]
	[JsonProperty("timestamp")]
	public long Timestamp { get; set; }

	[JsonProperty("action_id")]
	[DataMember]
	public long ActionId { get; set; }

	[JsonProperty("last_visible_add_action_id")]
	[DataMember]
	public long LastVisibleAddActionId { get; set; }

	[JsonProperty("refetch_action_id")]
	[DataMember]
	public long RefetchActionId { get; set; }

	[JsonProperty("unread")]
	[DataMember]
	public bool Unread { get; set; }

	[DataMember]
	[JsonProperty("single_recipient")]
	public string SingleRecipient { get; set; }

	[JsonProperty("can_reply")]
	[DataMember]
	public bool CanReply { get; set; }

	[JsonProperty("is_subscribed")]
	[DataMember]
	public bool IsSubscribed { get; set; }

	[JsonProperty("archived")]
	[DataMember]
	public bool Archived { get; set; }

	[DataMember]
	[JsonProperty("sync_change_type")]
	public string SyncChangeType { get; set; }

	[DataMember]
	[JsonProperty("read_receipts")]
	public List<ReadReceipt> ReadReceipts { get; set; }

	[JsonProperty("delivery_receipts")]
	[DataMember]
	[JsonConverter(typeof(DeliveryReceiptConverter))]
	public List<DeliveryReceipt> DeliveryReceipts { get; set; }

	public bool Deleted => string.Equals("deleted", SyncChangeType, StringComparison.Ordinal);

	public MailThread()
	{
		IsSubscribed = true;
		Participants = new List<MailUser>();
		FormerParticipants = new List<MailUser>();
		ReadReceipts = new List<ReadReceipt>();
		DeliveryReceipts = new List<DeliveryReceipt>();
		LastMessageReadUserIds = new List<string>();
	}

	public MailThread(MailThread fromThread)
	{
		Name = fromThread.Name;
		AdminSnippet = fromThread.AdminSnippet;
		MuteUntil = fromThread.MuteUntil;
		PictureHash = fromThread.PictureHash;
		Snippet = fromThread.Snippet;
		SnippetSender = fromThread.SnippetSender;
		Timestamp = fromThread.Timestamp;
		Unread = fromThread.Unread;
		Id = fromThread.Id;
		Fbid = fromThread.Fbid;
		SyncSequenceId = fromThread.SyncSequenceId;
		DeviceGeneratedId = fromThread.DeviceGeneratedId;
		ActionId = fromThread.ActionId;
		Archived = fromThread.Archived;
		CanReply = fromThread.CanReply;
		Folder = fromThread.Folder;
		LastMessageState = fromThread.LastMessageState;
		LastMessageReadUserIds = fromThread.LastMessageReadUserIds;
		FormerParticipants = fromThread.FormerParticipants;
		IsSubscribed = fromThread.IsSubscribed;
		LastVisibleAddActionId = fromThread.LastVisibleAddActionId;
		MuteMap = fromThread.MuteMap;
		Participants = fromThread.Participants;
		ReadReceipts = fromThread.ReadReceipts;
		DeliveryReceipts = fromThread.DeliveryReceipts;
		RefetchActionId = fromThread.RefetchActionId;
		SingleRecipient = fromThread.SingleRecipient;
		SnippetMessageId = fromThread.SnippetMessageId;
		SnippetFromMe = fromThread.SnippetFromMe;
		SyncChangeType = fromThread.SyncChangeType;
	}

	public bool IsThreadOffline()
	{
		if (!string.IsNullOrWhiteSpace(Id) && !string.IsNullOrWhiteSpace(DeviceGeneratedId))
		{
			return Id == DeviceGeneratedId;
		}
		return false;
	}

	public bool IsCanonical()
	{
		return !string.IsNullOrWhiteSpace(SingleRecipient);
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext context)
	{
		if (RefetchActionId == 0)
		{
			RefetchActionId = ActionId;
		}
		if (LastVisibleAddActionId == 0)
		{
			LastVisibleAddActionId = ActionId;
		}
	}
}

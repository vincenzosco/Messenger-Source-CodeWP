using System.Collections.Generic;
using System.Linq;
using Facebook.Models;
using Facebook.Services.Localization;

namespace Facebook.Services.Api.Cache;

public class ThreadCacheHelper
{
	public static void MarkThreadAsRead(MailThread thread)
	{
		thread.Unread = false;
	}

	public static void MarkThreadAsUnread(MailThread thread)
	{
		thread.Unread = true;
	}

	public static void MarkThreadMuteState(MailThread thread, long muteUntil)
	{
		thread.MuteUntil = muteUntil;
	}

	public static void SetThreadName(MailThread thread, string name)
	{
		thread.Name = name;
	}

	public static void SetThreadPictureHash(MailThread thread, string hash)
	{
		thread.PictureHash = hash;
	}

	public static void ClearPicture(MailThread thread)
	{
		thread.PictureHash = null;
	}

	public static void AddParticipants(MailThread thread, IEnumerable<FacebookUser> newParticipants)
	{
		foreach (FacebookUser newParticipant in newParticipants)
		{
			string newParticipantId = newParticipant.Id;
			thread.FormerParticipants.RemoveAll((MailUser p) => p.Id == newParticipantId);
			if (!thread.Participants.Any((MailUser p) => p.Id == newParticipantId))
			{
				thread.Participants.Add(new MailUser(newParticipant));
			}
		}
	}

	public static void RemoveParticipants(MailThread thread, IEnumerable<string> removeParticipantIds)
	{
		foreach (string removeParticipantId in removeParticipantIds)
		{
			string safeRemoveParticipantId = removeParticipantId;
			MailUser mailUser = thread.Participants.SingleOrDefaultLogErrorIfNotSingle((MailUser p) => p.Id == safeRemoveParticipantId, "RemoveParticipants");
			if (mailUser != null)
			{
				thread.Participants.Remove(mailUser);
				thread.FormerParticipants.Add(mailUser);
				ReadReceipt readReceipt = thread.ReadReceipts.FirstOrDefault((ReadReceipt rr) => rr.Id == removeParticipantId);
				if (readReceipt != null)
				{
					thread.ReadReceipts.Remove(readReceipt);
					thread.LastMessageReadUserIds.Remove(removeParticipantId);
				}
			}
		}
	}

	public static void SetDeliveryReceipt(MailThread thread, DeliveryReceipt deliveryReceipt)
	{
		DeliveryReceipt deliveryReceipt2 = thread.DeliveryReceipts.FirstOrDefault((DeliveryReceipt d) => d.Id == deliveryReceipt.Id);
		if (deliveryReceipt2 != null)
		{
			if (deliveryReceipt2.Timestamp < deliveryReceipt.Timestamp)
			{
				deliveryReceipt2.Timestamp = deliveryReceipt.Timestamp;
			}
		}
		else
		{
			thread.DeliveryReceipts.Add(deliveryReceipt);
		}
	}

	public static void UpdateThreadDetailsWithMessage(MailThread thread, MailMessage message, ThreadSnippetInfo snippetInfo)
	{
		if (message.Timestamp >= thread.Timestamp)
		{
			thread.ActionId = message.ActionId;
			thread.Timestamp = message.Timestamp;
			thread.SnippetMessageId = message.Id;
			thread.SnippetSender = message.Sender;
			thread.SnippetFromMe = snippetInfo.FromLoggedInUser;
			thread.Snippet = snippetInfo.Snippet;
			thread.AdminSnippet = snippetInfo.AdminSnippet;
			if (!snippetInfo.FromLoggedInUser)
			{
				thread.Unread = true;
			}
		}
	}

	public static List<MailMessage> RemoveParticipantsReadReceipt(CachedThreadInfo cachedThreadInfo, IEnumerable<string> removeParticipantIds)
	{
		List<MailMessage> list = new List<MailMessage>();
		foreach (string removeParticipantId in removeParticipantIds)
		{
			foreach (MailMessage message in cachedThreadInfo.Messages)
			{
				bool flag = message.ReadByUserIds.Remove(removeParticipantId);
				bool flag2 = message.MostRecentMessageReadByUserIds.Remove(removeParticipantId);
				if (!message.ReadByUserIds.Any())
				{
					message.State = MailMessage.MessageState.Delivered;
				}
				if (flag || flag2)
				{
					list.Add(message);
				}
			}
		}
		return list;
	}

	public static MailMessage HandleDeletionReadReceipt(CachedThreadInfo threadInfo, long timestamp, List<string> mostRecentMessageReadByUserIdsDeleted)
	{
		List<MailMessage> source = (from m in threadInfo.Messages
			where m.MessageType == MailMessage.Type.Regular
			orderby m.Timestamp descending
			select m).ToList();
		MailMessage mailMessage = source.FirstOrDefault((MailMessage m) => m.Timestamp <= timestamp);
		mailMessage?.MostRecentMessageReadByUserIds.AddRange(mostRecentMessageReadByUserIdsDeleted);
		return mailMessage;
	}

	public static List<MailMessage> SetReadReceipt(CachedThreadInfo cachedThreadInfo, ReadReceipt readReceipt)
	{
		MailThread thread = cachedThreadInfo.Thread;
		ReadReceipt cachedReadReceipt = thread.ReadReceipts.FirstOrDefault((ReadReceipt rr) => rr.Id == readReceipt.Id);
		List<MailMessage> result = UpdateMessageStateAndReadIds(cachedThreadInfo, readReceipt, cachedReadReceipt);
		UpdateThreadReadReceipt(readReceipt, cachedReadReceipt, thread);
		return result;
	}

	private static List<MailMessage> UpdateMessageStateAndReadIds(CachedThreadInfo cachedThreadInfo, ReadReceipt readReceipt, ReadReceipt cachedReadReceipt)
	{
		List<MailMessage> list = new List<MailMessage>();
		List<MailMessage> source = (from m in cachedThreadInfo.Messages
			where m.MessageType == MailMessage.Type.Regular
			orderby m.Timestamp descending
			select m).ToList();
		MailMessage mailMessage = source.FirstOrDefault((MailMessage m) => m.Timestamp <= readReceipt.Timestamp);
		if (cachedReadReceipt != null)
		{
			long previousTimestamp = cachedReadReceipt.Timestamp;
			MailMessage mailMessage2 = source.FirstOrDefault((MailMessage m) => m.Timestamp <= previousTimestamp);
			if (mailMessage2 != null)
			{
				mailMessage2.MostRecentMessageReadByUserIds.Remove(readReceipt.Id);
				list.Add(mailMessage2);
			}
		}
		if (mailMessage != null)
		{
			mailMessage.MostRecentMessageReadByUserIds.Add(readReceipt.Id);
			list.Add(mailMessage);
		}
		IEnumerable<MailMessage> enumerable = cachedThreadInfo.Messages.Where((MailMessage message) => message.IsMessageSent() && message.Timestamp <= readReceipt.Timestamp && (message.State != MailMessage.MessageState.Seen || !message.ReadByUserIds.Contains(readReceipt.Id)));
		foreach (MailMessage item in enumerable)
		{
			item.State = MailMessage.MessageState.Seen;
			item.ReadByUserIds.Add(readReceipt.Id);
			list.Add(item);
		}
		return list;
	}

	private static void UpdateThreadReadReceipt(ReadReceipt readReceipt, ReadReceipt cachedReadReceipt, MailThread thread)
	{
		if (cachedReadReceipt != null)
		{
			if (cachedReadReceipt.Timestamp < readReceipt.Timestamp)
			{
				cachedReadReceipt.Timestamp = readReceipt.Timestamp;
			}
		}
		else
		{
			thread.ReadReceipts.Add(readReceipt);
		}
		if (readReceipt.Timestamp > thread.Timestamp)
		{
			thread.LastMessageState = MailMessage.MessageState.Seen;
			if (!thread.LastMessageReadUserIds.Contains(readReceipt.Id))
			{
				thread.LastMessageReadUserIds = new List<string>(thread.LastMessageReadUserIds) { readReceipt.Id };
			}
		}
	}
}

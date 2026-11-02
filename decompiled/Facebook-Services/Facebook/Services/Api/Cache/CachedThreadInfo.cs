using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Facebook.Base.Logging;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Cache;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class CachedThreadInfo
{
	[JsonProperty("thread")]
	[DataMember]
	public MailThread Thread { get; private set; }

	[DataMember]
	[JsonProperty("messages")]
	public List<MailMessage> Messages { get; private set; }

	public CachedThreadInfo()
		: this(new MailThread())
	{
	}

	public CachedThreadInfo(MailThread thread)
	{
		UpdateThread(thread);
		Messages = new List<MailMessage>();
	}

	public void UpdateThread(MailThread thread)
	{
		if (Thread != null && Thread.ActionId > thread.ActionId)
		{
			Logger.Error(this, "Storing thread with older action ID", "UpdateThread", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\CachedThreadInfo.cs", 38);
		}
		Thread = thread;
	}

	public void ClearMessages()
	{
		Messages = new List<MailMessage>();
	}

	public void TrimToMostRecentAndUnsentMessages(int maxLocalMessagesPerThread, int maxRecentMessageCount)
	{
		IEnumerable<MailMessage> first = (from m in Messages
			where !m.IsMessageSent()
			orderby m.Timestamp descending
			select m).Take(maxLocalMessagesPerThread);
		IEnumerable<MailMessage> second = (from m in Messages
			where m.IsMessageSent()
			orderby m.ActionId descending
			select m).Take(maxRecentMessageCount);
		Messages = first.Concat(second).ToList();
	}

	public List<MailUser> GetParticipants()
	{
		return Thread.Participants;
	}

	public void AddMessage(MailMessage message)
	{
		if (message.Deleted)
		{
			RemoveMessage(message.Id);
			return;
		}
		if (!string.IsNullOrWhiteSpace(message.OfflineThreadingId) && !message.HasSendTimestamp())
		{
			MailMessage mailMessage = Messages.FirstOrDefault((MailMessage m) => m.Id == message.OfflineThreadingId);
			if (mailMessage != null)
			{
				message.SendTimestamp = mailMessage.SendTimestamp;
			}
			Messages.RemoveAll((MailMessage m) => m.OfflineThreadingId == message.OfflineThreadingId);
		}
		MailMessage mailMessage2 = Messages.SingleOrDefaultLogErrorIfNotSingle((MailMessage m) => m.Id == message.Id, "AddMessage");
		if (mailMessage2 != null)
		{
			if (mailMessage2.ActionId > message.ActionId)
			{
				Logger.Error(this, "Storing message with older action ID", "AddMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\CachedThreadInfo.cs", 93);
			}
			if (mailMessage2.State != MailMessage.MessageState.Incomplete && message.State == MailMessage.MessageState.Incomplete)
			{
				return;
			}
			Messages.Remove(mailMessage2);
		}
		MailUser mailUser = Thread.Participants.SingleOrDefault((MailUser p) => p.Id == message.Sender.Id) ?? Thread.FormerParticipants.SingleOrDefault((MailUser p) => p.Id == message.Sender.Id);
		if (mailUser != null)
		{
			message.Sender = mailUser;
		}
		Messages.Add(message);
	}

	public MailMessage MarkMessageStateByOfflineId(string offlineThreadingId, MailMessage.MessageState state)
	{
		MailMessage mailMessage = Messages.SingleOrDefaultLogErrorIfNotSingle((MailMessage m) => m.OfflineThreadingId == offlineThreadingId, "MarkMessageStateByOfflineId");
		if (mailMessage != null)
		{
			if (mailMessage.State == state)
			{
				return mailMessage;
			}
			if (!mailMessage.IsMessageSent())
			{
				mailMessage.State = state;
			}
			else
			{
				Logger.Warning(this, string.Format("Attempted to mark a message as {0} after it has already been sent: {1}", new object[2] { state, offlineThreadingId }), "MarkMessageStateByOfflineId", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\CachedThreadInfo.cs", 130);
			}
			return mailMessage;
		}
		return null;
	}

	public List<MailMessage> MarkAllQueuedMessagesFailed()
	{
		List<MailMessage> list = Messages.Where((MailMessage m) => m.State == MailMessage.MessageState.Queued).ToList();
		Enumerable.Count(list);
		foreach (MailMessage item in list)
		{
			item.State = MailMessage.MessageState.Failed;
		}
		return list;
	}

	public List<MailMessage> MarkQueuedMessagesSent(List<string> requestIds)
	{
		IEnumerable<MailMessage> source = Messages.Where((MailMessage m) => !m.IsMessageSent());
		List<MailMessage> list = source.Where((MailMessage m) => requestIds.Any((string requestId) => requestId == m.OfflineThreadingId)).ToList();
		foreach (MailMessage item in list)
		{
			item.State = MailMessage.MessageState.Sent;
		}
		return list;
	}

	public MailMessage PatchSentMessage(MailMessage message)
	{
		MailMessage mailMessage = Messages.FirstOrDefault((MailMessage m) => m.OfflineThreadingId == message.OfflineThreadingId);
		if (mailMessage == null)
		{
			Logger.Error(this, "Unable to find message to patch", "PatchSentMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\CachedThreadInfo.cs", 166);
			return null;
		}
		MarkMessageStateByOfflineId(message.OfflineThreadingId, MailMessage.MessageState.Sent);
		mailMessage.Id = message.Id;
		mailMessage.ActionId = message.ActionId;
		mailMessage.ServerTimestamp = message.ServerTimestamp;
		mailMessage.Attachments = message.Attachments;
		return mailMessage;
	}

	public void MarkMessagesAsDelivered(List<MailMessage> messages)
	{
		foreach (MailMessage message in messages)
		{
			message.State = MailMessage.MessageState.Delivered;
		}
	}

	public MailMessage RemoveMessage(string messageId)
	{
		MailMessage mailMessage = Messages.SingleOrDefaultLogErrorIfNotSingle((MailMessage m) => m.Id == messageId, "RemoveMessage");
		if (mailMessage != null)
		{
			Messages.Remove(mailMessage);
		}
		return mailMessage;
	}

	public bool HasFailedMessage()
	{
		return Messages.Any((MailMessage m) => m.State == MailMessage.MessageState.Failed);
	}
}

using System;
using System.Collections.Generic;
using Facebook.Models;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Services.Api.Methods.SaveOutgoingMessage;

public class OfflineThreadUtil
{
	public static MailThread CreateThread(Request request, string singleRecipientId, string loggedInUserId, string loggedInUserName)
	{
		string text = CreateFakeThreadId();
		ThreadFbid fbid = new ThreadFbid(singleRecipientId);
		MailUser mailUser = CreateMailUser(loggedInUserId, loggedInUserName);
		List<MailUser> list = new List<MailUser>();
		list.Add(mailUser);
		List<MailUser> participants = CreateUserList(request.Users, mailUser);
		MailThread mailThread = new MailThread();
		mailThread.Id = text;
		mailThread.Fbid = fbid;
		mailThread.DeviceGeneratedId = text;
		mailThread.Snippet = request.Message;
		mailThread.CanReply = true;
		mailThread.Folder = "inbox";
		mailThread.IsSubscribed = true;
		mailThread.Participants = participants;
		mailThread.SnippetFromMe = true;
		mailThread.MuteMap = null;
		mailThread.Timestamp = request.SentTimestamp;
		mailThread.SingleRecipient = singleRecipientId;
		mailThread.ActionId = 0L;
		mailThread.AdminSnippet = null;
		mailThread.Archived = false;
		mailThread.LastVisibleAddActionId = 0L;
		mailThread.MuteUntil = 0L;
		mailThread.Name = null;
		mailThread.PictureHash = null;
		mailThread.ReadReceipts = null;
		mailThread.RefetchActionId = 0L;
		mailThread.SnippetMessageId = null;
		mailThread.SnippetSender = mailUser;
		mailThread.SyncChangeType = null;
		mailThread.Unread = false;
		return mailThread;
	}

	private static List<MailUser> CreateUserList(IEnumerable<MailUser> users, params MailUser[] additionalUsers)
	{
		List<MailUser> list = new List<MailUser>(users);
		list.AddRange(additionalUsers);
		return list;
	}

	private static string CreateFakeThreadId()
	{
		return Guid.NewGuid().ToString();
	}

	private static MailUser CreateMailUser(string userId, string userName)
	{
		MailUser mailUser = new MailUser();
		mailUser.Id = userId;
		mailUser.Name = userName;
		return mailUser;
	}
}

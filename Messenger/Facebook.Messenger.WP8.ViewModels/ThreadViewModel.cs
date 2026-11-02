using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Base.Logging;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Localization;
using GalaSoft.MvvmLight;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ThreadViewModel : ViewModelBase
	{
		public const string ActionIdProperty = "ActionId";

		public const string NameProperty = "Name";

		public const string SnippetProperty = "Snippet";

		public const string LastMessageStatePropertyName = "LastMessageState";

		public const string LastMessageSeenHeadsPropertyName = "LastMessageReadUserIds";

		public const string UnreadProperty = "Unread";

		public const string MuteUntilProperty = "MuteUntil";

		public const string IsMutedProperty = "IsMuted";

		public const string IsSubscribedProperty = "IsSubscribed";

		public const string PictureUrlProperty = "PictureUrl";

		public const string TimestampProperty = "Timestamp";

		public const string ParticipantsProperty = "Participants";

		public const string FormerParticipantsProperty = "FormerParticipants";

		private const long ClearReadMessageTimestamp = -1L;

		private long _actionId;

		private string _name;

		private string _displayName;

		private ThreadSnippet _snippet;

		private MailMessage.MessageState _lastMessageState;

		private List<string> _lastMessageReadUserIds;

		private long _muteUntil;

		private bool _isSubscribed;

		private string _pictureUrl;

		private long _timestamp;

		private List<MailUser> _participants;

		private List<MailUser> _formerParticipants;

		private MailUser _canonicalParticipant;

		private long _readMessageTimestamp;

		private bool _hasFailedUnsentMessages;

		private bool _isMessengerUser;

		private bool _isPartialContact;

		private bool _isThreadOffline;

		private string _cachedEllipsizedThreadName;

		private bool _isSelected;

		private IEnumerable<MailUser> _cachedParticipantsByLastSend;

		public string Id { get; private set; }

		public string SingleRecipient { get; private set; }

		public long ActionId
		{
			get
			{
				return _actionId;
			}
			private set
			{
				Set("ActionId", ref _actionId, value);
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			private set
			{
				Set("Name", ref _name, value);
			}
		}

		public string DisplayName
		{
			get
			{
				if (string.IsNullOrEmpty(Name))
				{
					return _displayName;
				}
				return Name;
			}
		}

		public ThreadSnippet Snippet
		{
			get
			{
				return _snippet;
			}
			private set
			{
				Set(ref _snippet, value, false, "Snippet");
			}
		}

		public MailMessage.MessageState LastMessageState
		{
			get
			{
				return _lastMessageState;
			}
			private set
			{
				Set("LastMessageState", ref _lastMessageState, value);
			}
		}

		public List<string> LastMessageReadUserIds
		{
			get
			{
				return _lastMessageReadUserIds;
			}
			private set
			{
				Set("LastMessageReadUserIds", ref _lastMessageReadUserIds, value);
			}
		}

		public bool Unread
		{
			get
			{
				return Snippet.SnippetTimestamp > ReadMessageTimestamp;
			}
		}

		public long MuteUntil
		{
			get
			{
				return _muteUntil;
			}
			private set
			{
				Set("MuteUntil", ref _muteUntil, value);
				RaisePropertyChanged("IsMuted");
			}
		}

		public bool IsMuted
		{
			get
			{
				return MuteHelper.IsThreadMuted(MuteUntil);
			}
		}

		public bool IsSubscribed
		{
			get
			{
				return _isSubscribed;
			}
			set
			{
				Set("IsSubscribed", ref _isSubscribed, value);
			}
		}

		public string PictureUrl
		{
			get
			{
				return _pictureUrl;
			}
			set
			{
				Set("PictureUrl", ref _pictureUrl, value);
			}
		}

		public long Timestamp
		{
			get
			{
				return _timestamp;
			}
			set
			{
				Set("Timestamp", ref _timestamp, value);
			}
		}

		public IReadOnlyList<MailUser> Participants
		{
			get
			{
				return _participants;
			}
			private set
			{
				_participants = value.ToList();
				UpdateCanonicalParticipant();
				ParticipantsOrSendersChanged();
			}
		}

		public IReadOnlyList<MailUser> FormerParticipants
		{
			get
			{
				return _formerParticipants;
			}
			private set
			{
				Set("FormerParticipants", ref _formerParticipants, value.ToList());
			}
		}

		public MailUser CanonicalParticipant
		{
			get
			{
				return _canonicalParticipant;
			}
			private set
			{
				_canonicalParticipant = value;
				RaisePropertyChanged("CanonicalParticipant");
			}
		}

		public bool CanLeaveConversation
		{
			get
			{
				if (IsSubscribed)
				{
					return !IsCanonical();
				}
				return false;
			}
		}

		public long ReadMessageTimestamp
		{
			get
			{
				return _readMessageTimestamp;
			}
			set
			{
				if (value == -1 || value > _readMessageTimestamp)
				{
					Set(ref _readMessageTimestamp, value, false, "ReadMessageTimestamp");
					RaisePropertyChanged("Unread");
				}
			}
		}

		public bool HasFailedUnsentMessages
		{
			get
			{
				return _hasFailedUnsentMessages;
			}
			set
			{
				Set(ref _hasFailedUnsentMessages, value, false, "HasFailedUnsentMessages");
			}
		}

		public bool IsMessengerUser
		{
			get
			{
				return _isMessengerUser;
			}
			private set
			{
				Set(ref _isMessengerUser, value, false, "IsMessengerUser");
			}
		}

		public bool IsPartialContact
		{
			get
			{
				return _isPartialContact;
			}
			private set
			{
				Set(ref _isPartialContact, value, false, "IsPartialContact");
			}
		}

		public bool IsThreadOffline
		{
			get
			{
				return _isThreadOffline;
			}
			private set
			{
				Set(ref _isThreadOffline, value, false, "IsThreadOffline");
			}
		}

		public string CachedEllipsizedThreadName
		{
			get
			{
				return _cachedEllipsizedThreadName;
			}
			set
			{
				Set(ref _cachedEllipsizedThreadName, value, false, "CachedEllipsizedThreadName");
				RaisePropertyChanged("Name");
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				Set(ref _isSelected, value, false, "IsSelected");
			}
		}

		public bool HasName()
		{
			return !string.IsNullOrWhiteSpace(Name);
		}

		public bool IsCanonical()
		{
			return !string.IsNullOrWhiteSpace(SingleRecipient);
		}

		public bool IsCanonicalWithParticipant(MailUser user)
		{
			if (IsCanonical())
			{
				return Participants.Any((MailUser u) => u.Id == user.Id);
			}
			return false;
		}

		public string GetCanonicalParticipantId(MailThread thread, FacebookContext context)
		{
			if (!IsCanonical())
			{
				return string.Empty;
			}
			IEnumerable<MailUser> source = Participants.Where((MailUser t) => !UserUtilities.IsCurrentUserId(t.Id, context));
			MailUser mailUser = source.FirstOrDefault();
			if (mailUser != null)
			{
				return mailUser.Id;
			}
			return string.Empty;
		}

		public bool HasPicture()
		{
			return !string.IsNullOrWhiteSpace(PictureUrl);
		}

		public void MoveParticipantToFront(string participantId)
		{
			MailUser mailUser = Participants.FirstOrDefault((MailUser user) => user.Id == participantId);
			if (mailUser != null)
			{
				_participants.Remove(mailUser);
				_participants.Insert(0, mailUser);
				ParticipantsOrSendersChanged();
			}
		}

		public void MoveSenderToFront(MailUser sender)
		{
			_participants.Remove(sender);
			_participants.Insert(0, sender);
			ParticipantsOrSendersChanged();
		}

		public void UpdateSnippetAndReadState(MailMessage message, bool isSnippetFromMe, ThreadSnippetInfo snippet)
		{
			string body = ((string.IsNullOrEmpty(message.Body) && string.IsNullOrEmpty(snippet.AdminSnippet)) ? snippet.Snippet : message.Body);
			UpdateSnippetAndReadState(message.Sender, body, message.Timestamp, isSnippetFromMe, snippet.AdminSnippet);
		}

		private void UpdateSnippetAndReadState(MailUser sender, string body, long timestamp, bool isSnippetFromMe, string adminSnippet)
		{
			Timestamp = timestamp;
			if (isSnippetFromMe && ReadMessageTimestamp != -1)
			{
				ReadMessageTimestamp = timestamp;
			}
			MailUser mailUser = ((sender == null) ? null : Participants.FirstOrDefault((MailUser p) => p.Id == sender.Id));
			if (mailUser != null)
			{
				MoveSenderToFront(mailUser);
			}
			Snippet = new ThreadSnippet(mailUser, timestamp, body, isSnippetFromMe, IsCanonical(), adminSnippet);
		}

		[Obsolete]
		public void RaiseThreadChanged()
		{
			RaisePropertyChanged("RaiseThreadChanged");
		}

		public IEnumerable<MailUser> CalculateParticipantsByLastSend(string loggedInUserId)
		{
			return _cachedParticipantsByLastSend ?? (_cachedParticipantsByLastSend = GenerateRecentSendersList(Participants, loggedInUserId));
		}

		public ThreadViewModel(MailUser user)
			: this(new MailThread
			{
				SingleRecipient = user.Id,
				Participants = new List<MailUser> { user }
			})
		{
		}

		public ThreadViewModel(string threadId = null, string threadName = null, string recipientId = null)
			: this(new MailThread
			{
				Id = threadId,
				Name = threadName,
				SingleRecipient = recipientId
			})
		{
			_displayName = threadName;
			if (!string.IsNullOrEmpty(recipientId))
			{
				CanonicalParticipant = new MailUser
				{
					Id = recipientId,
					Name = threadName
				};
			}
		}

		public ThreadViewModel(MailThread thread)
		{
			Id = thread.Id;
			SingleRecipient = thread.SingleRecipient;
			UpdateThread(thread);
		}

		public bool IsSameThread(MailThread thread)
		{
			string text = ((IsThreadOffline && !thread.IsThreadOffline()) ? thread.DeviceGeneratedId : thread.Id);
			return Id == text;
		}

		public bool ShouldUpdate(MailThread newerThread)
		{
			bool flag = !string.IsNullOrEmpty(Id);
			bool flag2 = !string.IsNullOrEmpty(newerThread.Id);
			bool flag3 = flag && flag2 && Id == newerThread.Id;
			bool flag4 = IsCanonical() && newerThread.IsCanonical() && SingleRecipient == newerThread.SingleRecipient;
			bool result = !flag && flag2 && flag4;
			if (!flag3)
			{
				return result;
			}
			return true;
		}

		public void UpdateThread(MailThread thread)
		{
			if (!IsSameThread(thread))
			{
				Logger.Warning(this, string.Format("Thread is not the same. Current IsOffline:{0}, New IsOffline:{1}, Current ID:{2}, New ID:{3}, New DeviceGeneratedId:{4}", IsThreadOffline, thread.IsThreadOffline(), Id, thread.Id, thread.DeviceGeneratedId), "UpdateThread", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\ThreadViewModel.cs", 360);
				if (!ShouldUpdate(thread))
				{
					Logger.Warning(this, string.Format("Thread should not be updated. Current IsCanonical:{0}, New IsCanonical:{1}, Current SingleRecipient:{2}, New SingleRecipient:{3}", IsCanonical(), thread.IsCanonical(), SingleRecipient, thread.SingleRecipient), "UpdateThread", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\ThreadViewModel.cs", 363);
					throw new Exception(string.Format("Fatal Error: Thread Id must not change. Attempting to update thread that should not be updated."));
				}
			}
			Id = thread.Id;
			SingleRecipient = thread.SingleRecipient;
			ActionId = thread.ActionId;
			FormerParticipants = thread.FormerParticipants;
			IsSubscribed = thread.IsSubscribed;
			MuteUntil = thread.MuteUntil;
			Name = thread.Name;
			Participants = thread.Participants;
			PictureUrl = GetPictureUrl(thread);
			IsMessengerUser = GetIsMessengerUser(IsCanonical(), SingleRecipient, Participants);
			IsPartialContact = GetIsPartialContact(IsCanonical(), SingleRecipient, Participants);
			IsThreadOffline = thread.IsThreadOffline();
			ReadMessageTimestamp = (thread.Unread ? (-1) : thread.Timestamp);
			LastMessageState = thread.LastMessageState;
			LastMessageReadUserIds = thread.LastMessageReadUserIds;
			UpdateSnippetAndReadState(thread.SnippetSender, thread.Snippet, thread.Timestamp, thread.SnippetFromMe, thread.AdminSnippet);
			CachedEllipsizedThreadName = null;
		}

		public void UpdateDisplayName(FacebookContext context)
		{
			_displayName = ThreadNameUtils.GetThreadNameOrPlaceholder(this, context);
		}

		private void UpdateCanonicalParticipant()
		{
			CanonicalParticipant = (IsCanonical() ? Participants.FirstOrDefault((MailUser p) => p.Id == SingleRecipient) : null);
		}

		private void ParticipantsOrSendersChanged()
		{
			if (!HasName())
			{
				CachedEllipsizedThreadName = null;
			}
			_cachedParticipantsByLastSend = null;
			base.RaisePropertyChanged("Participants");
		}

		private static string GetPictureUrl(MailThread thread)
		{
			if (!string.IsNullOrEmpty(thread.PictureUrl))
			{
				return thread.PictureUrl;
			}
			if (!string.IsNullOrEmpty(thread.PictureHash))
			{
				return AttachmentUriGenerator.UrlForThreadImage(thread.Id, thread.PictureHash, 100);
			}
			return null;
		}

		private bool GetIsPartialContact(bool isCanonical, string singleRecipientId, IEnumerable<MailUser> participants)
		{
			if (!isCanonical)
			{
				return false;
			}
			MailUser mailUser = participants.SingleOrDefault((MailUser p) => p.Id == singleRecipientId);
			if (mailUser != null)
			{
				return mailUser.IsPartial;
			}
			return false;
		}

		private static bool GetIsMessengerUser(bool isCanonical, string singleRecipientId, IEnumerable<MailUser> participants)
		{
			if (!isCanonical)
			{
				return false;
			}
			MailUser mailUser = participants.SingleOrDefault((MailUser p) => p.Id == singleRecipientId);
			if (mailUser != null)
			{
				return mailUser.IsMessengerUser;
			}
			return false;
		}

		private static IEnumerable<MailUser> GenerateRecentSendersList(IEnumerable<MailUser> participants, string loggedInUserId)
		{
			return participants.Where((MailUser p) => p != null && p.Id != loggedInUserId);
		}
	}
}

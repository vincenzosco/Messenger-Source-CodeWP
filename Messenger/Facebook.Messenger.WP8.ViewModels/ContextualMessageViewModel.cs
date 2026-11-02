using System;
using System.Collections.Generic;
using Facebook.Models;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ContextualMessageViewModel : MessageViewModel
	{
		public enum GroupState
		{
			None,
			FirstOfGroup,
			MiddleOfGroup,
			LastOfGroup
		}

		public const string MessageGroupStatePropertyName = "MessageGroupState";

		private long _actionId;

		private string _threadId;

		private string _offlineThreadingId;

		private string _syncChangeType;

		private IList<string> _targetUserIds;

		private GroupState _messageGroupState;

		private bool _shouldAutoAnimateSticker;

		private bool _isCancellable;

		private bool _isSingleRecipient;

		public long ActionId
		{
			get
			{
				return _actionId;
			}
			private set
			{
				Set(ref _actionId, value, false, "ActionId");
			}
		}

		public string ThreadId
		{
			get
			{
				return _threadId;
			}
			private set
			{
				Set(ref _threadId, value, false, "ThreadId");
			}
		}

		public string OfflineThreadingId
		{
			get
			{
				return _offlineThreadingId;
			}
			private set
			{
				Set(ref _offlineThreadingId, value, false, "OfflineThreadingId");
			}
		}

		public string SyncChangeType
		{
			get
			{
				return _syncChangeType;
			}
			private set
			{
				Set(ref _syncChangeType, value, false, "SyncChangeType");
			}
		}

		public IList<string> TargetUsersIds
		{
			get
			{
				return _targetUserIds;
			}
			private set
			{
				Set(ref _targetUserIds, value, false, "TargetUsersIds");
			}
		}

		public GroupState MessageGroupState
		{
			get
			{
				return _messageGroupState;
			}
			set
			{
				if (_messageGroupState != value)
				{
					_messageGroupState = value;
					RaisePropertyChanged("MessageGroupState");
				}
			}
		}

		public bool ShouldAutoAnimateSticker
		{
			get
			{
				return _shouldAutoAnimateSticker;
			}
			set
			{
				Set(ref _shouldAutoAnimateSticker, value, false, "ShouldAutoAnimateSticker");
			}
		}

		public bool IsCancellable
		{
			get
			{
				return _isCancellable;
			}
			set
			{
				Set(ref _isCancellable, value, false, "IsCancellable");
			}
		}

		public bool Deleted
		{
			get
			{
				return string.Equals("deleted", SyncChangeType, StringComparison.Ordinal);
			}
		}

		public bool IsSingleRecipient
		{
			get
			{
				return _isSingleRecipient;
			}
			set
			{
				Set(ref _isSingleRecipient, value, false, "IsSingleRecipient");
			}
		}

		public ContextualMessageViewModel(MailMessage message, bool isSingleRecipient)
			: base(message)
		{
			ActionId = message.ActionId;
			ThreadId = message.ThreadId;
			OfflineThreadingId = message.OfflineThreadingId;
			SyncChangeType = message.SyncChangeType;
			TargetUsersIds = message.TargetUsersIds;
			IsSingleRecipient = isSingleRecipient;
		}

		public override void UpdateMessage(MailMessage message)
		{
			_actionId = message.ActionId;
			_offlineThreadingId = message.OfflineThreadingId;
			_threadId = message.ThreadId;
			_syncChangeType = message.SyncChangeType;
			base.UpdateMessage(message);
		}

		public override bool MatchesMessageId(string id)
		{
			if (!(id == Id))
			{
				return id == OfflineThreadingId;
			}
			return true;
		}

		public bool IsEarlierInGroup()
		{
			if (MessageGroupState != GroupState.FirstOfGroup)
			{
				return MessageGroupState == GroupState.MiddleOfGroup;
			}
			return true;
		}

		public bool IsLaterInGroup()
		{
			if (MessageGroupState != GroupState.LastOfGroup)
			{
				return MessageGroupState == GroupState.MiddleOfGroup;
			}
			return true;
		}
	}
}

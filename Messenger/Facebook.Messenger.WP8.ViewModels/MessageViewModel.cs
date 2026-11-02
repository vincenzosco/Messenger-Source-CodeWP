using System.Collections.Generic;
using System.Linq;
using Facebook.Base;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Models;
using GalaSoft.MvvmLight;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class MessageViewModel : ViewModelBase
	{
		public const string LikeStickerUrl = "/Assets/Stickers/LikeSticker.png";

		public const string AttachmentsPropertyName = "Attachments";

		public const string SharesPropertyName = "Shares";

		public const string SeenHeadsPropertyName = "MostRecentMessageReadByUserIds";

		public const string ReadByUserIdsPropertyName = "ReadByUserIds";

		public const string HasCoordinatesProperty = "HasCoordinates";

		public const string MessageStatePropertyName = "State";

		private string _id;

		private MailUser _sender;

		private long _serverTimestamp;

		private long _sendTimestamp;

		private List<string> _tags;

		private string _body;

		private List<MailAttachment> _attachments;

		private List<MailShare> _shares;

		private List<string> _shareIds;

		private MailMessage.Type _messageType;

		private MailCoordinates _coordinates;

		private List<MailUser> _recipients;

		private bool _showTimestamp;

		private List<string> _mostRecentMessageReadByUserIds;

		private HashSet<string> _readByUserIds;

		private MailMessage.MessageState _state;

		public string Id
		{
			get
			{
				return _id;
			}
			private set
			{
				Set(ref _id, value, false, "Id");
			}
		}

		public MailUser Sender
		{
			get
			{
				return _sender;
			}
			private set
			{
				Set(ref _sender, value, false, "Sender");
			}
		}

		public long ServerTimestamp
		{
			get
			{
				return _serverTimestamp;
			}
			private set
			{
				Set(ref _serverTimestamp, value, false, "ServerTimestamp");
			}
		}

		public long SendTimestamp
		{
			get
			{
				return _sendTimestamp;
			}
			set
			{
				Set(ref _sendTimestamp, value, false, "SendTimestamp");
			}
		}

		public List<string> Tags
		{
			get
			{
				return _tags;
			}
			private set
			{
				Set(ref _tags, value, false, "Tags");
			}
		}

		public string Body
		{
			get
			{
				return _body ?? string.Empty;
			}
			private set
			{
				Set(ref _body, (value != null) ? value.Trim() : null, false, "Body");
			}
		}

		public List<MailAttachment> Attachments
		{
			get
			{
				return _attachments;
			}
			private set
			{
				Set(ref _attachments, value, "Attachments");
			}
		}

		public IEnumerable<MailAttachment> ImageAttachments
		{
			get
			{
				if (Attachments != null)
				{
					return Attachments.Where((MailAttachment x) => x.IsImage());
				}
				return Enumerable.Empty<MailAttachment>();
			}
		}

		public IEnumerable<MailAttachment> AudioAttachments
		{
			get
			{
				if (Attachments != null)
				{
					return Attachments.Where((MailAttachment x) => x.IsAudioClip());
				}
				return Enumerable.Empty<MailAttachment>();
			}
		}

		public IEnumerable<MailAttachment> GenericFileAttachments
		{
			get
			{
				if (Attachments != null)
				{
					return Attachments.Where((MailAttachment x) => x.IsGenericFile());
				}
				return Enumerable.Empty<MailAttachment>();
			}
		}

		public IEnumerable<MailAttachment> VideoAttachments
		{
			get
			{
				if (Attachments != null)
				{
					return Attachments.Where((MailAttachment x) => x.IsVideo());
				}
				return Enumerable.Empty<MailAttachment>();
			}
		}

		public List<MailShare> Shares
		{
			get
			{
				return _shares;
			}
			private set
			{
				Set(ref _shares, value, "Shares");
			}
		}

		public List<string> ShareIds
		{
			get
			{
				return _shareIds;
			}
			private set
			{
				Set(ref _shareIds, value, false, "ShareIds");
			}
		}

		public MailMessage.Type MessageType
		{
			get
			{
				return _messageType;
			}
			private set
			{
				Set(ref _messageType, value, false, "MessageType");
			}
		}

		public MailCoordinates Coordinates
		{
			get
			{
				return _coordinates;
			}
			private set
			{
				if (Set(ref _coordinates, value, false, "Coordinates"))
				{
					RaisePropertyChanged("HasCoordinates");
				}
			}
		}

		public List<MailUser> Recipients
		{
			get
			{
				return _recipients;
			}
			set
			{
				Set(ref _recipients, value, false, "Recipients");
			}
		}

		public long Timestamp
		{
			get
			{
				return MailMessageUtil.GetTimestamp(SendTimestamp, ServerTimestamp);
			}
		}

		public bool ShowTimestamp
		{
			get
			{
				return _showTimestamp;
			}
			set
			{
				Set(ref _showTimestamp, value, false, "ShowTimestamp");
			}
		}

		public List<string> MostRecentMessageReadByUserIds
		{
			get
			{
				return _mostRecentMessageReadByUserIds;
			}
			private set
			{
				_mostRecentMessageReadByUserIds = value;
				RaisePropertyChanged("MostRecentMessageReadByUserIds");
			}
		}

		public HashSet<string> ReadByUserIds
		{
			get
			{
				return _readByUserIds;
			}
			private set
			{
				_readByUserIds = value;
				RaisePropertyChanged("ReadByUserIds");
			}
		}

		public bool HasCoordinates
		{
			get
			{
				return Coordinates != null;
			}
		}

		public MailMessage.MessageState State
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state != value)
				{
					_state = value;
					RaisePropertyChanged("State");
				}
			}
		}

		public bool HasStaleData
		{
			get
			{
				return State == MailMessage.MessageState.Incomplete;
			}
		}

		public bool IsForwardable
		{
			get
			{
				bool flag = IsMessageSent();
				bool flag2 = Attachments != null && Attachments.Count > 0;
				bool flag3 = HasShares() && !HasSticker() && !HasLike();
				bool result = flag2 || flag3;
				if (flag)
				{
					return result;
				}
				return false;
			}
		}

		public int ImageAttachmentsCount
		{
			get
			{
				if (!HasImageAttachments())
				{
					return 0;
				}
				return Enumerable.Count(Attachments, (MailAttachment attachment) => attachment.IsImage());
			}
		}

		protected MessageViewModel()
		{
		}

		public MessageViewModel(MailMessage message)
		{
			Id = message.Id;
			Sender = message.Sender;
			ServerTimestamp = message.ServerTimestamp;
			SendTimestamp = message.SendTimestamp;
			Tags = message.Tags;
			Body = message.Body;
			Attachments = message.Attachments;
			Shares = message.Shares;
			ShareIds = message.ShareIds;
			MessageType = message.MessageType;
			State = message.State;
			Coordinates = message.Coordinates;
			Recipients = message.Recipients;
			MostRecentMessageReadByUserIds = message.MostRecentMessageReadByUserIds;
			ReadByUserIds = message.ReadByUserIds;
		}

		public virtual void UpdateMessage(MailMessage message)
		{
			_id = message.Id;
			_sender = message.Sender;
			_sendTimestamp = ((message.SendTimestamp > 0) ? message.SendTimestamp : SendTimestamp);
			_serverTimestamp = message.ServerTimestamp;
			_tags = message.Tags;
			_body = message.Body;
			_shareIds = message.ShareIds;
			_messageType = message.MessageType;
			_coordinates = message.Coordinates;
			_recipients = message.Recipients;
			State = message.State;
			MostRecentMessageReadByUserIds = message.MostRecentMessageReadByUserIds;
			Attachments = message.Attachments;
			Shares = message.Shares;
			RaisePropertyChanged("UpdateMessage");
		}

		public virtual bool MatchesMessageId(string id)
		{
			return Id == id;
		}

		public bool IsRegularMessage()
		{
			return MessageType == MailMessage.Type.Regular;
		}

		public bool HasSendTimestamp()
		{
			return SendTimestamp != 0;
		}

		public bool HasImageAttachments()
		{
			return Attachments.NotNullAndAny((MailAttachment attachment) => attachment.IsImage());
		}

		public bool HasNonStickerImageAttachments()
		{
			if (HasImageAttachments())
			{
				return !HasSticker();
			}
			return false;
		}

		public bool HasAudioAttachments()
		{
			return Attachments.NotNullAndAny((MailAttachment attachment) => attachment.IsAudioClip());
		}

		public bool HasVideoAttachments()
		{
			return Attachments.NotNullAndAny((MailAttachment attachment) => attachment.IsVideo());
		}

		public bool HasGenericFileAttachments()
		{
			return Attachments.NotNullAndAny((MailAttachment attachment) => attachment.IsGenericFile());
		}

		public bool HasAttachments()
		{
			if (!HasNonStickerImageAttachments() && !HasAudioAttachments() && !HasVideoAttachments())
			{
				return HasGenericFileAttachments();
			}
			return true;
		}

		public bool HasShares()
		{
			return !Shares.IsNullOrEmpty();
		}

		public bool HasSharesHiddenByPrivacy()
		{
			if (!HasShares())
			{
				return !ShareIds.IsNullOrEmpty();
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

		public bool HasLike()
		{
			if (HasSticker())
			{
				return MailShare.LikeStickerIds.Contains(Shares[0].StickerId);
			}
			return false;
		}

		public bool IsMessageSent()
		{
			if (State != MailMessage.MessageState.Sent && State != MailMessage.MessageState.Delivered)
			{
				return State == MailMessage.MessageState.Seen;
			}
			return true;
		}
	}
}

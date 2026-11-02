using System;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchMessage;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class MessageDetailsViewModel : FacebookMessengerViewModel
	{
		private const string MessageViewModelStateKey = "messageViewModel";

		private string _messageId;

		private string _threadId;

		private string _threadName;

		private string _lastActiveText;

		private RecipientChatOrigin _recipientChatOrigin;

		private bool? _isSingleRecipient = null;

		private MappedMessageViewModel _messageViewModel;

		private RelayCommand<MessageActionArgs> _messageActionCommand;

		private RelayCommand _forwardCommand;

		private RelayCommand _viewMoreCommand;

		public string ThreadName
		{
			get
			{
				return _threadName;
			}
			set
			{
				Set(ref _threadName, value, false, "ThreadName");
			}
		}

		public string LastActiveText
		{
			get
			{
				return _lastActiveText;
			}
			set
			{
				Set(ref _lastActiveText, value, false, "LastActiveText");
				RaisePropertyChanged("HasLastActive");
			}
		}

		public RecipientChatOrigin RecipientChatOrigin
		{
			get
			{
				return _recipientChatOrigin;
			}
			set
			{
				Set(ref _recipientChatOrigin, value, false, "RecipientChatOrigin");
			}
		}

		public bool? IsSingleRecipient
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

		public bool HasLastActive => !string.IsNullOrEmpty(LastActiveText);

		public bool MessageFromPartialUser
		{
			get
			{
				if (MessageViewModel != null && MessageViewModel.Sender != null)
				{
					return MessageViewModel.Sender.IsPartial;
				}
				return false;
			}
		}

		public MappedMessageViewModel MessageViewModel
		{
			get
			{
				return _messageViewModel;
			}
			set
			{
				Set(ref _messageViewModel, value, false, "MessageViewModel");
			}
		}

		public RelayCommand<MessageActionArgs> MessageActionCommand => _messageActionCommand ?? (_messageActionCommand = new RelayCommand<MessageActionArgs>(ExecuteMessageActionCommand));

		public RelayCommand ForwardCommand => _forwardCommand ?? (_forwardCommand = new RelayCommand(ExecuteForwardCommand));

		public RelayCommand ViewMoreCommand => _viewMoreCommand ?? (_viewMoreCommand = new RelayCommand(ExecuteViewMoreCommand));

		public MessageDetailsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("messageId"))
			{
				_messageId = PathUri.Params["messageId"];
			}
			if (PathUri.Params.ContainsKey("threadId"))
			{
				_threadId = PathUri.Params["threadId"];
			}
			if (PathUri.Params.ContainsKey("name"))
			{
				ThreadName = PathUri.Params["name"];
			}
			if (PathUri.Params.ContainsKey("lastActiveText"))
			{
				LastActiveText = PathUri.Params["lastActiveText"];
			}
			RecipientChatOrigin result;
			if (PathUri.Params.ContainsKey("recipientChatOrigin") && Enum.TryParse<RecipientChatOrigin>(PathUri.Params["recipientChatOrigin"], out result))
			{
				_recipientChatOrigin = result;
			}
			bool result2;
			if (PathUri.Params.ContainsKey("isSingleRecipient") && bool.TryParse(PathUri.Params["isSingleRecipient"], out result2))
			{
				IsSingleRecipient = result2;
			}
			if (MessageViewModel == null)
			{
				FetchMessage().DoNotWait();
			}
		}

		public void LaunchTimeline()
		{
			if (MessageViewModel != null)
			{
				FacebookAppDeepLinkUtil.LaunchProfile(MessageViewModel.Sender);
			}
		}

		private void ExecuteMessageActionCommand(MessageActionArgs obj)
		{
			switch (obj.Action)
			{
			case MessageAction.Forward:
				ExecuteForwardCommand();
				break;
			case MessageAction.LongMessageDetails:
				ExecuteViewMoreCommand();
				break;
			case MessageAction.ViewDetails:
				break;
			}
		}

		private void ExecuteForwardCommand()
		{
			if (MessageViewModel != null)
			{
				FacebookUri forwardMessageUri = FacebookUri.GetForwardMessageUri(MessageViewModel.Id);
				Navigator.Navigate(forwardMessageUri);
			}
		}

		protected void ExecuteViewMoreCommand()
		{
			if (MessageViewModel != null)
			{
				FacebookUri longMessageDetailsUri = FacebookUri.GetLongMessageDetailsUri(_messageId, _threadId, ThreadName, LastActiveText, RecipientChatOrigin, IsSingleRecipient);
				Navigator.Navigate(longMessageDetailsUri);
			}
		}

		private async Task FetchMessage()
		{
			Facebook.Services.Api.Methods.FetchMessage.Request request = new Facebook.Services.Api.Methods.FetchMessage.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				MessageId = _messageId,
				ThreadId = _threadId
			};
			Facebook.Services.Api.Methods.FetchMessage.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchMessage.Request, Facebook.Services.Api.Methods.FetchMessage.Response>(request);
			if (response.Message == null)
			{
				Navigator.GoBack();
				await Dispatcher.RunAsync(() =>
				{
					OnError(new Error
					{
						Message = StringLibrary.MessageDetails_Error_Unexpected
					});
				});
				Logger.Error(this, $"Unable to find message in cache with id: {_messageId} and thread id: {_threadId}", "FetchMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\MessageDetailsViewModel.cs", 265);
			}
			else
			{
				MappedMessageViewModel messageViewModel = new MappedMessageViewModel(response.Message)
				{
					ShowTimestamp = false
				};
				MessageViewModel = messageViewModel;
			}
		}
	}
}

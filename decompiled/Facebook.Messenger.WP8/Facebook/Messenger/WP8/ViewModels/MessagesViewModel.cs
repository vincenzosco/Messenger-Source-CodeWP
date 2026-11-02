using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.FBGeoLocator;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.DeleteMessage;
using Facebook.Services.Api.Methods.FetchFriendLastActive;
using Facebook.Services.Api.Methods.SearchImages;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.MessageSender;
using Facebook.Services.Presence;
using Facebook.Services.Utilities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class MessagesViewModel : MessageBaseViewModel
	{
		public const string ThreadUpdatedPropertyName = "ThreadUpdated";

		public const string DisplayTypingIndicatorPropertyName = "DisplayTypingIndicator";

		public const string CanSendToThreadPropertyName = "CanSendToThread";

		public const string CanLikePropertyName = "CanLike";

		private DispatcherTimer _removeTypingIndicatorTimer;

		private DispatcherTimer _typingTimer;

		private TimeSpan _typingTimeout;

		private DateTime _lastTypingTime;

		private bool _isTyping;

		private readonly Facebook.Services.Presence.Service _presenceService;

		private bool _navigatedBecauseOfPush;

		private CancellationTokenSource _messageTokenSource;

		private string _messageFetchId;

		private string _previousFetchId;

		private bool _sendingLike;

		private MessageCollection _messages;

		private bool _isOtherUserTyping;

		private bool _displayTypingIndicator;

		private bool _canSendToThread;

		private bool _canLike;

		private bool _hasLastActive;

		private string _lastActiveText;

		private RecipientChatOrigin _recipientChatOrigin;

		private string _emptyText;

		private string _emptyTitleText;

		private RelayCommand _navigateToDetailsCommand;

		private RelayCommand<ListEventArgs> _listEventCommand;

		private RelayCommand _fetchUpdatedMessagesCommand;

		private RelayCommand<MessageActionArgs> _messageActionCommand;

		private RelayCommand<string> _searchImagesCommand;

		public MessageCollection Messages
		{
			get
			{
				return _messages;
			}
			set
			{
				Set(ref _messages, value, false, "Messages");
			}
		}

		public bool IsOtherUserTyping
		{
			get
			{
				return _isOtherUserTyping;
			}
			set
			{
				if (Set(ref _isOtherUserTyping, value, false, "IsOtherUserTyping"))
				{
					UpdateTypingIndicator();
				}
			}
		}

		public bool DisplayTypingIndicator
		{
			get
			{
				return _displayTypingIndicator;
			}
			set
			{
				Set(ref _displayTypingIndicator, value, false, "DisplayTypingIndicator");
			}
		}

		public override bool MessageComposerHasText
		{
			get
			{
				return base.MessageComposerHasText;
			}
			set
			{
				base.MessageComposerHasText = value;
				SetCanLike();
				UpdateLastTypingTime();
			}
		}

		public override bool MessageComposerHasContent
		{
			get
			{
				if (CanSendToThread)
				{
					return base.MessageComposerHasContent;
				}
				return false;
			}
		}

		public bool CanSendToThread
		{
			get
			{
				return _canSendToThread;
			}
			set
			{
				Set(ref _canSendToThread, value, false, "CanSendToThread");
				RaisePropertyChanged("MessageComposerHasContent");
			}
		}

		public bool CanLike
		{
			get
			{
				return _canLike;
			}
			set
			{
				Set(ref _canLike, value, false, "CanLike");
			}
		}

		public bool HasLastActive
		{
			get
			{
				return _hasLastActive;
			}
			set
			{
				Set(ref _hasLastActive, value, false, "HasLastActive");
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

		public string EmptyText
		{
			get
			{
				return _emptyText;
			}
			set
			{
				Set(ref _emptyText, value, false, "EmptyText");
			}
		}

		public string EmptyTitleText
		{
			get
			{
				return _emptyTitleText;
			}
			set
			{
				Set(ref _emptyTitleText, value, false, "EmptyTitleText");
			}
		}

		public RelayCommand NavigateToDetailsCommand => _navigateToDetailsCommand ?? (_navigateToDetailsCommand = new RelayCommand(ExecuteNavigateToDetailsCommand));

		public RelayCommand<ListEventArgs> ListEventCommand => _listEventCommand ?? (_listEventCommand = new RelayCommand<ListEventArgs>(ExecuteListEventCommand));

		public RelayCommand FetchUpdatedMessagesCommand => _fetchUpdatedMessagesCommand ?? (_fetchUpdatedMessagesCommand = new RelayCommand(ExecuteFetchUpdatedMessagesCommand));

		public RelayCommand<MessageActionArgs> MessageActionCommand => _messageActionCommand ?? (_messageActionCommand = new RelayCommand<MessageActionArgs>(ExecuteMessageActionCommand));

		public RelayCommand<string> SearchImagesCommand => _searchImagesCommand ?? (_searchImagesCommand = new RelayCommand<string>(ExecuteSearchImagesCommand));

		public event EventHandler SendMessageStarted;

		public event EventHandler<List<Image>> SearchImagesCompleted;

		private void TypingTimerOnTick(object sender, object o)
		{
			if (DateTime.UtcNow - _lastTypingTime > _typingTimeout)
			{
				SignalIsTyping(false);
				_typingTimer.Stop();
			}
			else
			{
				SignalIsTyping(true);
			}
		}

		private void RemoveTypingIndicatorTimerOnTick(object sender, EventArgs e)
		{
			if (_removeTypingIndicatorTimer != null)
			{
				_removeTypingIndicatorTimer.Stop();
			}
			DisplayTypingIndicator = false;
		}

		protected void OnPresenceUpdate(OnlineUserSignal signal)
		{
			if (Messages.Thread.IsCanonical())
			{
				FacebookUser facebookUser = signal.OnlineUsers.FirstOrDefault((FacebookUser u) => u.Id == Messages.Thread.SingleRecipient);
				if (facebookUser != null)
				{
					SetUserLastActive(StringLibrary.Messages_Active_Now, facebookUser.IsMessengerUser);
				}
			}
		}

		private void SignalIsTyping(bool isTyping)
		{
			if (!string.IsNullOrWhiteSpace(Messages.Thread.SingleRecipient) && _isTyping != isTyping)
			{
				_isTyping = isTyping;
				GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new MessageTypingSentSignal
				{
					ToId = Messages.Thread.SingleRecipient,
					State = isTyping
				});
			}
		}

		private void OnTypingReceived(MessageTypingReceivedSignal signal)
		{
			if (signal != null && !(signal.SenderId != Messages.Thread.SingleRecipient))
			{
				IsOtherUserTyping = signal.State;
			}
		}

		private async Task GetPresence()
		{
			if (!HasLastActive && Messages.Thread.IsCanonical())
			{
				await FetchLastActive();
			}
		}

		private void SetUserLastActive(string lastActiveText, bool isMessengerUser)
		{
			LastActiveText = lastActiveText;
			UpdateRecipientChatOrigin(isMessengerUser);
			HasLastActive = true;
		}

		private async Task FetchLastActive()
		{
			if (Messages.Thread.IsCanonical())
			{
				LastActiveParams lastActiveParams = await _presenceService.GetLastActiveForUser(Messages.Thread.SingleRecipient);
				if (lastActiveParams != null && lastActiveParams.LastActive != null)
				{
					SetUserLastActive(lastActiveParams.LastActive.Active ? StringLibrary.Messages_Active_Now : PresenceUtilities.GenerateLastActiveText(lastActiveParams.LastActive.Timestamp), lastActiveParams.IsMessengerUser);
				}
			}
		}

		private void InitTypingTimers()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected Obj, but got Unknown
			_typingTimeout = new TimeSpan(0, 0, 10);
			DispatcherTimer val = new DispatcherTimer();
			val.Interval = _typingTimeout;
			_typingTimer = val;
			_typingTimer.Tick += TypingTimerOnTick;
			DispatcherTimer val2 = new DispatcherTimer();
			val2.Interval = TimeSpan.FromSeconds(1.0);
			_removeTypingIndicatorTimer = val2;
			_removeTypingIndicatorTimer.Tick += RemoveTypingIndicatorTimerOnTick;
		}

		private void UpdateTypingIndicator(bool removeImmediately = false)
		{
			if (IsOtherUserTyping)
			{
				DisplayTypingIndicator = true;
				AudioController.PlayTypingIndicator();
			}
			else if (removeImmediately)
			{
				RemoveTypingIndicatorTimerOnTick(null, null);
			}
			else
			{
				_removeTypingIndicatorTimer.Start();
			}
		}

		private void UpdateLastTypingTime()
		{
			if (!MessageComposerHasText)
			{
				_typingTimer.Stop();
				SignalIsTyping(false);
				return;
			}
			_lastTypingTime = DateTime.UtcNow;
			if (!_typingTimer.IsEnabled)
			{
				_typingTimer.Start();
				SignalIsTyping(true);
			}
		}

		public MessagesViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, Facebook.Services.MessageSender.Service messageSender, Facebook.Messenger.WP8.FBGeoLocator.Service geolocator, StickerDrawerViewModel stickerDrawerViewModel, Facebook.Messenger.WP8.AudioController.Service audioController, Facebook.Services.Presence.Service presenceService, IFileAccess fileAccess)
			: base(apiService, context, navigator, dispatcher, networkStatus, messageSender, geolocator, stickerDrawerViewModel, audioController, fileAccess)
		{
			_presenceService = presenceService;
			CanSendToThread = true;
			CanLike = true;
			HasLastActive = false;
			IsLoadingLocation = true;
			_messageTokenSource = new CancellationTokenSource();
			InitTypingTimers();
			PropertyChanged += OnPropertyChanged;
			networkStatus.AddHandler(OnNetworkStatusChanged);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MessageTypingReceivedSignal>(this, OnTypingReceived);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<OnlineUserSignal>(this, OnPresenceUpdate);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheInboxDataClearedSignal>(this, OnCacheDataClearedSignal);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheThreadUpdatedSignal>(this, OnCacheThreadUpdatedSignal);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheMessagesUpdatedSignal>(this, OnCacheMessageUpdatedSignal);
			Title = T("messages-title");
		}

		public override void Cleanup()
		{
			_typingTimer.Stop();
			_typingTimer.Tick -= TypingTimerOnTick;
			_removeTypingIndicatorTimer.Stop();
			_removeTypingIndicatorTimer.Tick -= RemoveTypingIndicatorTimerOnTick;
			PropertyChanged -= OnPropertyChanged;
			NetworkStatus.RemoveHandler(OnNetworkStatusChanged);
			SignalIsTyping(false);
			ResetMessageToken();
			CleanUpCollection();
			base.Cleanup();
		}

		protected override void AttachedFilesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			base.AttachedFilesOnCollectionChanged(sender, args);
			SetCanLike();
		}

		private void MessagesOnLoadFinished(MessageCollection messageCollection, bool fromWeb, bool isFirstPage, Error error)
		{
			Dispatcher.RunAsync(() =>
			{
				IsLoading = false;
				string settingsId = (Messages.Thread.IsCanonical() ? Messages.Thread.SingleRecipient : Messages.Thread.Id);
				SendLocationEnabled = Geolocator.IsLocationEnabledForThread(settingsId);
				IsLoadingLocation = false;
				IsOtherUserTyping = _presenceService.IsUserTyping(Messages.Thread.SingleRecipient);
				UpdateEmptyMessagesTypeWithError(error);
				GetPresence().DoNotWait();
				RaisePropertyChanged("ThreadUpdated");
			});
			if (isFirstPage && !fromWeb)
			{
				Dispatcher.RunAsync(() =>
				{
					FetchUpdatedMessages().DoNotWait();
				});
			}
		}

		private void MessagesOnLoadStarted(object sender, EventArgs eventArgs)
		{
			Dispatcher.RunAsync(() =>
			{
				IsLoading = true;
			});
		}

		private void MessagesOnUpdatedMessagesFetched(MessageCollection collection, Error error)
		{
			CanSendToThread = collection != null && collection.Thread != null && collection.Any() && collection.Thread.IsSubscribed;
			if (IsMessagesEmpty())
			{
				SetEmptyMessages(EmptyMessageType.NoMessages);
			}
		}

		private void LogNavigationOnMessagesLoadFinished(MessageCollection messageCollection, bool fromWeb, bool isFirstPage, Error error)
		{
			if (Messages != null)
			{
				Messages.LoadFinished -= LogNavigationOnMessagesLoadFinished;
			}
			LogPageReady();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			string propertyName;
			if ((propertyName = args.PropertyName) != null && propertyName == "CanSendToThread")
			{
				SetCanLike();
			}
		}

		private void OnNetworkStatusChanged(INetworkStatus sender, bool connected)
		{
			if (connected && IsMessagesEmpty())
			{
				CanSendToThread = true;
				Messages.FetchFirstPage().DoNotWait();
			}
		}

		private void ExecuteNavigateToDetailsCommand()
		{
			FacebookUri appUri;
			if (Messages.Thread.IsCanonical())
			{
				UriQuery uriQuery = new UriQuery();
				uriQuery.Add("thread_id", Messages.Thread.Id);
				uriQuery.Add("last_active_text", LastActiveText);
				UriQuery query = uriQuery;
				appUri = FacebookUri.GetAppUri(UriId.CanonicalThreadDetails, query);
			}
			else
			{
				UriQuery uriQuery2 = new UriQuery();
				uriQuery2.Add("thread_id", Messages.Thread.Id);
				UriQuery query2 = uriQuery2;
				appUri = FacebookUri.GetAppUri(UriId.GroupDetails, query2);
			}
			Navigator.Navigate(appUri);
		}

		private async void ExecuteListEventCommand(ListEventArgs e)
		{
			if (e.ListEvent != ListEvent.ScrolledToBottom || IsMessagesEmpty())
			{
				return;
			}
			string messageId = Messages.Last().Id;
			if (_messageFetchId != messageId)
			{
				_previousFetchId = _messageFetchId;
				_messageFetchId = messageId;
				if (!(await Messages.FetchOlderMessages()))
				{
					_messageFetchId = _previousFetchId;
				}
			}
		}

		private async void ExecuteFetchUpdatedMessagesCommand()
		{
			await FetchUpdatedMessages();
		}

		protected void ExecuteMessageActionCommand(MessageActionArgs args)
		{
			ContextualMessageViewModel contextualMessageViewModel = args.Message as ContextualMessageViewModel;
			if (contextualMessageViewModel != null)
			{
				switch (args.Action)
				{
				case MessageAction.ViewDetails:
				case MessageAction.LongMessageDetails:
					ViewMessageDetails(contextualMessageViewModel, args.Action == MessageAction.LongMessageDetails);
					break;
				case MessageAction.Forward:
					ForwardMessage(contextualMessageViewModel);
					break;
				case MessageAction.Retry:
					RetryMessage(contextualMessageViewModel).DoNotWait();
					break;
				case MessageAction.Delete:
					DeleteMessage(contextualMessageViewModel).DoNotWait();
					break;
				case MessageAction.Cancel:
					CancelMessage(contextualMessageViewModel);
					break;
				}
			}
		}

		protected override async Task SendMessage(SendMessageContent content)
		{
			if (content == null)
			{
				return;
			}
			ShowLocationWarning = false;
			if ((string.IsNullOrWhiteSpace(Messages.Thread.Id) && string.IsNullOrWhiteSpace(Messages.Thread.SingleRecipient)) || !Context.IsAuthenticated())
			{
				return;
			}
			if (content.StickerId == "227878347358915")
			{
				if (_sendingLike)
				{
					return;
				}
				_sendingLike = true;
			}
			try
			{
				Facebook.Services.Api.Methods.SendMessage.Request request = CreateSendMessageRequest(content);
				SetEmptyMessages(EmptyMessageType.None);
				PlaySentMessageSound(content);
				MailMessage message = Facebook.Services.Api.Methods.SendMessage.Request.CreateStubMessage(request, Context);
				message.State = MailMessage.MessageState.Queued;
				Messages.InsertAtBottom(message);
				await ExecuteSendMessageWithRequest(Messages[0], request);
			}
			finally
			{
				_sendingLike = false;
			}
		}

		private async Task ExecuteSendMessageWithRequest(ContextualMessageViewModel message, Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			await PerformWithMessageTokenAsync(async (CancellationToken token) =>
			{
				if (SendMessageStarted != null)
				{
					SendMessageStarted(this, EventArgs.Empty);
				}
				if (request.HasUploadableMedia())
				{
					request.SetIsCancellableAction((bool cancellable) =>
					{
						Dispatcher.RunAsync(() =>
						{
							message.IsCancellable = cancellable;
						});
					});
					request.InvokeIsCancellableAction(true);
				}
				if (token.IsCancellationRequested)
				{
					Logger.Warning(this, "Message token was cancelled before send message occurred", "ExecuteSendMessageWithRequest", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\MessagesViewModel.cs", 497);
				}
				else
				{
					Facebook.Services.Api.Methods.SendMessage.Response response = await MessageSender.ExecuteAsync<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(request);
					if (token.IsCancellationRequested)
					{
						Logger.Warning(this, "Message token was cancelled after send message occurred", "ExecuteSendMessageWithRequest", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\MessagesViewModel.cs", 505);
					}
					else if (response.HasError())
					{
						if (!response.SessionExpired)
						{
							message.State = MailMessage.MessageState.Failed;
						}
						if (response.ApiError != null && response.ApiError.ErrorCode == 230)
						{
							MessageBox.Show(StringLibrary.Message_Sending_Failed_Permission_Description, StringLibrary.Message_Sending_Failed, (MessageBoxButton)0);
						}
					}
					else if (string.IsNullOrEmpty(Messages.Thread.Id))
					{
						Messages.UpdateThread(response.ThreadId).DoNotWait();
					}
				}
			});
		}

		public void MarkThreadRead()
		{
			if (Messages != null)
			{
				Messages.MarkThreadRead();
			}
		}

		private async void ExecuteSearchImagesCommand(string term)
		{
			Facebook.Services.Api.Methods.SearchImages.Request request = new Facebook.Services.Api.Methods.SearchImages.Request
			{
				Page = 0,
				SearchTerm = term
			};
			Facebook.Services.Api.Methods.SearchImages.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SearchImages.Request, Facebook.Services.Api.Methods.SearchImages.Response>(request);
			List<Image> results = ((!response.HasError() && response.SearchResponse != null && response.SearchResponse.ImageResult != null && response.SearchResponse.ImageResult.Results != null) ? response.SearchResponse.ImageResult.Results : new List<Image>());
			if (SearchImagesCompleted != null)
			{
				SearchImagesCompleted(this, results);
			}
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			_navigatedBecauseOfPush = PathUri.Params.ContainsKey(UriParams.FromExternal);
			long recentPushActionId = 0L;
			string threadName = null;
			string threadId = null;
			string recipientId = null;
			if (PathUri.Params.ContainsKey("actionId"))
			{
				recentPushActionId = long.Parse(PathUri.Params["actionId"]);
			}
			if (PathUri.Params.ContainsKey("name"))
			{
				threadName = PathUri.Params["name"];
			}
			if (PathUri.Params.ContainsKey("tid"))
			{
				threadId = PathUri.Params["tid"];
			}
			if (PathUri.Params.ContainsKey("recipientId"))
			{
				recipientId = PathUri.Params["recipientId"];
			}
			InitializeMessageCollection(threadId, threadName, recipientId, recentPushActionId);
			GetPresence().DoNotWait();
		}

		public void CleanUpCollection()
		{
			if (Messages != null)
			{
				Messages.CleanUp();
				Messages.UpdatedMessagesFetched -= MessagesOnUpdatedMessagesFetched;
				Messages.LoadStarted -= MessagesOnLoadStarted;
				Messages.LoadFinished -= MessagesOnLoadFinished;
				Messages.LoadFinished -= LogNavigationOnMessagesLoadFinished;
			}
		}

		private void OnCacheDataClearedSignal(CacheInboxDataClearedSignal signal)
		{
			ResetMessageToken();
			Messages.CleanUp();
			Messages.FetchFirstPage().DoNotWait();
		}

		private void OnCacheThreadUpdatedSignal(CacheThreadUpdatedSignal signal)
		{
			if (Messages != null && Messages.Thread.IsSameThread(signal.Thread))
			{
				if (ReadReceiptAdded(signal.Thread.LastMessageReadUserIds))
				{
					AudioController.PlaySeen();
				}
				Messages.Thread.UpdateThread(signal.Thread);
				Messages.Thread.UpdateDisplayName(Context);
				RaisePropertyChanged("ThreadUpdated");
			}
		}

		private void OnCacheMessageUpdatedSignal(CacheMessagesUpdatedSignal signal)
		{
			if (!Context.IsAuthenticated() || Messages == null || Messages.Thread == null || Messages.Thread.Id != signal.Thread.Id)
			{
				return;
			}
			List<MailMessage> messages = signal.Messages;
			if (messages.Any((MailMessage m) => m.Sender != null && m.Sender.Id != Context.Session.Profile.Id && signal.ShouldNotify))
			{
				_isOtherUserTyping = false;
				UpdateTypingIndicator(true);
			}
			foreach (MailMessage item in messages)
			{
				Messages.HandleMessage(item);
			}
		}

		public bool MatchesCurrentThread(MailThread thread)
		{
			if (thread.IsCanonical() && Messages.Thread.IsCanonical())
			{
				return thread.SingleRecipient == Messages.Thread.CanonicalParticipant.Id;
			}
			return thread.Id == Messages.Thread.Id;
		}

		private bool ReadReceiptAdded(List<string> updatedLastReadReceiptIds)
		{
			return updatedLastReadReceiptIds.Count > Messages.Thread.LastMessageReadUserIds.Count;
		}

		private void ResetMessageToken()
		{
			if (_messageTokenSource != null)
			{
				_messageTokenSource.Cancel();
			}
			_messageTokenSource = new CancellationTokenSource();
		}

		protected async Task PerformWithMessageTokenAsync(Func<CancellationToken, Task> action)
		{
			CancellationTokenSource tokenSource = _messageTokenSource;
			if (tokenSource != null && !tokenSource.IsCancellationRequested)
			{
				await action(tokenSource.Token);
			}
		}

		private void InitializeMessageCollection(string threadId = null, string threadName = null, string recipientId = null, long recentPushActionId = 0L)
		{
			if (string.IsNullOrEmpty(threadId) && string.IsNullOrEmpty(recipientId))
			{
				throw new Exception("Cannot initialize a thread without either a thread ID or a recipient ID");
			}
			Messages = ((!string.IsNullOrEmpty(threadId)) ? MessageCollection.FromThreadId(Context, ApiService, CancelTokenSource.Token, threadId, threadName, recentPushActionId) : MessageCollection.FromRecipientId(Context, ApiService, CancelTokenSource.Token, recipientId, threadName));
			if (_navigatedBecauseOfPush && recentPushActionId == 0)
			{
				_navigatedBecauseOfPush = false;
				Messages.MarkCollectionStale();
			}
			Messages.UpdatedMessagesFetched += MessagesOnUpdatedMessagesFetched;
			Messages.LoadStarted += MessagesOnLoadStarted;
			Messages.LoadFinished += MessagesOnLoadFinished;
			Messages.FetchFirstPage().DoNotWait();
		}

		private List<MailUser> CreateParticipantList()
		{
			List<MailUser> list = new List<MailUser>();
			list.Add(new MailUser
			{
				Id = Messages.Thread.SingleRecipient,
				Name = Messages.Thread.Name
			});
			return list;
		}

		private void UpdateRecipientChatOrigin(bool isMessengerUser)
		{
			RecipientChatOrigin recipientChatOrigin = RecipientChatOrigin.None;
			if (isMessengerUser)
			{
				recipientChatOrigin = RecipientChatOrigin.Messenger;
			}
			else if (!IsPartialUser && !Messages.Thread.CanonicalParticipant.IsPartial)
			{
				recipientChatOrigin = RecipientChatOrigin.Facebook;
			}
			RecipientChatOrigin = recipientChatOrigin;
		}

		private async Task FetchUpdatedMessages()
		{
			if (Messages != null)
			{
				await Messages.FetchUpdatedMessages();
			}
		}

		private void UpdateEmptyMessagesTypeWithError(Error error)
		{
			EmptyMessageType emptyMessages = EmptyMessageType.None;
			if (IsMessagesEmpty())
			{
				emptyMessages = ((error == null) ? EmptyMessageType.NoMessages : ((!HasConnection) ? EmptyMessageType.NoConnection : EmptyMessageType.ServerError));
			}
			SetEmptyMessages(emptyMessages);
		}

		private void SetEmptyMessages(EmptyMessageType type)
		{
			switch (type)
			{
			case EmptyMessageType.None:
				EmptyTitleText = null;
				EmptyText = null;
				break;
			case EmptyMessageType.NoMessages:
			{
				MailUser mailUser = (Messages.Thread.IsCanonical() ? Messages.Thread.CanonicalParticipant : null);
				if (mailUser != null && !string.IsNullOrEmpty(mailUser.Name))
				{
					EmptyText = string.Format(mailUser.IsMessengerUser ? StringLibrary.Empty_Conversation_Placeholder_Text : StringLibrary.Empty_Conversation_Placeholder_Text_For_Non_Messenger_User, mailUser.Name);
				}
				else
				{
					EmptyText = string.Empty;
				}
				EmptyTitleText = StringLibrary.Empty_Conversation_Placeholder_Title;
				break;
			}
			case EmptyMessageType.NoConnection:
			case EmptyMessageType.ServerError:
				CanSendToThread = false;
				EmptyText = null;
				EmptyTitleText = ((type == EmptyMessageType.NoConnection) ? StringLibrary.Error_No_Connection : StringLibrary.Error_Cannot_Fetch_Messages);
				break;
			}
		}

		private bool IsMessagesEmpty()
		{
			if (Messages != null)
			{
				return !Messages.Any();
			}
			return true;
		}

		private Facebook.Services.Api.Methods.SendMessage.Request CreateSendMessageRequest(ContextualMessageViewModel message)
		{
			Facebook.Services.Api.Methods.SendMessage.Request request = new Facebook.Services.Api.Methods.SendMessage.Request(message.ThreadId, message.Body, message.OfflineThreadingId);
			if (message.HasShares())
			{
				request.ObjectAttachmentId = message.Shares[0].StickerId;
				request.ObjectAttachmentHref = message.Shares[0].Href;
			}
			if (message.Coordinates != null)
			{
				request.Location = new FacebookPlace
				{
					Latitude = message.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture),
					Longitude = message.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture)
				};
			}
			if (message.Attachments != null && message.Attachments.Any())
			{
				List<MailAttachment> attachments = message.Attachments;
				request.FilePaths = new List<Facebook.Services.Api.Methods.SendMessage.Attachment>();
				foreach (MailAttachment item in attachments)
				{
					Facebook.Services.Api.Methods.SendMessage.Attachment attachment = new Facebook.Services.Api.Methods.SendMessage.Attachment();
					attachment.Name = item.FileName;
					attachment.MimeType = item.MimeType;
					attachment.Path = FileUtil.ConvertUrlToPath(item.LocalUrl);
					attachment.ThumbnailPath = FileUtil.ConvertUrlToPath(item.LocalThumbnailUrl);
					Facebook.Services.Api.Methods.SendMessage.Attachment attachment2 = attachment;
					if (item.HasValidImageData())
					{
						attachment2.Height = item.Image.Height;
						attachment2.Width = item.Image.Width;
					}
					else if (item.HasValidVideoData())
					{
						attachment2.Length = item.Video.Length;
						attachment2.Height = item.Video.Height;
						attachment2.Width = item.Video.Width;
						attachment2.Rotation = item.Video.Rotation;
					}
					request.FilePaths.Add(attachment2);
				}
			}
			return request;
		}

		protected override Facebook.Services.Api.Methods.SendMessage.Request CreateSendMessageRequest(SendMessageContent content)
		{
			string offlineThreadingId = OfflineIdHelper.CreateOfflineThreadingId(Context);
			Facebook.Services.Api.Methods.SendMessage.Request request = ((!string.IsNullOrWhiteSpace(Messages.Thread.Id) && !Messages.Thread.IsThreadOffline) ? new Facebook.Services.Api.Methods.SendMessage.Request(Messages.Thread.Id, content.Text, offlineThreadingId) : new Facebook.Services.Api.Methods.SendMessage.Request(CreateParticipantList(), content.Text, offlineThreadingId));
			request.SingleRecipientId = Messages.Thread.SingleRecipient;
			request.AccessToken = Context.Session.Credentials.AccessToken;
			SetRequestContent(request, content);
			return request;
		}

		private void SetCanLike()
		{
			CanLike = CanSendToThread && !MessageComposerHasContent;
		}

		protected override bool SetLocationSendingState(bool enable)
		{
			string settingsId = (Messages.Thread.IsCanonical() ? Messages.Thread.SingleRecipient : Messages.Thread.Id);
			bool flag = Geolocator.SetLocationForThread(enable, settingsId);
			if (flag)
			{
				SendLocationEnabled = enable;
			}
			return flag;
		}

		protected override void OnGeolocatorStatusChanged(object sender, EventArgs eventArgs)
		{
			string settingsId = (Messages.Thread.IsCanonical() ? Messages.Thread.SingleRecipient : Messages.Thread.Id);
			SendLocationEnabled = Geolocator.IsLocationEnabledForThread(settingsId);
		}

		private void ViewMessageDetails(ContextualMessageViewModel message, bool isLongMessage = false)
		{
			Uri uri = ((!isLongMessage) ? FacebookUri.GetMessageDetailsUri(message.Id, message.ThreadId, Messages.Thread.DisplayName, LastActiveText, RecipientChatOrigin, Messages.Thread.IsCanonical()) : FacebookUri.GetLongMessageDetailsUri(message.Id, message.ThreadId, Messages.Thread.DisplayName, LastActiveText, RecipientChatOrigin, Messages.Thread.IsCanonical()));
			Navigator.Navigate(uri);
		}

		private void ForwardMessage(ContextualMessageViewModel message)
		{
			FacebookUri forwardMessageUri = FacebookUri.GetForwardMessageUri(message.Id);
			Navigator.Navigate(forwardMessageUri);
		}

		private async Task DeleteMessage(ContextualMessageViewModel message)
		{
			Messages.Remove(message);
			bool isLocal = message.State == MailMessage.MessageState.Failed || message.State == MailMessage.MessageState.Queued;
			if (isLocal)
			{
				Facebook.Services.Api.Methods.SendMessage.Request request = CreateSendMessageRequest(message);
				DeleteMessageFiles(request).DoNotWait();
			}
			Facebook.Services.Api.Methods.DeleteMessage.Request deleteRequest = new Facebook.Services.Api.Methods.DeleteMessage.Request(message.Id, message.ThreadId, isLocal)
			{
				AccessToken = Context.Session.Credentials.AccessToken
			};
			await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.DeleteMessage.Request, Facebook.Base.Response>(deleteRequest);
			if (!Messages.Any())
			{
				NavigateBack();
			}
		}

		private async Task DeleteMessageFiles(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			if (!request.HasFiles())
			{
				return;
			}
			foreach (Facebook.Services.Api.Methods.SendMessage.Attachment attachments in request.FilePaths)
			{
				await FileAccess.DeleteFileAtPath(attachments.ThumbnailPath);
				await FileAccess.DeleteFileAtPath(attachments.Path);
			}
		}

		private void CancelMessage(ContextualMessageViewModel message)
		{
			Facebook.Services.Api.Methods.SendMessage.Request request = CreateSendMessageRequest(message);
			string contextIdForRequest = MessageSender.GetContextIdForRequest(request);
			List<Facebook.Services.Api.Methods.SendMessage.Request> outstandingRequestsByContextId = MessageSender.GetOutstandingRequestsByContextId(contextIdForRequest);
			foreach (Facebook.Services.Api.Methods.SendMessage.Request item in outstandingRequestsByContextId)
			{
				if (item.OfflineThreadingId == message.OfflineThreadingId)
				{
					item.Cancel();
					DeleteMessage(message).DoNotWait();
				}
			}
		}

		private async Task RetryMessage(ContextualMessageViewModel message)
		{
			if (CanSendToThread)
			{
				Facebook.Services.Api.Methods.SendMessage.Request request = CreateSendMessageRequest(message);
				if (Messages.Thread.IsThreadOffline || string.IsNullOrWhiteSpace(Messages.Thread.Id))
				{
					request.ClearLocalAndServerIds();
					request.Users = CreateParticipantList();
				}
				else
				{
					request.ServerThreadId = Messages.Thread.Id;
				}
				message.State = MailMessage.MessageState.Queued;
				message.SendTimestamp = request.SentTimestamp;
				request.AccessToken = Context.Session.Credentials.AccessToken;
				await ExecuteSendMessageWithRequest(message, request);
			}
		}

		private void NavigateBack()
		{
			if (PathUri.Params.ContainsKey(UriParams.FromExternal))
			{
				Navigator.Reset(FacebookUri.RootUri);
			}
			else
			{
				Navigator.GoBack();
			}
		}

		public override void LogNavigatedTo()
		{
			base.LogNavigatedTo();
			if (Messages == null || Messages.MessagesFetched)
			{
				LogPageReady();
			}
			else
			{
				Messages.LoadFinished += LogNavigationOnMessagesLoadFinished;
			}
		}
	}
}

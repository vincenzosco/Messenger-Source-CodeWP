using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.FetchMessages;
using Facebook.Services.Api.Methods.FetchThread;
using Facebook.Services.Api.Methods.MarkThreadState;
using Facebook.Services.Utilities;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class MessageCollection : ObservableCollection<ContextualMessageViewModel>
	{
		public delegate void FetchCollectionDelegate(MessageCollection c, bool fromWeb, bool isFirstPage, Error error);

		public delegate void UpdateCollectionDelegate(MessageCollection c, Error error);

		private const int TimestampSeperatorThresholdMs = 600000;

		private readonly FacebookContext _context;

		private readonly Facebook.Services.Api.Service _api;

		private readonly CancellationToken _token;

		private readonly List<MailMessage> _pendingMessages;

		private CancellationTokenSource _localTokenSource;

		private CancellationTokenSource _linkedTokenSource;

		private long _recentPushActionId;

		private bool _hasMoreItems;

		private bool _busy;

		private bool _busyFetchingUpdatedThreads;

		private bool _nextRequestShouldForceToServer;

		private bool _firstPageFetched;

		private bool _threadHasAllInfo;

		public ThreadViewModel Thread { get; set; }

		public bool IsCanonical
		{
			get
			{
				if (Thread != null)
				{
					return Thread.IsCanonical();
				}
				return false;
			}
		}

		public bool MessagesFetched { get; private set; }

		public long MaxActionId
		{
			get
			{
				if (!this.Any())
				{
					return 0L;
				}
				return this.Max((ContextualMessageViewModel m) => (!m.HasStaleData) ? m.ActionId : 0);
			}
		}

		public long MaxTimestamp
		{
			get
			{
				if (!this.Any())
				{
					return 0L;
				}
				return this.Max((ContextualMessageViewModel m) => (!m.HasStaleData) ? m.Timestamp : 0);
			}
		}

		public event EventHandler LoadStarted;

		public event EventHandler MessagesAddedToCollection;

		public event FetchCollectionDelegate LoadFinished;

		public event UpdateCollectionDelegate UpdatedMessagesFetched;

		public MessageCollection(FacebookContext context)
		{
			_context = context;
			_pendingMessages = new List<MailMessage>();
		}

		private MessageCollection(FacebookContext context, Facebook.Services.Api.Service service, CancellationToken token)
		{
			_context = context;
			_api = service;
			_token = token;
			_pendingMessages = new List<MailMessage>();
			InitLinkedToken(token);
		}

		public static MessageCollection FromThreadId(FacebookContext context, Facebook.Services.Api.Service service, CancellationToken token, string threadId, string threadName = null, long pushActionId = 0L)
		{
			MessageCollection messageCollection = new MessageCollection(context, service, token);
			messageCollection._hasMoreItems = true;
			messageCollection._recentPushActionId = pushActionId;
			messageCollection.Thread = new ThreadViewModel(threadId, threadName);
			return messageCollection;
		}

		public static MessageCollection FromRecipientId(FacebookContext context, Facebook.Services.Api.Service service, CancellationToken token, string recipientId, string threadName = null)
		{
			MessageCollection messageCollection = new MessageCollection(context, service, token);
			messageCollection._hasMoreItems = false;
			messageCollection.Thread = new ThreadViewModel(null, threadName, recipientId);
			return messageCollection;
		}

		public void CleanUp()
		{
			InitLinkedToken(_token);
			Clear();
			lock (_pendingMessages)
			{
				_pendingMessages.Clear();
			}
			_busy = false;
			_busyFetchingUpdatedThreads = false;
			_nextRequestShouldForceToServer = false;
			_firstPageFetched = false;
			_threadHasAllInfo = false;
			_hasMoreItems = !string.IsNullOrEmpty(Thread.Id);
		}

		public async Task UpdateThread(string threadId)
		{
			if (string.IsNullOrEmpty(Thread.Id))
			{
				Facebook.Services.Api.Methods.FetchThread.Request request = Facebook.Services.Api.Methods.FetchThread.Request.ByThreadId(threadId);
				request.CancelToken = GetCancellationToken();
				request.AccessToken = _context.Session.Credentials.AccessToken;
				Facebook.Services.Api.Methods.FetchThread.Response response = await _api.ExecuteAsync<Facebook.Services.Api.Methods.FetchThread.Request, Facebook.Services.Api.Methods.FetchThread.Response>(request);
				if (response.Thread != null)
				{
					Thread.UpdateThread(response.Thread);
				}
			}
		}

		public void MarkCollectionStale()
		{
			_nextRequestShouldForceToServer = true;
		}

		public bool AddIncomingMessage(MailMessage message)
		{
			if (OfflineIdHelper.IsMessageFromThisDevice(message, _context))
			{
				return false;
			}
			if (!_firstPageFetched)
			{
				lock (_pendingMessages)
				{
					_pendingMessages.Add(message);
				}
				return false;
			}
			ProcessMessage(message);
			return true;
		}

		public void InsertAtBottom(MailMessage message)
		{
			InsertMessage(0, new ContextualMessageViewModel(message, Thread.IsCanonical()));
		}

		public async Task FetchFirstPage()
		{
			if (await EnsureThread())
			{
				_firstPageFetched = await FetchMessages(0L);
				if (_firstPageFetched)
				{
					ProcessPendingMessages();
				}
			}
		}

		public async Task<bool> FetchOlderMessages()
		{
			if (!_hasMoreItems)
			{
				return false;
			}
			return await FetchMessages(this.Any() ? this.Last().Timestamp : 0);
		}

		public async Task FetchUpdatedMessages()
		{
			if (_busyFetchingUpdatedThreads || !this.Any() || string.IsNullOrWhiteSpace(Thread.Id))
			{
				return;
			}
			_busyFetchingUpdatedThreads = true;
			Facebook.Services.Api.Methods.FetchMessages.Response response = await ExecuteFetchRequest(0L, MaxActionId);
			if (!response.HasError() && response.Messages != null)
			{
				if (response.ShouldPurgeMessageCache)
				{
					Clear();
				}
				foreach (MailMessage message in response.Messages)
				{
					ProcessMessage(message);
				}
			}
			_busyFetchingUpdatedThreads = false;
			if (MessagesAddedToCollection != null)
			{
				MessagesAddedToCollection(this, EventArgs.Empty);
			}
			if (UpdatedMessagesFetched != null)
			{
				UpdatedMessagesFetched(this, response.Error);
			}
		}

		public void MarkThreadRead()
		{
			Facebook.Services.Api.Methods.MarkThreadState.Request request = new Facebook.Services.Api.Methods.MarkThreadState.Request();
			request.AccessToken = _context.Session.Credentials.AccessToken;
			request.ThreadId = Thread.Id;
			request.Enabled = true;
			request.Tag = MailThread.Tag.Read;
			Facebook.Services.Api.Methods.MarkThreadState.Request request2 = request;
			_api.ExecuteAsync<Facebook.Services.Api.Methods.MarkThreadState.Request, Facebook.Services.Api.Methods.MarkThreadState.Response>(request2).DoNotWait();
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			base.OnCollectionChanged(args);
			AnimateMostRecentSticker();
			if (args.NewItems != null)
			{
				foreach (ContextualMessageViewModel item in args.NewItems.Cast<ContextualMessageViewModel>())
				{
					item.PropertyChanged += MailMessagePropertyChanged;
				}
			}
			if (args.OldItems == null)
			{
				return;
			}
			foreach (ContextualMessageViewModel item2 in args.OldItems.Cast<ContextualMessageViewModel>())
			{
				item2.PropertyChanged -= MailMessagePropertyChanged;
			}
		}

		private void MailMessagePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Shares" || args.PropertyName == "CopyFrom")
			{
				AnimateMostRecentSticker();
			}
		}

		private async Task<bool> EnsureThread()
		{
			if (!string.IsNullOrEmpty(Thread.Id))
			{
				Facebook.Services.Api.Methods.FetchThread.Request request = Facebook.Services.Api.Methods.FetchThread.Request.ByThreadId(Thread.Id);
				request.CancelToken = GetCancellationToken();
				request.AccessToken = _context.Session.Credentials.AccessToken;
				Facebook.Services.Api.Methods.FetchThread.Response response = await _api.ExecuteAsync<Facebook.Services.Api.Methods.FetchThread.Request, Facebook.Services.Api.Methods.FetchThread.Response>(request);
				if (response.Thread != null)
				{
					Thread.UpdateThread(response.Thread);
				}
				return true;
			}
			if (!string.IsNullOrEmpty(Thread.SingleRecipient))
			{
				Facebook.Services.Api.Methods.FetchThread.Request request2 = Facebook.Services.Api.Methods.FetchThread.Request.ByRecipient(Thread.SingleRecipient);
				request2.CancelToken = GetCancellationToken();
				request2.AccessToken = _context.Session.Credentials.AccessToken;
				Facebook.Services.Api.Methods.FetchThread.Response response2 = await _api.ExecuteAsync<Facebook.Services.Api.Methods.FetchThread.Request, Facebook.Services.Api.Methods.FetchThread.Response>(request2);
				if (response2.Thread != null)
				{
					Thread.UpdateThread(response2.Thread);
					_hasMoreItems = true;
					return true;
				}
				if (LoadFinished != null)
				{
					LoadFinished(this, true, !response2.HasError(), response2.Error);
				}
				return false;
			}
			throw new Exception("Thread contains no id or recipient id, unable to continue");
		}

		public void HandleMessage(MailMessage message)
		{
			ContextualMessageViewModel contextualMessageViewModel = this.SingleOrDefaultLogErrorIfNotSingle((ContextualMessageViewModel m) => m.Id == message.Id || m.Id == message.OfflineThreadingId, "HandleMessage");
			if (contextualMessageViewModel != null)
			{
				if (message.SyncChangeType == "deleted")
				{
					Remove(contextualMessageViewModel);
				}
				else
				{
					contextualMessageViewModel.UpdateMessage(message);
				}
			}
			else if (message.SourceIsPush)
			{
				FetchUpdatedMessages().DoNotWait();
			}
		}

		private ContextualMessageViewModel ProcessMessage(MailMessage message)
		{
			ContextualMessageViewModel contextualMessageViewModel = new ContextualMessageViewModel(message, Thread.IsCanonical());
			UpdateMessageFromThread(contextualMessageViewModel);
			if (message.Deleted)
			{
				Remove(contextualMessageViewModel);
			}
			else
			{
				AddMessage(message, contextualMessageViewModel);
			}
			return contextualMessageViewModel;
		}

		protected override void RemoveItem(int index)
		{
			ContextualMessageViewModel contextualMessageViewModel = ((index - 1 >= 0 && index - 1 < Count) ? base[index - 1] : null);
			ContextualMessageViewModel contextualMessageViewModel2 = ((index + 1 >= 0 && index + 1 < Count) ? base[index + 1] : null);
			ContextualMessageViewModel contextualMessageViewModel3 = base[index];
			if (contextualMessageViewModel != null)
			{
				if (contextualMessageViewModel3.MessageGroupState == ContextualMessageViewModel.GroupState.FirstOfGroup)
				{
					contextualMessageViewModel.MessageGroupState = ContextualMessageViewModel.GroupState.FirstOfGroup;
				}
				if (contextualMessageViewModel2 == null || Math.Abs(contextualMessageViewModel.Timestamp - contextualMessageViewModel2.Timestamp) > 600000)
				{
					contextualMessageViewModel.ShowTimestamp = true;
				}
			}
			if (contextualMessageViewModel2 != null && contextualMessageViewModel3.MessageGroupState == ContextualMessageViewModel.GroupState.LastOfGroup)
			{
				contextualMessageViewModel2.MessageGroupState = ContextualMessageViewModel.GroupState.LastOfGroup;
			}
			base.RemoveItem(index);
		}

		private void AddMessage(MailMessage message, ContextualMessageViewModel vm)
		{
			ContextualMessageViewModel messageInCollection = FindMatchingMessage(vm);
			if (messageInCollection != null)
			{
				messageInCollection.UpdateMessage(message);
				if (messageInCollection.IsRegularMessage() && messageInCollection.Sender != null && messageInCollection.ServerTimestamp > 0)
				{
					int num = this.TakeWhile((ContextualMessageViewModel m) => messageInCollection.Timestamp < m.Timestamp).Count();
					if (num < Count)
					{
						GroupMessageWithSiblings(num, messageInCollection);
					}
				}
			}
			else
			{
				InsertMessage(vm);
			}
		}

		private void GroupMessageWithSiblings(int index, ContextualMessageViewModel messageInCollection)
		{
			ContextualMessageViewModel contextualMessageViewModel = null;
			bool flag = false;
			if (index + 1 < Count)
			{
				contextualMessageViewModel = base[index + 1];
				flag = ShouldMessagesBeGrouped(contextualMessageViewModel, messageInCollection);
			}
			ContextualMessageViewModel contextualMessageViewModel2 = null;
			bool flag2 = false;
			if (index - 1 < Count && index > 0)
			{
				contextualMessageViewModel2 = base[index - 1];
				flag2 = ShouldMessagesBeGrouped(contextualMessageViewModel2, messageInCollection);
			}
			if (flag && flag2)
			{
				messageInCollection.MessageGroupState = ContextualMessageViewModel.GroupState.MiddleOfGroup;
			}
			else if (flag)
			{
				GroupMessageAsLastOfGroup(messageInCollection, contextualMessageViewModel);
			}
			else if (flag2)
			{
				GroupMessageAsFirstOfGroup(messageInCollection, contextualMessageViewModel2);
			}
		}

		private void GroupMessageAsLastOfGroup(ContextualMessageViewModel message, ContextualMessageViewModel previousMessage)
		{
			message.MessageGroupState = ContextualMessageViewModel.GroupState.LastOfGroup;
			if (previousMessage != null)
			{
				if (previousMessage.MessageGroupState == ContextualMessageViewModel.GroupState.None)
				{
					previousMessage.MessageGroupState = ContextualMessageViewModel.GroupState.FirstOfGroup;
				}
				else if (previousMessage.MessageGroupState == ContextualMessageViewModel.GroupState.LastOfGroup)
				{
					previousMessage.MessageGroupState = ContextualMessageViewModel.GroupState.MiddleOfGroup;
				}
			}
		}

		private void GroupMessageAsFirstOfGroup(ContextualMessageViewModel message, ContextualMessageViewModel nextMessage)
		{
			message.MessageGroupState = ContextualMessageViewModel.GroupState.FirstOfGroup;
			if (nextMessage != null)
			{
				if (nextMessage.MessageGroupState == ContextualMessageViewModel.GroupState.None)
				{
					nextMessage.MessageGroupState = ContextualMessageViewModel.GroupState.LastOfGroup;
				}
				else if (nextMessage.MessageGroupState == ContextualMessageViewModel.GroupState.FirstOfGroup)
				{
					nextMessage.MessageGroupState = ContextualMessageViewModel.GroupState.MiddleOfGroup;
				}
			}
		}

		private ContextualMessageViewModel FindMatchingMessage(ContextualMessageViewModel message)
		{
			List<ContextualMessageViewModel> list = new List<ContextualMessageViewModel>(this);
			string id = message.Id;
			string offlineId = message.OfflineThreadingId;
			return list.Find((ContextualMessageViewModel m) => id == m.Id || (!string.IsNullOrWhiteSpace(offlineId) && offlineId == m.OfflineThreadingId));
		}

		private void UpdateMessageFromThread(ContextualMessageViewModel message)
		{
			if (Thread != null && message.Sender != null)
			{
				MailUser mailUser = Thread.Participants.FirstOrDefault((MailUser u) => u.Id == message.Sender.Id);
				if (mailUser != null)
				{
					message.Sender.IsMessengerUser = mailUser.IsMessengerUser;
				}
				if (IsCanonical)
				{
					message.IsSingleRecipient = true;
				}
			}
		}

		private bool ShouldMessagesBeGrouped(ContextualMessageViewModel message1, ContextualMessageViewModel message2)
		{
			if (message1 != null && message2 != null && message1.IsRegularMessage() && message2.IsRegularMessage() && message1.Sender != null && message2.Sender != null && message1.Sender.Id.Equals(message2.Sender.Id))
			{
				if ((message1.ServerTimestamp <= 0 || message2.ServerTimestamp <= 0 || Math.Abs(message1.ServerTimestamp - message2.ServerTimestamp) > 60000) && (message1.SendTimestamp <= 0 || message2.SendTimestamp <= 0 || Math.Abs(message1.SendTimestamp - message2.SendTimestamp) > 60000))
				{
					return Math.Abs(message1.Timestamp - message2.Timestamp) <= 60000;
				}
				return true;
			}
			return false;
		}

		private bool ShouldShowTimestamp(ContextualMessageViewModel newMessage, ContextualMessageViewModel oldMessage)
		{
			if (newMessage.IsRegularMessage())
			{
				return newMessage.Timestamp - oldMessage.Timestamp > 600000;
			}
			return false;
		}

		private void InsertMessage(ContextualMessageViewModel newMessage)
		{
			int index = this.TakeWhile((ContextualMessageViewModel m) => newMessage.Timestamp < m.Timestamp).Count();
			InsertMessage(index, newMessage);
		}

		private void InsertMessage(int index, ContextualMessageViewModel newMessage)
		{
			if (index < Count && Count != 0)
			{
				ContextualMessageViewModel oldMessage = base[index];
				bool showTimestamp = ShouldShowTimestamp(newMessage, oldMessage);
				if (index == 0)
				{
					newMessage.ShowTimestamp = showTimestamp;
				}
				else
				{
					ContextualMessageViewModel contextualMessageViewModel = base[index - 1];
					newMessage.ShowTimestamp = showTimestamp;
					contextualMessageViewModel.ShowTimestamp = ShouldShowTimestamp(contextualMessageViewModel, newMessage);
				}
				Insert(index, newMessage);
			}
			else
			{
				newMessage.ShowTimestamp = newMessage.IsRegularMessage();
				if (Count != 0)
				{
					ContextualMessageViewModel contextualMessageViewModel2 = this.Last();
					contextualMessageViewModel2.ShowTimestamp = ShouldShowTimestamp(contextualMessageViewModel2, newMessage);
				}
				Add(newMessage);
			}
			if (newMessage.IsRegularMessage() && newMessage.Sender != null && Count > 1)
			{
				GroupMessageWithSiblings(index, newMessage);
			}
		}

		private void ProcessPendingMessages()
		{
			lock (_pendingMessages)
			{
				if (!_pendingMessages.Any())
				{
					return;
				}
				foreach (MailMessage pendingMessage in _pendingMessages)
				{
					AddIncomingMessage(pendingMessage);
				}
				_pendingMessages.Clear();
				FetchUpdatedMessages().DoNotWait();
			}
		}

		private async Task<bool> FetchMessages(long timestamp = 0L)
		{
			if (_busy || string.IsNullOrWhiteSpace(Thread.Id))
			{
				return false;
			}
			_busy = true;
			if (LoadStarted != null)
			{
				LoadStarted(this, EventArgs.Empty);
			}
			Facebook.Services.Api.Methods.FetchMessages.Response response = await ExecuteFetchRequest(timestamp, 0L, !_threadHasAllInfo, _nextRequestShouldForceToServer);
			bool succeeded = !response.HasError();
			if (succeeded)
			{
				if (!_threadHasAllInfo)
				{
					Thread.UpdateThread(response.Thread);
					Thread.UpdateDisplayName(_context);
					_threadHasAllInfo = true;
				}
				ProcessResponse(response);
			}
			_busy = false;
			MessagesFetched = true;
			if (LoadFinished != null)
			{
				LoadFinished(this, response.ReceivedMessagesFromWeb, timestamp == 0, response.Error);
			}
			return succeeded;
		}

		private async Task<Facebook.Services.Api.Methods.FetchMessages.Response> ExecuteFetchRequest(long timestamp = 0L, long actionId = 0L, bool fetchAllThreadInfo = false, bool forceToServer = true)
		{
			Facebook.Services.Api.Methods.FetchMessages.Request request = new Facebook.Services.Api.Methods.FetchMessages.Request
			{
				AccessToken = _context.Session.Credentials.AccessToken,
				CancelToken = GetCancellationToken(),
				ThreadId = Thread.Id,
				MaxTimestamp = timestamp,
				ActionId = actionId,
				RecentPushActionId = _recentPushActionId,
				FetchAllThreadInfo = fetchAllThreadInfo,
				ForceToServer = forceToServer
			};
			return await _api.ExecuteAsync<Facebook.Services.Api.Methods.FetchMessages.Request, Facebook.Services.Api.Methods.FetchMessages.Response>(request);
		}

		private void ProcessResponse(Facebook.Services.Api.Methods.FetchMessages.Response response)
		{
			_nextRequestShouldForceToServer = false;
			foreach (MailMessage message in response.Messages)
			{
				ProcessMessage(message);
			}
			if (response.Messages.Count == 0)
			{
				_hasMoreItems = false;
			}
		}

		private void AnimateMostRecentSticker()
		{
			bool flag = false;
			using (IEnumerator<ContextualMessageViewModel> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ContextualMessageViewModel current = enumerator.Current;
					if (!flag && current.Shares != null && current.Shares.Any((MailShare s) => s.StickerId != null))
					{
						current.ShouldAutoAnimateSticker = true;
						flag = true;
					}
					else
					{
						current.ShouldAutoAnimateSticker = false;
					}
				}
			}
		}

		private void InitLinkedToken(CancellationToken token)
		{
			if (_localTokenSource != null)
			{
				_localTokenSource.Cancel();
			}
			_localTokenSource = new CancellationTokenSource();
			_linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_localTokenSource.Token, token);
		}

		private CancellationToken GetCancellationToken()
		{
			if (_linkedTokenSource == null)
			{
				return _localTokenSource.Token;
			}
			return _linkedTokenSource.Token;
		}
	}
}

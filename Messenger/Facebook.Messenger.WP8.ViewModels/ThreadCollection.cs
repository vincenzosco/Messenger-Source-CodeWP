using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Messenger.WP8.Controls;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchLocalThreadData;
using Facebook.Services.Api.Methods.FetchThreads;
using Facebook.Services.Localization;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ThreadCollection : ObservableCollection<ThreadViewModel>
	{
		public delegate void CollectionDelegate(ThreadCollection c);

		public delegate void FetchCollectionDelegate(ThreadCollection c, bool receivedThreadsFromWebOrSync, Error error = null);

		private bool _fetchingUpdatedThreads;

		private bool _canFetchMoreItems;

		private bool _fetchingThreads;

		private FacebookContext Context { get; set; }

		public CancellationToken Token { get; private set; }

		private Facebook.Services.Api.Service ApiService { get; set; }

		public long RecentPushActionId { get; set; }

		public bool ThreadsFetched { get; set; }

		public bool FetchingThreads
		{
			get
			{
				return _fetchingThreads;
			}
			set
			{
				_fetchingThreads = value;
				if (value)
				{
					if (FetchingThreadsStarted != null)
					{
						FetchingThreadsStarted(this, null);
					}
				}
				else if (FetchingThreadsComplete != null)
				{
					FetchingThreadsComplete(this, null);
				}
			}
		}

		public event FetchCollectionDelegate FirstPageFetched;

		public event CollectionDelegate UpdatedThreadsStarting;

		public event CollectionDelegate UpdatedThreadsFinished;

		public event EventHandler FetchingThreadsStarted;

		public event EventHandler FetchingThreadsComplete;

		public ThreadCollection(FacebookContext context, CancellationToken token, Facebook.Services.Api.Service service)
		{
			Context = context;
			ApiService = service;
			Token = token;
			_canFetchMoreItems = true;
		}

		public async Task FetchOlderThreads()
		{
			if (!FetchingThreads && _canFetchMoreItems)
			{
				FetchingThreads = true;
				long timestamp = this.Last().Timestamp;
				Facebook.Services.Api.Methods.FetchThreads.Response response = await ExecuteFetchRequest(timestamp, 0L);
				if (!response.HasError())
				{
					ProcessResponse(response);
				}
				FetchingThreads = false;
			}
		}

		public async Task FetchFirstPage()
		{
			if (!FetchingThreads)
			{
				FetchingThreads = true;
				Facebook.Services.Api.Methods.FetchThreads.Response response = await ExecuteFetchRequest(0L, 0L);
				if (response.HasError())
				{
					_canFetchMoreItems = false;
				}
				else
				{
					ProcessResponse(response);
				}
				FetchingThreads = false;
				ThreadsFetched = true;
				if (FirstPageFetched != null)
				{
					FirstPageFetched(this, response.ReceivedThreadsFromWebOrSync, response.Error);
				}
			}
		}

		public void ThreadUpdated(MailThread thread)
		{
			thread.SnippetFromMe = thread.SnippetSender != null && Context.IsActiveUser(thread.SnippetSender.Id);
			ThreadViewModel threadViewModel = this.FirstOrDefault((ThreadViewModel x) => x.IsSameThread(thread));
			if (threadViewModel == null)
			{
				if (Count == 0)
				{
					FetchFirstPage().DoNotWait();
				}
				else if (base[0].ActionId < thread.ActionId)
				{
					FetchUpdatedThreads().DoNotWait();
				}
			}
			else if (thread.Deleted || thread.Archived)
			{
				Remove(threadViewModel);
			}
			else
			{
				threadViewModel.UpdateThread(thread);
				if (Count > 0)
				{
					ArrangeThread(threadViewModel);
				}
			}
		}

		public async Task FetchUpdatedThreads()
		{
			if (Count == 0 || _fetchingUpdatedThreads)
			{
				return;
			}
			_fetchingUpdatedThreads = true;
			SignalRefreshStatusUpdate(ActionState.Started);
			if (UpdatedThreadsStarting != null)
			{
				UpdatedThreadsStarting(this);
			}
			Facebook.Services.Api.Methods.FetchThreads.Response response = await ExecuteFetchRequest(0L, this.Max((ThreadViewModel r) => r.ActionId), true);
			if (response.HasError() || response.Threads == null)
			{
				_fetchingUpdatedThreads = false;
				SignalRefreshStatusUpdate(ActionState.Failed);
				return;
			}
			if (response.ShouldPurgeThreadCache)
			{
				Clear();
			}
			foreach (MailThread thread in response.Threads)
			{
				thread.SnippetFromMe = thread.SnippetSender != null && Context.IsActiveUser(thread.SnippetSender.Id);
				ThreadViewModel matchingThread = GetMatchingThread(thread);
				bool flag = thread.Deleted || thread.Archived;
				if (flag && matchingThread != null)
				{
					Remove(matchingThread);
				}
				else if (matchingThread != null)
				{
					matchingThread.UpdateThread(thread);
					ArrangeThread(matchingThread);
				}
				else if (!flag)
				{
					InsertThread(new ThreadViewModel(thread));
				}
			}
			if (response.ShouldPurgeThreadCache)
			{
				_canFetchMoreItems = true;
			}
			MarkThreadFailureState(response.ThreadIdsWithFailedSentMessages);
			if (UpdatedThreadsFinished != null)
			{
				UpdatedThreadsFinished(this);
			}
			_fetchingUpdatedThreads = false;
			SignalRefreshStatusUpdate(response.ReceivedThreadsFromWebOrSync ? ActionState.Succeeded : ActionState.Failed);
		}

		private async Task<Facebook.Services.Api.Methods.FetchThreads.Response> ExecuteFetchRequest(long timestamp = 0L, long maxActionId = 0L, bool forceToServer = false)
		{
			Facebook.Services.Api.Methods.FetchThreads.Request request = new Facebook.Services.Api.Methods.FetchThreads.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = Token,
				MaxTimestamp = timestamp,
				RecentPushActionId = RecentPushActionId,
				ForceToServer = forceToServer,
				ActionId = maxActionId
			};
			return await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchThreads.Request, Facebook.Services.Api.Methods.FetchThreads.Response>(request);
		}

		private void ProcessResponse(Facebook.Services.Api.Methods.FetchThreads.Response response)
		{
			foreach (MailThread thread in response.Threads)
			{
				if (!thread.Deleted && !thread.Archived)
				{
					if (thread.SnippetSender != null)
					{
						thread.SnippetFromMe = Context.IsActiveUser(thread.SnippetSender.Id);
					}
					Add(new ThreadViewModel(thread));
				}
			}
			if (response.Threads.Count == 0)
			{
				_canFetchMoreItems = false;
			}
			MarkThreadFailureState(response.ThreadIdsWithFailedSentMessages);
		}

		private ThreadViewModel GetMatchingThread(MailThread thread)
		{
			return GetMatchingThread(thread.Id, thread.IsCanonical(), thread.SingleRecipient);
		}

		private ThreadViewModel GetMatchingThread(string threadId, bool isCanonical = false, string singleRecipient = null)
		{
			List<ThreadViewModel> list = new List<ThreadViewModel>(this);
			return list.Find((ThreadViewModel rowItem) => (threadId != null && threadId == rowItem.Id) || (isCanonical && singleRecipient == rowItem.SingleRecipient));
		}

		public void MarkThreadFailureState(List<string> failedIds)
		{
			if (failedIds == null)
			{
				return;
			}
			ILookup<string, string> lookup = failedIds.ToLookup((string s) => s);
			using (IEnumerator<ThreadViewModel> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThreadViewModel current = enumerator.Current;
					current.HasFailedUnsentMessages = lookup.Contains(current.Id);
				}
			}
		}

		public void MarkThreadRead(string threadId, long timestamp)
		{
			ThreadViewModel matchingThread = GetMatchingThread(threadId);
			if (matchingThread != null)
			{
				matchingThread.ReadMessageTimestamp = timestamp;
			}
		}

		public void AddMessage(MailMessage message, ThreadSnippetInfo snippet, bool isOutgoingMessage)
		{
			ThreadViewModel matchingThread = GetMatchingThread(message.ThreadId);
			if (matchingThread == null)
			{
				FetchUpdatedThreads().DoNotWait();
			}
			else if (isOutgoingMessage || (matchingThread.Timestamp < message.Timestamp && matchingThread.Snippet.SnippetTimestamp < message.Timestamp))
			{
				bool isSnippetFromMe = Context.IsActiveUser(message.Sender.Id);
				matchingThread.UpdateSnippetAndReadState(message, isSnippetFromMe, snippet);
				ArrangeThread(matchingThread);
			}
		}

		public void RemoveByThreadId(string threadId)
		{
			ThreadViewModel matchingThread = GetMatchingThread(threadId);
			if (matchingThread != null)
			{
				Remove(matchingThread);
			}
		}

		public async Task UpdateVisibleThreads()
		{
			if (Count != 0)
			{
				Facebook.Services.Api.Methods.FetchLocalThreadData.Request request = new Facebook.Services.Api.Methods.FetchLocalThreadData.Request(this.Select((ThreadViewModel rowItem) => rowItem.Id).ToList());
				Facebook.Services.Api.Methods.FetchLocalThreadData.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchLocalThreadData.Request, Facebook.Services.Api.Methods.FetchLocalThreadData.Response>(request);
				if (!response.HasError())
				{
					MarkThreadFailureState(response.ThreadIdsWithFailedUnsentMessages);
				}
			}
		}

		private void SignalRefreshStatusUpdate(ActionState state)
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(state);
		}

		public void ExecuteActionOnThread(string threadId, Action<ThreadViewModel> action)
		{
			ThreadViewModel matchingThread = GetMatchingThread(threadId);
			if (matchingThread != null)
			{
				action(matchingThread);
				matchingThread.Timestamp = DateTime.UtcNow.ToUnixTimeMilliseconds();
				ArrangeThread(matchingThread);
			}
		}

		public void ArrangeThread(ThreadViewModel threadViewModel)
		{
			if (Contains(threadViewModel))
			{
				Remove(threadViewModel);
				InsertThread(threadViewModel);
			}
		}

		private void InsertThread(ThreadViewModel threadViewModel)
		{
			int num = this.TakeWhile((ThreadViewModel rowItem) => threadViewModel.Timestamp <= rowItem.Timestamp).Count();
			if (num < Count)
			{
				Insert(num, threadViewModel);
			}
			else
			{
				Add(threadViewModel);
			}
		}
	}
}

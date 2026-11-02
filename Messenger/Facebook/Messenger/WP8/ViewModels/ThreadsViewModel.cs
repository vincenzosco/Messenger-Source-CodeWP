using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.DeleteThread;
using Facebook.Services.Api.Methods.LeaveThread;
using Facebook.Services.Api.Methods.MarkInboxSeen;
using Facebook.Services.Api.Methods.MarkThreadState;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ThreadsViewModel : FacebookMessengerViewModel
	{
		private readonly Facebook.Messenger.WP8.AudioController.Service _audioService;

		private readonly ConnectionBannerViewModel _connectionBannerViewModel;

		private RelayCommand<ListEventArgs> _listEventCommand;

		private string _emptyText;

		private ThreadCollection _threads;

		private bool _fetchingThreads;

		private RelayCommand _forcedRefresh;

		private RelayCommand<ThreadViewModel> _markThreadUnreadCommand;

		public RelayCommand<ListEventArgs> ListEventCommand => _listEventCommand ?? (_listEventCommand = new RelayCommand<ListEventArgs>(OnListEvent));

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

		public ThreadCollection Threads
		{
			get
			{
				return _threads;
			}
			set
			{
				Set(ref _threads, value, false, "Threads");
			}
		}

		public bool ThreadsFetched
		{
			get
			{
				if (Threads != null)
				{
					return Threads.ThreadsFetched;
				}
				return false;
			}
		}

		public bool FetchingThreads
		{
			get
			{
				return _fetchingThreads;
			}
			set
			{
				Set(ref _fetchingThreads, value, false, "FetchingThreads");
			}
		}

		public RelayCommand ForcedRefreshCommand => _forcedRefresh ?? (_forcedRefresh = new RelayCommand(ExecuteForcedRefresh));

		public RelayCommand<ThreadViewModel> MarkThreadUnreadCommand => _markThreadUnreadCommand ?? (_markThreadUnreadCommand = new RelayCommand<ThreadViewModel>(ExecuteMarkThreadUnreadCommand));

		public event ThreadCollection.FetchCollectionDelegate FirstPageFetched;

		public event ThreadCollection.CollectionDelegate UpdatedThreadsStarting
		{
			add
			{
				Threads.UpdatedThreadsStarting += value;
			}
			remove
			{
				Threads.UpdatedThreadsStarting -= value;
			}
		}

		public event ThreadCollection.CollectionDelegate UpdatedThreadsFinished
		{
			add
			{
				Threads.UpdatedThreadsFinished += value;
			}
			remove
			{
				Threads.UpdatedThreadsFinished -= value;
			}
		}

		public ThreadsViewModel(Facebook.Services.Api.Service apiService, Facebook.Messenger.WP8.AudioController.Service audioService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, ConnectionBannerViewModel connectionBannerViewModel)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_audioService = audioService;
			_connectionBannerViewModel = connectionBannerViewModel;
			IsLoading = true;
			InitializeThreads();
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheThreadUpdatedSignal>(this, OnCacheThreadUpdatedSignal);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheInboxDataClearedSignal>(this, OnCacheDataClearedSignal);
			Title = T("messages-title");
			Threads.FetchFirstPage().DoNotWait();
		}

		private void InitializeThreads()
		{
			Threads = new ThreadCollection(Context, CancelTokenSource.Token, ApiService);
			Threads.FirstPageFetched += Threads_FirstPageFetched;
			Threads.UpdatedThreadsFinished += Threads_OnUpdatedThreadsFinished;
			Threads.CollectionChanged += Threads_OnCollectionChanged;
			Threads.FetchingThreadsStarted += Threads_OnFetchingStarted;
			Threads.FetchingThreadsComplete += Threads_OnFetchingComplete;
		}

		private void RemoveThreadsEventHandlers()
		{
			Threads.FirstPageFetched -= Threads_FirstPageFetched;
			Threads.UpdatedThreadsFinished -= Threads_OnUpdatedThreadsFinished;
			Threads.CollectionChanged -= Threads_OnCollectionChanged;
			Threads.FetchingThreadsStarted -= Threads_OnFetchingStarted;
			Threads.FetchingThreadsComplete -= Threads_OnFetchingComplete;
		}

		private void OnCacheThreadUpdatedSignal(CacheThreadUpdatedSignal signal)
		{
			Threads.ThreadUpdated(signal.Thread);
		}

		private void OnCacheDataClearedSignal(CacheInboxDataClearedSignal obj)
		{
			if (obj.DateTypeCleared != DataTypeCleared.Messages)
			{
				CancelTokenSource.Cancel();
				CancelTokenSource = new CancellationTokenSource();
				RemoveThreadsEventHandlers();
				InitializeThreads();
				IsLoading = true;
				EmptyText = string.Empty;
				Threads.FetchFirstPage().DoNotWait();
			}
		}

		public override void Cleanup()
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
			RemoveThreadsEventHandlers();
			base.Cleanup();
		}

		public override void OnApplicationResumed()
		{
			base.OnApplicationResumed();
			ExecuteForcedRefresh(false);
		}

		private void OnListEvent(ListEventArgs e)
		{
			if (e.ListEvent == ListEvent.ScrolledToBottom && Threads != null && Threads.Any())
			{
				Threads.FetchOlderThreads().DoNotWait();
			}
		}

		private void ExecuteForcedRefresh()
		{
			_connectionBannerViewModel.DisplayCurrentBannerState();
		}

		private async void ExecuteForcedRefresh(bool playSound)
		{
			if (IsLoading || Threads == null)
			{
				return;
			}
			IsLoading = true;
			if (Threads.Count == 0)
			{
				Threads.FetchFirstPage().DoNotWait();
			}
			else
			{
				if (playSound)
				{
					_audioService.PlayRefreshStart();
				}
				await Threads.FetchUpdatedThreads();
				if (playSound)
				{
					_audioService.PlayRefreshEnd();
				}
			}
			IsLoading = false;
		}

		private void SetEmptyThread(EmptyThreadType type)
		{
			switch (type)
			{
			case EmptyThreadType.None:
				EmptyText = string.Empty;
				break;
			case EmptyThreadType.NoThreads:
				EmptyText = (Context.IsPartialUser() ? StringLibrary.RecentThreadList_Nux_EmptyList_Message : StringLibrary.Error_No_Conversations);
				break;
			case EmptyThreadType.NoConnection:
				EmptyText = (CheckEmptyThreads() ? StringLibrary.Error_No_Connection : string.Empty);
				break;
			case EmptyThreadType.ServerError:
				EmptyText = (CheckEmptyThreads() ? StringLibrary.Error_Cannot_Fetch_Conversations : string.Empty);
				break;
			}
		}

		private void Threads_FirstPageFetched(ThreadCollection threads, bool receivedThreadsFromWebOrSync, Error error)
		{
			IsLoading = false;
			MarkInboxSeen();
			if (CheckEmptyThreads())
			{
				EmptyThreadType emptyThread = EmptyThreadType.NoThreads;
				if (!HasConnection)
				{
					emptyThread = EmptyThreadType.NoConnection;
				}
				else if (error != null)
				{
					emptyThread = EmptyThreadType.ServerError;
				}
				SetEmptyThread(emptyThread);
			}
			else
			{
				SetEmptyThread(EmptyThreadType.None);
			}
			if (FirstPageFetched != null)
			{
				FirstPageFetched(threads, receivedThreadsFromWebOrSync, error);
			}
			if (!receivedThreadsFromWebOrSync)
			{
				Dispatcher.Schedule(() =>
				{
					Threads.FetchUpdatedThreads().DoNotWait();
				});
			}
		}

		private void MarkInboxSeen()
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(JewelType.Messages);
			Facebook.Services.Api.Methods.MarkInboxSeen.Request request = new Facebook.Services.Api.Methods.MarkInboxSeen.Request();
			request.AccessToken = Context.Session.Credentials.AccessToken;
			Facebook.Services.Api.Methods.MarkInboxSeen.Request request2 = request;
			ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MarkInboxSeen.Request, BoolResponse>(request2).DoNotWait();
		}

		private void Threads_OnUpdatedThreadsFinished(ThreadCollection threadCollection)
		{
			SetEmptyThread(CheckEmptyThreads() ? EmptyThreadType.NoThreads : EmptyThreadType.None);
		}

		private void Threads_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			SetEmptyThread(CheckEmptyThreads() ? EmptyThreadType.NoThreads : EmptyThreadType.None);
		}

		private void Threads_OnFetchingStarted(object sender, EventArgs e)
		{
			FetchingThreads = true;
		}

		private void Threads_OnFetchingComplete(object sender, EventArgs e)
		{
			FetchingThreads = false;
		}

		private bool CheckEmptyThreads()
		{
			if (Threads != null)
			{
				return !Threads.Any();
			}
			return true;
		}

		public void DeleteThread(string threadId)
		{
			Threads.RemoveByThreadId(threadId);
			Facebook.Services.Api.Methods.DeleteThread.Request request = new Facebook.Services.Api.Methods.DeleteThread.Request(threadId);
			request.AccessToken = Context.Session.Credentials.AccessToken;
			Facebook.Services.Api.Methods.DeleteThread.Request request2 = request;
			ApiService.ExecuteAsync<Facebook.Services.Api.Methods.DeleteThread.Request, Facebook.Base.Response>(request2).DoNotWait();
		}

		public void ArchiveThread(string threadId)
		{
			Threads.RemoveByThreadId(threadId);
			Facebook.Services.Api.Methods.MarkThreadState.Request request = new Facebook.Services.Api.Methods.MarkThreadState.Request();
			request.AccessToken = Context.Session.Credentials.AccessToken;
			request.ThreadId = threadId;
			request.Enabled = true;
			request.Tag = MailThread.Tag.Archived;
			Facebook.Services.Api.Methods.MarkThreadState.Request request2 = request;
			ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MarkThreadState.Request, Facebook.Services.Api.Methods.MarkThreadState.Response>(request2).DoNotWait();
		}

		public void SpamThread(string threadId)
		{
			Threads.RemoveByThreadId(threadId);
			Facebook.Services.Api.Methods.MarkThreadState.Request request = new Facebook.Services.Api.Methods.MarkThreadState.Request();
			request.AccessToken = Context.Session.Credentials.AccessToken;
			request.ThreadId = threadId;
			request.Enabled = true;
			request.Tag = MailThread.Tag.Spam;
			Facebook.Services.Api.Methods.MarkThreadState.Request request2 = request;
			ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MarkThreadState.Request, Facebook.Services.Api.Methods.MarkThreadState.Response>(request2).DoNotWait();
		}

		public void LeaveThread(string threadId)
		{
			Threads.RemoveByThreadId(threadId);
			Facebook.Services.Api.Methods.LeaveThread.Request request = new Facebook.Services.Api.Methods.LeaveThread.Request();
			request.AccessToken = Context.Session.Credentials.AccessToken;
			request.ThreadId = threadId;
			Facebook.Services.Api.Methods.LeaveThread.Request request2 = request;
			ApiService.ExecuteAsync<Facebook.Services.Api.Methods.LeaveThread.Request, BoolResponse>(request2).DoNotWait();
		}

		public async Task TurnOffMute(ThreadViewModel thread)
		{
			if ((await MuteHelper.SetThreadMuteState(MuteDuration.Off, Context, ApiService, thread.Id)).HasError())
			{
				OnError(new Error
				{
					Message = StringLibrary.Error_Changing_Unmute
				});
			}
		}

		public void UpdateVisibleThreads()
		{
			Threads.UpdateVisibleThreads().DoNotWait();
		}

		private void ExecuteMarkThreadUnreadCommand(ThreadViewModel thread)
		{
			Facebook.Services.Api.Methods.MarkThreadState.Request request = new Facebook.Services.Api.Methods.MarkThreadState.Request();
			request.AccessToken = Context.Session.Credentials.AccessToken;
			request.ThreadId = thread.Id;
			request.Enabled = false;
			request.Tag = MailThread.Tag.Read;
			Facebook.Services.Api.Methods.MarkThreadState.Request request2 = request;
			ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MarkThreadState.Request, Facebook.Services.Api.Methods.MarkThreadState.Response>(request2).DoNotWait();
		}
	}
}

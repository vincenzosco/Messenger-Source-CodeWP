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
using Facebook.Base.Network;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.AudioController;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchPinnedThreads;
using Facebook.Services.Api.Methods.FetchThread;
using Facebook.Services.Api.Methods.SetPinnedThreads;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class PinnedThreadsViewModel : FacebookMessengerViewModel
	{
		private const int StaleIntervalMs = 1800000;

		private bool _isRearranging;

		private ObservableCollection<ThreadViewModel> _pinnedThreads;

		private bool _showGroupsNux;

		private readonly Facebook.Messenger.WP8.AudioController.Service _audioService;

		private CancellationTokenSource _saveOrderTokenSource;

		private List<string> _lastAuthoritativeOrder;

		private bool _isThreadOrderDirty;

		public bool IsRearranging
		{
			get
			{
				return _isRearranging;
			}
			set
			{
				Set(ref _isRearranging, value, false, "IsRearranging");
			}
		}

		public ObservableCollection<ThreadViewModel> PinsToAdd { get; set; }

		public ObservableCollection<ThreadViewModel> PinnedThreads
		{
			get
			{
				return _pinnedThreads;
			}
			set
			{
				if (_pinnedThreads != null)
				{
					_pinnedThreads.CollectionChanged -= PinnedThreadsOnCollectionChanged;
				}
				Set(ref _pinnedThreads, value, false, "PinnedThreads");
				if (value != null)
				{
					value.CollectionChanged += PinnedThreadsOnCollectionChanged;
				}
			}
		}

		public bool ShowGroupsNux
		{
			get
			{
				return _showGroupsNux;
			}
			set
			{
				Set(ref _showGroupsNux, value, false, "ShowGroupsNux");
			}
		}

		public PinnedThreadsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, Facebook.Messenger.WP8.AudioController.Service audioService)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_audioService = audioService;
			_saveOrderTokenSource = new CancellationTokenSource();
			PinnedThreads = new ObservableCollection<ThreadViewModel>();
			PinsToAdd = new ObservableCollection<ThreadViewModel>();
			PropertyChanged += OnPropertyChanged;
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<GroupPinnedSignal>(this, OnGroupPinned);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CachePinnedThreadUpdatedSignal>(this, OnCachePinnedThreadUpdatedSignal);
		}

		private void PinnedThreadsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Move)
			{
				_isThreadOrderDirty = true;
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset || e.Action == NotifyCollectionChangedAction.Add)
			{
				UpdateShowGroupNux();
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "IsRearranging" && !IsRearranging && _isThreadOrderDirty)
			{
				_isThreadOrderDirty = false;
				List<string> newOrder = PinnedThreads.Select((ThreadViewModel t) => t.Id).ToList();
				SavePinnedThreadOrder(newOrder).DoNotWait();
			}
		}

		private async void OnGroupPinned(GroupPinnedSignal signal)
		{
			Facebook.Services.Api.Methods.FetchThread.Request request = Facebook.Services.Api.Methods.FetchThread.Request.ByThreadId(signal.ThreadId);
			request.AccessToken = Context.Session.Credentials.AccessToken;
			request.CancelToken = CancelTokenSource.Token;
			Facebook.Services.Api.Methods.FetchThread.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchThread.Request, Facebook.Services.Api.Methods.FetchThread.Response>(request);
			if (!response.HasError() && response.Thread != null)
			{
				PinsToAdd.Add(new ThreadViewModel(response.Thread));
			}
		}

		private void OnCachePinnedThreadUpdatedSignal(CachePinnedThreadUpdatedSignal signal)
		{
			PinnedThread thread = signal.Thread;
			ThreadViewModel threadViewModel = PinnedThreads.FirstOrDefault((ThreadViewModel x) => x.IsSameThread(thread));
			if (threadViewModel != null)
			{
				if (thread.Deleted || thread.Archived)
				{
					PinnedThreads.Remove(threadViewModel);
				}
				else
				{
					threadViewModel.UpdateThread(signal.Thread);
				}
			}
		}

		public async Task FetchPinnedThreads()
		{
			if (IsLoading)
			{
				return;
			}
			IsLoading = true;
			Facebook.Services.Api.Methods.FetchPinnedThreads.Request request = new Facebook.Services.Api.Methods.FetchPinnedThreads.Request
			{
				AccessToken = Context.Session.Credentials.AccessToken,
				CancelToken = CancelTokenSource.Token
			};
			long lastSync = Context.GetLastPinnedGroupSync();
			long now = DateTime.UtcNow.ToUnixTimeMilliseconds();
			if (Math.Abs(lastSync - now) > 1800000)
			{
				request.ForceToServer = true;
			}
			Facebook.Services.Api.Methods.FetchPinnedThreads.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchPinnedThreads.Request, Facebook.Services.Api.Methods.FetchPinnedThreads.Response>(request);
			if (!response.HasError())
			{
				_lastAuthoritativeOrder = response.PinnedThreads.Select((PinnedThread t) => t.Id).ToList();
				IEnumerable<ThreadViewModel> collection = response.PinnedThreads.Select((PinnedThread t) => new ThreadViewModel(t));
				PinnedThreads = new ObservableCollection<ThreadViewModel>(collection);
				if (response.ResultWasPersisted)
				{
					Context.SetLastPinnedGroupSync(DateTime.UtcNow.ToUnixTimeMilliseconds());
				}
			}
			UpdateShowGroupNux();
			IsLoading = false;
		}

		private void UpdateShowGroupNux()
		{
			bool flag = !HasPinnedThreads();
			if (flag == ShowGroupsNux)
			{
				RaisePropertyChanged("ShowGroupsNux");
			}
			else
			{
				ShowGroupsNux = flag;
			}
		}

		public async Task SavePinnedThreadOrder(List<string> newOrder, List<string> idsToRemove = null)
		{
			if (_lastAuthoritativeOrder != null)
			{
				_saveOrderTokenSource.Cancel();
				_saveOrderTokenSource = new CancellationTokenSource();
				CancellationToken requestToken = _saveOrderTokenSource.Token;
				Facebook.Services.Api.Methods.SetPinnedThreads.Request request = new Facebook.Services.Api.Methods.SetPinnedThreads.Request
				{
					AccessToken = Context.Session.Credentials.AccessToken,
					OrderedThreadIds = newOrder,
					RemovedThreadIds = idsToRemove
				};
				Facebook.Services.Api.Methods.SetPinnedThreads.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SetPinnedThreads.Request, Facebook.Services.Api.Methods.SetPinnedThreads.Response>(request);
				if ((!response.HasError() || requestToken.IsCancellationRequested) && !response.HasError())
				{
					_lastAuthoritativeOrder = newOrder;
				}
			}
		}

		public void PlayCardLandedSound()
		{
			_audioService.PlayCardLanded();
		}

		public void PlayCardPickedUpSound()
		{
			_audioService.PlayCardPickedUp();
		}

		public void PlayCardSlide1Sound()
		{
			_audioService.PlayCardSlide1();
		}

		public void PlayCardSlide2Sound()
		{
			_audioService.PlayCardSlide2();
		}

		public void PlayNewGroupAppearingSound()
		{
			_audioService.PlayNewGroupAppearing();
		}

		public void PlayNewGroupBounceSound()
		{
			_audioService.PlayNewGroupBounce();
		}

		public void PlayRemovingPinSound()
		{
			_audioService.PlayRemovingPin();
		}

		public bool HasPinnedThreads()
		{
			if (PinnedThreads != null)
			{
				return PinnedThreads.Any();
			}
			return false;
		}

		public bool HasPinnedThreadsToAdd()
		{
			if (PinsToAdd != null)
			{
				return PinsToAdd.Any();
			}
			return false;
		}

		public void DismissNux()
		{
			ShowGroupsNux = false;
		}

		public void OpenGroupComposer()
		{
			NavigateCommand.Execute(new NavigationEventArgs(FacebookUri.GetAppUri(UriId.GroupComposer)));
		}

		public void UnpinThread(ThreadViewModel thread)
		{
			if (thread != null)
			{
				List<string> list = new List<string>();
				list.Add(thread.Id);
				List<string> idsToRemove = list;
				List<string> newOrder = PinnedThreads.Select((ThreadViewModel t) => t.Id).ToList();
				SavePinnedThreadOrder(newOrder, idsToRemove).DoNotWait();
			}
		}
	}
}

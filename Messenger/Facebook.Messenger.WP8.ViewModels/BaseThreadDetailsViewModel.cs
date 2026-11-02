using System;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchThread;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public abstract class BaseThreadDetailsViewModel : FacebookMessengerViewModel
	{
		public const string ThreadIdParameter = "thread_id";

		public const string ThreadPropertyName = "Thread";

		public const string IsMuteInProgressPropertyName = "IsMuteInProgress";

		private string _threadId;

		private ThreadViewModel _thread;

		private bool _muteInProgress;

		private RelayCommand _muteCommand;

		private RelayCommand _unmuteCommand;

		private RelayCommand _fetchThreadCommand;

		public ThreadViewModel Thread
		{
			get
			{
				return _thread;
			}
			set
			{
				if (Set(ref _thread, value, false, "Thread"))
				{
					OnThreadUpdated();
				}
			}
		}

		public bool IsMuteInProgress
		{
			get
			{
				return _muteInProgress;
			}
			set
			{
				Set(ref _muteInProgress, value, false, "IsMuteInProgress");
			}
		}

		public RelayCommand MuteCommand
		{
			get
			{
				return _muteCommand ?? (_muteCommand = new RelayCommand(ExecuteMuteCommand));
			}
		}

		public RelayCommand UnmuteCommand
		{
			get
			{
				return _unmuteCommand ?? (_unmuteCommand = new RelayCommand(ExecuteUnmuteCommand));
			}
		}

		public RelayCommand FetchThreadCommand
		{
			get
			{
				return _fetchThreadCommand ?? (_fetchThreadCommand = new RelayCommand(ExecuteFetchThreadCommand));
			}
		}

		protected BaseThreadDetailsViewModel(Facebook.Services.Api.Service apiService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheThreadUpdatedSignal>(this, OnCacheThreadUpdatedSignal);
		}

		private void ExecuteMuteCommand()
		{
			if (Thread != null)
			{
				Navigator.Navigate(FacebookUri.MuteSelectionUri, Thread);
			}
		}

		private async void ExecuteUnmuteCommand()
		{
			if (Thread != null)
			{
				IsMuteInProgress = true;
				Facebook.Base.Response response = await MuteHelper.SetThreadMuteState(MuteDuration.Off, Context, ApiService, Thread.Id);
				IsMuteInProgress = false;
				if (response.HasError())
				{
					OnError(new Error
					{
						Message = StringLibrary.Error_Changing_Unmute
					});
				}
			}
		}

		public async void ExecuteFetchThreadCommand()
		{
			IsLoading = true;
			Facebook.Services.Api.Methods.FetchThread.Request fetchThreadRequest = Facebook.Services.Api.Methods.FetchThread.Request.ByThreadId(_threadId);
			fetchThreadRequest.AccessToken = Context.Session.Credentials.AccessToken;
			fetchThreadRequest.CancelToken = CancelTokenSource.Token;
			Facebook.Services.Api.Methods.FetchThread.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchThread.Request, Facebook.Services.Api.Methods.FetchThread.Response>(fetchThreadRequest);
			if (!response.HasError())
			{
				Thread = new ThreadViewModel(response.Thread);
			}
			IsLoading = false;
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("thread_id"))
			{
				_threadId = PathUri.Params["thread_id"];
			}
		}

		protected abstract void OnThreadUpdated();

		private void OnCacheThreadUpdatedSignal(CacheThreadUpdatedSignal signal)
		{
			if (Thread != null && Thread.IsSameThread(signal.Thread))
			{
				Thread.UpdateThread(signal.Thread);
				RaisePropertyChanged("Thread");
				OnThreadUpdated();
			}
		}
	}
}

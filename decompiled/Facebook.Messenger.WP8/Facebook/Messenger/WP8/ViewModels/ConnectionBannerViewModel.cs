using System;
using Facebook.Base.Mqtt;
using Facebook.Base.Network;
using Facebook.Models;
using Facebook.Models.Signals;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ConnectionBannerViewModel : ViewModelBase
	{
		public const string PropertyNameBannerState = "BannerState";

		private readonly FacebookContext _context;

		private readonly INetworkStatus _networkStatus;

		private SyncConnectionState _syncState;

		private BannerState _bannerState;

		public BannerState BannerState
		{
			get
			{
				return _bannerState;
			}
			private set
			{
				Set(ref _bannerState, value, false, "BannerState");
			}
		}

		public event EventHandler RefreshStateTriggered;

		public ConnectionBannerViewModel(FacebookContext context, INetworkStatus networkStatus)
		{
			_context = context;
			_networkStatus = networkStatus;
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<SyncConnectionStateChangedSignal>(this, OnSyncStateChanged);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<LogoutSignal>(this, OnLogoutSignal);
			BannerState = ((!_networkStatus.IsConnected() && _context.IsAuthenticated()) ? BannerState.NoInternet : BannerState.Hidden);
			_networkStatus.AddHandler(OnNetworkStatusChanged);
		}

		public void DisplayCurrentBannerState()
		{
			if (_syncState == SyncConnectionState.Connected && RefreshStateTriggered != null)
			{
				RefreshStateTriggered(this, null);
			}
		}

		private void OnSyncStateChanged(SyncConnectionStateChangedSignal signal)
		{
			_syncState = signal.State;
			if (!_context.IsAuthenticated())
			{
				BannerState = BannerState.Hidden;
				return;
			}
			switch (_syncState)
			{
			case SyncConnectionState.Connecting:
				BannerState = BannerState.Connecting;
				break;
			case SyncConnectionState.Connected:
				BannerState = BannerState.Connected;
				break;
			case SyncConnectionState.Disconnected:
				BannerState = (_networkStatus.IsConnected() ? BannerState.Disconnected : BannerState.NoInternet);
				break;
			case SyncConnectionState.Syncing:
				BannerState = BannerState.Syncing;
				break;
			default:
				throw new ArgumentOutOfRangeException("Unexpected SyncConnectionState: " + _syncState);
			}
		}

		private void OnNetworkStatusChanged(INetworkStatus sender, bool connected)
		{
			if (_context.IsAuthenticated() && !_networkStatus.IsConnected())
			{
				BannerState = BannerState.NoInternet;
			}
		}

		private void OnLogoutSignal(LogoutSignal signal)
		{
			BannerState = BannerState.Hidden;
		}
	}
}

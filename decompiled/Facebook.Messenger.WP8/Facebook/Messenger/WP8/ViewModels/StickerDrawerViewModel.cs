using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.AddRecentSticker;
using Facebook.Services.Api.Methods.DownloadStickerPack;
using Facebook.Services.Api.Methods.FetchRecentStickers;
using Facebook.Services.Api.Methods.FetchStickerPacks;
using Facebook.Services.Common;
using Facebook.Services.Stickers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class StickerDrawerViewModel : ViewModelBase, IStickerPackProgressHandler
	{
		public const string IsLoadingPropertyName = "IsLoading";

		public const string DisplayNewStickerPacksBadgePropertyName = "DisplayNewStickerPacksBadge";

		private readonly Navigator _navigator;

		private readonly Facebook.Services.Stickers.Service _stickersService;

		private readonly IStickerStorage _stickerStorage;

		private readonly FacebookContext _context;

		private readonly StickerDrawerBadgeEnabledProvider _stickerDrawerBadgeEnabledProvider;

		private readonly StickerPackCollection _stickerPacks;

		private bool _isLoading;

		private bool _displayNewStickerPacksBadge;

		private RelayCommand _openStickerStoreCommand;

		private RelayCommand<StickerPackViewModel> _downloadStickerPackCommand;

		private RelayCommand<StickerViewModel> _sendStickerCommand;

		public StickerPackCollection StickerPacks => _stickerPacks;

		public bool IsLoading
		{
			get
			{
				return _isLoading;
			}
			private set
			{
				Set(ref _isLoading, value, false, "IsLoading");
			}
		}

		public bool DisplayNewStickerPacksBadge
		{
			get
			{
				return _displayNewStickerPacksBadge;
			}
			private set
			{
				Set(ref _displayNewStickerPacksBadge, value, false, "DisplayNewStickerPacksBadge");
			}
		}

		public RelayCommand OpenStickerStoreCommand => _openStickerStoreCommand ?? (_openStickerStoreCommand = new RelayCommand(ExecuteOpenStickerStoreCommand));

		public RelayCommand<StickerPackViewModel> DownloadStickerPackCommand => _downloadStickerPackCommand ?? (_downloadStickerPackCommand = new RelayCommand<StickerPackViewModel>(ExecuteDownloadStickerPack));

		public RelayCommand<StickerViewModel> SendStickerCommand => _sendStickerCommand ?? (_sendStickerCommand = new RelayCommand<StickerViewModel>(ExecuteSendStickerCommand));

		public event EventHandler<StickerSelectedArgs> StickerSelected;

		public event EventHandler StickerPacksUpdated;

		public StickerDrawerViewModel(FacebookContext context, Navigator navigator, Facebook.Services.Stickers.Service stickersService, StickerDrawerBadgeEnabledProvider stickerDrawerBadgeEnabledProvider, IStickerStorage stickerStorage)
		{
			_context = context;
			_navigator = navigator;
			_stickersService = stickersService;
			_stickerStorage = stickerStorage;
			_stickerPacks = new StickerPackCollection(_stickerStorage);
			_stickerDrawerBadgeEnabledProvider = stickerDrawerBadgeEnabledProvider;
			_stickersService.AddProgressHandler(this);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheStickerPacksUpdatedSignal>(this, OnCacheStickerPacksUpdatedSignal);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheRecentStickersUpdatedSignal>(this, OnRecentStickerUpdatedSignal);
		}

		public override void Cleanup()
		{
			_stickersService.RemoveProgressHandler(this);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
			base.Cleanup();
		}

		private void ExecuteOpenStickerStoreCommand()
		{
			_navigator.Navigate(FacebookUri.StickerStoreUri);
		}

		private async void ExecuteDownloadStickerPack(StickerPackViewModel stickerPack)
		{
			if (stickerPack.State != StickerPackViewModel.DownloadState.Downloading && stickerPack.State != StickerPackViewModel.DownloadState.Downloaded)
			{
				stickerPack.State = StickerPackViewModel.DownloadState.Downloading;
				Facebook.Services.Api.Methods.DownloadStickerPack.Request request = new Facebook.Services.Api.Methods.DownloadStickerPack.Request
				{
					StickerPackId = stickerPack.Id
				};
				BoolResponse response = await _stickersService.ExecuteAsync<Facebook.Services.Api.Methods.DownloadStickerPack.Request, BoolResponse>(request);
				bool succeeded = !response.HasError() && response.Succeeded;
				stickerPack.State = (succeeded ? StickerPackViewModel.DownloadState.Downloaded : StickerPackViewModel.DownloadState.Failed);
			}
		}

		private void ExecuteSendStickerCommand(StickerViewModel sticker)
		{
			if (StickerSelected != null)
			{
				string stickerUrl = ((sticker.ImageUri != null) ? sticker.ImageUri.AbsoluteUri : string.Empty);
				StickerSelectedArgs e = new StickerSelectedArgs(sticker.Id, stickerUrl);
				StickerSelected(this, e);
			}
			Facebook.Services.Api.Methods.AddRecentSticker.Request request = new Facebook.Services.Api.Methods.AddRecentSticker.Request();
			request.StickerId = sticker.Id;
			Facebook.Services.Api.Methods.AddRecentSticker.Request request2 = request;
			_stickersService.ExecuteAsync<Facebook.Services.Api.Methods.AddRecentSticker.Request, BoolResponse>(request2).DoNotWait();
		}

		public async Task FetchStickerPacks()
		{
			if (IsLoading)
			{
				return;
			}
			try
			{
				IsLoading = true;
				Facebook.Services.Api.Methods.FetchStickerPacks.Request request = new Facebook.Services.Api.Methods.FetchStickerPacks.Request
				{
					StickerPackType = StickerPackType.Tray
				};
				Facebook.Services.Api.Methods.FetchStickerPacks.Response response = await _stickersService.ExecuteAsync<Facebook.Services.Api.Methods.FetchStickerPacks.Request, Facebook.Services.Api.Methods.FetchStickerPacks.Response>(request);
				if (response.HasError())
				{
					Logger.Warning(this, $"Failed to fetch Tray Sticker Packs. {response}", "FetchStickerPacks", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\StickerDrawerViewModel.cs", 180);
					return;
				}
				if (response.StickerPacks.Count > 0)
				{
					StickerPacks.AddStickerPacks(response.StickerPacks, response.DownloadingStickerPackIds);
				}
				if (response.RecentStickers.Count > 0)
				{
					StickerPacks.UpdateRecentStickers(response.RecentStickers);
				}
				InvokeStickerPacksUpdated();
			}
			finally
			{
				IsLoading = false;
			}
		}

		public void FetchHasNewStickerPacks()
		{
			DisplayNewStickerPacksBadge = _stickerDrawerBadgeEnabledProvider.Enabled && _context.GetHasNewStickerPacks();
		}

		public void DownloadStarted(string stickerPackId)
		{
			StickerPacks.UpdateStickerPackWithId(stickerPackId, (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Downloading;
			});
		}

		public void DownloadFailed(string stickerPackId)
		{
			StickerPacks.UpdateStickerPackWithId(stickerPackId, (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Failed;
			});
		}

		public void ProgressUpdated(string stickerPackId, double progress)
		{
			StickerPacks.UpdateStickerPackWithId(stickerPackId, (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Downloading;
			});
			StickerPacks.UpdateStickerPackWithId(stickerPackId, (StickerPackViewModel sp) =>
			{
				sp.DownloadProgress = progress;
			});
		}

		private void OnCacheStickerPacksUpdatedSignal(CacheStickerPacksUpdatedSignal signal)
		{
			if (!IsLoading && signal.StickerPackType == StickerPackType.Tray)
			{
				switch (signal.State)
				{
				case CacheStickerPacksUpdatedSignal.StickerPackState.Added:
					UpdateStickerPacksForAdded(signal.StickerPacks);
					break;
				case CacheStickerPacksUpdatedSignal.StickerPackState.Downloaded:
					UpdateStickerPacksForDownloaded(signal.StickerPacks);
					break;
				case CacheStickerPacksUpdatedSignal.StickerPackState.Deleted:
					UpdateStickerPacksForDeleted(signal.StickerPacks);
					break;
				}
			}
		}

		private async void OnRecentStickerUpdatedSignal(CacheRecentStickersUpdatedSignal signal)
		{
			Facebook.Services.Api.Methods.FetchRecentStickers.Request request = new Facebook.Services.Api.Methods.FetchRecentStickers.Request();
			Facebook.Services.Api.Methods.FetchRecentStickers.Response response = await _stickersService.ExecuteAsync<Facebook.Services.Api.Methods.FetchRecentStickers.Request, Facebook.Services.Api.Methods.FetchRecentStickers.Response>(request);
			if (!response.HasError())
			{
				StickerPacks.UpdateRecentStickers(response.RecentStickers);
			}
		}

		private void UpdateStickerPacksForAdded(IEnumerable<StickerPack> addedStickerPacks)
		{
			IEnumerable<StickerPackViewModel> stickerPacks = addedStickerPacks.Select((StickerPack p) => new StickerPackViewModel(p, _stickerStorage));
			StickerPacks.AddStickerPacks(stickerPacks);
			InvokeStickerPacksUpdated();
		}

		private void UpdateStickerPacksForDownloaded(IEnumerable<StickerPack> downloadedStickerPacks)
		{
			foreach (StickerPack downloadedStickerPack in downloadedStickerPacks)
			{
				StickerPacks.UpdateStickerPackWithId(downloadedStickerPack.Id, (StickerPackViewModel sp) =>
				{
					sp.State = StickerPackViewModel.DownloadState.Downloaded;
				});
			}
		}

		private void UpdateStickerPacksForDeleted(IEnumerable<StickerPack> deletedStickerPacks)
		{
			foreach (StickerPack deletedStickerPack in deletedStickerPacks)
			{
				StickerPacks.RemoveStickerPackWithId(deletedStickerPack.Id);
			}
			InvokeStickerPacksUpdated();
		}

		private void InvokeStickerPacksUpdated()
		{
			if (StickerPacksUpdated != null)
			{
				StickerPacksUpdated(this, null);
			}
		}
	}
}

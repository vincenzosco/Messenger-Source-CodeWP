using System;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.DeleteStickerPack;
using Facebook.Services.Api.Methods.DownloadStickerPack;
using Facebook.Services.Api.Methods.FetchStickerPack;
using Facebook.Services.Common;
using Facebook.Services.Stickers;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class StickerStorePackViewModel : FacebookMessengerViewModel, IStickerPackProgressHandler
	{
		private readonly Facebook.Services.Stickers.Service _stickersService;

		private readonly IStickerStorage _stickerStorage;

		private string _stickerPackId;

		private StickerPackViewModel _stickerPack;

		public StickerPackViewModel StickerPack
		{
			get
			{
				return _stickerPack;
			}
			set
			{
				Set(ref _stickerPack, value, false, "StickerPack");
			}
		}

		public StickerStorePackViewModel(Facebook.Services.Api.Service apiService, Facebook.Services.Stickers.Service stickersService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, IStickerStorage stickerStorage)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_stickersService = stickersService;
			_stickerStorage = stickerStorage;
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheStickerPacksUpdatedSignal>(this, OnCacheStickerPackUpdatedSignal);
			_stickersService.AddProgressHandler(this);
		}

		public override void Cleanup()
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
			_stickersService.RemoveProgressHandler(this);
			base.Cleanup();
		}

		public override void SetPath(Uri uri)
		{
			base.SetPath(uri);
			if (PathUri.Params.ContainsKey("id"))
			{
				_stickerPackId = PathUri.Params["id"];
			}
		}

		public async Task FetchStickerPack()
		{
			if (IsLoading)
			{
				return;
			}
			try
			{
				IsLoading = true;
				Facebook.Services.Api.Methods.FetchStickerPack.Request request = new Facebook.Services.Api.Methods.FetchStickerPack.Request
				{
					StickerPackId = _stickerPackId
				};
				Facebook.Services.Api.Methods.FetchStickerPack.Response response = await _stickersService.ExecuteAsync<Facebook.Services.Api.Methods.FetchStickerPack.Request, Facebook.Services.Api.Methods.FetchStickerPack.Response>(request);
				if (response.HasError())
				{
					Logger.Warning(this, string.Format("Failed to fetch sticker packs. {0}", response), "FetchStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\ViewModels\\StickerStorePackViewModel.cs", 86);
					return;
				}
				StickerPack = new StickerPackViewModel(response.StickerPack, _stickerStorage);
				if (response.StickerPack.IsDownloaded)
				{
					StickerPack.State = StickerPackViewModel.DownloadState.Downloaded;
				}
				else if (response.IsDownloading)
				{
					StickerPack.State = StickerPackViewModel.DownloadState.Downloading;
				}
			}
			finally
			{
				IsLoading = false;
			}
		}

		public async Task DownloadStickerPack()
		{
			if (IsLoading || StickerPack == null)
			{
				return;
			}
			try
			{
				IsLoading = true;
				StickerPack.State = StickerPackViewModel.DownloadState.Downloading;
				Facebook.Services.Api.Methods.DownloadStickerPack.Request request = new Facebook.Services.Api.Methods.DownloadStickerPack.Request
				{
					StickerPackId = _stickerPackId
				};
				BoolResponse response = await _stickersService.ExecuteAsync<Facebook.Services.Api.Methods.DownloadStickerPack.Request, BoolResponse>(request);
				StickerPack.State = ((!response.HasError() && response.Succeeded) ? StickerPackViewModel.DownloadState.Downloaded : StickerPackViewModel.DownloadState.Failed);
			}
			finally
			{
				IsLoading = false;
			}
		}

		public async Task<bool> DeleteStickerPack()
		{
			if (IsLoading || StickerPack == null)
			{
				return true;
			}
			try
			{
				IsLoading = true;
				StickerPack.State = StickerPackViewModel.DownloadState.Available;
				Facebook.Services.Api.Methods.DeleteStickerPack.Request request = new Facebook.Services.Api.Methods.DeleteStickerPack.Request
				{
					StickerPackId = StickerPack.Id
				};
				BoolResponse response = await _stickersService.ExecuteAsync<Facebook.Services.Api.Methods.DeleteStickerPack.Request, BoolResponse>(request);
				if (response.HasError() || !response.Succeeded)
				{
					StickerPack.State = StickerPackViewModel.DownloadState.Downloaded;
					return false;
				}
				StickerPack.DownloadProgress = 0.0;
				return true;
			}
			finally
			{
				IsLoading = false;
			}
		}

		public void DownloadStarted(string stickerPackId)
		{
			if (!(stickerPackId != _stickerPackId))
			{
				StickerPack.State = StickerPackViewModel.DownloadState.Downloading;
			}
		}

		public void DownloadFailed(string stickerPackId)
		{
			if (!(stickerPackId != _stickerPackId))
			{
				StickerPack.State = StickerPackViewModel.DownloadState.Failed;
			}
		}

		public void ProgressUpdated(string stickerPackId, double progress)
		{
			if (StickerPack != null && !(stickerPackId != _stickerPackId))
			{
				StickerPack.State = StickerPackViewModel.DownloadState.Downloading;
				StickerPack.DownloadProgress = progress;
			}
		}

		private void OnCacheStickerPackUpdatedSignal(CacheStickerPacksUpdatedSignal signal)
		{
			if (!IsLoading && StickerPack != null && signal.StickerPacks.Any((StickerPack p) => p.Id == _stickerPackId))
			{
				switch (signal.State)
				{
				case CacheStickerPacksUpdatedSignal.StickerPackState.Downloaded:
					StickerPack.State = StickerPackViewModel.DownloadState.Downloaded;
					break;
				case CacheStickerPacksUpdatedSignal.StickerPackState.Deleted:
					StickerPack.State = StickerPackViewModel.DownloadState.Available;
					break;
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.DeleteStickerPack;
using Facebook.Services.Api.Methods.FetchStickerPacks;
using Facebook.Services.Common;
using Facebook.Services.Stickers;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class StickerStoreViewModel : FacebookMessengerViewModel, IStickerPackProgressHandler
	{
		public const string IsLoadingAvailablePropertyName = "IsLoadingAvailable";

		public const string IsLoadingOwnedPropertyName = "IsLoadingOwned";

		private readonly Facebook.Services.Stickers.Service _stickerService;

		private readonly IStickerStorage _stickerStorage;

		private readonly StickerPackCollection _availableStickerPacks;

		private readonly StickerPackCollection _featuredStickerPacks;

		private readonly StickerPackCollection _ownedStickerPacks;

		private bool _isLoadingAvailable;

		private bool _isLoadingOwned;

		private string _availableErrorMessage;

		private string _ownedErrorMessage;

		public StickerPackCollection AvailableStickerPacks
		{
			get
			{
				return _availableStickerPacks;
			}
		}

		public StickerPackCollection FeaturedStickerPacks
		{
			get
			{
				return _featuredStickerPacks;
			}
		}

		public StickerPackCollection OwnedStickerPacks
		{
			get
			{
				return _ownedStickerPacks;
			}
		}

		public bool IsLoadingAvailable
		{
			get
			{
				return _isLoadingAvailable;
			}
			private set
			{
				Set(ref _isLoadingAvailable, value, false, "IsLoadingAvailable");
			}
		}

		public bool IsLoadingOwned
		{
			get
			{
				return _isLoadingOwned;
			}
			private set
			{
				Set(ref _isLoadingOwned, value, false, "IsLoadingOwned");
			}
		}

		public string AvailableErrorMessage
		{
			get
			{
				return _availableErrorMessage;
			}
			private set
			{
				Set(ref _availableErrorMessage, value, false, "AvailableErrorMessage");
			}
		}

		public string OwnedErrorMessage
		{
			get
			{
				return _ownedErrorMessage;
			}
			private set
			{
				Set(ref _ownedErrorMessage, value, false, "OwnedErrorMessage");
			}
		}

		public event EventHandler AvailableStickerPacksUpdated;

		public event EventHandler OwnedStickerPacksUpdated;

		public StickerStoreViewModel(Facebook.Services.Api.Service apiService, Facebook.Services.Stickers.Service stickerService, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, IStickerStorage stickerStorage)
			: base(apiService, context, navigator, dispatcher, networkStatus)
		{
			_stickerService = stickerService;
			_stickerStorage = stickerStorage;
			_availableStickerPacks = new StickerPackCollection(stickerStorage);
			_featuredStickerPacks = new StickerPackCollection(stickerStorage);
			_ownedStickerPacks = new StickerPackCollection(stickerStorage);
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<CacheStickerPacksUpdatedSignal>(this, OnCacheStickerPacksUpdatedSignal);
		}

		public override void Cleanup()
		{
			GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
			base.Cleanup();
		}

		public async Task FetchStickers()
		{
			await FetchAvailableStickerPacks();
			await FetchTrayStickerPacks();
			Context.SetHasNewStickerPacks(false);
		}

		public async Task RemoveStickerPack(StickerPackViewModel stickerPack)
		{
			OwnedStickerPacks.RemoveStickerPackWithId(stickerPack.Id);
			InvokeStickerPacksUpdated(OwnedStickerPacksUpdated);
			Facebook.Services.Api.Methods.DeleteStickerPack.Request request = new Facebook.Services.Api.Methods.DeleteStickerPack.Request
			{
				StickerPackId = stickerPack.Id
			};
			await _stickerService.ExecuteAsync<Facebook.Services.Api.Methods.DeleteStickerPack.Request, BoolResponse>(request);
		}

		public void NavigateToStickerPack(StickerPackViewModel stickerPack)
		{
			Navigator.Navigate(FacebookUri.GetStickerPackUri(stickerPack.Id));
		}

		public void RegisterProgressHandler()
		{
			_stickerService.AddProgressHandler(this);
		}

		public void UnRegisterProgressHandler()
		{
			_stickerService.RemoveProgressHandler(this);
		}

		private async Task FetchAvailableStickerPacks()
		{
			if (IsLoadingAvailable)
			{
				return;
			}
			try
			{
				IsLoadingAvailable = true;
				Facebook.Services.Api.Methods.FetchStickerPacks.Response response = await FetchStickerPacks(StickerPackType.Available);
				if (response.HasError())
				{
					AvailableErrorMessage = GetStickerLoadingErrorMessage();
					return;
				}
				List<StickerPack> availableStickerPacks = response.StickerPacks;
				List<StickerPack> featuredStickerPacks = response.StickerPacks.Where((StickerPack p) => p.IsFeatured).ToList();
				List<string> downloadingStickerPackIds = response.DownloadingStickerPackIds;
				FeaturedStickerPacks.AddStickerPacks(featuredStickerPacks, downloadingStickerPackIds);
				AvailableStickerPacks.AddStickerPacks(availableStickerPacks, downloadingStickerPackIds);
				InvokeStickerPacksUpdated(AvailableStickerPacksUpdated);
			}
			finally
			{
				IsLoadingAvailable = false;
			}
		}

		private async Task FetchTrayStickerPacks()
		{
			if (IsLoadingOwned)
			{
				return;
			}
			try
			{
				IsLoadingOwned = true;
				Facebook.Services.Api.Methods.FetchStickerPacks.Response response = await FetchStickerPacks(StickerPackType.Tray);
				if (response.HasError())
				{
					OwnedErrorMessage = GetStickerLoadingErrorMessage();
					return;
				}
				OwnedStickerPacks.AddStickerPacks(response.StickerPacks, response.DownloadingStickerPackIds);
				InvokeStickerPacksUpdated(OwnedStickerPacksUpdated);
			}
			finally
			{
				IsLoadingOwned = false;
			}
		}

		private async Task<Facebook.Services.Api.Methods.FetchStickerPacks.Response> FetchStickerPacks(StickerPackType type)
		{
			Facebook.Services.Api.Methods.FetchStickerPacks.Request request = new Facebook.Services.Api.Methods.FetchStickerPacks.Request
			{
				StickerPackType = type
			};
			return await _stickerService.ExecuteAsync<Facebook.Services.Api.Methods.FetchStickerPacks.Request, Facebook.Services.Api.Methods.FetchStickerPacks.Response>(request);
		}

		private void InvokeStickerPacksUpdated(EventHandler eventHandler)
		{
			if (eventHandler != null)
			{
				eventHandler(this, null);
			}
		}

		private string GetStickerLoadingErrorMessage()
		{
			if (!HasConnection)
			{
				return StringLibrary.Error_No_Connection;
			}
			return StringLibrary.Error_Cannot_Fetch_Sticker_Packs;
		}

		private void OnCacheStickerPacksUpdatedSignal(CacheStickerPacksUpdatedSignal signal)
		{
			switch (signal.StickerPackType)
			{
			case StickerPackType.Available:
				OnCacheAvailableStickerPacksUpdated(signal.StickerPacks, signal.State).DoNotWait();
				break;
			case StickerPackType.Tray:
				OnCacheTrayStickerPacksUpdated(signal.StickerPacks, signal.State);
				break;
			}
		}

		private async Task OnCacheAvailableStickerPacksUpdated(List<StickerPack> stickerPacks, CacheStickerPacksUpdatedSignal.StickerPackState state)
		{
			if (!IsLoadingAvailable && state == CacheStickerPacksUpdatedSignal.StickerPackState.Added && !AvailableStickerPacks.Any())
			{
				AvailableErrorMessage = null;
				await FetchAvailableStickerPacks();
			}
		}

		private void OnCacheTrayStickerPacksUpdated(IEnumerable<StickerPack> stickerPacks, CacheStickerPacksUpdatedSignal.StickerPackState state)
		{
			if (!IsLoadingOwned)
			{
				switch (state)
				{
				case CacheStickerPacksUpdatedSignal.StickerPackState.Added:
					UpdateStickerPacksForAdded(stickerPacks);
					break;
				case CacheStickerPacksUpdatedSignal.StickerPackState.Downloaded:
					UpdateStickerPacksForDownloaded(stickerPacks);
					break;
				case CacheStickerPacksUpdatedSignal.StickerPackState.Deleted:
					UpdateStickerPacksForDeleted(stickerPacks);
					break;
				}
			}
		}

		private void UpdateStickerPacksForAdded(IEnumerable<StickerPack> addedStickerPacks)
		{
			OwnedErrorMessage = null;
			IEnumerable<StickerPackViewModel> stickerPacks = addedStickerPacks.Select((StickerPack p) => new StickerPackViewModel(p, _stickerStorage));
			OwnedStickerPacks.AddStickerPacks(stickerPacks);
			InvokeStickerPacksUpdated(OwnedStickerPacksUpdated);
		}

		private void UpdateStickerPacksForDeleted(IEnumerable<StickerPack> deletedStickerPacks)
		{
			Action<StickerPackViewModel> updateAction = (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Available;
			};
			foreach (StickerPack deletedStickerPack in deletedStickerPacks)
			{
				OwnedStickerPacks.RemoveStickerPackWithId(deletedStickerPack.Id);
				UpdateAllStickerPacksWithId(deletedStickerPack.Id, updateAction);
			}
			InvokeStickerPacksUpdated(OwnedStickerPacksUpdated);
		}

		private void UpdateStickerPacksForDownloaded(IEnumerable<StickerPack> downloadedStickerPacks)
		{
			Action<StickerPackViewModel> updateAction = (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Downloaded;
			};
			foreach (StickerPack downloadedStickerPack in downloadedStickerPacks)
			{
				UpdateAllStickerPacksWithId(downloadedStickerPack.Id, updateAction);
			}
		}

		private void UpdateAllStickerPacksWithId(string stickerPackId, Action<StickerPackViewModel> updateAction)
		{
			AvailableStickerPacks.UpdateStickerPackWithId(stickerPackId, updateAction);
			FeaturedStickerPacks.UpdateStickerPackWithId(stickerPackId, updateAction);
			OwnedStickerPacks.UpdateStickerPackWithId(stickerPackId, updateAction);
		}

		public void DownloadStarted(string stickerPackId)
		{
			Action<StickerPackViewModel> updateAction = (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Downloading;
			};
			UpdateAllStickerPacksWithId(stickerPackId, updateAction);
		}

		public void DownloadFailed(string stickerPackId)
		{
			Action<StickerPackViewModel> updateAction = (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Failed;
			};
			UpdateAllStickerPacksWithId(stickerPackId, updateAction);
		}

		public void ProgressUpdated(string stickerPackId, double progress)
		{
			Action<StickerPackViewModel> updateAction = (StickerPackViewModel sp) =>
			{
				sp.State = StickerPackViewModel.DownloadState.Downloading;
			};
			Action<StickerPackViewModel> updateAction2 = (StickerPackViewModel sp) =>
			{
				sp.DownloadProgress = progress;
			};
			UpdateAllStickerPacksWithId(stickerPackId, updateAction);
			UpdateAllStickerPacksWithId(stickerPackId, updateAction2);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Network;
using Facebook.Base.Strategy;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.AddRecentSticker;
using Facebook.Services.Api.Methods.AddStickerPack;
using Facebook.Services.Api.Methods.DeleteStickerPack;
using Facebook.Services.Api.Methods.DownloadStickerPack;
using Facebook.Services.Api.Methods.FetchRecentStickers;
using Facebook.Services.Api.Methods.FetchSticker;
using Facebook.Services.Api.Methods.FetchStickerPack;
using Facebook.Services.Api.Methods.FetchStickerPacks;
using Facebook.Services.Api.Methods.SetStickerTrayPacks;
using Facebook.Services.Api.Methods.SyncStickerPacks;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Stickers
{
	public class Service : BackgroundService
	{
		private const int RefreshIntervalMs = 604800000;

		private readonly StickerPackCache _stickerPackCache;

		private readonly Facebook.Services.Api.Service _apiService;

		private readonly INetworkStatus _networkStatus;

		private readonly IStickerStorage _stickerStorage;

		private readonly StickerPackProgressHelper _stickerPackProgressHelper;

		private readonly DisposableMutex _syncMutex = new DisposableMutex();

		private readonly AutoRenewCancellationTokenSource _cancelTokenSource = new AutoRenewCancellationTokenSource();

		private readonly ExponentialFallback<Facebook.Services.Api.Methods.SyncStickerPacks.Request, Facebook.Services.Api.Methods.SyncStickerPacks.Response> _syncStickerPacksWithBackoff;

		private FacebookContext _facebookContext;

		public Service(StickerPackCache stickerPackCache, Facebook.Services.Api.Service apiService, IStickerStorage stickerStorage, INetworkStatus networkStatus, IDispatcher ui)
			: base(ui)
		{
			_stickerPackCache = stickerPackCache;
			_apiService = apiService;
			_stickerStorage = stickerStorage;
			_networkStatus = networkStatus;
			_stickerPackProgressHelper = new StickerPackProgressHelper(UiDispatcher, _stickerStorage, _stickerPackCache);
			_syncStickerPacksWithBackoff = new ExponentialFallback<Facebook.Services.Api.Methods.SyncStickerPacks.Request, Facebook.Services.Api.Methods.SyncStickerPacks.Response>(ExecuteSyncStickerPacks);
			Register<Facebook.Services.Api.Methods.FetchStickerPacks.Request, Facebook.Services.Api.Methods.FetchStickerPacks.Response>(ProcessFetchStickerPacks);
			Register<Facebook.Services.Api.Methods.AddStickerPack.Request, BoolResponse>(ProcessAddStickerPacks);
			Register<Facebook.Services.Api.Methods.DownloadStickerPack.Request, BoolResponse>(ProcessDownloadStickerPack);
			Register<Facebook.Services.Api.Methods.DeleteStickerPack.Request, BoolResponse>(ProcessDeleteStickerPack);
			Register<Facebook.Services.Api.Methods.AddRecentSticker.Request, BoolResponse>(ProcessAddRecentSticker);
			Register<Facebook.Services.Api.Methods.FetchRecentStickers.Request, Facebook.Services.Api.Methods.FetchRecentStickers.Response>(ProcessFetchRecentStickers);
			Register<Facebook.Services.Api.Methods.FetchStickerPack.Request, Facebook.Services.Api.Methods.FetchStickerPack.Response>(ProcessFetchStickerPack);
			Register<Facebook.Services.Api.Methods.FetchSticker.Request, Facebook.Services.Api.Methods.FetchSticker.Response>(ProcessFetchSticker);
		}

		public override void StartService(FacebookContext context)
		{
			_facebookContext = context;
			_networkStatus.AddHandler(OnNetworkStatusChanged);
			Messenger.Default.Register<LoginSignal>(this, OnLoginSignal);
			Messenger.Default.Register<LogoutSignal>(this, OnLogoutSignal);
			if (_facebookContext.IsAuthenticated())
			{
				SyncStickerPacks().DoNotWait();
			}
		}

		public override void StopService()
		{
			_networkStatus.RemoveHandler(OnNetworkStatusChanged);
			Messenger.Default.Unregister(this);
		}

		public void AddProgressHandler(IStickerPackProgressHandler handler)
		{
			_stickerPackProgressHelper.AddProgressHandler(handler);
		}

		public void RemoveProgressHandler(IStickerPackProgressHandler handler)
		{
			_stickerPackProgressHelper.RemoveProgressHandler(handler);
		}

		private async Task<StrategyResponse<Facebook.Services.Api.Methods.SyncStickerPacks.Response>> ExecuteSyncStickerPacks(int attempt, CancellationToken token, Facebook.Services.Api.Methods.SyncStickerPacks.Request request)
		{
			Facebook.Services.Api.Methods.SyncStickerPacks.Response response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.SyncStickerPacks.Request, Facebook.Services.Api.Methods.SyncStickerPacks.Response>(request);
			ExecutionResult state = ((response != null && !response.HasError() && !request.CancelToken.IsCancellationRequested) ? ExecutionResult.Success : ExecutionResult.Failed);
			Logger.Info(this, string.Format("Syncing Sticker Packs, Attempt {0}, State {1}", new object[2] { attempt, state }), "ExecuteSyncStickerPacks", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 102);
			return StrategyResponse.Create(state, response);
		}

		private async Task SyncStickerPacks()
		{
			using (await _syncMutex.WaitAsync())
			{
				if (await ShouldSyncStickers())
				{
					Logger.Info(this, "Syncing sticker packs.", "SyncStickerPacks", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 112);
					Facebook.Services.Api.Methods.SyncStickerPacks.Request request = new Facebook.Services.Api.Methods.SyncStickerPacks.Request
					{
						AccessToken = _facebookContext.Session.Credentials.AccessToken,
						CancelToken = _cancelTokenSource.Token
					};
					await _syncStickerPacksWithBackoff.Execute(_cancelTokenSource.Token, request);
				}
				await DownloadStickersNotOnDevice();
			}
		}

		private async Task<bool> ShouldSyncStickers()
		{
			long lastSyncTime = await _stickerPackCache.GetLastSyncTime();
			long now = DateExtensions.MSecsSinceEpoch();
			return Math.Abs(now - lastSyncTime) >= 604800000;
		}

		private async Task DownloadStickersNotOnDevice()
		{
			List<StickerPack> stickerPacksToDownload = await _stickerPackCache.GetStickerPacksToDownload();
			if (stickerPacksToDownload == null || !stickerPacksToDownload.Any())
			{
				Logger.Info(this, "Skipping download sticker packs; nothing to download.", "DownloadStickersNotOnDevice", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 136);
				return;
			}
			List<Task<bool>> tasks = stickerPacksToDownload.Select((StickerPack p) => _stickerPackProgressHelper.DownloadStickerPack(p, _cancelTokenSource.Token)).ToList();
			await Task.WhenAll(tasks);
		}

		private void OnNetworkStatusChanged(INetworkStatus sender, bool connected)
		{
			Logger.Info(this, "Network status updated, connected: " + connected, "OnNetworkStatusChanged", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 146);
			if (connected)
			{
				if (_facebookContext != null && _facebookContext.IsAuthenticated())
				{
					SyncStickerPacks().DoNotWait();
				}
			}
			else
			{
				_cancelTokenSource.CancelAndRenew();
			}
		}

		private void OnLoginSignal(LoginSignal signal)
		{
			_stickerStorage.Reset();
			SyncStickerPacks().DoNotWait();
		}

		private void OnLogoutSignal(LogoutSignal signal)
		{
			_cancelTokenSource.CancelAndRenew();
			_stickerStorage.Reset();
		}

		private async Task<Facebook.Services.Api.Methods.FetchStickerPacks.Response> ProcessFetchStickerPacks(Facebook.Services.Api.Methods.FetchStickerPacks.Request request)
		{
			Facebook.Services.Api.Methods.FetchStickerPacks.Response response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchStickerPacks.Request, Facebook.Services.Api.Methods.FetchStickerPacks.Response>(request);
			response.DownloadingStickerPackIds = _stickerPackProgressHelper.GetDownloadingStickerPackIds();
			return response;
		}

		private async Task<BoolResponse> ProcessAddStickerPacks(Facebook.Services.Api.Methods.AddStickerPack.Request request)
		{
			return await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.AddStickerPack.Request, BoolResponse>(request);
		}

		private async Task<BoolResponse> ProcessDownloadStickerPack(Facebook.Services.Api.Methods.DownloadStickerPack.Request request)
		{
			string stickerPackId = request.StickerPackId;
			StickerPack stickerPack = await _stickerPackCache.GetStickerPackById(stickerPackId);
			List<string> availableStickerPack = await _stickerPackCache.GetStickerPackIds(StickerPackType.Available);
			List<string> ownedStickerPack = await _stickerPackCache.GetStickerPackIds(StickerPackType.Owned);
			List<string> trayStickerPack = await _stickerPackCache.GetStickerPackIds(StickerPackType.Tray);
			bool unavailableStickerPack = !availableStickerPack.Contains(stickerPackId);
			bool needToPurchaseStickerPack = !ownedStickerPack.Contains(stickerPackId);
			bool needToAddStickerPackToTray = !trayStickerPack.Contains(stickerPackId);
			if (unavailableStickerPack)
			{
				Logger.Error(this, string.Format("Attempted to download unavailable sticker pack. Id = {0}", new object[1] { stickerPackId }), "ProcessDownloadStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 200);
				BoolResponse boolResponse = new BoolResponse();
				boolResponse.Succeeded = false;
				return boolResponse;
			}
			if (needToPurchaseStickerPack && !(await PurchaseStickerPack(stickerPackId)))
			{
				Logger.Error(this, string.Format("Failed to purchase sticker pack. Id = {0}", new object[1] { stickerPackId }), "ProcessDownloadStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 209);
				BoolResponse boolResponse2 = new BoolResponse();
				boolResponse2.Succeeded = false;
				return boolResponse2;
			}
			if (needToAddStickerPackToTray && !(await AddStickerPackToTray(stickerPack)))
			{
				Logger.Error(this, string.Format("Failed to add sticker pack to tray. Id = {0}", new object[1] { stickerPackId }), "ProcessDownloadStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 219);
				BoolResponse boolResponse3 = new BoolResponse();
				boolResponse3.Succeeded = false;
				return boolResponse3;
			}
			bool downloadSucceeded = await _stickerPackProgressHelper.DownloadStickerPack(stickerPack, _cancelTokenSource.Token);
			return new BoolResponse
			{
				Succeeded = downloadSucceeded
			};
		}

		private async Task<BoolResponse> ProcessDeleteStickerPack(Facebook.Services.Api.Methods.DeleteStickerPack.Request request)
		{
			string stickerPackId = request.StickerPackId;
			if (!(await _stickerPackCache.GetStickerPackIds(StickerPackType.Tray)).Contains(stickerPackId))
			{
				Logger.Error(this, string.Format("Attempted to delete sticker pack not in the tray. Id = {0}", new object[1] { stickerPackId }), "ProcessDeleteStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 236);
				BoolResponse boolResponse = new BoolResponse();
				boolResponse.Succeeded = false;
				return boolResponse;
			}
			StickerPack stickerPack = await _stickerPackCache.GetStickerPackById(stickerPackId);
			if (!(await RemoveStickerPackFromTray(stickerPack)))
			{
				Logger.Error(this, string.Format("Failed to delete sticker pack. Id = {0}", new object[1] { stickerPackId }), "ProcessDeleteStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Stickers\\Service.cs", 244);
				BoolResponse boolResponse2 = new BoolResponse();
				boolResponse2.Succeeded = false;
				return boolResponse2;
			}
			await _stickerStorage.DeleteStickerPack(stickerPack);
			return new BoolResponse
			{
				Succeeded = true
			};
		}

		private async Task<BoolResponse> ProcessAddRecentSticker(Facebook.Services.Api.Methods.AddRecentSticker.Request request)
		{
			await _stickerPackCache.SaveRecentStickerId(request.StickerId);
			return new BoolResponse
			{
				Succeeded = true
			};
		}

		private async Task<Facebook.Services.Api.Methods.FetchRecentStickers.Response> ProcessFetchRecentStickers(Facebook.Services.Api.Methods.FetchRecentStickers.Request request)
		{
			return await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchRecentStickers.Request, Facebook.Services.Api.Methods.FetchRecentStickers.Response>(request);
		}

		private async Task<Facebook.Services.Api.Methods.FetchStickerPack.Response> ProcessFetchStickerPack(Facebook.Services.Api.Methods.FetchStickerPack.Request request)
		{
			Facebook.Services.Api.Methods.FetchStickerPack.Response response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchStickerPack.Request, Facebook.Services.Api.Methods.FetchStickerPack.Response>(request);
			List<string> downloadingStickerPackIds = _stickerPackProgressHelper.GetDownloadingStickerPackIds();
			response.IsDownloading = downloadingStickerPackIds.Contains(request.StickerPackId);
			return response;
		}

		private async Task<Facebook.Services.Api.Methods.FetchSticker.Response> ProcessFetchSticker(Facebook.Services.Api.Methods.FetchSticker.Request request)
		{
			string stickerId = request.StickerId;
			Uri stickerImageUri = await _stickerStorage.GetStickerUri(stickerId, false);
			if (stickerImageUri != null)
			{
				Uri animatedImageUri = await _stickerStorage.GetStickerUri(stickerId, true);
				string animatedImageUrl = ((animatedImageUri != null) ? animatedImageUri.AbsoluteUri : null);
				Sticker sticker = new Sticker
				{
					Id = stickerId,
					ThreadImage = new FacebookImage
					{
						Url = stickerImageUri.AbsoluteUri
					},
					AnimatedImage = new FacebookImage
					{
						Url = animatedImageUrl
					}
				};
				return new Facebook.Services.Api.Methods.FetchSticker.Response
				{
					Sticker = sticker
				};
			}
			return await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchSticker.Request, Facebook.Services.Api.Methods.FetchSticker.Response>(request);
		}

		private async Task<bool> AddStickerPackToTray(StickerPack stickerPack)
		{
			string stickerPackId = stickerPack.Id;
			List<string> trayPacks = await _stickerPackCache.GetStickerPackIds(StickerPackType.Tray);
			trayPacks.Add(stickerPackId);
			bool succeeded = await SetStickerTrayPacks(trayPacks);
			if (succeeded)
			{
				await _stickerPackCache.InsertStickerPack(stickerPack, StickerPackType.Tray, 0);
			}
			return succeeded;
		}

		private async Task<bool> RemoveStickerPackFromTray(StickerPack stickerPack)
		{
			string stickerPackId = stickerPack.Id;
			List<string> trayPacks = await _stickerPackCache.GetStickerPackIds(StickerPackType.Tray);
			trayPacks.Remove(stickerPackId);
			bool succeeded = await SetStickerTrayPacks(trayPacks);
			if (succeeded)
			{
				await _stickerPackCache.RemoveStickerPackById(stickerPackId);
			}
			return succeeded;
		}

		private async Task<bool> SetStickerTrayPacks(List<string> stickerPackIds)
		{
			Facebook.Services.Api.Methods.SetStickerTrayPacks.Request request = new Facebook.Services.Api.Methods.SetStickerTrayPacks.Request
			{
				CancelToken = _cancelTokenSource.Token,
				AccessToken = _facebookContext.Session.Credentials.AccessToken,
				StickerPackIds = stickerPackIds
			};
			BoolResponse response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.SetStickerTrayPacks.Request, BoolResponse>(request);
			return !response.HasError() && response.Succeeded;
		}

		private async Task<bool> PurchaseStickerPack(string stickerPackId)
		{
			Facebook.Services.Api.Methods.AddStickerPack.Request request = new Facebook.Services.Api.Methods.AddStickerPack.Request
			{
				AccessToken = _facebookContext.Session.Credentials.AccessToken,
				CancelToken = _cancelTokenSource.Token,
				StickerPackId = stickerPackId
			};
			BoolResponse response = await _apiService.ExecuteAsync<Facebook.Services.Api.Methods.AddStickerPack.Request, BoolResponse>(request);
			return !response.HasError() && response.Succeeded;
		}
	}
}

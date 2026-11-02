using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Models.Signals;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Api.Cache;

public class StickerPackCache : ICache
{
	private const string CacheSaveKey = "sticker_pack_cache";

	private readonly IUserDataStorage _storage;

	private readonly IDispatcher _uiDispatcher;

	private readonly DisposableMutexInit _lock;

	private readonly FacebookContext _context;

	private StickerCacheData _data;

	private bool _dirty;

	private bool _recentStickersDirty;

	private List<string> _recentStickerIds;

	public StickerPackCache(IUserDataStorage storage, IDispatcher uiDispatcher, FacebookContext context)
	{
		_storage = storage;
		_lock = new DisposableMutexInit(Init);
		_uiDispatcher = uiDispatcher;
		_context = context;
	}

	private async Task Init()
	{
		_data = (await SaveCacheUtil.Load<StickerCacheData>(_storage, "sticker_pack_cache")) ?? new StickerCacheData();
		_recentStickerIds = _context.GetRecentStickerIds();
	}

	public async Task EnsureSave()
	{
		Logger.Info(this, "Acquiring Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerPackCache.cs", 44);
		using (await _lock.WaitAsync())
		{
			Logger.Info(this, "Acquired Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerPackCache.cs", 47);
			if (_dirty)
			{
				await SaveCacheUtil.Save(_storage, "sticker_pack_cache", _data);
				_dirty = false;
			}
			else
			{
				Logger.Info(this, $"Did not save cache of type: {GetType()}.  No changes to StickerPacks", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerPackCache.cs", 55);
			}
			if (_recentStickersDirty)
			{
				_context.SetRecentStickerIds(_recentStickerIds);
				_recentStickersDirty = false;
				Logger.Info("Saved changes to recent stickers", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerPackCache.cs", 62);
			}
			else
			{
				Logger.Info("No changes to recent stickers", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerPackCache.cs", 66);
			}
		}
	}

	public async Task Purge()
	{
		using (await _lock.WaitAsync())
		{
			_data.ClearAll();
			_dirty = true;
			_recentStickersDirty = true;
		}
	}

	public async Task AddStickerPacks(IEnumerable<StickerPack> availableStickerPacks, IEnumerable<StickerPack> trayStickerPacks, IEnumerable<StickerPack> ownedStickerPacks, bool hasNewStickerPacks)
	{
		using (await _lock.WaitAsync())
		{
			_data.AddStickerPacks(availableStickerPacks);
			_data.AddStickerPacks(trayStickerPacks);
			_context.SetHasNewStickerPacks(hasNewStickerPacks);
			UnsafeAddStickerPacks(availableStickerPacks, StickerPackType.Available);
			UnsafeAddStickerPacks(trayStickerPacks, StickerPackType.Tray);
			UnsafeAddStickerPacks(ownedStickerPacks, StickerPackType.Owned);
		}
	}

	public async Task AddStickerPacks(IEnumerable<StickerPack> stickerPacks, StickerPackType type)
	{
		using (await _lock.WaitAsync())
		{
			UnsafeAddStickerPacks(stickerPacks, type);
		}
	}

	public async Task InsertStickerPack(StickerPack stickerPack, StickerPackType type, int index)
	{
		using (await _lock.WaitAsync())
		{
			UnsafeInsertStickerPack(stickerPack, type, index);
		}
	}

	public async Task RemoveStickerPackById(string stickerPackId)
	{
		using (await _lock.WaitAsync())
		{
			UnsafeRemoveRecentStickersByStickerPackId(stickerPackId);
			UnsafeRemoveStickerPackById(stickerPackId);
		}
	}

	public async Task<List<string>> GetStickerPackIds(StickerPackType type)
	{
		using (await _lock.WaitAsync())
		{
			return type switch
			{
				StickerPackType.Available => new List<string>(_data.AvailableStickerPackIds), 
				StickerPackType.Tray => new List<string>(_data.TrayStickerPackIds), 
				StickerPackType.Owned => new List<string>(_data.OwnedListStickerPackIds), 
				_ => throw new Exception($"Attempted to Get Sticker Packs Ids for invalid type: {type}"), 
			};
		}
	}

	public async Task<List<StickerPack>> GetStickerPacks(StickerPackType type)
	{
		using (await _lock.WaitAsync())
		{
			return type switch
			{
				StickerPackType.Available => GetStickerPackByIds(_data.AvailableStickerPackIds), 
				StickerPackType.Tray => GetStickerPackByIds(_data.TrayStickerPackIds), 
				StickerPackType.Owned => GetStickerPackByIds(_data.OwnedListStickerPackIds), 
				_ => throw new Exception($"Attempted to Get Sticker Packs for invalid type: {type}"), 
			};
		}
	}

	public async Task<StickerPack> GetStickerPackById(string stickerPackId)
	{
		using (await _lock.WaitAsync())
		{
			StickerPack stickerPack = UnsafeGetStickerPackById(stickerPackId);
			return CopyToProtectUntilImmutable(stickerPack);
		}
	}

	public async Task<List<StickerPack>> GetStickerPacksToDownload()
	{
		using (await _lock.WaitAsync())
		{
			List<StickerPack> stickerPackByIds = GetStickerPackByIds(_data.TrayStickerPackIds);
			IEnumerable<StickerPack> stickerPacks = stickerPackByIds.Where((StickerPack s) => !s.IsDownloaded);
			return CopyToProtectUntilImmutable(stickerPacks);
		}
	}

	public async Task<List<string>> GetRecentStickerIds()
	{
		using (await _lock.WaitAsync())
		{
			return new List<string>(_recentStickerIds);
		}
	}

	public async Task<List<Sticker>> GetRecentStickers()
	{
		using (await _lock.WaitAsync())
		{
			List<Sticker> list = new List<Sticker>();
			foreach (string recentStickerId in _recentStickerIds)
			{
				List<StickerPack> stickerPackByIds = GetStickerPackByIds(_data.TrayStickerPackIds);
				Func<StickerPack, Sticker> selector = (StickerPack p) => p.Stickers.FirstOrDefault((Sticker s) => s.Id == recentStickerId);
				list.Add(stickerPackByIds.Select(selector).FirstOrDefault((Sticker stick) => stick != null));
			}
			return CopyToProtectUntilImmutable(list);
		}
	}

	public async Task SaveStickerPackDownloaded(string stickerPackId)
	{
		using (await _lock.WaitAsync())
		{
			StickerPack stickerPack = GetStickerPackByIds(_data.TrayStickerPackIds).FirstOrDefault((StickerPack sp) => sp.Id == stickerPackId);
			if (stickerPack != null)
			{
				UpdateIsDownloaded(stickerPackId, isDownloaded: true);
				_dirty = true;
				UnsafeSignalStickerPackDownloaded(stickerPack);
			}
			else
			{
				Logger.Error(this, $"Attempted to download sticker pack not in Tray for sticker: {stickerPackId}", "SaveStickerPackDownloaded", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerPackCache.cs", 220);
			}
		}
	}

	public async Task SaveRecentStickerId(string stickerId)
	{
		using (await _lock.WaitAsync())
		{
			bool flag = _recentStickerIds.Contains(stickerId);
			if (flag)
			{
				_recentStickerIds.Remove(stickerId);
			}
			_recentStickerIds.Insert(0, stickerId);
			_recentStickersDirty = true;
			if (!flag)
			{
				UnsafeSignalRecentStickersUpdated();
			}
		}
	}

	private List<StickerPack> GetStickerPackByIds(List<string> stickerPackIds)
	{
		List<StickerPack> stickerPacks = (from stickerPackId in stickerPackIds
			select _data.StickerPacks.SingleOrDefault((StickerPack sp) => sp.Id == stickerPackId) into stickerPack
			where stickerPack != null
			select stickerPack).ToList();
		return CopyToProtectUntilImmutable(stickerPacks);
	}

	private void UpdateIsDownloaded(string stickerPackId, bool isDownloaded)
	{
		StickerPack stickerPack = _data.StickerPacks.FirstOrDefault((StickerPack sp) => sp.Id == stickerPackId);
		if (stickerPack != null)
		{
			stickerPack.IsDownloaded = isDownloaded;
		}
	}

	private void UnsafeInsertStickerPack(StickerPack stickerPack, StickerPackType type, int index)
	{
		switch (type)
		{
		case StickerPackType.Available:
			_data.InsertAvailableStickerPack(stickerPack, index);
			break;
		case StickerPackType.Tray:
			_data.InsertTrayStickerPack(stickerPack, index);
			break;
		case StickerPackType.Owned:
			_data.InsertOwnedStickerPack(stickerPack, index);
			break;
		default:
			throw new Exception($"Attempted to Insert Sticker Pack for invalid type: {type}");
		}
		_dirty = true;
		UnsafeSignalStickerPacksAdded(new List<StickerPack> { stickerPack }, type);
	}

	private void UnsafeAddStickerPacks(IEnumerable<StickerPack> stickerPacks, StickerPackType type)
	{
		switch (type)
		{
		case StickerPackType.Available:
			_data.AddAvailableStickerPacks(stickerPacks);
			break;
		case StickerPackType.Tray:
			_data.AddTrayStickerPacks(stickerPacks);
			break;
		case StickerPackType.Owned:
			_data.AddOwnedStickerPacks(stickerPacks);
			break;
		default:
			throw new Exception($"Attempted to Add Sticker Pack for invalid type: {type}");
		}
		_dirty = true;
		UnsafeSignalStickerPacksAdded(stickerPacks, type);
	}

	private void UnsafeRemoveRecentStickersByStickerPackId(string stickerPackId)
	{
		List<string> trayStickerPackIds = _data.TrayStickerPackIds;
		if (!trayStickerPackIds.Contains(stickerPackId))
		{
			return;
		}
		StickerPack stickerPack = _data.StickerPacks.FirstOrDefault((StickerPack sp) => sp.Id == stickerPackId);
		int num = _recentStickerIds.RemoveAll((string r) => stickerPack.Stickers.Any((Sticker s) => s.Id == r));
		if (num > 0)
		{
			_recentStickersDirty = true;
			UnsafeSignalRecentStickersUpdated();
		}
	}

	private void UnsafeRemoveStickerPackById(string stickerPackId)
	{
		List<string> trayStickerPackIds = _data.TrayStickerPackIds;
		if (trayStickerPackIds.Contains(stickerPackId))
		{
			StickerPack stickerPack = _data.StickerPacks.FirstOrDefault((StickerPack sp) => sp.Id == stickerPackId);
			_data.TrayStickerPackIds.Remove(stickerPackId);
			UpdateIsDownloaded(stickerPackId, isDownloaded: false);
			_dirty = true;
			UnsafeSignalStickerPackDeleted(stickerPack);
		}
	}

	private StickerPack UnsafeGetStickerPackById(string stickPackId)
	{
		StickerPack stickerPack = _data.StickerPacks.FirstOrDefault((StickerPack sp) => sp.Id == stickPackId);
		if (stickerPack == null)
		{
			return null;
		}
		return CopyToProtectUntilImmutable(stickerPack);
	}

	public async Task<long> GetLastSyncTime()
	{
		using (await _lock.WaitAsync())
		{
			return _data.LastSyncTime;
		}
	}

	public async Task SetLastSyncTime(long syncTime)
	{
		using (await _lock.WaitAsync())
		{
			_data.LastSyncTime = syncTime;
			_dirty = true;
		}
	}

	private void UnsafeSignalStickerPacksAdded(IEnumerable<StickerPack> stickerPacks, StickerPackType stickerPackType)
	{
		List<StickerPack> copyStickerPacks = CopyToProtectUntilImmutable(stickerPacks);
		_uiDispatcher.RunAsync(() =>
		{
			Messenger.Default.Send(new CacheStickerPacksUpdatedSignal(copyStickerPacks, stickerPackType, CacheStickerPacksUpdatedSignal.StickerPackState.Added));
		});
	}

	private void UnsafeSignalStickerPackDownloaded(StickerPack stickerPack)
	{
		StickerPack copyStickerPack = CopyToProtectUntilImmutable(stickerPack);
		_uiDispatcher.RunAsync(() =>
		{
			Messenger.Default.Send(new CacheStickerPacksUpdatedSignal(new List<StickerPack> { copyStickerPack }, StickerPackType.Tray, CacheStickerPacksUpdatedSignal.StickerPackState.Downloaded));
		});
	}

	private void UnsafeSignalStickerPackDeleted(StickerPack stickerPack)
	{
		StickerPack copyStickerPack = CopyToProtectUntilImmutable(stickerPack);
		_uiDispatcher.RunAsync(() =>
		{
			Messenger.Default.Send(new CacheStickerPacksUpdatedSignal(new List<StickerPack> { copyStickerPack }, StickerPackType.Tray, CacheStickerPacksUpdatedSignal.StickerPackState.Deleted));
		});
	}

	private void UnsafeSignalRecentStickersUpdated()
	{
		_uiDispatcher.RunAsync(() =>
		{
			Messenger.Default.Send(new CacheRecentStickersUpdatedSignal());
		});
	}

	private static StickerPack CopyToProtectUntilImmutable(StickerPack stickerPack)
	{
		if (stickerPack == null)
		{
			return null;
		}
		return new StickerPack(stickerPack);
	}

	private static Sticker CopyToProtectUntilImmutable(Sticker sticker)
	{
		if (sticker == null)
		{
			return null;
		}
		return new Sticker(sticker);
	}

	private static List<StickerPack> CopyToProtectUntilImmutable(IEnumerable<StickerPack> stickerPacks)
	{
		return stickerPacks?.Select(CopyToProtectUntilImmutable).ToList();
	}

	private static List<Sticker> CopyToProtectUntilImmutable(IEnumerable<Sticker> stickers)
	{
		return stickers?.Select(CopyToProtectUntilImmutable).ToList();
	}
}

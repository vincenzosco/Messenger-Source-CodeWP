using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Facebook.Base.Extensions;
using Facebook.Models;
using Facebook.Services.Stickers;

namespace Facebook.Messenger.WP8.ViewModels;

public class StickerPackCollection : Collection<StickerPackViewModel>, INotifyCollectionChanged
{
	private const string RecentStickerPackId = "RecentStickerPack";

	private const string RecentStickerPackThumbnailUrl = "/Assets/Stickers/RecentStickers.png";

	private readonly StickerPackViewModel _recentStickerPack;

	private readonly IStickerStorage _stickerStorage;

	private bool _hasRecentStickers;

	public event NotifyCollectionChangedEventHandler CollectionChanged;

	public StickerPackCollection(IStickerStorage stickerStorage)
	{
		_stickerStorage = stickerStorage;
		_recentStickerPack = new StickerPackViewModel(stickerStorage)
		{
			Id = "RecentStickerPack",
			State = StickerPackViewModel.DownloadState.Downloaded,
			ThumbnailImage = new FacebookImage
			{
				Url = "/Assets/Stickers/RecentStickers.png"
			}
		};
	}

	protected override void ClearItems()
	{
		base.ClearItems();
		InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	protected override void InsertItem(int index, StickerPackViewModel item)
	{
		base.InsertItem(index, item);
		InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
	}

	protected override void RemoveItem(int index)
	{
		StickerPackViewModel changedItem = base[index];
		base.RemoveItem(index);
		InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, index));
	}

	protected override void SetItem(int index, StickerPackViewModel item)
	{
		StickerPackViewModel oldItem = base[index];
		base.SetItem(index, item);
		InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
	}

	public void AddStickerPacks(IEnumerable<StickerPackViewModel> stickerPacks)
	{
		List<StickerPackViewModel> list = stickerPacks.Where((StickerPackViewModel p) => this.All((StickerPackViewModel sp) => sp.Id != p.Id)).ToList();
		if (list.Any())
		{
			if (list.Count == 1)
			{
				Insert(_hasRecentStickers ? 1 : 0, list[0]);
			}
			else
			{
				AddRange(list);
			}
		}
	}

	public void AddStickerPacks(List<StickerPack> stickerPacks, List<string> downloadingStickerPackIds)
	{
		List<StickerPackViewModel> stickerPacks2 = CreateStickerPacks(stickerPacks, downloadingStickerPackIds);
		AddStickerPacks(stickerPacks2);
	}

	public void UpdateStickerPackWithId(string stickerPackId, Action<StickerPackViewModel> updateAction)
	{
		StickerPackViewModel stickerPackWithId = GetStickerPackWithId(stickerPackId);
		if (stickerPackWithId != null)
		{
			updateAction(stickerPackWithId);
		}
	}

	public bool RemoveStickerPackWithId(string stickerPackId)
	{
		StickerPackViewModel stickerPackWithId = GetStickerPackWithId(stickerPackId);
		if (stickerPackWithId != null)
		{
			return Remove(stickerPackWithId);
		}
		return false;
	}

	public void UpdateRecentStickers(List<Sticker> recentStickers)
	{
		if (recentStickers.Any())
		{
			IEnumerable<StickerViewModel> collection = recentStickers.Select((Sticker s) => new StickerViewModel(s, "RecentStickerPack"));
			_recentStickerPack.Stickers = new ObservableCollection<StickerViewModel>(collection);
			_recentStickerPack.LoadStickerImageUrisAsync().DoNotWait();
			if (!_hasRecentStickers)
			{
				_hasRecentStickers = true;
				Insert(0, _recentStickerPack);
			}
		}
		else if (_hasRecentStickers)
		{
			RemoveStickerPackWithId("RecentStickerPack");
			_hasRecentStickers = false;
		}
	}

	private List<StickerPackViewModel> CreateStickerPacks(List<StickerPack> stickerPacks, List<string> downloadingStickerPackIds)
	{
		List<StickerPackViewModel> stickerPacks2 = stickerPacks.Select((StickerPack p) => new StickerPackViewModel(p, _stickerStorage)).ToList();
		PatchStickerPacks(ref stickerPacks2, downloadingStickerPackIds, StickerPackViewModel.DownloadState.Downloading);
		return stickerPacks2;
	}

	private void PatchStickerPacks(ref List<StickerPackViewModel> stickerPacks, List<string> stickerPackIds, StickerPackViewModel.DownloadState state)
	{
		if (!stickerPackIds.Any())
		{
			return;
		}
		IEnumerable<StickerPackViewModel> enumerable = stickerPacks.Where((StickerPackViewModel p) => stickerPackIds.Contains(p.Id));
		foreach (StickerPackViewModel item in enumerable)
		{
			item.State = state;
		}
	}

	private StickerPackViewModel GetStickerPackWithId(string stickerPackId)
	{
		return this.SingleOrDefault((StickerPackViewModel p) => p.Id == stickerPackId);
	}

	private void AddRange(IEnumerable<StickerPackViewModel> items)
	{
		foreach (StickerPackViewModel item in items)
		{
			Add(item);
		}
		InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	private void InvokeCollectionChanged(NotifyCollectionChangedEventArgs args)
	{
		if (CollectionChanged != null)
		{
			CollectionChanged(this, args);
		}
	}
}

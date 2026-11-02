using System.Collections.Generic;

namespace Facebook.Models.Signals;

public class CacheStickerPacksUpdatedSignal
{
	public enum StickerPackState
	{
		Added,
		Downloaded,
		Deleted
	}

	public readonly List<StickerPack> StickerPacks;

	public readonly StickerPackType StickerPackType;

	public readonly StickerPackState State;

	public CacheStickerPacksUpdatedSignal(List<StickerPack> stickerPacks, StickerPackType stickerPackType, StickerPackState state)
	{
		StickerPacks = stickerPacks;
		StickerPackType = stickerPackType;
		State = state;
	}
}

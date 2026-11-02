using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Facebook.Base.Logging;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Cache
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class StickerCacheData
	{
		[JsonProperty("sticker_packs")]
		[DataMember]
		public List<StickerPack> StickerPacks { get; set; }

		[JsonProperty("available_sticker_packs_ids")]
		[DataMember]
		public List<string> AvailableStickerPackIds { get; set; }

		[DataMember]
		[JsonProperty("tray_sticker_packs_ids")]
		public List<string> TrayStickerPackIds { get; set; }

		[DataMember]
		[JsonProperty("owned_sticker_packs_ids")]
		public List<string> OwnedListStickerPackIds { get; set; }

		[JsonProperty("last_sticker_sync_time")]
		[DataMember]
		public long LastSyncTime { get; set; }

		public StickerCacheData()
		{
			StickerPacks = new List<StickerPack>();
			AvailableStickerPackIds = new List<string>();
			TrayStickerPackIds = new List<string>();
			OwnedListStickerPackIds = new List<string>();
		}

		public void ClearAll()
		{
			ClearStickerPacks();
			ClearAvailable();
			ClearTray();
			ClearOwned();
			LastSyncTime = 0L;
		}

		public void ClearStickerPacks()
		{
			StickerPacks.Clear();
		}

		public void ClearAvailable()
		{
			AvailableStickerPackIds.Clear();
		}

		public void ClearTray()
		{
			TrayStickerPackIds.Clear();
		}

		public void ClearOwned()
		{
			OwnedListStickerPackIds.Clear();
		}

		public void AddStickerPacks(IEnumerable<StickerPack> stickerPacks)
		{
			foreach (StickerPack stickerPack in stickerPacks)
			{
				List<StickerPack> stickerPacks2 = StickerPacks;
				Func<StickerPack, bool> predicate = (StickerPack sp) => sp.Id == stickerPack.Id;
				if (stickerPacks2.SingleOrDefault(predicate) == null)
				{
					StickerPacks.Add(stickerPack);
				}
			}
		}

		public void AddAvailableStickerPacks(IEnumerable<StickerPack> stickerPacks)
		{
			foreach (string item in stickerPacks.Select((StickerPack s) => s.Id))
			{
				AvailableStickerPackIds.Add(item);
			}
		}

		public void AddTrayStickerPacks(IEnumerable<StickerPack> stickerPacks)
		{
			foreach (string item in stickerPacks.Select((StickerPack s) => s.Id))
			{
				TrayStickerPackIds.Add(item);
			}
		}

		public void AddOwnedStickerPacks(IEnumerable<StickerPack> stickerPacks)
		{
			foreach (string item in stickerPacks.Select((StickerPack s) => s.Id))
			{
				OwnedListStickerPackIds.Add(item);
			}
		}

		public void InsertAvailableStickerPack(StickerPack stickerPack, int index)
		{
			if (index < AvailableStickerPackIds.Count)
			{
				AvailableStickerPackIds.Insert(index, stickerPack.Id);
				return;
			}
			Logger.Error(this, string.Format("Attempted to insert into Available Sticker Packs at invalid index: {0}", new object[1] { index }), "InsertAvailableStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerCacheData.cs", 110);
		}

		public void InsertOwnedStickerPack(StickerPack stickerPack, int index)
		{
			if (index < OwnedListStickerPackIds.Count)
			{
				OwnedListStickerPackIds.Insert(index, stickerPack.Id);
				return;
			}
			Logger.Error(this, string.Format("Attempted to insert into Owned Sticker Packs at invalid index: {0}", new object[1] { index }), "InsertOwnedStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerCacheData.cs", 122);
		}

		public void InsertTrayStickerPack(StickerPack stickerPack, int index)
		{
			if (index < TrayStickerPackIds.Count)
			{
				TrayStickerPackIds.Insert(index, stickerPack.Id);
				return;
			}
			Logger.Error(this, string.Format("Attempted to insert into Tray Sticker Packs at invalid index: {0}", new object[1] { index }), "InsertTrayStickerPack", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\StickerCacheData.cs", 134);
		}
	}
}

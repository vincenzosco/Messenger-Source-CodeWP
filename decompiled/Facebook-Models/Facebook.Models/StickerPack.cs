using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class StickerPack
	{
		public static readonly string[] DefaultStickerPackIds = new string[2] { "126361870881943", "350357561732812" };

		[JsonProperty("id")]
		[DataMember]
		public string Id { get; set; }

		[JsonProperty("name")]
		[DataMember]
		public string Name { get; set; }

		[JsonProperty("artist")]
		[DataMember]
		public string Artist { get; set; }

		[DataMember]
		[JsonProperty("description")]
		public string Description { get; set; }

		[DataMember]
		[JsonProperty("is_featured")]
		public bool IsFeatured { get; set; }

		[DataMember]
		[JsonProperty("is_promoted")]
		public bool IsPromoted { get; set; }

		[DataMember]
		[JsonProperty("price")]
		public int Price { get; set; }

		[DataMember]
		[JsonProperty("thumbnail_image")]
		public FacebookImage ThumbnailImage { get; set; }

		[DataMember]
		[JsonProperty("preview_image")]
		public FacebookImage PreviewImage { get; set; }

		[JsonProperty("is_downloaded")]
		[DataMember]
		public bool IsDownloaded { get; set; }

		[JsonConverter(typeof(StickersConverter))]
		[DataMember]
		[JsonProperty("stickers")]
		public List<Sticker> Stickers { get; set; }

		public StickerPack()
		{
		}

		public StickerPack(StickerPack stickerPack)
		{
			Id = stickerPack.Id;
			Name = stickerPack.Name;
			Artist = stickerPack.Artist;
			Description = stickerPack.Description;
			IsFeatured = stickerPack.IsFeatured;
			IsPromoted = stickerPack.IsPromoted;
			Price = stickerPack.Price;
			ThumbnailImage = stickerPack.ThumbnailImage;
			PreviewImage = stickerPack.PreviewImage;
			Stickers = stickerPack.Stickers;
			IsDownloaded = stickerPack.IsDownloaded;
		}
	}
}

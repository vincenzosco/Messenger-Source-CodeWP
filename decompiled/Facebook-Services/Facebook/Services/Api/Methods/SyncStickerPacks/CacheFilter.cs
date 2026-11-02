using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.SyncStickerPacks;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
public class CacheFilter : FacebookApiFilter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		Response response = await next.ExecuteAsync<Request, Response>(request);
		if (response.HasError())
		{
			return response;
		}
		StickerPackCache stickerCache = Context.StickerPackCache;
		bool previousHasNewStickerPacks = Context.FBContext.GetHasNewStickerPacks();
		List<StickerPack> cachedStickerPacks = await stickerCache.GetStickerPacks(StickerPackType.Available);
		IList<StickerPack> fetchedStickerPacks = response.AvailableStickerPacks;
		bool newStickerPacks = previousHasNewStickerPacks || fetchedStickerPacks.Except(cachedStickerPacks).Any();
		await stickerCache.Purge();
		await stickerCache.AddStickerPacks(fetchedStickerPacks, response.TrayStickerPacks, response.OwnedStickerPacks, newStickerPacks);
		await stickerCache.SetLastSyncTime(DateExtensions.MSecsSinceEpoch());
		return response;
	}
}

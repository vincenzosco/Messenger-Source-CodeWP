using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchStickerPacks;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
public class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		StickerPackCache cache = Context.StickerPackCache;
		Response response = new Response();
		List<StickerPack> stickerPacks = await cache.GetStickerPacks(request.StickerPackType);
		List<Sticker> list = ((request.StickerPackType == StickerPackType.Tray) ? (await cache.GetRecentStickers()) : null);
		List<Sticker> recentStickers = list;
		response.StickerPacks = stickerPacks;
		response.RecentStickers = recentStickers;
		return response;
	}
}

using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.FetchStickerPack;

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
		StickerPack stickerPack = await cache.GetStickerPackById(request.StickerPackId);
		return new Response
		{
			StickerPack = stickerPack
		};
	}
}

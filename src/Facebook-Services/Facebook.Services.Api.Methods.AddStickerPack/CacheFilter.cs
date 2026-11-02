using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;

namespace Facebook.Services.Api.Methods.AddStickerPack
{
	[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(BoolResponse))]
	internal class CacheFilter : Filter<Request, BoolResponse>
	{
		public CacheFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
		{
			BoolResponse response = await next.ExecuteAsync<Request, BoolResponse>(request);
			if (!response.HasError() && response.Succeeded)
			{
				List<StickerPack> newOwnedStickers = new List<StickerPack>
				{
					new StickerPack
					{
						Id = request.StickerPackId
					}
				};
				await Context.StickerPackCache.AddStickerPacks(newOwnedStickers, StickerPackType.Owned);
			}
			return response;
		}
	}
}

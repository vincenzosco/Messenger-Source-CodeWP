using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;

namespace Facebook.Services.Api.Methods.SetThreadMute;

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
		if (!response.HasError())
		{
			await Context.ThreadCache.MarkThreadMuteState(request.ThreadId, request.MuteUntil);
		}
		return response;
	}
}

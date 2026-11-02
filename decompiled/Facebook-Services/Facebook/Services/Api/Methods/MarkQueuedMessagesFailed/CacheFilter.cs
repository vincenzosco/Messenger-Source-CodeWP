using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.MarkQueuedMessagesFailed;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
public class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		await Context.ThreadCache.MarkAllQueuedMessagesFailed();
		return new Response();
	}
}

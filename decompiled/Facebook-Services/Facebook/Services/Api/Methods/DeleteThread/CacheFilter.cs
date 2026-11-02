using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.DeleteThread;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
public class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		if (string.IsNullOrWhiteSpace(request.ThreadId))
		{
			Response response = new Response();
			response.Error = new Error
			{
				Message = "Missing thread id"
			};
			return response;
		}
		Response response2 = await next.ExecuteAsync<Request, Response>(request);
		if (!response2.HasError())
		{
			await Context.ThreadCache.RemoveThreadAndPinnedThreadByThreadId(request.ThreadId);
		}
		return response2;
	}
}

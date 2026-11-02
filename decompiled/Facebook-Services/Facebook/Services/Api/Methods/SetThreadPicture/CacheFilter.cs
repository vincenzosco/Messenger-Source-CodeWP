using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.SetThreadPicture;

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
		if (!response.HasError() && !string.IsNullOrEmpty(response.PictureHash))
		{
			await Context.ThreadCache.SetThreadPicture(request.ThreadId, response.PictureHash);
		}
		return response;
	}
}

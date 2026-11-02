using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.AddThreadParticipants;

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
		if (!response.HasError())
		{
			await Context.ThreadCache.AddThreadParticipants(request.ThreadId, response.AddedParticipants);
		}
		return response;
	}
}

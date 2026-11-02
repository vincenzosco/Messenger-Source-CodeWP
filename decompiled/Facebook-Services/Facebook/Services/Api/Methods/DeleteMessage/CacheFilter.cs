using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.DeleteMessage;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
public class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		if (!request.IsLocalMessage)
		{
			Response nextResponse = await next.ExecuteAsync<Request, Response>(request);
			if (nextResponse.HasError())
			{
				return nextResponse;
			}
		}
		if (!request.HasValidMessageId())
		{
			Response response = new Response();
			response.Error = new Error
			{
				Message = "Missing message id"
			};
			return response;
		}
		await Context.ThreadCache.RemoveMessage(request.ThreadId, request.MessageId);
		return new Response();
	}
}

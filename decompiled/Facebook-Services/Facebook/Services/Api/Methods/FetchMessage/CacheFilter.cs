using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.FetchMessage;

[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
public class CacheFilter : Filter<Request, Response>
{
	public CacheFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		MailMessage message = await Context.ThreadCache.GetMessageByIdAndThreadId(request.ThreadId, request.MessageId, request.OfflineThreadingId);
		if (message == null)
		{
			Response response = await next.ExecuteAsync<Request, Response>(request);
			if (response.HasError())
			{
				return response;
			}
			message = response.Message;
		}
		return new Response
		{
			Message = message
		};
	}
}

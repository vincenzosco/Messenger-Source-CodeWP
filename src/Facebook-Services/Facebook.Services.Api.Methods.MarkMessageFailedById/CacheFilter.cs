using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.MarkMessageFailedById
{
	[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(Response))]
	public class CacheFilter : Filter<Request, Response>
	{
		public CacheFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			await Context.ThreadCache.UpdateThreadLastMessageState(request.ThreadId, request.OfflineThreadingId, MailMessage.MessageState.Failed);
			return new Response();
		}
	}
}

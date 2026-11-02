using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Services.Api.Methods.MarkMessagesSent
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
			Dictionary<string, List<string>> threadIdToRequestIds = new Dictionary<string, List<string>>();
			foreach (Facebook.Services.Api.Methods.SendMessage.Response sentMessage in request.SentMessages)
			{
				string threadId = sentMessage.ThreadId;
				if (!threadIdToRequestIds.ContainsKey(threadId))
				{
					threadIdToRequestIds.Add(threadId, new List<string>());
				}
				threadIdToRequestIds[threadId].Add(sentMessage.OfflineId);
			}
			await Context.ThreadCache.MarkMessageSent(threadIdToRequestIds);
			return new Response();
		}
	}
}

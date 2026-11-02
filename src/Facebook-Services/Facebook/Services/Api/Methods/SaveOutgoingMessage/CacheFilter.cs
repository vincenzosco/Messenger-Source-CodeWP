using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Services.Api.Methods.SaveOutgoingMessage
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
			SendEventLogger.LogQueueEvent(request, Context.AnalyticsService);
			Response response = new Response();
			string threadId;
			if (request.HasServerThreadId())
			{
				threadId = request.ServerThreadId;
			}
			else if (request.HasLocalThreadId())
			{
				threadId = request.LocalThreadId;
			}
			else
			{
				threadId = await SaveThreadIfNecessary(request);
				if (!string.IsNullOrWhiteSpace(threadId))
				{
					response.GeneratedLocalThreadId = threadId;
				}
			}
			if (string.IsNullOrWhiteSpace(threadId))
			{
				return response;
			}
			MailMessage message = Request.CreateStubMessage(request, Context.FBContext);
			message.ThreadId = threadId;
			message.State = MailMessage.MessageState.Queued;
			await Context.ThreadCache.AddMessage(message, false);
			return response;
		}

		private async Task<string> SaveThreadIfNecessary(Request request)
		{
			if (request.HasValidThreadId() || !request.IsValidSingleRecipientMessage())
			{
				return null;
			}
			string singleRecipientId = request.Users.First().Id;
			ThreadCache cache = Context.ThreadCache;
			MailThread thread = await cache.GetThreadBySingleRecipientId(singleRecipientId);
			if (thread != null && !thread.IsThreadOffline())
			{
				return thread.Id;
			}
			FacebookSession session = Context.FBContext.Session;
			thread = OfflineThreadUtil.CreateThread(request, singleRecipientId, session.UserId, session.Profile.Name);
			await cache.AddThread(thread);
			return thread.Id;
		}
	}
}

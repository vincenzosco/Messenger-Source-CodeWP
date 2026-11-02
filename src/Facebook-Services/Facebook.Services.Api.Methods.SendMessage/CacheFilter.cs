using System;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.SendMessage
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
			SendEventLogger.LogSendEvent(request, Context.AnalyticsService);
			await MarkMessageState(request, MailMessage.MessageState.Queued);
			return await Next(request, next);
		}

		private async Task MarkMessageState(Request request, MailMessage.MessageState state)
		{
			if (request.HasValidThreadId())
			{
				await Context.ThreadCache.UpdateThreadLastMessageState(request.GetValidThreadId(), request.OfflineThreadingId, state);
			}
		}

		private static async Task<Response> Next(Request request, IHandler next)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				return await next.ExecuteAsync<Request, Response>(request);
			}
			catch (Exception e)
			{
				Response response = new Response();
				response.Error = Error.FromException(e);
				return response;
			}
		}
	}
}

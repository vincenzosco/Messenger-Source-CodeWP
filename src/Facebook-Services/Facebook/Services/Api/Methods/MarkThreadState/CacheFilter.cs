using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.Api.Methods.MarkThreadState
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
			Response response = await next.ExecuteAsync<Request, Response>(request);
			await HandleThreadActionInCache(request, Context.ThreadCache);
			if (response.HasError())
			{
				Logger.Warning(this, $"Request failed - ThreadId: {request.ThreadId}, Tag: {request.Tag}, Enabled: {request.Enabled}, Error: {response.Error}", "ExecuteAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Methods\\MarkThreadState\\CacheFilter.cs", 27);
			}
			return response;
		}

		private static async Task HandleThreadActionInCache(Request request, ThreadCache cache)
		{
			string threadId = request.ThreadId;
			if (request.Tag == MailThread.Tag.Read)
			{
				Task task = (request.Enabled ? cache.MarkThreadAsRead(threadId) : cache.MarkThreadAsUnread(threadId));
				await task;
			}
			if (request.ActionRemovesThread())
			{
				Task task2 = ((request.Tag != MailThread.Tag.Archived) ? cache.RemoveThreadAndPinnedThreadByThreadId(threadId) : cache.RemoveThreadByThreadId(threadId));
				await task2;
			}
		}
	}
}

using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;

namespace Facebook.Services.Api.Methods.LeaveThread
{
	[Bundle(BundleType.InMemoryCache, typeof(Request), typeof(BoolResponse))]
	public class CacheFilter : Filter<Request, BoolResponse>
	{
		public CacheFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
		{
			if (string.IsNullOrWhiteSpace(request.ThreadId))
			{
				BoolResponse boolResponse = new BoolResponse();
				boolResponse.Error = new Error
				{
					Message = "Missing thread id"
				};
				return boolResponse;
			}
			BoolResponse response = await next.ExecuteAsync<Request, BoolResponse>(request);
			if (response.Succeeded)
			{
				await Context.ThreadCache.RemoveThreadAndPinnedThreadByThreadId(request.ThreadId);
			}
			return response;
		}
	}
}

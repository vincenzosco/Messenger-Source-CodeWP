using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;

namespace Facebook.Services.Api.Methods.RemoveThreadParticipants
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
			if (string.IsNullOrWhiteSpace(request.ThreadId) || !request.UserIds.Any())
			{
				BoolResponse boolResponse = new BoolResponse();
				boolResponse.Error = new Error
				{
					Message = "Missing ThreadId or UserIds"
				};
				return boolResponse;
			}
			BoolResponse response = await next.ExecuteAsync<Request, BoolResponse>(request);
			if (!response.HasError())
			{
				await Context.ThreadCache.RemoveThreadParticipants(request.ThreadId, request.UserIds);
			}
			return response;
		}
	}
}

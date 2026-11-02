using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;

namespace Facebook.Services.Api.Methods.FetchMessages
{
	[Bundle(BundleType.PreCache, typeof(Request), typeof(Response))]
	internal class SendFilter : Filter<Request, Response>
	{
		public SendFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			bool isFirstPage = IsFirstPageRequest(request);
			Response response = await next.ExecuteAsync<Request, Response>(request);
			if (response.HasError() || !isFirstPage)
			{
				return response;
			}
			Context.MessageSender.GetContext(request.ThreadId);
			return response;
		}

		private bool IsFirstPageRequest(Request request)
		{
			if (request.ActionId == 0)
			{
				return request.MaxTimestamp == 0;
			}
			return false;
		}
	}
}

using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchFriendLastActive
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			string query = string.Format("SELECT last_active, is_pushable, is_messenger_user FROM user WHERE uid = {0}", new object[1] { request.Id });
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", query);
			return await ExecuteHttpRequestAndParseResponse<Response>(http);
		}
	}
}

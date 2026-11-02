using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchPresence
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
			string query = "SELECT uid, name, last_active from user where online_presence = 'active' and  uid in (select uid2 from friend where uid1 = me())";
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", query);
			return await ExecuteHttpRequestAndParseResponse<Response>(http);
		}
	}
}

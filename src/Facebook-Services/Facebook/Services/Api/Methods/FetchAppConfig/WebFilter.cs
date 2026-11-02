using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchAppConfig
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
			string query = $"SELECT client_config FROM application WHERE app_id='{Context.Config.ApplicationId}'";
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", query);
			return await ExecuteHttpRequestAndParseResponse<Response>(http);
		}
	}
}

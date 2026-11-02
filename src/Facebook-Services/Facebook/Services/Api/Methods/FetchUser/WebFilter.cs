using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchUser
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
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", "select uid, name, phones, affiliations from user where uid = me()");
			return await ExecuteHttpRequestAndParseResponse<Response>(http);
		}
	}
}

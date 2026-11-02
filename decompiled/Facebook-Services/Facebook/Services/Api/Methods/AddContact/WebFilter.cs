using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.AddContact;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request http = CreateGraphRequest(request, "me/contacts").SetHttpMethod(Method.Post).AddParam("phone", request.PhoneNumber).AddParam("country_code", request.CountryCode);
		return await ExecuteHttpRequestAndParseResponse<Response>(http);
	}
}

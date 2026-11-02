using System.Linq;
using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchSearchability;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	private const string PrivacyTypeEveryone = "EVERYONE";

	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", "select id, type from privacy_option where user_id = me() and privacy_field ='search_by_phone' and is_currently_selected=1");
		Response response = await ExecuteHttpRequestAndParseResponse<Response>(http);
		if (!response.HasError() || (response.PrivacyOptions != null && response.PrivacyOptions.Any()))
		{
			response.Searchability = ((!response.PrivacyOptions.FirstOrDefault().Type.Equals("EVERYONE")) ? PhoneSearchabilityTypes.Self : PhoneSearchabilityTypes.Everyone);
		}
		return response;
	}
}

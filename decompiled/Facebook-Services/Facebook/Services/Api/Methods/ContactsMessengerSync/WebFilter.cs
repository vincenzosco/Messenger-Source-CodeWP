using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.ContactsMessengerSync;

[Bundle(BundleType.Web, typeof(Request), typeof(BoolResponse))]
public class WebFilter : FacebookApiFilter<Request, BoolResponse>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<BoolResponse> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request httpRequest = CreateGraphRequest(request, "me/contactsmessengersync");
		httpRequest.Method = Method.Post;
		httpRequest.AddParam("enabled", request.Enabled.ToString());
		Response response = await ExecuteHttpRequest(httpRequest);
		if (response.HasError())
		{
			BoolResponse boolResponse = new BoolResponse();
			boolResponse.Error = response.Error;
			return boolResponse;
		}
		bool success = bool.TryParse(response.DataAsString(), out success);
		return new BoolResponse
		{
			Succeeded = success
		};
	}
}

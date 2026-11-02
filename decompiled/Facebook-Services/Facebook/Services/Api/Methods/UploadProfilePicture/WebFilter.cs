using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.UploadProfilePicture;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		Facebook.Services.Http.Request httpRequest = CreateGraphRequest(request, "me/picture").SetHttpMethod(Method.Post).AddParam("image", request.ImageUrl, "application/octet-stream", "Image");
		return await ExecuteHttpRequestAndParseResponse<Response>(httpRequest);
	}
}

using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.DeleteContact
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
			Facebook.Services.Http.Request graphRequest = CreateGraphRequest(request, request.ContactWriteId).SetHttpMethod(Method.Delete).AddParam("contactWriteId", request.ContactWriteId).AddParam("contactId", request.ContactId);
			Facebook.Services.Http.Response raw = await ExecuteHttpRequest(graphRequest);
			if (raw.HasError())
			{
				Response response = new Response();
				response.Error = raw.Error;
				return response;
			}
			return new Response();
		}
	}
}

using System.Threading.Tasks;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SyncContacts
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
			Facebook.Services.Http.Request graph = CreateGraphRequest(request, "me/bulkcontacts").SetHttpMethod(Method.Post).AddParam("contact_changes", JsonConvert.SerializeObject(request.ContactChanges));
			Facebook.Services.Http.Response raw = await ExecuteHttpRequest(graph);
			if (raw.HasError())
			{
				return new Response
				{
					Error = raw.Error
				};
			}
			return ParseResponse<Response>(raw);
		}
	}
}

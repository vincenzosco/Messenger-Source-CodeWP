using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.MarkThreadState
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	internal class WebFilter : FacebookApiFilter<Request, Response>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request graph = CreateGraphRequest(request, "tags").SetHttpMethod(Method.Post).AddParam("id", request.ThreadId).AddParam("state", request.Enabled ? "1" : "0")
				.AddParam("name", TagToString(request.Tag));
			Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(graph);
			if (httpResponse.HasError())
			{
				Response response = new Response();
				response.Error = httpResponse.Error;
				return response;
			}
			bool success;
			if (bool.TryParse(httpResponse.DataAsString(), out success))
			{
				Response response2 = new Response();
				response2.Success = success;
				return response2;
			}
			return new Response
			{
				Success = false
			};
		}

		private string TagToString(MailThread.Tag tag)
		{
			switch (tag)
			{
			case MailThread.Tag.Archived:
				return "archived";
			case MailThread.Tag.Inbox:
				return "inbox";
			case MailThread.Tag.Other:
				return "other";
			case MailThread.Tag.Read:
				return "read";
			case MailThread.Tag.Spam:
				return "spam";
			default:
				return "";
			}
		}
	}
}

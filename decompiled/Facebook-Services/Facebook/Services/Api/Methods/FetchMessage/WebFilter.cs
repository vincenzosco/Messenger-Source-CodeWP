using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchMessage
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		private const string MessagesDictionaryKey = "messages";

		private const string UsersDictionaryKey = "users";

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			string query = UnifiedMessage.GetIndividual(request.MessageId, request.ThreadId, true);
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", query);
			Facebook.Services.Http.Response response = await ExecuteHttpRequest(http);
			if (response.HasError())
			{
				Response response2 = new Response();
				response2.Error = response.Error;
				return response2;
			}
			Dictionary<string, Type> mapping = new Dictionary<string, Type>
			{
				{
					"messages",
					typeof(MailMessage)
				},
				{
					"users",
					typeof(FacebookUser)
				}
			};
			MultiQueryResponse fql = Facebook.Services.Api.Fql.Common.ParseMulti(mapping, response.DataAsString());
			MailMessage message = fql.Get<MailMessage>("messages").FirstOrDefault();
			List<FacebookUser> users = fql.Get<FacebookUser>("users");
			ParseUtils.PatchUser(details: ParseUtils.CreateIdMap(users), u: message.Sender);
			message.Source = MailMessage.MessageSource.Fetch;
			return new Response
			{
				Message = message
			};
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchThread;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		string query = request.KeyType switch
		{
			Request.RequestKeyType.Recipient => UnifiedThread.GetThreadByRecipient(request.RecipientId), 
			Request.RequestKeyType.ThreadFbid => UnifiedThread.GetThreadByFbid(request.ThreadFbid), 
			Request.RequestKeyType.ThreadId => UnifiedThread.GetThreadById(request.ThreadId), 
			_ => throw new NotImplementedException(), 
		};
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
				"threads",
				typeof(MailThread)
			},
			{
				"users",
				typeof(FacebookUser)
			},
			{
				"me",
				typeof(FacebookProfile)
			}
		};
		MultiQueryResponse fql = Facebook.Services.Api.Fql.Common.ParseMulti(mapping, response.DataAsString());
		if (fql.HasError())
		{
			Response response3 = new Response();
			response3.Error = fql.Error;
			return response3;
		}
		List<FacebookUser> users = fql.Get<FacebookUser>("users");
		List<MailThread> threads = fql.Get<MailThread>("threads");
		List<FacebookProfile> me = fql.Get<FacebookProfile>("me");
		Dictionary<string, FacebookUser> userMap = ParseUtils.CreateIdMap(users);
		foreach (MailThread item in threads)
		{
			ParseUtils.PatchUsers(item.FormerParticipants, userMap);
			ParseUtils.PatchUsers(item.Participants, userMap);
			ParseUtils.UpdateMuteUntil(item, me);
		}
		return new Response
		{
			Thread = threads.FirstOrDefault()
		};
	}
}

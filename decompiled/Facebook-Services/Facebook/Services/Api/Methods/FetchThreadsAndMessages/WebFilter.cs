using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchThreadsAndMessages;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	private static readonly Dictionary<string, Type> QueryMapping = new Dictionary<string, Type>
	{
		{
			"threads",
			typeof(MailThread)
		},
		{
			"me",
			typeof(FacebookProfile)
		},
		{
			"messages",
			typeof(MailMessage)
		},
		{
			"users",
			typeof(FacebookUser)
		}
	};

	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		Dictionary<string, string> threadQueryMap = (request.HasThreads ? UnifiedThread.CreateThreadsQueryMap(request.ThreadFbIds, request.ThreadRecipientIds) : new Dictionary<string, string>());
		Dictionary<string, string> messageQueryMap = (request.HasMessages ? UnifiedMessage.CreateMessagesQueryMap(request.MessageIds) : new Dictionary<string, string>());
		Dictionary<string, string> queryMap = threadQueryMap.Union(messageQueryMap).ToDictionary((KeyValuePair<string, string> k) => k.Key, (KeyValuePair<string, string> v) => v.Value);
		queryMap.Add("users", GetUserQuery(request));
		string query = Facebook.Services.Api.Fql.Common.CreateMultiQuery(queryMap);
		Facebook.Services.Http.Request httpRequest = CreateGraphRequest(request, "fql").AddParam("q", query);
		Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(httpRequest);
		if (httpResponse.HasError())
		{
			Response response = new Response();
			response.Error = httpResponse.Error;
			return response;
		}
		MultiQueryResponse fql = Facebook.Services.Api.Fql.Common.ParseMulti(QueryMapping, httpResponse.DataAsString());
		if (fql.HasError())
		{
			Response response2 = new Response();
			response2.Error = fql.Error;
			return response2;
		}
		List<MailThread> threads = fql.Get<MailThread>("threads");
		List<FacebookProfile> me = fql.Get<FacebookProfile>("me");
		List<MailMessage> messages = fql.Get<MailMessage>("messages");
		List<FacebookUser> users = fql.Get<FacebookUser>("users");
		Dictionary<string, FacebookUser> userMap = ParseUtils.CreateIdMap(users);
		foreach (MailThread item in threads)
		{
			ParseUtils.PatchUsers(item.FormerParticipants, userMap);
			ParseUtils.PatchUsers(item.Participants, userMap);
			ParseUtils.UpdateMuteUntil(item, me);
		}
		foreach (MailMessage item2 in messages)
		{
			ParseUtils.PatchUser(item2.Sender, userMap);
			item2.Source = MailMessage.MessageSource.Fetch;
		}
		return new Response
		{
			Threads = threads,
			Messages = messages
		};
	}

	private static string GetUserQuery(Request request)
	{
		List<string> list = new List<string>();
		if (request.HasThreads)
		{
			list.AddRange(UnifiedThread.GetUserWhereClauses());
		}
		if (request.HasMessages)
		{
			list.Add(UnifiedMessage.GetUserWhereClause());
		}
		return UnifiedThread.GetUsers(string.Join(" OR ", list));
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchMessages
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		private const int MaxIncrementalLimit = 50;

		private const string FetchThreadErrorMessage = "Request thread info failed, no thread returned.";

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			bool incremental = request.IsIncremental();
			Logger.Debug(this, "Fetching messages from web " + (incremental ? "incrementally" : "with timestamp"), "ExecuteAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Methods\\FetchMessages\\WebFilter.cs", 28);
			string query = (incremental ? UnifiedMessage.Sync(request.ThreadId, request.ActionId, 50, request.FetchAllThreadInfo) : UnifiedMessage.Get(request.ThreadId, request.MaxTimestamp, 10, request.FetchAllThreadInfo));
			Response response = await PerformRequest(request, query, incremental);
			if (response.HasError())
			{
				return response;
			}
			if (response.Messages.Count >= 50 && incremental)
			{
				query = UnifiedMessage.Get(request.ThreadId, 0L);
				response = await PerformRequest(request, query, false);
				response.ShouldPurgeMessageCache = true;
			}
			return response;
		}

		private async Task<Response> PerformRequest(Request request, string query, bool isIncremental)
		{
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
					"actions",
					typeof(MailThreadAction)
				},
				{
					"thread",
					typeof(MailThread)
				},
				{
					"users",
					typeof(FacebookUser)
				}
			};
			MultiQueryResponse fql = Facebook.Services.Api.Fql.Common.ParseMulti(mapping, response.DataAsString());
			List<MailMessage> messages = fql.Get<MailMessage>("messages");
			List<MailThreadAction> actions = fql.Get<MailThreadAction>("actions");
			List<MailThread> threads = fql.Get<MailThread>("thread");
			List<FacebookUser> users = fql.Get<FacebookUser>("users");
			Dictionary<string, FacebookUser> userMap = ParseUtils.CreateIdMap(users);
			messages.AddRange(actions);
			foreach (MailMessage item in messages)
			{
				item.Source = MailMessage.MessageSource.Fetch;
				ParseUtils.PatchUser(item.Sender, userMap);
			}
			List<ReadReceipt> receipts = ((threads.Count > 0) ? threads[0].ReadReceipts : new List<ReadReceipt>());
			MailThread thread = ((threads.Count > 0) ? threads[0] : null);
			if (thread != null)
			{
				ParseUtils.PatchUsers(thread.Participants, userMap);
				ParseUtils.PatchUsers(thread.FormerParticipants, userMap);
			}
			else if (request.FetchAllThreadInfo)
			{
				Logger.Warning(this, "Request thread info failed, no thread returned.", "PerformRequest", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Methods\\FetchMessages\\WebFilter.cs", 93);
				Response response3 = new Response();
				response3.Error = new Error
				{
					Message = "Request thread info failed, no thread returned."
				};
				return response3;
			}
			int limit = (isIncremental ? int.MaxValue : 10);
			return new Response
			{
				Messages = messages.OrderBy((MailMessage m) => m, MailMessageNewestFirstComparer.Comparer).Take(limit).ToList(),
				Receipts = receipts,
				Thread = thread,
				ReceivedMessagesFromWeb = true
			};
		}
	}
}

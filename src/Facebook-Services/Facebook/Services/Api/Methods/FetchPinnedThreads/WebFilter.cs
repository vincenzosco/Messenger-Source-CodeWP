using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchPinnedThreads
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
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", UnifiedThread.GetPinnedGroups());
			Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(http);
			if (httpResponse.HasError())
			{
				Response response = new Response();
				response.Error = httpResponse.Error;
				return response;
			}
			Dictionary<string, Type> mapping = new Dictionary<string, Type>
			{
				{
					"pinned",
					typeof(PinnedThread)
				},
				{
					"threads",
					typeof(MailThread)
				},
				{
					"users",
					typeof(FacebookUser)
				}
			};
			MultiQueryResponse fql = Facebook.Services.Api.Fql.Common.ParseMulti(mapping, httpResponse.DataAsString());
			if (fql.HasError())
			{
				Response response2 = new Response();
				response2.Error = fql.Error;
				return response2;
			}
			List<PinnedThread> pinned = fql.Get<PinnedThread>("pinned");
			List<MailThread> threads = fql.Get<MailThread>("threads");
			List<FacebookUser> users = fql.Get<FacebookUser>("users");
			Dictionary<string, FacebookUser> userMap = CreateIdMap(users);
			foreach (MailThread item in threads)
			{
				PatchUsers(item.Participants, userMap);
			}
			List<PinnedThread> validPins = new List<PinnedThread>();
			int position = 0;
			foreach (PinnedThread pin in pinned)
			{
				Func<MailThread, bool> predicate = (MailThread t) => t.Id == pin.Id;
				MailThread mailThread = threads.FirstOrDefault(predicate);
				if (mailThread != null)
				{
					validPins.Add(new PinnedThread(mailThread)
					{
						Position = position
					});
					position++;
				}
			}
			return new Response
			{
				PinnedThreads = validPins
			};
		}

		private static Dictionary<string, FacebookUser> CreateIdMap(IEnumerable<FacebookUser> profiles)
		{
			if (profiles == null)
			{
				return new Dictionary<string, FacebookUser>();
			}
			return profiles.ToDictionary((FacebookUser p) => p.Id, (FacebookUser p) => p);
		}

		private static void PatchUsers(IList<MailUser> users, Dictionary<string, FacebookUser> details)
		{
			if (users == null || details == null)
			{
				return;
			}
			foreach (MailUser user in users)
			{
				if (details.ContainsKey(user.Id))
				{
					FacebookUser facebookUser = details[user.Id];
					user.Name = facebookUser.Name;
					user.FirstName = facebookUser.FirstName;
					user.MiddleName = facebookUser.MiddleName;
					user.LastName = facebookUser.LastName;
					user.Pushable = facebookUser.Pushable;
					user.IsMessengerUser = facebookUser.IsMessengerUser;
					user.IsPartial = facebookUser.IsPartial;
					user.LastActive = facebookUser.LastActive;
				}
			}
		}
	}
}

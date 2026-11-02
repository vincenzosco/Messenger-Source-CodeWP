using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchGroupThreads
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
			string query = UnifiedThread.GetGroupThreads(DateTime.Now.AddDays(-7.0).ToUnixTimeMilliseconds());
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", query);
			Facebook.Services.Http.Response response = await ExecuteHttpRequest(http);
			if (response == null)
			{
				return new Response
				{
					Error = new Error
					{
						Message = "Zero length response from server"
					}
				};
			}
			if (response.HasError())
			{
				return new Response
				{
					Error = response.Error
				};
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
				}
			};
			MultiQueryResponse fql = Facebook.Services.Api.Fql.Common.ParseMulti(mapping, response.DataAsString());
			if (fql.HasError())
			{
				Response response2 = new Response();
				response2.Error = fql.Error;
				return response2;
			}
			List<FacebookUser> users = fql.Get<FacebookUser>("users");
			List<MailThread> threads = fql.Get<MailThread>("threads");
			Dictionary<string, FacebookUser> userMap = CreateIdMap(users);
			foreach (MailThread item in threads)
			{
				PatchUsers(item.FormerParticipants, userMap);
				PatchUsers(item.Participants, userMap);
			}
			return new Response
			{
				Threads = threads
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
					user.IsPartial = facebookUser.IsPartial;
					user.LastActive = facebookUser.LastActive;
				}
			}
		}
	}
}

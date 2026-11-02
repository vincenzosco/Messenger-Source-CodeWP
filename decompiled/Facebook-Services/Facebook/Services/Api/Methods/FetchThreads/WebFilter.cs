using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchThreads
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		private const int MaxIncrementalLimit = 50;

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			string query = (request.IsIncremental() ? UnifiedThread.Sync("inbox", request.ActionId, 50) : UnifiedThread.Get("inbox", request.MaxTimestamp, request.ThreadLimit));
			Response response = await PerformRequest(request, query);
			if (response.HasError())
			{
				return response;
			}
			if (response.Threads.Count >= 50 && request.IsIncremental())
			{
				query = UnifiedThread.Get("inbox", 0L);
				response = await PerformRequest(request, query);
				response.ShouldPurgeThreadCache = true;
			}
			return response;
		}

		private async Task<Response> PerformRequest(Request request, string query)
		{
			Logger.Debug(this, "Fetching threads from web " + (request.IsIncremental() ? "incrementally" : "non-incrementally"), "PerformRequest", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Methods\\FetchThreads\\WebFilter.cs", 47);
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", query);
			Facebook.Services.Http.Response response = await ExecuteHttpRequest(http);
			if (response.HasError())
			{
				Response response2 = new Response();
				response2.Error = response.Error;
				return response2;
			}
			return CreateResponse(response, request);
		}

		private Response CreateResponse(Facebook.Services.Http.Response response, Request request)
		{
			Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
			dictionary.Add("settings", typeof(FacebookSettings));
			dictionary.Add("threads", typeof(MailThread));
			dictionary.Add("users", typeof(FacebookUser));
			dictionary.Add("counts", typeof(MailFolderCount));
			dictionary.Add("me", typeof(FacebookProfile));
			Dictionary<string, Type> mapping = dictionary;
			MultiQueryResponse multiQueryResponse = Facebook.Services.Api.Fql.Common.ParseMulti(mapping, response.DataAsString());
			List<FacebookUser> profiles = multiQueryResponse.Get<FacebookUser>("users");
			List<MailThread> list = multiQueryResponse.Get<MailThread>("threads");
			List<FacebookProfile> profiles2 = multiQueryResponse.Get<FacebookProfile>("me");
			List<MailFolderCount> source = multiQueryResponse.Get<MailFolderCount>("counts");
			List<FacebookSettings> settings = multiQueryResponse.Get<FacebookSettings>("settings");
			ParseSettings(settings);
			Dictionary<string, FacebookUser> details = CreateIdMap(profiles);
			foreach (MailThread item in list)
			{
				PatchUsers(item.FormerParticipants, details);
				PatchUsers(item.Participants, details);
				UpdateMuteUntil(item, profiles2);
			}
			Response response2 = new Response();
			response2.Threads = list;
			response2.Counts = source.FirstOrDefault((MailFolderCount count) => count.Folder == request.Folder);
			response2.ReceivedThreadsFromWebOrSync = true;
			return response2;
		}

		private void ParseSettings(List<FacebookSettings> settings)
		{
			foreach (FacebookSettings setting in settings)
			{
				if (setting.Setting.Equals("mute_until"))
				{
					long result;
					long.TryParse(setting.Value, out result);
					if (!Context.FBContext.GetGlobalMuteDuration().Equals(result))
					{
						Context.FBContext.SetGlobalMuteDuration(result);
					}
				}
			}
		}

		private static Dictionary<string, FacebookUser> CreateIdMap(IEnumerable<FacebookUser> profiles)
		{
			if (profiles == null)
			{
				return new Dictionary<string, FacebookUser>();
			}
			return profiles.ToDictionary((FacebookUser p) => p.Id, (FacebookUser p) => p);
		}

		private static void UpdateMuteUntil(MailThread thread, List<FacebookProfile> profiles)
		{
			if (profiles != null && profiles.Any())
			{
				string id = profiles[0].Id;
				if (thread.MuteMap.ContainsKey(id))
				{
					thread.MuteUntil = thread.MuteMap[id];
				}
			}
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

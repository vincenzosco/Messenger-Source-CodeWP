using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SyncFriends
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		private static string[] FriendFields = new string[2] { "uid2", "communication_rank" };

		private static string[] FriendSyncFields = new string[2] { "uid2", "is_removed" };

		private static string[] UserFields = new string[12]
		{
			"uid", "name", "first_name", "middle_name", "last_name", "contact_email", "phones", "profile_url", "is_pushable", "is_messenger_user",
			"is_partial", "birthday_date"
		};

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			string fql = ((request.LastUpdated == 0) ? GetFriendInfoFql(request.Limit, request.Offset) : GetFriendInfoFql(request.LastUpdated));
			Facebook.Services.Http.Request http = CreateGraphRequest(request, "fql").AddParam("q", fql);
			Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(http);
			if (httpResponse.HasError())
			{
				return new Response
				{
					Error = httpResponse.Error
				};
			}
			MultiQueryResponse multiResponse = Facebook.Services.Api.Fql.Common.ParseMulti(new Dictionary<string, Type>
			{
				{
					"friends",
					typeof(Friend)
				},
				{
					"users",
					typeof(FacebookUser)
				},
				{
					"ranks",
					typeof(Rank)
				}
			}, httpResponse.DataAsString());
			if (multiResponse.HasError())
			{
				Response response = new Response();
				response.Error = multiResponse.Error;
				return response;
			}
			List<Friend> friends = multiResponse.Get<Friend>("friends");
			List<FacebookUser> users = multiResponse.Get<FacebookUser>("users");
			List<Rank> ranks = multiResponse.Get<Rank>("ranks");
			return CreateResponse(friends, users, ranks);
		}

		private Response CreateResponse(List<Friend> friends, List<FacebookUser> users, List<Rank> ranks)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			Dictionary<string, double> dictionary2 = new Dictionary<string, double>();
			foreach (Friend friend in friends)
			{
				dictionary2.Add(friend.Uid2, friend.CommunicationRank);
				if (friend.IsRemoved)
				{
					dictionary.Add(friend.Uid2, friend.IsRemoved);
				}
			}
			if (ranks != null && ranks.Any())
			{
				foreach (Rank rank in ranks)
				{
					if (dictionary2.ContainsKey(rank.Uid2))
					{
						dictionary2[rank.Uid2] = rank.CommunicationRank;
					}
					else
					{
						dictionary2.Add(rank.Uid2, rank.CommunicationRank);
					}
				}
			}
			List<FacebookUser> list = new List<FacebookUser>();
			List<FacebookUser> list2 = new List<FacebookUser>();
			foreach (FacebookUser user in users)
			{
				string key = user.Id.ToString();
				if (dictionary2.ContainsKey(key))
				{
					user.CommunicationRank = dictionary2[key];
				}
				if (dictionary.ContainsKey(key))
				{
					list2.Add(user);
				}
				else
				{
					list.Add(user);
				}
			}
			Response response = new Response();
			response.NewFriends = list;
			response.DeletedFriends = list2;
			return response;
		}

		private string GetFriendInfoFql(long lastUpdated)
		{
			string text = (lastUpdated / 1000).ToString();
			string text2 = $"uid1 = me() and update_time >= {text}";
			string value = Facebook.Services.Api.Fql.Common.Create(FriendSyncFields, "friend_sync", text2);
			string value2 = Facebook.Services.Api.Fql.Common.Create(UserFields, "user", "uid IN (SELECT uid2 from #friends)");
			string value3 = Facebook.Services.Api.Fql.Common.Create(FriendFields, "friend", "uid1 = me() AND uid2 IN (SELECT uid2 from #friends)");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("friends", value);
			dictionary.Add("users", value2);
			dictionary.Add("ranks", value3);
			return JsonConvert.SerializeObject(dictionary);
		}

		private string GetFriendInfoFql(int limit, int offset)
		{
			string value = Facebook.Services.Api.Fql.Common.Create(FriendFields, "friend", "uid1 = me() ORDER BY communication_rank DESC", limit, offset);
			string value2 = Facebook.Services.Api.Fql.Common.Create(UserFields, "user", "uid IN (SELECT uid2 from #friends)");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("friends", value);
			dictionary.Add("users", value2);
			return JsonConvert.SerializeObject(dictionary);
		}
	}
}

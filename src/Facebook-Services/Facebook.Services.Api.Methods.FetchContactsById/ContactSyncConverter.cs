using System;
using System.Collections.Generic;
using Facebook.Base;
using Facebook.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Api.Methods.FetchContactsById
{
	public class ContactSyncConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Response response = new Response();
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject jObject = JObject.Load(reader);
				List<FacebookUser> facebookUsers = GetFacebookUsers(jObject);
				if (facebookUsers != null)
				{
					response.FetchedUsers = facebookUsers;
				}
				else
				{
					response = FacebookApiResponse.FromApiResponse<Response>(jObject.ToObject<FacebookApiResponse>());
				}
			}
			else
			{
				serializer.Populate(reader, response);
			}
			return response;
		}

		private List<FacebookUser> GetFacebookUsers(JObject jObject)
		{
			List<FacebookUser> list = new List<FacebookUser>();
			foreach (JToken item in jObject.Values())
			{
				JToken nestedDescendant = item.ToObject<JObject>().GetNestedDescendant("represented_profile");
				if (!nestedDescendant.HasValues)
				{
					continue;
				}
				FacebookUser facebookUser = new FacebookUser();
				facebookUser.Id = nestedDescendant.ToObject<JObject>().GetNestedDescendant("id").ToString();
				facebookUser.GraphApiWriteId = item.ToObject<JObject>().GetNestedDescendant("graph_api_write_id").ToString();
				facebookUser.Name = item.ToObject<JObject>().GetNestedDescendant("structured_name", "text").ToString();
				JToken nestedDescendant2 = item.ToObject<JObject>().GetNestedDescendant("structured_name", "parts");
				foreach (JToken item2 in (IEnumerable<JToken>)nestedDescendant2)
				{
					int startIndex = item2["offset"].ToObject<int>();
					int length = item2["length"].ToObject<int>();
					if (item2["part"].ToString() == "first")
					{
						facebookUser.FirstName = facebookUser.Name.Substring(startIndex, length);
					}
					else if (item2["part"].ToString() == "middle")
					{
						facebookUser.MiddleName = facebookUser.Name.Substring(startIndex, length);
					}
					else if (item2["part"].ToString() == "last")
					{
						facebookUser.LastName = facebookUser.Name.Substring(startIndex, length);
					}
				}
				facebookUser.Phones = new List<PhoneInfo>();
				JToken nestedDescendant3 = item.ToObject<JObject>().GetNestedDescendant("phone_entries");
				foreach (JToken item3 in (IEnumerable<JToken>)nestedDescendant3)
				{
					PhoneInfo phoneInfo = new PhoneInfo();
					phoneInfo.CountryCode = item3.ToObject<JObject>().GetNestedDescendant("primary_field", "phone", "country_code").ToString();
					phoneInfo.Number = item3.ToObject<JObject>().GetNestedDescendant("primary_field", "phone", "display_number").ToString();
					facebookUser.Phones.Add(phoneInfo);
				}
				facebookUser.Pushable = item.ToObject<JObject>().GetNestedDescendant("represented_profile", "is_mobile_pushable").ToObject<bool>();
				facebookUser.IsMessengerUser = item.ToObject<JObject>().GetNestedDescendant("represented_profile", "is_messenger_user").ToObject<bool>();
				facebookUser.IsPartial = item.ToObject<JObject>().GetNestedDescendant("represented_profile", "is_partial").ToObject<bool>();
				string text = item.ToObject<JObject>().GetNestedDescendant("represented_profile", "friendship_status").ToObject<string>();
				facebookUser.IsPhoneOnlyContact = !text.Equals("are_friends", StringComparison.OrdinalIgnoreCase);
				list.Add(facebookUser);
			}
			return list;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}

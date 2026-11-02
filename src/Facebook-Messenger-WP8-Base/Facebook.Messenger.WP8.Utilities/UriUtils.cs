using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Facebook.Base;
using Facebook.Messenger.Navigation;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Utilities
{
	public class UriUtils
	{
		public static Uri BuildDeepLinkUrl(string url)
		{
			if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
			{
				return null;
			}
			FacebookConfig instance = ServiceLocator.Current.GetInstance<FacebookConfig>();
			FacebookContext instance2 = ServiceLocator.Current.GetInstance<FacebookContext>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(instance.MobileApiUrl).Append("auth.php");
			UriBuilder uriBuilder = new UriBuilder(stringBuilder.ToString());
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (instance2.IsAuthenticated())
			{
				dictionary.Add("api_key", instance.ApplicationId);
				dictionary.Add("session_key", instance2.Session.Credentials.SessionKey);
				dictionary.Add("t", DateTime.UtcNow.ToUnixTimeSeconds().ToString());
				dictionary.Add("uid", instance2.Session.UserId);
				dictionary.Add("url", Uri.EscapeUriString(url));
				string value = BuildSignature(dictionary, instance2.Session.Credentials.Secret);
				dictionary.Add("sig", value);
				uriBuilder.Query = BuildQueryString(dictionary);
			}
			return uriBuilder.Uri;
		}

		public static string BuildQueryString(Dictionary<string, string> dictionary)
		{
			return string.Join("&", QueryParamList(dictionary, true));
		}

		public static string StripQueryString(string uri)
		{
			int num = uri.IndexOf('?');
			if (num >= 0)
			{
				return uri.Substring(0, num);
			}
			return uri;
		}

		public static UriQuery ParseQueryString(string queryString)
		{
			UriQuery uriQuery = new UriQuery();
			queryString = queryString.TrimStart('?');
			string[] array = queryString.Split(new char[1] { '&' }, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length == 2)
				{
					string key = Url.Decode(array3[0]);
					string value = Url.Decode(array3[1]);
					uriQuery[key] = value;
				}
			}
			return uriQuery;
		}

		public static UriQuery ParseQueryStringFromUri(string uri)
		{
			int num = uri.IndexOf('?');
			string queryString = ((num < 0) ? string.Empty : uri.Substring(num + 1));
			return ParseQueryString(queryString);
		}

		private static IEnumerable<string> QueryParamList(Dictionary<string, string> dictionary, bool urlEncoded = false)
		{
			if (urlEncoded)
			{
				return dictionary.Select((KeyValuePair<string, string> d) => WebUtility.UrlEncode(d.Key) + "=" + WebUtility.UrlEncode(d.Value));
			}
			return dictionary.Select((KeyValuePair<string, string> d) => d.Key + "=" + d.Value);
		}

		protected static string BuildSignature(Dictionary<string, string> dictionary, string sessionSecret)
		{
			List<string> list = QueryParamList(dictionary).ToList();
			list.Sort();
			ICrypto instance = ServiceLocator.Current.GetInstance<ICrypto>();
			string text = string.Join("", list);
			text += sessionSecret;
			return instance.ToMD5HexString(text);
		}
	}
}

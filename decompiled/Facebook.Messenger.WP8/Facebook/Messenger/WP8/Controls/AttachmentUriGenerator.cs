using System.Collections.Generic;
using System.Linq;
using Facebook.Base;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls
{
	public static class AttachmentUriGenerator
	{
		private static FacebookConfig _config;

		private static FacebookContext _context;

		private static void InitContextAndConfig()
		{
			if (_config == null)
			{
				_config = ServiceLocator.Current.GetInstance<FacebookConfig>();
				_context = ServiceLocator.Current.GetInstance<FacebookContext>();
			}
		}

		private static string Encode(string str)
		{
			return Url.Encode(str);
		}

		public static string UrlForThreadImage(string threadId, string hash = "", int size = 50)
		{
			return UrlForAttachment(null, null, threadId, hash, size);
		}

		public static string UrlForMessage(string messageId, int size = 0)
		{
			return UrlForAttachment(null, messageId, null, null, size);
		}

		public static string UrlForAttachment(string attachmentId, string messageId, int size = 0)
		{
			return UrlForAttachment(attachmentId, messageId, null, null, size);
		}

		private static string UrlForAttachment(string attachmentId, string messageId, string threadId, string hash, int size = 0)
		{
			InitContextAndConfig();
			if (!_context.IsAuthenticated())
			{
				return string.Empty;
			}
			string accessToken = _context.Session.Credentials.AccessToken;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("method", "messaging.getAttachment");
			dictionary.Add("format", "binary");
			dictionary.Add("access_token", accessToken);
			Dictionary<string, string> dictionary2 = dictionary;
			if (string.IsNullOrWhiteSpace(attachmentId))
			{
				dictionary2["aid"] = "1";
			}
			else
			{
				dictionary2["aid"] = Encode(attachmentId);
			}
			if (!string.IsNullOrWhiteSpace(messageId))
			{
				dictionary2["mid"] = Encode(messageId);
			}
			if (!string.IsNullOrWhiteSpace(threadId))
			{
				dictionary2["tid"] = Encode(threadId);
			}
			if (!string.IsNullOrWhiteSpace(hash))
			{
				dictionary2["pic_hash"] = Encode(hash);
			}
			if (size != 0)
			{
				dictionary2["preview"] = "1";
				dictionary2["max_width"] = size.ToString();
				dictionary2["max_height"] = size.ToString();
			}
			string arg = string.Join("&", dictionary2.Select((KeyValuePair<string, string> entry) => $"{entry.Key}={entry.Value}"));
			return $"{_config.RestApiUrl}?{arg}";
		}
	}
}

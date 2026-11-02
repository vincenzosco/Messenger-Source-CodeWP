using System.Collections.Generic;
using System.Linq;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Utilities
{
	public class UserUtilities
	{
		public static bool IsCurrentUser(FacebookUser sender, FacebookContext context)
		{
			if (sender != null)
			{
				return IsCurrentUserId(sender.Id, context);
			}
			return false;
		}

		public static bool IsCurrentUserId(string id, FacebookContext context)
		{
			if (context != null)
			{
				return context.Session.UserId.Equals(id);
			}
			return false;
		}

		public static string GetCanonicalParticipantId(ThreadViewModel thread, FacebookContext context)
		{
			if (!thread.IsCanonical())
			{
				return string.Empty;
			}
			IEnumerable<MailUser> source = thread.Participants.Where((MailUser t) => !IsCurrentUserId(t.Id, context));
			MailUser mailUser = source.FirstOrDefault();
			if (mailUser != null)
			{
				return mailUser.Id;
			}
			return string.Empty;
		}
	}
}

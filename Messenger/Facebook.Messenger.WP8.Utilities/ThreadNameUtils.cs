using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class ThreadNameUtils
	{
		public static string GetThreadNamePreferCache(ThreadViewModel thread, FacebookContext context)
		{
			if (!string.IsNullOrWhiteSpace(thread.CachedEllipsizedThreadName))
			{
				return thread.CachedEllipsizedThreadName;
			}
			return GetThreadNameOrId(thread, context);
		}

		public static string GetThreadNameOrId(ThreadViewModel thread, FacebookContext context, bool skipSetName = false)
		{
			return GetThreadName(thread, context, (MailUser user) => (!string.IsNullOrWhiteSpace(user.Name)) ? user.Name : user.Id, skipSetName);
		}

		public static string GetThreadNameOrPlaceholder(ThreadViewModel thread, FacebookContext context, bool skipSetName = false)
		{
			return GetThreadName(thread, context, (MailUser user) => (!string.IsNullOrWhiteSpace(user.Name)) ? user.Name : StringLibrary.Thread_UnnamedUserPlaceholder_Title, skipSetName);
		}

		private static string GetThreadName(ThreadViewModel thread, FacebookContext context, Func<MailUser, string> getThreadNameForSingleParticipant, bool skipSetName = false)
		{
			if (thread == null)
			{
				return string.Empty;
			}
			if (thread.HasName() && !skipSetName)
			{
				return thread.Name;
			}
			return GetThreadParticipantsName(thread, context, getThreadNameForSingleParticipant);
		}

		public static string GetThreadParticipantsNameOrId(ThreadViewModel thread, FacebookContext context)
		{
			return GetThreadParticipantsName(thread, context, (MailUser user) => (!string.IsNullOrWhiteSpace(user.Name)) ? user.Name : user.Id);
		}

		public static string GetThreadParticipantsName(ThreadViewModel thread, FacebookContext context, Func<MailUser, string> getThreadNameForSingleParticipant)
		{
			string loggedInUserId = string.Empty;
			if (context.IsAuthenticated())
			{
				loggedInUserId = context.Session.Profile.Id;
			}
			IEnumerable<MailUser> source = thread.CalculateParticipantsByLastSend(loggedInUserId);
			if (!source.Any())
			{
				if (!thread.Participants.Any())
				{
					return StringLibrary.Thread_Name_NoParticipant_Title;
				}
				return thread.Participants.First().Name;
			}
			if (source.Count() == 1)
			{
				return getThreadNameForSingleParticipant(source.First());
			}
			IEnumerable<string> values = source.Select(GetDisplayName);
			return string.Join(", ", values);
		}

		public static string GetDisplayName(MailUser participant)
		{
			if (participant == null)
			{
				return string.Empty;
			}
			if (!string.IsNullOrWhiteSpace(participant.FirstName))
			{
				return participant.FirstName;
			}
			if (!string.IsNullOrWhiteSpace(participant.Name))
			{
				return participant.Name;
			}
			return participant.Id;
		}
	}
}

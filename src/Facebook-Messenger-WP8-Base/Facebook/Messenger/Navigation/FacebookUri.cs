using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Facebook.Base;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;

namespace Facebook.Messenger.Navigation
{
	public class FacebookUri : Uri
	{
		public const string MessengerHost = "fbmessenger";

		public const string ComposerUriParamTextToSend = "textToSend";

		public static FacebookUri PhoneNumberRegistration = new FacebookUri(UriId.PhoneNumberRegistration);

		public static FacebookUri NewUserRegistration = GetAppUri(UriId.NewUserRegistration);

		public static FacebookUri ExistingUser = GetAppUri(UriId.ExistingUser);

		public static FacebookUri CreateProfilePic = GetAppUri(UriId.CreateProfilePic);

		public static FacebookUri SyncContacts = GetAppUri(UriId.SyncContacts);

		public static FacebookUri ProfilePicConfirmation = GetAppUri(UriId.ProfilePicConfirmation);

		public static FacebookUri LoginUri = new FacebookUri(UriId.Login);

		public static FacebookUri LoginApprovalUri = GetAppUri(UriId.LoginApproval);

		public static FacebookUri NuxOnMessengerUri = new FacebookUri(UriId.NuxOnMessenger);

		public static FacebookUri RootUri = new FacebookUri(UriId.Root);

		public static FacebookUri PhoneNumberUri = GetAppUri(UriId.PhoneNumber);

		public static FacebookUri VerifyPhoneNumberUri = GetAppUri(UriId.VerifyPhoneNumber);

		public static FacebookUri StickerStoreUri = GetAppUri(UriId.StickerStore);

		public static FacebookUri StickerPackUri = GetAppUri(UriId.StickerPack);

		public static FacebookUri UnknownUri = new FacebookUri("unknown");

		public static FacebookUri SettingsUri = new FacebookUri("settings");

		public static FacebookUri ComposerUri = GetAppUri(UriId.Composer);

		public static FacebookUri GroupComposerUri = GetAppUri(UriId.GroupComposer);

		public static FacebookUri GroupDetailsUri = GetAppUri(UriId.GroupDetails);

		public static FacebookUri CanonicalThreadDetailsUri = GetAppUri(UriId.CanonicalThreadDetails);

		public static FacebookUri WebViewUri = GetAppUri(UriId.WebView);

		public static FacebookUri PushHomeUri = new FacebookUri("/Views/Home.xaml", UriKind.Relative);

		public static FacebookUri FirstLaunchUri = new FacebookUri("/Pages/FirstLaunchPage.xaml", UriKind.Relative);

		public static FacebookUri ExternalLaunchHomeUri = new FacebookUri("/Views/ExternalLaunch.xaml", UriKind.Relative);

		public static FacebookUri NameConversationUri = GetAppUri(UriId.NameConversation);

		public static FacebookUri ReportProblemUri = GetAppUri(UriId.ReportProblem);

		public static FacebookUri MuteSelectionUri = GetAppUri(UriId.MuteSelection);

		public static FacebookUri SuggestedPinsUri = GetAppUri(UriId.SuggestedPins);

		public static FacebookUri InternalSettingsUri = GetAppUri(UriId.InternalSettings);

		public static FacebookUri AddContactUri = GetAppUri(UriId.AddContact);

		private UriQuery _params;

		public object NativePayload { get; set; }

		public string Type => GetComponents(UriComponents.Host, UriFormat.Unescaped);

		public string Id => GetComponents(UriComponents.Path, UriFormat.Unescaped);

		public Dictionary<string, string> Params
		{
			get
			{
				if (_params == null)
				{
					_params = UriUtils.ParseQueryString(Query);
				}
				return _params;
			}
		}

		public static FacebookUri GetComposerUri(string photoId = null, bool fromExternal = false, string recipientID = null, string recipientName = null, string textToSend = null)
		{
			UriQuery uriQuery = new UriQuery();
			if (!string.IsNullOrWhiteSpace(photoId))
			{
				uriQuery.Add("photoId", photoId);
			}
			if (!string.IsNullOrWhiteSpace(recipientID))
			{
				uriQuery.Add("recipientId", recipientID);
			}
			if (!string.IsNullOrWhiteSpace(recipientName))
			{
				uriQuery.Add("recipientName", recipientName);
			}
			if (!string.IsNullOrWhiteSpace(textToSend))
			{
				uriQuery.Add("textToSend", Uri.EscapeDataString(textToSend));
			}
			if (fromExternal)
			{
				uriQuery[UriParams.FromExternal] = "1";
			}
			return new FacebookUri("app", UriId.Composer, uriQuery);
		}

		public static FacebookUri GetFirstNavigationUri(bool sessionExpired = false)
		{
			bool flag = false;
			UriQuery uriQuery = new UriQuery();
			if (sessionExpired)
			{
				uriQuery["expired"] = "1";
			}
			return new FacebookUri(flag ? UriId.PhoneNumberRegistration : UriId.Login, "", uriQuery);
		}

		public static FacebookUri GetLoginUri(bool sessionExpired)
		{
			UriQuery uriQuery = new UriQuery();
			if (sessionExpired)
			{
				uriQuery["expired"] = "1";
			}
			return new FacebookUri(UriId.Login, "", uriQuery);
		}

		public static FacebookUri GetWebViewUri(Uri uri)
		{
			return GetWebViewUri(uri.AbsoluteUri);
		}

		public static FacebookUri GetWebViewUri(string url)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("url", url);
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.WebView, query);
		}

		public static FacebookUri GetAppUri(string id, UriQuery query = null)
		{
			return new FacebookUri("app", id, query);
		}

		public static FacebookUri GetAppUri(string[] paths, UriQuery query = null)
		{
			return new FacebookUri("app", string.Join("/", paths), query);
		}

		public static FacebookUri GetThreadUri(string threadId, string threadName = null, string actionId = null, bool? fromExternal = null)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("tid", threadId);
			UriQuery uriQuery2 = uriQuery;
			if (!string.IsNullOrEmpty(threadName))
			{
				uriQuery2.Add("name", threadName);
			}
			if (actionId != null)
			{
				uriQuery2.Add("actionId", actionId);
			}
			if (fromExternal ?? false)
			{
				uriQuery2.Add(UriParams.FromExternal, "1");
			}
			return new FacebookUri("app", UriId.Thread, uriQuery2);
		}

		public static FacebookUri GetAttachmentsUri(string messageId, string attachmentIds, string attachmentFbids, int selectedIndex)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("messageId", messageId);
			uriQuery.Add("photoIds", attachmentIds);
			uriQuery.Add("selectedIndex", selectedIndex.ToString(CultureInfo.InvariantCulture));
			UriQuery uriQuery2 = uriQuery;
			if (attachmentFbids != null)
			{
				uriQuery2.Add("photoFbids", attachmentFbids);
			}
			return new FacebookUri("app", UriId.Attachments, uriQuery2);
		}

		public static FacebookUri GetAttachmentsUriFromFilePaths(string messageId, string filePaths, int selectedIndex)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("messageId", messageId);
			uriQuery.Add("filePaths", filePaths);
			uriQuery.Add("selectedIndex", selectedIndex.ToString(CultureInfo.InvariantCulture));
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.Attachments, query);
		}

		public static FacebookUri GetConversationUri(string userId, string name = "", bool? fromExternal = null)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("recipientId", userId);
			uriQuery.Add("name", name);
			UriQuery uriQuery2 = uriQuery;
			if (fromExternal ?? false)
			{
				uriQuery2.Add(UriParams.FromExternal, "1");
			}
			return new FacebookUri("app", UriId.Thread, uriQuery2);
		}

		public static FacebookUri GetStickerPackUri(string stickerPackId)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("id", stickerPackId);
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.StickerPack, query);
		}

		public static FacebookUri GetAddPeopleToGroupUri(string threadId, IReadOnlyList<MailUser> participantsToExclude = null)
		{
			string value = ((participantsToExclude != null) ? string.Join(",", participantsToExclude.Select((MailUser p) => p.Id)) : string.Empty);
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("threadId", threadId);
			uriQuery.Add("contactsIdsToExclude", value);
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.AddPeopleToGroup, query);
		}

		public static string NullableBoolToString(bool? value)
		{
			string result = null;
			if (value.HasValue)
			{
				result = value.ToString();
			}
			return result;
		}

		public static FacebookUri GetMessageDetailsUri(string messageId, string threadId, string threadName, string lastActiveText, RecipientChatOrigin recipientChatOrigin, bool? isSingleRecipient)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("name", threadName);
			uriQuery.Add("lastActiveText", lastActiveText);
			uriQuery.Add("recipientChatOrigin", recipientChatOrigin.ToString());
			uriQuery.Add("isSingleRecipient", NullableBoolToString(isSingleRecipient));
			uriQuery.Add("messageId", messageId);
			uriQuery.Add("threadId", threadId);
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.MessageDetails, query);
		}

		public static FacebookUri GetLongMessageDetailsUri(string messageId, string threadId, string threadName, string lastActiveText, RecipientChatOrigin recipientChatOrigin, bool? isSingleRecipient)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("name", threadName);
			uriQuery.Add("lastActiveText", lastActiveText);
			uriQuery.Add("recipientChatOrigin", recipientChatOrigin.ToString());
			uriQuery.Add("isSingleRecipient", NullableBoolToString(isSingleRecipient));
			uriQuery.Add("messageId", messageId);
			uriQuery.Add("threadId", threadId);
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.LongMessageDetails, query);
		}

		public static FacebookUri GetGroupComposerAddUsersUri(string groupName, string threadImageUri = null)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add("groupName", groupName);
			UriQuery uriQuery2 = uriQuery;
			if (!string.IsNullOrWhiteSpace(threadImageUri))
			{
				uriQuery2.Add("threadImageUri", threadImageUri);
			}
			return new FacebookUri("app", UriId.GroupComposerAddUser, uriQuery2);
		}

		public static FacebookUri GetForwardMessageUri(string copiedMessageId)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add(UriParams.ForwardedMessage, copiedMessageId);
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.Forwarding, query);
		}

		public static FacebookUri GetForwardAttachmentUri(string copiedAttachmentId)
		{
			UriQuery uriQuery = new UriQuery();
			uriQuery.Add(UriParams.ForwardedAttachment, copiedAttachmentId);
			UriQuery query = uriQuery;
			return new FacebookUri("app", UriId.Forwarding, query);
		}

		public FacebookUri(string type, string id = "", UriQuery query = null)
			: base(BuildQuery(type, id, query), UriKind.Absolute)
		{
			_params = query;
		}

		public FacebookUri(Uri uri)
			: base(uri.ToString(), uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative)
		{
		}

		public FacebookUri(string uriString, UriKind kind)
			: base(uriString, kind)
		{
		}

		private static string BuildQuery(string type, string id, Dictionary<string, string> query)
		{
			UriBuilder uriBuilder = new UriBuilder("fbmessenger", type, 0, id);
			if (query != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<string, string> item in query)
				{
					if (!string.IsNullOrEmpty(item.Value))
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("&");
						}
						stringBuilder.Append($"{Url.Encode(item.Key)}={Url.Encode(item.Value)}");
					}
				}
				uriBuilder.Query = stringBuilder.ToString();
			}
			return uriBuilder.ToString();
		}
	}
}

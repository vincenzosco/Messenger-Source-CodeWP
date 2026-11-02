using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Base.Logging;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Views
{
	public partial class ExternalLaunch : PhoneApplicationPage
	{
		private const string FbMessengerProtocol = "fbmessenger://";

		private const string PageHome = "fbmessenger://home";

		private const string PageComposer = "fbmessenger://composer";

		private const string PageUser = "fbmessenger://user";

		private const string RecipientIdKey = "recipientId";

		private const string RecipientFriendlyNameKey = "name";



		public ExternalLaunch()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Uri redirectUri = GetRedirectUri(e.Uri);
			Navigator instance = ServiceLocator.Current.GetInstance<Navigator>();
			instance.Reset(redirectUri);
		}

		private Uri GetRedirectUri(Uri navigationUri)
		{
			FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
			if (!instance.IsAuthenticated())
			{
				return FacebookUri.GetFirstNavigationUri();
			}
			UriQuery uriQuery = UriUtils.ParseQueryStringFromUri(navigationUri.OriginalString);
			string uri = uriQuery["encodedLaunchUri"];
			string text = UriUtils.StripQueryString(uri).TrimEnd('/');
			UriQuery paramDictionary = UriUtils.ParseQueryStringFromUri(uri);
			switch (text)
			{
			case "fbmessenger://user":
				return GetRecipientUri(paramDictionary);
			case "fbmessenger://composer":
				return FacebookUri.GetComposerUri(null, true);
			case "fbmessenger://home":
				return FacebookUri.RootUri;
			default:
				Logger.Error(this, "External navigation failed as page was invalid: " + text, "GetRedirectUri", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\ExternalLaunch.xaml.cs", 72);
				return FacebookUri.RootUri;
			}
		}

		private Uri GetRecipientUri(IReadOnlyDictionary<string, string> paramDictionary)
		{
			if (!paramDictionary.ContainsKey("recipientId"))
			{
				Logger.Error(this, "Attempted to navigate to a recipient thread without supplied recipient ID", "GetRecipientUri", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\ExternalLaunch.xaml.cs", 81);
				return FacebookUri.RootUri;
			}
			string userId = paramDictionary["recipientId"];
			string value = string.Empty;
			if (!paramDictionary.TryGetValue("name", out value))
			{
				Logger.Error(this, "Navigating to recipient thread without a friend name, this is sub-optimal", "GetRecipientUri", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\ExternalLaunch.xaml.cs", 90);
			}
			return FacebookUri.GetConversationUri(userId, value, true);
		}

	}
}

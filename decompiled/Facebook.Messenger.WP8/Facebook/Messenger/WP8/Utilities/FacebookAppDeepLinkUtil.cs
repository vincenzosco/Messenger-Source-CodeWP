using System;
using Facebook.Messenger.Extensions;
using Facebook.Messenger.Navigation;
using Facebook.Models;
using Microsoft.Phone.Tasks;
using Microsoft.Practices.ServiceLocation;
using Windows.System;

namespace Facebook.Messenger.WP8.Utilities;

public class FacebookAppDeepLinkUtil
{
	public enum Page
	{
		OtherFolder,
		ContactSyncLearnMore,
		DataUsePolicies,
		MessengerHelp
	}

	private const string FacebookAppDeepLinkingProfileUrl = "fb:profile?id={0}";

	private static readonly Version FacebookAppDeepLinkingMinVersion = new Version(8, 1, 0);

	public static FacebookUri GetWebViewDeepLinkUri(Page page)
	{
		FacebookConfig instance = ServiceLocator.Current.GetInstance<FacebookConfig>();
		string url = $"{instance.MobileApiUrl}{GetDeepLinkPath(page)}";
		Uri uri = UriUtils.BuildDeepLinkUrl(url);
		return FacebookUri.GetWebViewUri(uri);
	}

	private static string GetDeepLinkPath(Page page)
	{
		return page switch
		{
			Page.OtherFolder => "/messages?folder=other", 
			Page.ContactSyncLearnMore => "/mobile/messenger/contacts", 
			Page.DataUsePolicies => "/policies", 
			Page.MessengerHelp => "/help/messenger-app", 
			_ => throw new InvalidOperationException("Unknown page"), 
		};
	}

	public static void LaunchProfile(FacebookProfile profile)
	{
		if (profile != null)
		{
			LaunchProfile(profile.Id);
		}
	}

	public static void LaunchProfile(string id)
	{
		if (!string.IsNullOrWhiteSpace(id))
		{
			if (Environment.OSVersion.Version >= FacebookAppDeepLinkingMinVersion)
			{
				LaunchDeepLinkToFbApp(id);
			}
			else
			{
				LaunchDeepLinkProfile(id);
			}
		}
	}

	private static void LaunchDeepLinkToFbApp(string id)
	{
		string uriString = $"fb:profile?id={id}";
		Launcher.LaunchUriAsync(new Uri(uriString)).DoNotWait<bool>();
	}

	private static void LaunchDeepLinkProfile(string id)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected Obj, but got Unknown
		FacebookConfig instance = ServiceLocator.Current.GetInstance<FacebookConfig>();
		string url = $"{instance.MobileApiUrl}{id}";
		Uri uri = UriUtils.BuildDeepLinkUrl(url);
		if (!(uri == null))
		{
			WebBrowserTask val = new WebBrowserTask();
			val.Uri = uri;
			WebBrowserTask val2 = val;
			val2.Show();
		}
	}
}

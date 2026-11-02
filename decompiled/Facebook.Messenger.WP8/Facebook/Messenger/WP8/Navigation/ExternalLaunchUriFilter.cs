using System;
using System.Windows.Navigation;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Utilities;

namespace Facebook.Messenger.WP8.Navigation;

public class ExternalLaunchUriFilter : UriMapperBase
{
	public const string ExternalUrlEncodedLaunchParameter = "encodedLaunchUri";

	private const string ExternalUrlLaunchPath = "/Protocol?";

	public override Uri MapUri(Uri uri)
	{
		if (!IsExternalLaunchUri(uri))
		{
			return uri;
		}
		return FacebookUri.ExternalLaunchHomeUri;
	}

	private static bool IsExternalLaunchUri(Uri uri)
	{
		if (uri.OriginalString.StartsWith("/Protocol?"))
		{
			return UriUtils.ParseQueryStringFromUri(uri.OriginalString).ContainsKey("encodedLaunchUri");
		}
		return false;
	}
}

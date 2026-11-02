using System;

namespace Facebook.Messenger.WP8.Events;

public class NavigationEventArgs : EventArgs
{
	public Uri Uri { get; private set; }

	public object NavigationData { get; private set; }

	public NavigationEventArgs(Uri uri)
	{
		Uri = uri;
	}

	public NavigationEventArgs(Uri uri, object navigationData)
	{
		Uri = uri;
		NavigationData = navigationData;
	}
}

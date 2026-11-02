using System;
using System.Windows.Controls;

namespace Facebook.Messenger.WP8.Extensions;

public static class FrameExtension
{
	private static object _navigationData;

	public static bool Navigate(this Frame frame, Uri pageUri, object obj)
	{
		_navigationData = obj;
		return frame.Navigate(pageUri);
	}

	public static object GetAndClearNavigationData(this Frame service)
	{
		object navigationData = _navigationData;
		_navigationData = null;
		return navigationData;
	}
}

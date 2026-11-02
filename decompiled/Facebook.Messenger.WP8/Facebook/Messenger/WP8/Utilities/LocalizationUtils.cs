using System.Threading;
using System.Windows;

namespace Facebook.Messenger.WP8.Utilities;

public class LocalizationUtils
{
	public static FlowDirection GetFlowDirectionForCurrentLocale()
	{
		switch (Thread.CurrentThread.CurrentUICulture.Name)
		{
		case "ar-SA":
		case "he-IL":
			return (FlowDirection)1;
		default:
			return (FlowDirection)0;
		}
	}
}

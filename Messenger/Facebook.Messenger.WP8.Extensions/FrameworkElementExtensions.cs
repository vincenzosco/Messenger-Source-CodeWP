using System;
using System.Windows;
using Facebook.Base.Logging;

namespace Facebook.Messenger.WP8.Extensions
{
	public static class FrameworkElementExtensions
	{
		public static bool CallWithViewModel<T>(this FrameworkElement element, Action<T> action) where T : class
		{
			T val = element.DataContext as T;
			if (val == null)
			{
				Logger.Warning(element, "DataContext is null - possibly recycled between UI operations", "CallWithViewModel", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Extensions\\FrameworkElementExtensions.cs", 15);
				return false;
			}
			action(val);
			return true;
		}
	}
}

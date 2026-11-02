using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Extensions
{
	internal static class TreeHelpers
	{
		public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
		{
			for (FrameworkElement parent = node.GetVisualParent(); parent != null; parent = parent.GetVisualParent())
			{
				yield return parent;
			}
		}

		public static FrameworkElement GetVisualParent(this FrameworkElement node)
		{
			DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject)(object)node);
			return (FrameworkElement)(object)((parent is FrameworkElement) ? parent : null);
		}
	}
}

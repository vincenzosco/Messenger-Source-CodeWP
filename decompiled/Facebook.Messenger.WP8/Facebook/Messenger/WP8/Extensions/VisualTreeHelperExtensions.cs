using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Extensions;

public static class VisualTreeHelperExtensions
{
	public static T GetFirstDescendantOfType<T>(this DependencyObject start, bool applyTemplates = false)
	{
		return start.GetDescendantsOfType<T>(applyTemplates).FirstOrDefault();
	}

	public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start, bool applyTemplates = false)
	{
		return start.GetDescendants(applyTemplates).OfType<T>();
	}

	public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start, bool applyTemplates = false)
	{
		if (start == null)
		{
			yield break;
		}
		Queue<DependencyObject> queue = new Queue<DependencyObject>();
		queue.Enqueue(start);
		while (queue.Count > 0)
		{
			DependencyObject parent = queue.Dequeue();
			AttemptToApplyTemplate(parent, applyTemplates);
			int count = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < count; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				yield return child;
				queue.Enqueue(child);
			}
		}
	}

	private static void AttemptToApplyTemplate(DependencyObject obj, bool applyTemplates)
	{
		Control val = (Control)(object)((obj is Control) ? obj : null);
		if (applyTemplates && val != null)
		{
			val.ApplyTemplate();
		}
	}

	public static T GetFirstAncestorOfType<T>(this DependencyObject start) where T : DependencyObject
	{
		return start.GetAncestorsOfType<T>().FirstOrDefault();
	}

	public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject start) where T : DependencyObject
	{
		return start.GetAncestors().OfType<T>();
	}

	public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject start)
	{
		for (DependencyObject parent = VisualTreeHelper.GetParent(start); parent != null; parent = VisualTreeHelper.GetParent(parent))
		{
			yield return parent;
		}
	}

	public static bool IsInVisualTree(this DependencyObject dob)
	{
		if (App.RootFrame != null)
		{
			return dob.GetAncestors().Contains((DependencyObject)(object)App.RootFrame);
		}
		return false;
	}

	public static bool IsChildElementOf(object child, object parent)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (child != parent)
		{
			if (child is FrameworkElement)
			{
				return IsChildElementOf(((FrameworkElement)child).Parent, parent);
			}
			return false;
		}
		return true;
	}
}

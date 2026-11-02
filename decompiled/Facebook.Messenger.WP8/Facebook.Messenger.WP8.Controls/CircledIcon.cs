using System;
using System.Windows;

namespace Facebook.Messenger.WP8.Controls
{
	public class CircledIcon : ImageButton
	{
		public CircledIcon()
		{
			BackgroundSource = new Uri("\\Assets\\AppBar\\BaseCircle.png", UriKind.Relative);
			((FrameworkElement)this).Width = 48.0;
			((FrameworkElement)this).Height = 48.0;
		}
	}
}

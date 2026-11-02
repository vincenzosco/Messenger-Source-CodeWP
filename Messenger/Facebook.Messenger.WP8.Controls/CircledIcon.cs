using System;
using System.Windows;

namespace Facebook.Messenger.WP8.Controls
{
	public class CircledIcon : ImageButton
	{
		public CircledIcon()
		{
			BackgroundSource = new Uri("\\Assets\\AppBar\\BaseCircle.png", UriKind.Relative);
			base.Width = 48.0;
			base.Height = 48.0;
		}
	}
}

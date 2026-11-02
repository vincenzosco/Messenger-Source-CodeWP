using System;

namespace Facebook.Messenger.WP8.Utilities
{
	public class StateValues
	{
		public string Text;

		public Uri IconUri;

		public EventHandler EventHandler;

		public bool IsEnabled;

		public StateValues(string text, Uri uri, EventHandler handler, bool enabled)
		{
			Text = text;
			IconUri = uri;
			EventHandler = handler;
			IsEnabled = enabled;
		}
	}
}

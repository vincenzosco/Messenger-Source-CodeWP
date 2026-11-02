using System;

namespace Facebook.Messenger.WP8.Utilities
{
	public class EntryState<TStateType> : StateValues
	{
		public TStateType State;

		public EntryState(TStateType state, string text, EventHandler handler, Uri uri = null, bool enabled = true)
			: base(text, uri, handler, enabled)
		{
			State = state;
		}
	}
}

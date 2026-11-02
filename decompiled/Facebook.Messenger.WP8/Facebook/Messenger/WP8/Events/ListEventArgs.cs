using System;

namespace Facebook.Messenger.WP8.Events;

public class ListEventArgs : EventArgs
{
	public ListEvent ListEvent { get; private set; }

	public ListEventArgs(ListEvent listEvent)
	{
		ListEvent = listEvent;
	}
}

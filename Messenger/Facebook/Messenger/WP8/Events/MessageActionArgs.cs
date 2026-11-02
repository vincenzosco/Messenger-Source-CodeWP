using System;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Events
{
	public class MessageActionArgs : EventArgs
	{
		public MessageViewModel Message { get; private set; }

		public MessageAction Action { get; private set; }

		public MessageActionArgs(MessageViewModel message, MessageAction action)
		{
			Message = message;
			Action = action;
		}
	}
}

using System;
using Facebook.Messenger.WP8.Controls;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ImageSaveEventArgs : EventArgs
	{
		public ActionState State { get; set; }

		public ImageSaveEventArgs(ActionState state)
		{
			State = state;
		}
	}
}

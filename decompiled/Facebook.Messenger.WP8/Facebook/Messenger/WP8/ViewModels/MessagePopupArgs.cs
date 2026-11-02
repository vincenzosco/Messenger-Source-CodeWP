using System;
using Facebook.Models;

namespace Facebook.Messenger.WP8.ViewModels;

public class MessagePopupArgs : EventArgs
{
	public MailThread Thread { get; set; }

	public MailMessage Message { get; set; }

	public FacebookContext Context { get; set; }
}

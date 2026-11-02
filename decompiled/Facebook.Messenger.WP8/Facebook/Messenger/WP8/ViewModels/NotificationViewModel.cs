using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Localization;
using GalaSoft.MvvmLight;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class NotificationViewModel : ViewModelBase
	{
		public MailUser Sender { get; private set; }

		public string Snippet { get; private set; }

		public string ThreadId { get; private set; }

		public string ThreadName { get; private set; }

		public NotificationViewModel(MailMessage message, ThreadSnippetInfo snippet, MailThread thread, FacebookContext context)
		{
			Sender = message.Sender;
			Snippet = snippet.AdminSnippet ?? snippet.Snippet;
			ThreadId = thread.Id;
			ThreadName = ThreadNameUtils.GetThreadNameOrPlaceholder(new ThreadViewModel(thread), context);
		}
	}
}

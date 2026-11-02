using Facebook.Models;

namespace Facebook.Services.Localization
{
	public interface IThreadSnippetGenerator
	{
		ThreadSnippetInfo GenerateSnippet(MailMessage message, MailThread thread);
	}
}

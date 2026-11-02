using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Models;
using Facebook.Services.Localization;

namespace Facebook.Messenger.Localization;

public class ThreadSnippetGenerator : IThreadSnippetGenerator
{
	private const string UnicodeThumbsUp = "\ud83d\udc4d";

	private readonly FacebookContext _context;

	public ThreadSnippetGenerator(FacebookContext context)
	{
		_context = context;
	}

	public ThreadSnippetInfo GenerateSnippet(MailMessage message, MailThread thread)
	{
		return GenerateSnippet(message, _context);
	}

	public static ThreadSnippetInfo GenerateSnippet(MailMessage message, FacebookContext context)
	{
		bool fromLoggedInUser = context.IsActiveUser(message.Sender.Id);
		string firstName = message.Sender.FirstName;
		MessageClassifier.Classification classification = MessageClassifier.Classify(message);
		switch (classification)
		{
		case MessageClassifier.Classification.LikeSticker:
			return ThreadSnippetInfo.CreateNormalSnippet("\ud83d\udc4d", fromLoggedInUser);
		case MessageClassifier.Classification.Sticker:
			return GetSnippetForSticker(fromLoggedInUser, firstName);
		case MessageClassifier.Classification.Audio:
			return GetSnippetForAudio(fromLoggedInUser, firstName);
		case MessageClassifier.Classification.Image:
			return GetSnippetForImage(fromLoggedInUser, firstName);
		case MessageClassifier.Classification.Video:
			return GetSnippetForVideo(fromLoggedInUser, firstName);
		case MessageClassifier.Classification.ThreadNameSet:
		case MessageClassifier.Classification.ThreadNameRemoved:
		case MessageClassifier.Classification.ThreadImageSet:
		case MessageClassifier.Classification.ThreadImageRemoved:
			return ThreadSnippetInfo.CreateAdminSnippet(message.Body, fromLoggedInUser);
		default:
			if (!string.IsNullOrWhiteSpace(message.Body))
			{
				return ThreadSnippetInfo.CreateNormalSnippet(message.Body, fromLoggedInUser);
			}
			Logger.Warning("Message type should have had body - using generic snippet: " + classification, "GenerateSnippet", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Localization\\ThreadSnippetGenerator.cs", 60);
			return GetSnippetForGenericMessage(fromLoggedInUser, firstName);
		}
	}

	private static ThreadSnippetInfo GetSnippetForSticker(bool fromLoggedInUser, string senderName)
	{
		string snippet = (fromLoggedInUser ? StringLibrary.MessageSnippetStickerLoggedInUser : string.Format(StringLibrary.MessageSnippetStickerOtherUser, senderName));
		return ThreadSnippetInfo.CreateAdminSnippet(snippet, fromLoggedInUser);
	}

	private static ThreadSnippetInfo GetSnippetForAudio(bool fromLoggedInUser, string senderName)
	{
		string snippet = (fromLoggedInUser ? StringLibrary.MessageSnippetAudioClipLoggedInUser : string.Format(StringLibrary.MessageSnippetAudioClipOtherUser, senderName));
		return ThreadSnippetInfo.CreateAdminSnippet(snippet, fromLoggedInUser);
	}

	private static ThreadSnippetInfo GetSnippetForImage(bool fromLoggedInUser, string senderName)
	{
		string snippet = (fromLoggedInUser ? StringLibrary.MessageSnippetPhotoLoggedInUser : string.Format(StringLibrary.MessageSnippetPhotoOtherUser, senderName));
		return ThreadSnippetInfo.CreateAdminSnippet(snippet, fromLoggedInUser);
	}

	private static ThreadSnippetInfo GetSnippetForVideo(bool fromLoggedInUser, string senderName)
	{
		string snippet = (fromLoggedInUser ? StringLibrary.MessageSnippetVideoLoggedInUser : string.Format(StringLibrary.MessageSnippetVideoOtherUser, senderName));
		return ThreadSnippetInfo.CreateAdminSnippet(snippet, fromLoggedInUser);
	}

	private static ThreadSnippetInfo GetSnippetForGenericMessage(bool fromLoggedInUser, string senderName)
	{
		string snippet = (fromLoggedInUser ? StringLibrary.MessageSnippetGenericLoggedInUser : string.Format(StringLibrary.MessageSnippetGenericOtherUser, senderName));
		return ThreadSnippetInfo.CreateAdminSnippet(snippet, fromLoggedInUser);
	}
}

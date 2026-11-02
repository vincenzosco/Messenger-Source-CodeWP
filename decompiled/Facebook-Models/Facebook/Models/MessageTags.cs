namespace Facebook.Models;

public class MessageTags
{
	public const string MessageTagMessenger = "messenger";

	private const string MessageTagChatOrca = "source:chat:orca";

	private const string MessageTagTitanOrca = "source:titan:orca";

	public const string MessageTagWeb = "source:web";

	private const string MessageTagChatWeb = "source:chat:web";

	private const string MessageTagTitanWeb = "source:titan:web";

	private const string MessageTagChat = "source:chat";

	public const string MessageTagMobile = "source:mobile";

	private const string MessageTagChatIphone = "source:chat:iphone";

	public static readonly string[] OriginMessengerTags = new string[3] { "messenger", "source:chat:orca", "source:titan:orca" };

	public static readonly string[] OriginWebTags = new string[4] { "source:web", "source:chat:web", "source:titan:web", "source:chat" };

	public static readonly string[] OriginMobileTags = new string[2] { "source:mobile", "source:chat:iphone" };
}

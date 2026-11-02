namespace Facebook.Services.Localization;

public class ThreadSnippetInfo
{
	public readonly string Snippet;

	public readonly string AdminSnippet;

	public readonly bool FromLoggedInUser;

	private ThreadSnippetInfo(string snippet, string adminSnippet, bool fromLoggedInUser)
	{
		Snippet = snippet;
		AdminSnippet = adminSnippet;
		FromLoggedInUser = fromLoggedInUser;
	}

	public static ThreadSnippetInfo CreateAdminSnippet(string snippet, bool fromLoggedInUser)
	{
		return new ThreadSnippetInfo(null, snippet, fromLoggedInUser);
	}

	public static ThreadSnippetInfo CreateNormalSnippet(string snippet, bool fromLoggedInUser)
	{
		return new ThreadSnippetInfo(snippet, null, fromLoggedInUser);
	}
}

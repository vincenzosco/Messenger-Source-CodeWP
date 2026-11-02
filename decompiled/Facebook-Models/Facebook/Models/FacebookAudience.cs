namespace Facebook.Models;

public class FacebookAudience : IGroupHeader
{
	public bool IsGroupHeader { get; set; }

	public string GroupName { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public string Allow { get; set; }

	public string Deny { get; set; }
}

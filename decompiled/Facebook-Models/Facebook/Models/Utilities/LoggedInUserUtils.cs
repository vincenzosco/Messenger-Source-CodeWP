namespace Facebook.Models.Utilities;

public class LoggedInUserUtils
{
	private readonly FacebookContext _context;

	public LoggedInUserUtils(FacebookContext context)
	{
		_context = context;
	}

	public bool IsLoggedInUser(string userId)
	{
		return _context.IsActiveUser(userId);
	}

	public bool IsLoggedInUser(long userId)
	{
		return IsLoggedInUser(userId.ToString());
	}
}

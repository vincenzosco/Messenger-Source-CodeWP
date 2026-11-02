namespace Facebook.Models.Signals;

public class ChatSettingSignal
{
	public bool IsChatEnabled { get; set; }

	public ChatSettingSignal(bool isChatEnabled)
	{
		IsChatEnabled = isChatEnabled;
	}
}

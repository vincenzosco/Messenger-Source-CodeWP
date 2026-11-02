using Facebook.Models;

namespace Facebook.Services.SyncProtocol;

public class SyncProtocolSettings
{
	private const string SyncProtocolTokenSettingsKey = "SyncTokenSetting";

	private const string SyncProtocolVersionSettingsKey = "SyncVersionSetting";

	public string SyncToken { get; private set; }

	public int SyncApiVersion { get; private set; }

	public SyncProtocolSettings(string syncToken, int syncApiVersion)
	{
		SyncToken = syncToken;
		SyncApiVersion = syncApiVersion;
	}

	public static SyncProtocolSettings GetSyncSettings(FacebookContext context, int currentSyncApiVersion)
	{
		string setting = context.Settings.GetSetting<string>("SyncTokenSetting");
		int setting2 = context.Settings.GetSetting<int>("SyncVersionSetting");
		if (string.IsNullOrEmpty(setting) || setting2 == 0)
		{
			return new SyncProtocolSettings(null, currentSyncApiVersion);
		}
		return new SyncProtocolSettings(setting, setting2);
	}

	public static void ClearSystemSyncSettings(FacebookContext context)
	{
		context.Settings.RemoveSetting("SyncTokenSetting");
		context.Settings.RemoveSetting("SyncVersionSetting");
	}

	public void SaveSyncSettings(FacebookContext context)
	{
		context.Settings.SaveSetting("SyncTokenSetting", SyncToken);
		context.Settings.SaveSetting("SyncVersionSetting", SyncApiVersion);
	}
}

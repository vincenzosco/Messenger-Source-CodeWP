using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Settings;
using Facebook.Models.Signals;

namespace Facebook.Models;

public class FacebookContext
{
	private const string CrashFolderName = "Crashes";

	public const string PartialUserKey = "PartialUser";

	public const string IsChatEnabledKey = "IsChatEnabled";

	public const string ShowLocationWarningKey = "ShowLocationWarning";

	private const string PhoneLanguageSettingKey = "PhoneLanguageSetting";

	private const string GatekeeperSetting_Set = "Gatekeeper_Set";

	private const string GatekeeperSetting_LastSuccessfulQueryHash = "Gatekeeper_QueryHash";

	private const string ScheduledTasksSuccessSettingPrefix = "ScheduledTasksSuccess_";

	private const string BackgroundSendingShouldRunKey = "BackgroundSendingShouldRun";

	public const string GlobalMuteKey = "GlobalMuteDuration";

	public const string AudioKey = "AudioEnabled";

	public const string VibrationKey = "VibrationEnabled";

	public const string ContactSyncKey = "ContactSyncEnabled";

	public const string LocationKey = "LocationEnabled";

	public const string SendOnEnterKey = "SendOnEnterEnabled";

	public const string OutstandingRequestsKey = "OutstandingRequests";

	private readonly IPasswordSettings _security;

	private readonly IUserSettingsProvider _settingsProvider;

	public string PhoneNumberState = "PhoneNumberState";

	public string PhoneNumberVerified = "verified";

	public string PhoneNumberUnverified = "unverified";

	public string PhoneNumberSkipped = "skipped";

	public string PhoneNumber = "PhoneNumber";

	public static string RecentStickers = "RecentStickers";

	public static string HasNewStickerPacks = "HasNewStickerPacks";

	public static string LastFullFriendRequestSyncTime = "LastFullFriendRequestSyncTime";

	public static string LastFriendRequestSyncTime = "LastFriendRequestSyncTime";

	public static string LastFullNotificationSyncTime = "LastFullNotificationSyncTime";

	public static string LastNotificationSyncTime = "LastNotificationSyncTime";

	public static readonly string NewsFeedSectionId = "NewsFeedSectionId";

	public static string PinnedGroupSyncTime = "PinnedGroupSyncTime";

	public static string ShutdownErrorStep = "ShutdownErrorStep";

	public FacebookSession Session { get; private set; }

	public IUserSettings Settings { get; private set; }

	public IGlobalSettings GlobalSettings { get; private set; }

	public ApplicationState ApplicationState { get; private set; }

	public event EventHandler<FacebookSession> LoggedIn;

	public event EventHandler<LogoutEventArgs> LoggedOut;

	public event EventHandler<GlobalMuteSignal> GlobalMuteEvent;

	public FacebookContext(IPasswordSettings security, IGlobalSettings settings, IUserSettingsProvider settingsProvider, ApplicationState applicationState)
	{
		GlobalSettings = settings;
		ApplicationState = applicationState;
		_security = security;
		_settingsProvider = settingsProvider;
		FacebookCredentials facebookCredentials = _security.RetrieveCredentials();
		FacebookSession facebookSession = GlobalSettings.LoadSession();
		if (facebookCredentials == null || facebookSession == null || !IsTheSameUser(facebookCredentials, facebookSession))
		{
			_security.RemoveCredentials();
			GlobalSettings.ClearSession((facebookCredentials != null) ? facebookCredentials.Id : string.Empty);
			Session = FacebookSession.CreateInvalidSession();
			Settings = new SlopBucket();
		}
		else
		{
			facebookSession.Credentials = facebookCredentials;
			Settings = _settingsProvider.GetOrCreate(facebookCredentials.Id);
			Session = facebookSession;
		}
	}

	private bool IsTheSameUser(FacebookCredentials creds, FacebookSession session)
	{
		if (creds == null || session == null)
		{
			return false;
		}
		return creds.Id == session.UserId;
	}

	public bool IsActiveUser(string id)
	{
		return Session.UserId == id;
	}

	private void SignalLogin(FacebookSession session)
	{
		if (LoggedIn != null)
		{
			LoggedIn(this, session);
		}
	}

	private void SignalLogout(FacebookSession session, bool expired)
	{
		if (LoggedOut != null)
		{
			LoggedOut(this, new LogoutEventArgs(session, expired));
		}
	}

	public bool Login(FacebookSession session)
	{
		if (!_security.SaveCredentials(session.Credentials))
		{
			return false;
		}
		if (!GlobalSettings.SaveSession(session))
		{
			_security.RemoveCredentials();
			return false;
		}
		Session = session;
		Settings = _settingsProvider.GetOrCreate(session.UserId);
		SignalLogin(session);
		return true;
	}

	public async void Logout(bool expiredSession = false)
	{
		if (IsAuthenticated())
		{
			FacebookSession oldSession = Session;
			IUserSettings oldSettings = Settings;
			Session = FacebookSession.CreateInvalidSession();
			Settings = new SlopBucket();
			_security.RemoveCredentials();
			_settingsProvider.Clear(oldSession.UserId);
			GlobalSettings.ClearSession(oldSession.UserId);
			SignalLogout(oldSession, expiredSession);
			await oldSettings.DeleteUserFolder();
		}
	}

	public bool IsAuthenticated()
	{
		if (Session != null)
		{
			return Session.IsValid();
		}
		return false;
	}

	public async Task PurgeOldFolders()
	{
		await GlobalSettings.PurgeUserFolders(Session.UserId, "Crashes");
	}

	public bool IsPartialUser()
	{
		return Settings.GetSetting<bool?>("PartialUser") ?? false;
	}

	public void SetIsPartialUser()
	{
		Settings.SaveSetting("PartialUser", true);
	}

	public bool HasUserCompletedPhoneNumberFlow()
	{
		string setting = Settings.GetSetting<string>(PhoneNumberState);
		return !string.IsNullOrEmpty(setting);
	}

	public void UpdatePhoneNumberUnverified()
	{
		UpdatePhoneNumberState(PhoneNumberUnverified);
	}

	public void UpdatePhoneNumberVerified()
	{
		UpdatePhoneNumberState(PhoneNumberVerified);
	}

	public void UpdatePhoneNumberSkipped()
	{
		Settings.RemoveSetting(PhoneNumber);
		UpdatePhoneNumberState(PhoneNumberSkipped);
	}

	public void UpdatePhoneNumberState(string state)
	{
		Settings.SaveSetting(PhoneNumberState, state);
	}

	public string GetPhoneNumberState()
	{
		return Settings.GetSetting<string>(PhoneNumberState);
	}

	public string GetPhoneNumber()
	{
		return Settings.GetSetting<string>(PhoneNumber);
	}

	public void UpdatePhoneNumber(string phoneNumber)
	{
		if (Settings != null)
		{
			Settings.SaveSetting(PhoneNumber, phoneNumber);
		}
	}

	public List<string> GetRecentStickerIds()
	{
		return Settings.GetSetting<List<string>>(RecentStickers) ?? new List<string>();
	}

	public void SetRecentStickerIds(List<string> stickerIds)
	{
		Settings.SaveSetting(RecentStickers, stickerIds);
	}

	public bool GetHasNewStickerPacks()
	{
		return Settings.GetSetting<bool>(HasNewStickerPacks);
	}

	public void SetHasNewStickerPacks(bool hasNewStickerPacks)
	{
		Settings.SaveSetting(HasNewStickerPacks, hasNewStickerPacks);
	}

	public long GetLastFullFriendRequestSyncTime()
	{
		return Settings.GetSetting<long>(LastFullFriendRequestSyncTime);
	}

	public long GetLastFriendRequestSyncTime()
	{
		return Settings.GetSetting<long>(LastFriendRequestSyncTime);
	}

	public void SetLastFullFriendRequestSyncTime(long time)
	{
		Settings.SaveSetting(LastFullFriendRequestSyncTime, time);
	}

	public void SetLastFriendRequestSyncTime(long time)
	{
		Settings.SaveSetting(LastFriendRequestSyncTime, time);
	}

	public long GetLastFullNotificationSyncTime()
	{
		return Settings.GetSetting<long>(LastFullNotificationSyncTime);
	}

	public long GetLastNotificationSyncTime()
	{
		return Settings.GetSetting<long>(LastNotificationSyncTime);
	}

	public void SetLastFullNotificationSyncTime(long time)
	{
		Settings.SaveSetting(LastFullNotificationSyncTime, time);
	}

	public void SetLastNotificationSyncTime(long time)
	{
		Settings.SaveSetting(LastNotificationSyncTime, time);
	}

	public string GetSelectedNewsFeedSectionId()
	{
		return Settings.GetSetting<string>(NewsFeedSectionId);
	}

	public void SetSelectedNewsFeedSectionId(string id)
	{
		Settings.SaveSetting(NewsFeedSectionId, id);
	}

	public bool IsChatEnabled()
	{
		return Settings.GetSetting<bool?>("IsChatEnabled") ?? true;
	}

	public void SetChatEnabled(bool value)
	{
		Settings.SaveSetting("IsChatEnabled", value);
	}

	public bool ShowLocationWarning()
	{
		return Settings.GetSetting<bool?>("ShowLocationWarning") ?? true;
	}

	public void HideLocationWarning()
	{
		Settings.SaveSetting("ShowLocationWarning", false);
	}

	public long GetLastPinnedGroupSync()
	{
		return Settings.GetSetting<long>(PinnedGroupSyncTime);
	}

	public void SetLastPinnedGroupSync(long time)
	{
		Settings.SaveSetting(PinnedGroupSyncTime, time);
	}

	public bool HasPhoneLanguageChanged()
	{
		string phoneLanguageSetting = GetPhoneLanguageSetting();
		return !CultureInfo.CurrentUICulture.DisplayName.Equals(phoneLanguageSetting);
	}

	public void SavePhoneLanguage()
	{
		Settings.SaveSetting("PhoneLanguageSetting", CultureInfo.CurrentUICulture.DisplayName);
	}

	public string GetPhoneLanguageSetting()
	{
		if (!Settings.ContainsSetting("PhoneLanguageSetting"))
		{
			SavePhoneLanguage();
		}
		return Settings.GetSetting<string>("PhoneLanguageSetting");
	}

	public IReadOnlyDictionary<string, bool> GetGatekeepers()
	{
		return Settings.GetSetting<IReadOnlyDictionary<string, bool>>("Gatekeeper_Set") ?? new Dictionary<string, bool>();
	}

	public void SetGatekeepers(IReadOnlyDictionary<string, bool> gatekeepers)
	{
		Settings.SaveSetting("Gatekeeper_Set", gatekeepers);
	}

	public string GetLastSuccessfulGatekeeperQueryHash()
	{
		if (!Settings.ContainsSetting("Gatekeeper_QueryHash"))
		{
			return null;
		}
		return Settings.GetSetting<string>("Gatekeeper_QueryHash");
	}

	public void SetLastSuccessfulGatekeeperQueryHash(string hash)
	{
		Settings.SaveSetting("Gatekeeper_QueryHash", hash);
	}

	public DateTime GetLastScheduledTaskSuccessTime(string taskName)
	{
		string key = "ScheduledTasksSuccess_" + taskName;
		if (!Settings.ContainsSetting(key))
		{
			return DateTime.MinValue;
		}
		return Settings.GetSetting<DateTime>(key);
	}

	public void SetLastScheduledTaskSuccessTime(string taskName, DateTime time)
	{
		string key = "ScheduledTasksSuccess_" + taskName;
		Settings.SaveSetting(key, time);
	}

	public void SetBackgroundSendingAgentShouldRun()
	{
		Settings.SaveSetting("BackgroundSendingShouldRun", true);
	}

	public void ResetBackgroundSendingAgentShouldRun()
	{
		if (Settings.ContainsSetting("BackgroundSendingShouldRun"))
		{
			Settings.RemoveSetting("BackgroundSendingShouldRun");
		}
	}

	public bool GetBackgroundSendingAgentShouldRun()
	{
		return Settings.ContainsSetting("BackgroundSendingShouldRun");
	}

	public long GetGlobalMuteDuration()
	{
		return Settings.GetSetting<long>("GlobalMuteDuration");
	}

	public void SetGlobalMuteDuration(long value)
	{
		Settings.SaveSetting("GlobalMuteDuration", value);
		if (GlobalMuteEvent != null)
		{
			GlobalMuteEvent(this, new GlobalMuteSignal
			{
				MuteDuration = value
			});
		}
	}

	public bool IsAudioEnabled()
	{
		if (!Settings.ContainsSetting("AudioEnabled"))
		{
			return true;
		}
		return Settings.GetSetting<bool>("AudioEnabled");
	}

	public void SetAudioEnabled(bool value)
	{
		Settings.SaveSetting("AudioEnabled", value);
	}

	public bool IsVibrationEnabled()
	{
		if (!Settings.ContainsSetting("VibrationEnabled"))
		{
			return true;
		}
		return Settings.GetSetting<bool>("VibrationEnabled");
	}

	public void SetVibrationEnabled(bool value)
	{
		Settings.SaveSetting("VibrationEnabled", value);
	}

	public bool IsContactSyncEnabled()
	{
		if (!Settings.ContainsSetting("ContactSyncEnabled"))
		{
			return false;
		}
		return Settings.GetSetting<bool>("ContactSyncEnabled");
	}

	public void SetContactSyncEnabled(bool value)
	{
		Settings.SaveSetting("ContactSyncEnabled", value);
	}

	public bool IsLocationEnabled()
	{
		if (!Settings.ContainsSetting("LocationEnabled"))
		{
			return true;
		}
		return Settings.GetSetting<bool>("LocationEnabled");
	}

	public void SetLocationEnabled(bool value)
	{
		Settings.SaveSetting("LocationEnabled", value);
	}

	public bool IsSendOnEnterEnabled()
	{
		if (!Settings.ContainsSetting("SendOnEnterEnabled"))
		{
			return true;
		}
		return Settings.GetSetting<bool>("SendOnEnterEnabled");
	}

	public void SetSendOnEnterEnabled(bool value)
	{
		Settings.SaveSetting("SendOnEnterEnabled", value);
	}

	public void DeleteOutstandingRequestsFromSettings()
	{
		Settings.RemoveSetting("OutstandingRequests");
	}

	public string GetShutdownErrorStep()
	{
		return Settings.GetSetting<string>(ShutdownErrorStep);
	}

	public void SetShutdownErrorStep(string errorStep)
	{
		Settings.SaveSetting(ShutdownErrorStep, errorStep);
	}

	public void ClearShutdownErrorStep()
	{
		SetShutdownErrorStep("");
	}
}

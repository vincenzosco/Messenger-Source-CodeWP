namespace Facebook.Base.Settings;

public interface IUserSettingsProvider
{
	IUserSettings GetOrCreate(string id);

	void Clear(string id);
}

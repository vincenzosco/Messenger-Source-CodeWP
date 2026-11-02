using System.Collections.Generic;

namespace Facebook.Messenger.WP8.Controls;

public interface IPreferences
{
	void RestorePreferences(IDictionary<string, string> preferences);

	IDictionary<string, string> GetPreferences();

	IList<string> GetPreferenceKeys();
}

using System.Collections.Generic;
using Facebook.Messenger.WP8.Controls.Base;

namespace Facebook.Messenger.WP8.Controls
{
	public abstract class ComposerDrawer : BaseControl, IPreferences
	{
		public DrawerType DrawerType { get; private set; }

		protected ComposerDrawer(DrawerType drawerType)
		{
			DrawerType = drawerType;
		}

		public abstract void Show();

		public abstract void Hide();

		public abstract void Cleanup();

		public virtual void RestorePreferences(IDictionary<string, string> preferences)
		{
		}

		public virtual IDictionary<string, string> GetPreferences()
		{
			return new Dictionary<string, string>();
		}

		public virtual IList<string> GetPreferenceKeys()
		{
			return new string[0];
		}
	}
}

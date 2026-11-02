using System;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Utilities
{
	public class MenuBarEntry<TStateType> : AppBarEntry<TStateType, ApplicationBarMenuItem> where TStateType : struct
	{
		public MenuBarEntry(AppBarHelper helper, ApplicationBarMenuItem item)
			: base(helper, item, (Action<AppBarEntry<TStateType, ApplicationBarMenuItem>, StateValues>)SetState)
		{
			Item.Click += ItemOnClick;
		}

		private void ItemOnClick(object sender, EventArgs eventArgs)
		{
			if (ActiveHandler != null)
			{
				ActiveHandler(sender, eventArgs);
			}
		}

		private static void SetState(AppBarEntry<TStateType, ApplicationBarMenuItem> entry, StateValues values)
		{
			MenuBarEntry<TStateType> menuBarEntry = entry as MenuBarEntry<TStateType>;
			menuBarEntry.Item.Text = values.Text;
			menuBarEntry.Item.IsEnabled = values.IsEnabled;
		}
	}
}

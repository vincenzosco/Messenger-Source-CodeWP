using System;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Utilities;

public class IconBarEntry<TStateType> : AppBarEntry<TStateType, ApplicationBarIconButton> where TStateType : struct
{
	public IconBarEntry(AppBarHelper helper, ApplicationBarIconButton item)
		: base(helper, item, (Action<AppBarEntry<TStateType, ApplicationBarIconButton>, StateValues>)SetState)
	{
		Item.Click += ItemOnClick;
	}

	protected virtual void ItemOnClick(object sender, EventArgs eventArgs)
	{
		if (ActiveHandler != null)
		{
			ActiveHandler(sender, eventArgs);
		}
	}

	private static void SetState(AppBarEntry<TStateType, ApplicationBarIconButton> entry, StateValues values)
	{
		IconBarEntry<TStateType> iconBarEntry = entry as IconBarEntry<TStateType>;
		iconBarEntry.Item.Text = values.Text;
		iconBarEntry.Item.IconUri = values.IconUri;
		iconBarEntry.Item.IsEnabled = values.IsEnabled;
	}
}

using System;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Utilities
{
	public class DebounceIconBarEntry<TStateType> : IconBarEntry<TStateType> where TStateType : struct
	{
		private readonly TimeSpan _debounceTimeDuration = new TimeSpan(0, 0, 0, 0, 200);

		private DateTime _lastClickTime = DateTime.MinValue;

		public DebounceIconBarEntry(AppBarHelper helper, ApplicationBarIconButton item)
			: base(helper, item)
		{
		}

		public DebounceIconBarEntry(AppBarHelper helper, ApplicationBarIconButton item, TimeSpan debounceTimeDuration)
			: base(helper, item)
		{
			_debounceTimeDuration = debounceTimeDuration;
		}

		protected override void ItemOnClick(object sender, EventArgs eventArgs)
		{
			if (DateTime.UtcNow - _lastClickTime > _debounceTimeDuration)
			{
				_lastClickTime = DateTime.UtcNow;
				base.ItemOnClick(sender, eventArgs);
			}
		}
	}
}

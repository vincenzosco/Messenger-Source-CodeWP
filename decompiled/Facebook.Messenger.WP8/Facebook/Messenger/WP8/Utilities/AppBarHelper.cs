using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Utilities;

public class AppBarHelper
{
	public readonly Dictionary<IApplicationBarMenuItem, AppBarEntryBase> Buttons = new Dictionary<IApplicationBarMenuItem, AppBarEntryBase>();

	public readonly Dictionary<IApplicationBarMenuItem, AppBarEntryBase> MenuItems = new Dictionary<IApplicationBarMenuItem, AppBarEntryBase>();

	private readonly IList _oldButtons = new List<IApplicationBarIconButton>();

	private readonly IList _oldMenuItems = new List<IApplicationBarMenuItem>();

	public IApplicationBar Bar { get; private set; }

	internal void UpdateVisibility<TControlType>(TControlType item, int position, bool visible)
	{
		IList list = (((object)typeof(TControlType) == typeof(ApplicationBarIconButton)) ? Bar.Buttons : Bar.MenuItems);
		Dictionary<IApplicationBarMenuItem, AppBarEntryBase> dictionary = (((object)typeof(TControlType) == typeof(ApplicationBarIconButton)) ? Buttons : MenuItems);
		if (visible)
		{
			int? num = null;
			foreach (object item2 in list)
			{
				AppBarEntryBase appBarEntryBase = dictionary[(IApplicationBarMenuItem)((item2 is IApplicationBarMenuItem) ? item2 : null)];
				if (appBarEntryBase.Position > position)
				{
					num = list.IndexOf(item2);
					break;
				}
			}
			if (num.HasValue)
			{
				list.Insert(num.Value, item);
			}
			else
			{
				list.Add(item);
			}
		}
		else
		{
			list.Remove(item);
		}
	}

	public AppBarHelper(IApplicationBar bar)
	{
		Bar = bar;
	}

	public MenuBarEntry<TStateType> CreateMenuEntry<TStateType>(params EntryState<TStateType>[] items) where TStateType : struct
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		ApplicationBarMenuItem val = new ApplicationBarMenuItem();
		MenuBarEntry<TStateType> menuBarEntry = new MenuBarEntry<TStateType>(this, val);
		Initialize<TStateType, ApplicationBarMenuItem>((AppBarEntry<TStateType, ApplicationBarMenuItem>)menuBarEntry, (IApplicationBarMenuItem)(object)val, Bar.MenuItems, (IDictionary<IApplicationBarMenuItem, AppBarEntryBase>)MenuItems, items);
		return menuBarEntry;
	}

	public IconBarEntry<TStateType> CreateBarButtonEntry<TStateType>(params EntryState<TStateType>[] items) where TStateType : struct
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		ApplicationBarIconButton val = new ApplicationBarIconButton();
		IconBarEntry<TStateType> iconBarEntry = new IconBarEntry<TStateType>(this, val);
		Initialize<TStateType, ApplicationBarIconButton>((AppBarEntry<TStateType, ApplicationBarIconButton>)iconBarEntry, (IApplicationBarMenuItem)(object)val, Bar.Buttons, (IDictionary<IApplicationBarMenuItem, AppBarEntryBase>)Buttons, items);
		return iconBarEntry;
	}

	public DebounceIconBarEntry<TStateType> CreateDebounceBarButtonEntry<TStateType>(params EntryState<TStateType>[] items) where TStateType : struct
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		ApplicationBarIconButton val = new ApplicationBarIconButton();
		DebounceIconBarEntry<TStateType> debounceIconBarEntry = new DebounceIconBarEntry<TStateType>(this, val);
		Initialize<TStateType, ApplicationBarIconButton>((AppBarEntry<TStateType, ApplicationBarIconButton>)debounceIconBarEntry, (IApplicationBarMenuItem)(object)val, Bar.Buttons, (IDictionary<IApplicationBarMenuItem, AppBarEntryBase>)Buttons, items);
		return debounceIconBarEntry;
	}

	public DebounceIconBarEntry<TStateType> CreateDebounceBarButtonEntry<TStateType>(TimeSpan debounceTimeDuration, params EntryState<TStateType>[] items) where TStateType : struct
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		ApplicationBarIconButton val = new ApplicationBarIconButton();
		DebounceIconBarEntry<TStateType> debounceIconBarEntry = new DebounceIconBarEntry<TStateType>(this, val, debounceTimeDuration);
		Initialize<TStateType, ApplicationBarIconButton>((AppBarEntry<TStateType, ApplicationBarIconButton>)debounceIconBarEntry, (IApplicationBarMenuItem)(object)val, Bar.Buttons, (IDictionary<IApplicationBarMenuItem, AppBarEntryBase>)Buttons, items);
		return debounceIconBarEntry;
	}

	private void Initialize<TStateType, TControlType>(AppBarEntry<TStateType, TControlType> entry, IApplicationBarMenuItem menu, IList barItemsList, IDictionary<IApplicationBarMenuItem, AppBarEntryBase> entries, params EntryState<TStateType>[] items) where TStateType : struct where TControlType : IApplicationBarMenuItem
	{
		foreach (EntryState<TStateType> entryState in items)
		{
			entry.AddState(entryState.State, entryState.Text, entryState.EventHandler, entryState.IconUri, entryState.IsEnabled);
		}
		if (items.Length > 0)
		{
			entry.State = items[0].State;
		}
		entry.Position = barItemsList.Add(menu);
		entries[menu] = entry;
	}

	public void HideOldAppBar()
	{
		_oldButtons.Clear();
		_oldMenuItems.Clear();
		foreach (object button in Bar.Buttons)
		{
			_oldButtons.Add(button);
		}
		foreach (object menuItem in Bar.MenuItems)
		{
			_oldMenuItems.Add(menuItem);
		}
		Bar.Buttons.Clear();
		Bar.MenuItems.Clear();
	}

	public void RestoreOldAppBar()
	{
		Bar.Buttons.Clear();
		Bar.MenuItems.Clear();
		foreach (object oldButton in _oldButtons)
		{
			Bar.Buttons.Add(oldButton);
		}
		foreach (object oldMenuItem in _oldMenuItems)
		{
			Bar.MenuItems.Add(oldMenuItem);
		}
	}

	public void Cleanup()
	{
		Buttons.Clear();
		MenuItems.Clear();
		_oldButtons.Clear();
		_oldMenuItems.Clear();
	}
}

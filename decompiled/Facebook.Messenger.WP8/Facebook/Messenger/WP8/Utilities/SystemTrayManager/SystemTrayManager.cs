using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Facebook.Base.Logging;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Utilities.SystemTrayManager;

public class SystemTrayManager
{
	private readonly Dictionary<string, bool> _pageSystemTrayVisibilityPreference = new Dictionary<string, bool>();

	private readonly HashSet<Type> _hideSystemTrayRequests = new HashSet<Type>();

	private string _currentPage;

	public void AddOperationToStack(Type type)
	{
		_hideSystemTrayRequests.Add(type);
		SafeSetSystemTrayVisibility(visible: false);
	}

	public void RemoveOperationFromStack(Type type)
	{
		if (_hideSystemTrayRequests.Any())
		{
			_hideSystemTrayRequests.Remove(type);
			if (!_hideSystemTrayRequests.Any())
			{
				SafeSetSystemTrayVisibility(_pageSystemTrayVisibilityPreference[_currentPage]);
			}
		}
	}

	public void NavigateTo(Page page)
	{
		AddPageIfNotExists(page);
		_currentPage = ((object)page).GetType().ToString();
		if (_hideSystemTrayRequests.Any())
		{
			SafeSetSystemTrayVisibility(visible: false);
		}
		else
		{
			SafeSetSystemTrayVisibility(_pageSystemTrayVisibilityPreference[_currentPage]);
		}
	}

	public void EnsurePagePreferencesStored(Page page, bool visibility)
	{
		_pageSystemTrayVisibilityPreference[((object)page).GetType().ToString()] = visibility;
		if (!_hideSystemTrayRequests.Any())
		{
			SafeSetSystemTrayVisibility(visibility);
		}
	}

	private void AddPageIfNotExists(Page page)
	{
		if (!_pageSystemTrayVisibilityPreference.ContainsKey(((object)page).GetType().ToString()))
		{
			EnsurePagePreferencesStored(page, SystemTray.GetIsVisible((DependencyObject)(object)page));
		}
	}

	private void SafeSetSystemTrayVisibility(bool visible)
	{
		try
		{
			SystemTray.IsVisible = visible;
		}
		catch (ArgumentException arg)
		{
			Logger.Warning(this, $"Error when setting System Tray visibility: {arg}", "SafeSetSystemTrayVisibility", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\SystemTrayManager\\SystemTrayManager.cs", 77);
		}
	}
}

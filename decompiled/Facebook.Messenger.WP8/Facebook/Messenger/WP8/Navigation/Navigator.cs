using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Base.Logging;
using Facebook.Messenger.Extensions;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.Pages;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.Utilities.SystemTrayManager;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Services.Analytics;
using Facebook.Services.Analytics.Logging;
using Microsoft.Phone.Controls;
using Windows.Storage;
using Windows.System;

namespace Facebook.Messenger.WP8.Navigation;

public class Navigator
{
	private class Entry
	{
		public Page Page;

		public FacebookMessengerViewModel ViewModel;
	}

	private const string DefaultLastBackStackPage = "MessengerMainPage.xaml";

	private readonly UriMapper _mapper;

	private readonly Service _analyticsService;

	private readonly List<Entry> _backStack;

	private readonly Dictionary<string, PageNavigationLogger.NuxStep> _urlToAnalytics;

	private PhoneApplicationFrame _frame;

	private readonly SystemTrayManager _systemTrayManager;

	public SystemTrayManager SystemTrayManager => _systemTrayManager;

	public event EventHandler NavigatedToANewPage;

	public Navigator(UriMapper mapper, Service analyticsService, SystemTrayManager systemTrayManager)
	{
		_mapper = mapper;
		_analyticsService = analyticsService;
		_systemTrayManager = systemTrayManager;
		_backStack = new List<Entry>();
		_urlToAnalytics = new Dictionary<string, PageNavigationLogger.NuxStep>
		{
			{
				"/Pages/LoginPage.xaml",
				PageNavigationLogger.NuxStep.Login
			},
			{
				"/Pages/LoginApprovalPage.xaml",
				PageNavigationLogger.NuxStep.LoginApproval
			},
			{
				"/Pages/PhoneNumberPage.xaml",
				PageNavigationLogger.NuxStep.PhoneNumber
			},
			{
				"/Pages/VerifyPhoneNumberPage.xaml",
				PageNavigationLogger.NuxStep.VerifyPhoneNumber
			},
			{
				"/Pages/NuxOnMessengerPage.xaml",
				PageNavigationLogger.NuxStep.OnMessenger
			}
		};
	}

	public void ApplicationStarted()
	{
		PageNavigationLogger.LogApplicationStarted(_analyticsService);
		PageNavigationLogger.LogSessionStarted(isSessionPreserved: false, _analyticsService);
	}

	public void ApplicationResumed(bool isApplicationInstancePreserved)
	{
		foreach (Entry item in _backStack)
		{
			item.ViewModel.OnApplicationResumed();
		}
		PageNavigationLogger.LogApplicatoinResumed(_analyticsService);
		PageNavigationLogger.LogSessionStarted(isApplicationInstancePreserved, _analyticsService);
	}

	public void SetFrame(PhoneApplicationFrame frame)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected Obj, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected Obj, but got Unknown
		if (_frame != null)
		{
			((Frame)_frame).Navigating -= FrameOnNavigating;
		}
		_frame = frame;
		((Frame)_frame).Navigating += FrameOnNavigating;
		if (_frame is MessengerApplicationFrame messengerApplicationFrame)
		{
			messengerApplicationFrame.SystemTrayManager = _systemTrayManager;
		}
	}

	private void FrameOnNavigating(object sender, NavigatingCancelEventArgs args)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		if (args.Uri == FacebookUri.FirstLaunchUri)
		{
			Entry entry = _backStack.FirstOrDefault((Entry e) => (object)((object)e.Page).GetType() == typeof(MessengerMainPage));
			if (entry != null)
			{
				MessengerMainPage messengerMainPage = entry.Page as MessengerMainPage;
				messengerMainPage.ResetPanorama();
				((CancelEventArgs)(object)args).Cancel = true;
				((DependencyObject)_frame).Dispatcher.BeginInvoke((Action)GoBackToRoot);
				return;
			}
		}
		if ((int)args.NavigationMode == 4)
		{
			return;
		}
		string text = UriUtils.StripQueryString(args.Uri.ToString());
		if (ShouldLogPageNavigation(text))
		{
			string originPage = string.Empty;
			NavigationService val = (NavigationService)((sender is NavigationService) ? sender : null);
			if (val != null && val.Source != null)
			{
				originPage = UriUtils.StripQueryString(val.Source.ToString());
			}
			PageNavigationLogger.LogNavigationStart(text, originPage, _analyticsService);
		}
		else if (!text.Equals("/Pages/FirstLaunchPage.xaml"))
		{
			PageNavigationLogger.RemoveColdFlag();
		}
	}

	private bool ShouldLogPageNavigation(string strippedUri)
	{
		if (!strippedUri.Equals("/Pages/MessengerMainPage.xaml"))
		{
			return strippedUri.Equals("/Pages/ThreadPage.xaml");
		}
		return true;
	}

	public void AddEntry(Page page, FacebookMessengerViewModel viewModel)
	{
		_backStack.Add(new Entry
		{
			Page = page,
			ViewModel = viewModel
		});
		if (NavigatedToANewPage != null)
		{
			NavigatedToANewPage(this, EventArgs.Empty);
		}
	}

	public void RemoveEntry(Page page)
	{
		Entry entry = _backStack.Find((Entry e) => e.Page == page);
		if (entry != null)
		{
			_backStack.Remove(entry);
			if (_frame != null)
			{
				((DependencyObject)_frame).Dispatcher.BeginInvoke((Action)entry.ViewModel.Cleanup);
			}
			else
			{
				entry.ViewModel.Cleanup();
			}
		}
	}

	private string MakeUniqueUri(string uri)
	{
		if (uri.Contains('?'))
		{
			return $"{uri}&fbnt={DateTime.Now.Ticks}";
		}
		return $"{uri}?fbnt={DateTime.Now.Ticks}";
	}

	public bool Navigate(Uri uri, object parameter = null)
	{
		if (uri == null)
		{
			return false;
		}
		if (NavigateOutsideApp(uri))
		{
			return false;
		}
		if (_frame == null)
		{
			return false;
		}
		string mapping = _mapper.GetMapping(uri.AbsoluteUri);
		if (mapping == null)
		{
			Logger.Warning($"Uri has no mapping: {uri}", "Navigate", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Navigation\\Navigator.cs", 205);
			return false;
		}
		string uriString = MakeUniqueUri(mapping);
		if (!Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out var result))
		{
			Logger.Warning($"Mapped URI is not valid: {mapping}", "Navigate", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Navigation\\Navigator.cs", 213);
			return false;
		}
		LogAnalytics(mapping);
		if (parameter != null)
		{
			return ((Frame)(object)_frame).Navigate(result, parameter);
		}
		return ((Frame)_frame).Navigate(result);
	}

	private void LogAnalytics(string mapping)
	{
		if (_urlToAnalytics.TryGetValue(UriUtils.StripQueryString(mapping), out var value))
		{
			PageNavigationLogger.LogNuxStep(value, _analyticsService);
		}
	}

	public object GetAndClearNavigationData()
	{
		return ((Frame)(object)_frame).GetAndClearNavigationData();
	}

	public void ReplaceTopOfStack(Uri uri)
	{
		if (Navigate(uri))
		{
			NavigatedToANewPage += OnPopTopStackNavigation;
		}
	}

	private void OnPopTopStackNavigation(object sender, EventArgs args)
	{
		NavigatedToANewPage -= OnPopTopStackNavigation;
		RemoveBackEntry();
	}

	public void NavigateThenPopBackStack(Uri uri)
	{
		if (Navigate(uri))
		{
			NavigatedToANewPage += OnPopBackStackUntilDefault;
		}
	}

	private void OnPopBackStackUntilDefault(object sender, EventArgs args)
	{
		NavigatedToANewPage -= OnPopBackStackUntilDefault;
		JournalEntry val;
		while ((val = _frame.BackStack.FirstOrDefault()) != null && !val.Source.ToString().Contains("MessengerMainPage.xaml"))
		{
			RemoveBackEntry();
		}
	}

	public void Reset(Uri uri, object parameter = null)
	{
		if (Navigate(uri, parameter))
		{
			NavigatedToANewPage += OnResetNavigation;
		}
	}

	private void OnResetNavigation(object sender, EventArgs args)
	{
		NavigatedToANewPage -= OnResetNavigation;
		ClearBackStack();
	}

	public void ClearBackStack()
	{
		while (RemoveBackEntry() != null)
		{
		}
	}

	public Uri RemoveBackEntry()
	{
		if (CanGoBack())
		{
			if (_backStack.Count > 1)
			{
				Entry entry = _backStack[_backStack.Count - 2];
				_backStack.Remove(entry);
				if (entry.ViewModel != null)
				{
					entry.ViewModel.Cleanup();
				}
			}
			JournalEntry val = _frame.RemoveBackEntry();
			if (val != null)
			{
				return val.Source;
			}
			return null;
		}
		return null;
	}

	private bool CanGoBack()
	{
		if (((Frame)_frame).CanGoBack)
		{
			return _frame.BackStack.Any();
		}
		return false;
	}

	public bool GoBack()
	{
		if (!CanGoBack())
		{
			return false;
		}
		((Frame)_frame).GoBack();
		return true;
	}

	public void GoBackToRoot()
	{
		while (_frame.BackStack.Count() > 1)
		{
			_frame.RemoveBackEntry();
		}
		GoBack();
	}

	private bool NavigateOutsideApp(Uri uri)
	{
		string components = uri.GetComponents(UriComponents.Scheme, UriFormat.UriEscaped);
		if (!components.ToLower().Contains("fbmessenger"))
		{
			if (uri.IsFile)
			{
				try
				{
					StorageFile result = StorageFile.GetFileFromPathAsync(uri.LocalPath).AsTask<StorageFile>().Result;
					if (result != null)
					{
						Launcher.LaunchFileAsync((IStorageFile)(object)result).DoNotWait<bool>();
					}
				}
				catch
				{
				}
				return true;
			}
			if (uri.IsAbsoluteUri)
			{
				Launcher.LaunchUriAsync(uri).DoNotWait<bool>();
			}
			else
			{
				Logger.Error(this, $"Unable to navigate to relative url {uri}", "NavigateOutsideApp", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Navigation\\Navigator.cs", 378);
			}
			return true;
		}
		return false;
	}
}

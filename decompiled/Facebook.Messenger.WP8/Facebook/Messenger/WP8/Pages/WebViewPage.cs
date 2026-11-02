using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Messenger.WP8.Controls;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages;

public class WebViewPage : WebViewViewModelPage
{
	internal Grid LayoutRoot;

	internal WebBrowser WebBrowser;

	internal FBMessengerProgressBar LoadingBar;

	private bool _contentLoaded;

	public WebViewPage()
	{
		InitializeComponent();
	}

	private void WebViewPage_OnLoaded(object sender, RoutedEventArgs e)
	{
		((PhoneApplicationPage)this).BackKeyPress += OnBackKeyPress;
	}

	private void WebViewPage_OnUnloaded(object sender, RoutedEventArgs e)
	{
		((PhoneApplicationPage)this).BackKeyPress -= OnBackKeyPress;
	}

	private void OnBackKeyPress(object sender, CancelEventArgs cancelEventArgs)
	{
		if (WebBrowser.CanGoBack)
		{
			WebBrowser.GoBack();
			cancelEventArgs.Cancel = true;
		}
	}

	private void WebBrowser_OnNavigating(object sender, NavigatingEventArgs e)
	{
		SetLoading(isLoading: true);
	}

	private void WebBrowser_OnNavigated(object sender, NavigationEventArgs e)
	{
		SetLoading(isLoading: false);
	}

	private void WebBrowser_OnNavigationFailed(object sender, NavigationFailedEventArgs e)
	{
		SetLoading(isLoading: false);
	}

	private void SetLoading(bool isLoading)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		LoadingBar.IsIndeterminate = isLoading;
		((UIElement)LoadingBar).Visibility = VisibilityUtil.FromBool(isLoading);
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/WebViewPage.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			WebBrowser = (WebBrowser)((FrameworkElement)this).FindName("WebBrowser");
			LoadingBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("LoadingBar");
		}
	}
}

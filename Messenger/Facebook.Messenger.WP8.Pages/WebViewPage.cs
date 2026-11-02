using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Messenger.WP8.Controls;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class WebViewPage : WebViewViewModelPage
	{




		public WebViewPage()
		{
			InitializeComponent();
		}

		private void WebViewPage_OnLoaded(object sender, RoutedEventArgs e)
		{
			base.BackKeyPress += OnBackKeyPress;
		}

		private void WebViewPage_OnUnloaded(object sender, RoutedEventArgs e)
		{
			base.BackKeyPress -= OnBackKeyPress;
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
			SetLoading(true);
		}

		private void WebBrowser_OnNavigated(object sender, NavigationEventArgs e)
		{
			SetLoading(false);
		}

		private void WebBrowser_OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			SetLoading(false);
		}

		private void SetLoading(bool isLoading)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			LoadingBar.IsIndeterminate = isLoading;
			((UIElement)LoadingBar).Visibility = VisibilityUtil.FromBool(isLoading);
		}

	}
}

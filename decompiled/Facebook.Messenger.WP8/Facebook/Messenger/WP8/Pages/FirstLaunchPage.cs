using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Practices.ServiceLocation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace Facebook.Messenger.WP8.Pages
{
	public class FirstLaunchPage : PhoneApplicationPage
	{
		private bool _contentLoaded;

		public FirstLaunchPage()
		{
			InitializeComponent();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			_003C_003En__FabricatedMethode(e);
			FacebookContext context = ServiceLocator.Current.GetInstance<FacebookContext>();
			Navigator navigator = ServiceLocator.Current.GetInstance<Navigator>();
			FacebookUri uri;
			if (context.IsAuthenticated())
			{
				if (context.HasUserCompletedPhoneNumberFlow())
				{
					uri = FacebookUri.RootUri;
					string photoId = ParsePhotoLaunchParameters(((Page)this).NavigationContext.QueryString);
					if (!string.IsNullOrWhiteSpace(photoId))
					{
						uri = FacebookUri.GetComposerUri(photoId, true);
					}
					ShareOperation shareOperation = (Application.Current as App).PendingShare;
					if (shareOperation != null)
					{
						DataPackageView data = shareOperation.Data;
						if (data.Contains(StandardDataFormats.WebLink))
						{
							string title = data.Properties.Title;
							string text = $"{title} - {await data.GetWebLinkAsync()}";
							uri = FacebookUri.GetComposerUri(null, true, null, null, text);
						}
						else if (data.Contains(StandardDataFormats.Text))
						{
							uri = FacebookUri.GetComposerUri(null, true, null, null, await data.GetTextAsync());
						}
					}
				}
				else
				{
					App app = Application.Current as App;
					app.PrefetchLargeProfilePicture();
					uri = FacebookUri.GetAppUri(UriId.PhoneNumber, new UriQuery { { "loginflow", "true" } });
				}
			}
			else
			{
				uri = FacebookUri.GetFirstNavigationUri();
			}
			navigator.Reset(uri);
		}

		private string ParsePhotoLaunchParameters(IDictionary<string, string> queryStrings)
		{
			if (queryStrings == null)
			{
				return null;
			}
			if (queryStrings.ContainsKey("Action") && queryStrings["Action"] == "ShareContent" && queryStrings.ContainsKey("FileId"))
			{
				return queryStrings["FileId"];
			}
			return null;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/FirstLaunchPage.xaml", UriKind.Relative));
			}
		}

		[CompilerGenerated]
		private void _003C_003En__FabricatedMethode(NavigationEventArgs P_0)
		{
			((Page)this).OnNavigatedTo(P_0);
		}
	}
}

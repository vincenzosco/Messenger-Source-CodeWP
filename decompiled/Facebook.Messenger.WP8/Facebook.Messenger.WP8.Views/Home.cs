using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Facebook.Services.Analytics;
using Facebook.Services.Analytics.Logging;
using Microsoft.Phone.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Views
{
	public class Home : PhoneApplicationPage
	{
		internal Grid LayoutRoot;

		private bool _contentLoaded;

		public Home()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			((Page)this).OnNavigatedTo(e);
			Navigator instance = ServiceLocator.Current.GetInstance<Navigator>();
			FacebookContext instance2 = ServiceLocator.Current.GetInstance<FacebookContext>();
			if (!instance2.IsAuthenticated())
			{
				Logger.Info(this, string.Format("Context is not authenticated, resetting to login page: {0}", e.Uri), "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\Home.xaml.cs", 31);
				instance.Reset(FacebookUri.GetFirstNavigationUri());
				return;
			}
			string text = null;
			string actionId = null;
			try
			{
				string[] array = e.Uri.ToString().Split('?');
				if (array.Length == 2)
				{
					string[] array2 = array[1].Split('&');
					string[] array3 = array2;
					foreach (string text2 in array3)
					{
						string[] array4 = text2.Split('=');
						if (array4.Length == 2)
						{
							switch (array4[0])
							{
							case "unified_tid":
								text = Url.Decode(array4[1]);
								break;
							case "action_id":
								actionId = Url.Decode(array4[1]);
								break;
							case "entry_point":
								HandleEntryPoint(Url.Decode(array4[1]));
								break;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(this, string.Format("Error parsing uri for thread id from push : {0}", e.Uri), "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\Home.xaml.cs", 72);
				Logger.Error(this, ex.ToString(), "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\Home.xaml.cs", 73);
			}
			if (text == null)
			{
				Logger.Info(this, string.Format("Thread id is null, navigating to root : {0}", e.Uri), "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\Home.xaml.cs", 78);
				instance.Reset(FacebookUri.RootUri);
			}
			else
			{
				Logger.Info(this, string.Format("Thread id is {0}, navigating to thread : {1}", text, e.Uri), "OnNavigatedTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\Home.xaml.cs", 83);
				FacebookUri threadUri = FacebookUri.GetThreadUri(text, null, actionId, true);
				instance.Reset(threadUri);
			}
		}

		private void HandleEntryPoint(string e)
		{
			EntryPoint result;
			if (!Enum.TryParse<EntryPoint>(e, out result))
			{
				Logger.Warning(string.Format("Unable to parse Entry Point: {0}", e), "HandleEntryPoint", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Views\\Home.xaml.cs", 96);
				return;
			}
			ApplicationState instance = ServiceLocator.Current.GetInstance<ApplicationState>();
			Facebook.Services.Analytics.Service instance2 = ServiceLocator.Current.GetInstance<Facebook.Services.Analytics.Service>();
			switch (result)
			{
			case EntryPoint.FailedMessageToast:
				instance.ActivatedFromMessageFailedToast = true;
				BackgroundSendingEventLogger.LogAppStartedFromMessageFailedToast(instance2);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case EntryPoint.Unknown:
				break;
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Views/Home.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			}
		}
	}
}

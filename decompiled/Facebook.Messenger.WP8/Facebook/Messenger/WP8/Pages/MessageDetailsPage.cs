using System;
using System.Device.Location;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Facebook.Messenger.WP8.Pages
{
	public class MessageDetailsPage : MessageDetailsViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private MenuBarEntry<bool> _viewTimelineMenuItem;

		internal MessageDetailsViewModelPage MessageDetailsViewPage;

		internal Grid LayoutRoot;

		internal Border MapImageBorder;

		internal Image MapImage;

		private bool _contentLoaded;

		public MessageDetailsPage()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBar();
			InitializeAudioPlayback((Panel)(object)LayoutRoot);
			((FrameworkElement)this).Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				if (vm.MessageViewModel != null)
				{
					UpdateAppBarMenuItems();
					vm.MessageViewModel.MapImageWidth = ((FrameworkElement)this).ActualWidth;
					if (vm.MessageViewModel.HasCoordinates && DebuggingUtility.CanLogSensitiveInfo)
					{
						Logger.Info($"Message sent from coordinates: {vm.MessageViewModel.Coordinates.Latitude}, {vm.MessageViewModel.Coordinates.Longitude}", "OnLoaded", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\MessageDetailsPage.xaml.cs", 44);
						Logger.Info($"Map image url: {vm.MessageViewModel.MapImageUrl}", "OnLoaded", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Pages\\MessageDetailsPage.xaml.cs", 47);
					}
				}
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			InitializeAudioPlayback((Panel)(object)LayoutRoot);
		}

		private void InitializeAppBar()
		{
			_appBarHelper = new AppBarHelper(((PhoneApplicationPage)this).ApplicationBar);
			_viewTimelineMenuItem = _appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(true, StringLibrary.View_Timeline_Menu_Item, OnViewTimelineMenuItemClicked));
		}

		private void UpdateAppBarMenuItems()
		{
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				FacebookProfile profile = vm.Context.Session.Profile;
				bool flag = vm.MessageViewModel.Sender != null && profile.Id == vm.MessageViewModel.Sender.Id;
				IApplicationBar applicationBar = ((PhoneApplicationPage)this).ApplicationBar;
				bool isVisible = (_viewTimelineMenuItem.IsVisible = !flag && !vm.IsPartialUser && !vm.MessageFromPartialUser);
				applicationBar.IsVisible = isVisible;
			});
		}

		private void OnViewTimelineMenuItemClicked(object sender, EventArgs e)
		{
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				vm.LaunchTimeline();
			});
		}

		private void MapImage_OnTap(object sender, GestureEventArgs e)
		{
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Expected Obj, but got Unknown
				if (vm.MessageViewModel != null && vm.MessageViewModel.HasCoordinates)
				{
					BingMapsTask val = new BingMapsTask();
					val.Center = new GeoCoordinate(vm.MessageViewModel.Coordinates.Latitude, vm.MessageViewModel.Coordinates.Longitude);
					val.ZoomLevel = 16.0;
					BingMapsTask val2 = val;
					val2.Show();
				}
			});
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/MessageDetailsPage.xaml", UriKind.Relative));
				MessageDetailsViewPage = (MessageDetailsViewModelPage)((FrameworkElement)this).FindName("MessageDetailsViewPage");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				MapImageBorder = (Border)((FrameworkElement)this).FindName("MapImageBorder");
				MapImage = (Image)((FrameworkElement)this).FindName("MapImage");
			}
		}
	}
}

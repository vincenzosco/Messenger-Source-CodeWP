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
using GestureEventArgs = Microsoft.Phone.Controls.GestureEventArgs;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class MessageDetailsPage : MessageDetailsViewModelPage
	{
		private AppBarHelper _appBarHelper;

		private MenuBarEntry<bool> _viewTimelineMenuItem;






		public MessageDetailsPage()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			InitializeAppBar();
			InitializeAudioPlayback((Panel)(object)LayoutRoot);
			base.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				if (vm.MessageViewModel != null)
				{
					UpdateAppBarMenuItems();
					vm.MessageViewModel.MapImageWidth = base.ActualWidth;
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
			_appBarHelper = new AppBarHelper(base.ApplicationBar);
			_viewTimelineMenuItem = _appBarHelper.CreateMenuEntry<bool>(new EntryState<bool>(true, StringLibrary.View_Timeline_Menu_Item, OnViewTimelineMenuItemClicked));
		}

		private void UpdateAppBarMenuItems()
		{
			WithViewModel((MessageDetailsViewModel vm) =>
			{
				FacebookProfile profile = vm.Context.Session.Profile;
				bool flag = vm.MessageViewModel.Sender != null && profile.Id == vm.MessageViewModel.Sender.Id;
				IApplicationBar applicationBar = base.ApplicationBar;
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

	}
}

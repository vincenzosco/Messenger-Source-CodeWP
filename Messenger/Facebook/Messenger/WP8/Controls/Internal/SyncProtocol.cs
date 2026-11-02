using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels.Internal;

namespace Facebook.Messenger.WP8.Controls.Internal
{
	public partial class SyncProtocol : UserControl
	{

		public SyncProtocol()
		{
			InitializeComponent();
		}

		private void SimulateErrorButton_OnClick(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)(object)this).CallWithViewModel((SyncProtocolSettingsViewModel vm) =>
			{
				vm.SimulateSyncError();
			});
		}

		private void ClearThreadCacheButton_OnClick(object sender, RoutedEventArgs e)
		{
			((FrameworkElement)(object)this).CallWithViewModel((SyncProtocolSettingsViewModel vm) =>
			{
				vm.ClearThreadCache();
			});
		}

	}
}

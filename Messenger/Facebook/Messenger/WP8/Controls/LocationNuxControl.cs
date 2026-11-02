using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class LocationNuxControl : UserControl
	{


		public LocationNuxControl()
		{
			InitializeComponent();
		}

		private void LocationWarning_OnTap(object sender, GestureEventArgs e)
		{
			MessageBaseViewModel messageBaseViewModel = base.DataContext as MessageBaseViewModel;
			if (messageBaseViewModel != null)
			{
				messageBaseViewModel.ShowLocationWarning = false;
			}
		}

	}
}

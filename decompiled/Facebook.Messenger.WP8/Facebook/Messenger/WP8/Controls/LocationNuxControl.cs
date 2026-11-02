using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls;

public class LocationNuxControl : UserControl
{
	internal Grid LocationWarning;

	private bool _contentLoaded;

	public LocationNuxControl()
	{
		InitializeComponent();
	}

	private void LocationWarning_OnTap(object sender, GestureEventArgs e)
	{
		if (((FrameworkElement)this).DataContext is MessageBaseViewModel messageBaseViewModel)
		{
			messageBaseViewModel.ShowLocationWarning = false;
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
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/LocationNuxControl.xaml", UriKind.Relative));
			LocationWarning = (Grid)((FrameworkElement)this).FindName("LocationWarning");
		}
	}
}

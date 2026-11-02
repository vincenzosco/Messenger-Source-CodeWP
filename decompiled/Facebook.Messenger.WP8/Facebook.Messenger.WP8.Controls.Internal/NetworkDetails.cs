using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Facebook.Messenger.WP8.Controls.Internal
{
	public class NetworkDetails : UserControl
	{
		private bool _contentLoaded;

		public NetworkDetails()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Internal/NetworkDetails.xaml", UriKind.Relative));
			}
		}
	}
}

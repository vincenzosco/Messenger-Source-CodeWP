using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Facebook.Messenger.WP8.Controls.Internal;

public class BuildDetails : UserControl
{
	private bool _contentLoaded;

	public BuildDetails()
	{
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Internal/BuildDetails.xaml", UriKind.Relative));
		}
	}
}

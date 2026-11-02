using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Facebook.Messenger.WP8.Controls;

public class PeopleUserControl : UserControl
{
	internal Grid LayoutRoot;

	private bool _contentLoaded;

	public PeopleUserControl()
	{
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/PeopleUserControl..xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
		}
	}
}

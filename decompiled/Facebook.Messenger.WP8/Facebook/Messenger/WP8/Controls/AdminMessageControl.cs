using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls;

public class AdminMessageControl : UserControl
{
	private const string AdminPhotoId = "1";

	internal Grid LayoutRoot;

	internal TextBlock AdminMessageBlock;

	internal Border AdminImageBlock;

	internal Image AdminImage;

	private bool _contentLoaded;

	public event EventHandler<NavigationEventArgs> NavigateEvent;

	public AdminMessageControl()
	{
		InitializeComponent();
	}

	private void AdminImageBlock_OnTap(object sender, GestureEventArgs e)
	{
		if (((FrameworkElement)this).DataContext is MessageViewModel messageViewModel)
		{
			FacebookUri attachmentsUri = FacebookUri.GetAttachmentsUri(messageViewModel.Id, "1", null, 0);
			if (NavigateEvent != null)
			{
				NavigateEvent(this, new NavigationEventArgs(attachmentsUri));
			}
		}
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected Obj, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/AdminMessageControl.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			AdminMessageBlock = (TextBlock)((FrameworkElement)this).FindName("AdminMessageBlock");
			AdminImageBlock = (Border)((FrameworkElement)this).FindName("AdminImageBlock");
			AdminImage = (Image)((FrameworkElement)this).FindName("AdminImage");
		}
	}
}

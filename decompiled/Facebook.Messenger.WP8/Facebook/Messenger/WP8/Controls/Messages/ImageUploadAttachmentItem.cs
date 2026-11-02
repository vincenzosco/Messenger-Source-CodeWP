using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Messenger.WP8.Controls.Messages;

public class ImageUploadAttachmentItem : BaseControl
{
	internal Border Panel;

	internal Image Photo;

	private bool _contentLoaded;

	public ImageUploadAttachmentItem()
	{
		InitializeComponent();
	}

	protected override void ViewModelUpdated(object oldViewModel)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected Obj, but got Unknown
		base.ViewModelUpdated(oldViewModel);
		if (!(((FrameworkElement)this).DataContext is Attachment attachment))
		{
			((DependencyObject)Photo).ClearValue(Image.SourceProperty);
			return;
		}
		BitmapImage val = new BitmapImage(new Uri(attachment.Path, UriKind.Relative));
		if (attachment.Width > attachment.Height)
		{
			val.DecodePixelHeight = (int)((FrameworkElement)Photo).Height;
		}
		else
		{
			val.DecodePixelWidth = (int)((FrameworkElement)Photo).Width;
		}
		Photo.Source = (ImageSource)(object)val;
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Messages/ImageUploadAttachmentItem.xaml", UriKind.Relative));
			Panel = (Border)((FrameworkElement)this).FindName("Panel");
			Photo = (Image)((FrameworkElement)this).FindName("Photo");
		}
	}
}

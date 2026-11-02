using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Messenger.WP8.Controls.Messages
{
	public partial class ImageUploadAttachmentItem : BaseControl
	{



		public ImageUploadAttachmentItem()
		{
			InitializeComponent();
		}

		protected override void ViewModelUpdated(object oldViewModel)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected Obj, but got Unknown
			base.ViewModelUpdated(oldViewModel);
			Attachment attachment = base.DataContext as Attachment;
			if (attachment == null)
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

	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Events;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace Facebook.Messenger.WP8.Controls;

public class ImageAttachmentsControl : BaseControl, IResetableView
{
	private const int MaxImagePreviewsForDisplay = 6;

	private static readonly Uri PlaceholderImage = new Uri("/Assets/Messages/PhotoPlaceholder.png", UriKind.Relative);

	private static readonly Size MaxImageSizeFor1 = new Size(290.0, 290.0);

	private static readonly Size DefaultImageSizeFor2 = new Size(140.0, 140.0);

	private static readonly Size DefaultImageSizeFor4 = DefaultImageSizeFor2;

	private static readonly Size DefaultImageSize = new Size(90.0, 90.0);

	private string _messageId;

	public static readonly DependencyProperty ShowAllProperty = DependencyProperty.Register("ShowAll", typeof(bool), typeof(ImageAttachmentsControl), new PropertyMetadata((object)false));

	internal WrapPanel AttachmentPanel;

	private bool _contentLoaded;

	public bool ShowAll
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(ShowAllProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ShowAllProperty, (object)value);
		}
	}

	public event EventHandler<NavigationEventArgs> NavigateEvent;

	public ImageAttachmentsControl()
	{
		InitializeComponent();
	}

	private bool IsCurrentControlDisplayingMessage(MessageViewModel m)
	{
		if (m != null && _messageId != null)
		{
			return m.MatchesMessageId(_messageId);
		}
		return false;
	}

	public void UpdateView(MessageViewModel m)
	{
		if (IsCurrentControlDisplayingMessage(m))
		{
			UpdateExistingView(m);
			return;
		}
		_messageId = null;
		if (m != null && m.HasImageAttachments())
		{
			DisplayImageAttachments(m);
		}
	}

	public void UpdateExistingView(MessageViewModel m)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		List<MailAttachment> list = (ShowAll ? m.ImageAttachments : m.ImageAttachments.Take(6)).ToList();
		UpdateState(m);
		Size imageSize = GetImageSize(list);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			if (i < ((PresentationFrameworkCollection<UIElement>)(object)((Panel)AttachmentPanel).Children).Count)
			{
				UIElement val = ((PresentationFrameworkCollection<UIElement>)(object)((Panel)AttachmentPanel).Children)[i];
				Grid val2 = (Grid)(object)((val is Grid) ? val : null);
				if (val2 == null || ((PresentationFrameworkCollection<UIElement>)(object)((Panel)val2).Children).Count == 0)
				{
					continue;
				}
				UIElement val3 = ((PresentationFrameworkCollection<UIElement>)(object)((Panel)val2).Children)[0];
				FrameworkElement val4 = (FrameworkElement)(object)((val3 is FrameworkElement) ? val3 : null);
				if (val4 != null)
				{
					Uri imageUri = ImageCacheExtensions.GetImageUri(val4);
					Uri imageUrl = GetImageUrl(m, list, list[i], imageSize);
					if (imageUrl == null)
					{
						break;
					}
					if (imageUri == null || !imageUri.Equals(imageUrl))
					{
						ImageCacheExtensions.SetImageUri(val4, imageUrl, clearExistingImage: false);
					}
				}
			}
			else
			{
				Uri imageUrl2 = GetImageUrl(m, list, list[i], imageSize);
				AddImage(imageUrl2, imageSize);
			}
		}
	}

	private void DisplayImageAttachments(MessageViewModel m)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		List<MailAttachment> list = (ShowAll ? m.ImageAttachments : m.ImageAttachments.Take(6)).ToList();
		UpdateState(m);
		Size imageSize = GetImageSize(list);
		foreach (MailAttachment item in list)
		{
			Uri imageUrl = GetImageUrl(m, list, item, imageSize);
			AddImage(imageUrl, imageSize);
		}
	}

	private Uri GetImageUrl(MessageViewModel m, IList<MailAttachment> images, MailAttachment a, Size size)
	{
		string uriString = null;
		if (a.Id != null)
		{
			int size2 = (int)(size.Width * (double)Application.Current.Host.Content.ScaleFactor / 100.0);
			uriString = AttachmentUriGenerator.UrlForAttachment(a.Id, m.Id, size2);
		}
		else if (a.LocalUrl != null)
		{
			uriString = ((Enumerable.Count(images) > 1) ? a.Image.PreviewUrl : a.LocalUrl);
		}
		Uri.TryCreate(uriString, UriKind.Absolute, out var result);
		return result;
	}

	private void UpdateState(MessageViewModel m)
	{
		_messageId = m.Id;
	}

	private Size GetImageSize(IList<MailAttachment> imageAttachments)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		return Enumerable.Count(imageAttachments) switch
		{
			1 => GetBestImageSizeForSingleImage(imageAttachments.First()), 
			2 => DefaultImageSizeFor2, 
			4 => DefaultImageSizeFor4, 
			_ => DefaultImageSize, 
		};
	}

	private static Size GetBestImageSizeForSingleImage(MailAttachment attachment)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (!attachment.HasValidImageData())
		{
			return MaxImageSizeFor1;
		}
		int width = attachment.Image.Width;
		int height = attachment.Image.Height;
		Size maxImageSizeFor = MaxImageSizeFor1;
		double val = maxImageSizeFor.Width / (double)width;
		Size maxImageSizeFor2 = MaxImageSizeFor1;
		double num = Math.Min(val, maxImageSizeFor2.Height / (double)height);
		if (num > 1.0)
		{
			num = 1.0;
		}
		return new Size((double)width * num, (double)height * num);
	}

	private void AddImage(Uri uri, Size size)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected Obj, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected Obj, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected Obj, but got Unknown
		if (!(uri == null))
		{
			Grid val = new Grid();
			((FrameworkElement)val).Height = size.Height;
			((FrameworkElement)val).Width = size.Width;
			((FrameworkElement)val).Margin = new Thickness(5.0);
			((Panel)val).Background = (Brush)new SolidColorBrush(Colors.Transparent);
			Grid val2 = val;
			((UIElement)val2).Tap += ImageOnTap;
			Image val3 = new Image();
			((FrameworkElement)val3).HorizontalAlignment = (HorizontalAlignment)1;
			((FrameworkElement)val3).VerticalAlignment = (VerticalAlignment)1;
			val3.Stretch = (Stretch)3;
			Image val4 = val3;
			ImageCacheExtensions.SetPlaceholderUri((FrameworkElement)(object)val4, PlaceholderImage);
			ImageCacheExtensions.SetImageUri((FrameworkElement)(object)val4, uri);
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)val2).Children).Add((UIElement)(object)val4);
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)AttachmentPanel).Children).Add((UIElement)(object)val2);
		}
	}

	public void ResetView(bool recycled)
	{
		if (!recycled || AttachmentPanel == null || ((Panel)AttachmentPanel).Children == null || ((PresentationFrameworkCollection<UIElement>)(object)((Panel)AttachmentPanel).Children).Count <= 0)
		{
			return;
		}
		foreach (UIElement item in (PresentationFrameworkCollection<UIElement>)(object)((Panel)AttachmentPanel).Children)
		{
			Grid val = (Grid)(object)((item is Grid) ? item : null);
			if (val != null)
			{
				((UIElement)val).Tap -= ImageOnTap;
				UIElement val2 = ((PresentationFrameworkCollection<UIElement>)(object)((Panel)val).Children)[0];
				Image image = (Image)(object)((val2 is Image) ? val2 : null);
				ResetImage(image);
			}
		}
		((PresentationFrameworkCollection<UIElement>)(object)((Panel)AttachmentPanel).Children).Clear();
	}

	private static void ResetImage(Image image)
	{
		if (image != null)
		{
			ImageSource source = image.Source;
			BitmapImage val = (BitmapImage)(object)((source is BitmapImage) ? source : null);
			if (val != null)
			{
				val.UriSource = null;
			}
			image.Source = null;
		}
	}

	private void ImageOnTap(object sender, GestureEventArgs gestureEventArgs)
	{
		Grid val = (Grid)((sender is Grid) ? sender : null);
		if (val == null)
		{
			return;
		}
		int num = ((PresentationFrameworkCollection<UIElement>)(object)((Panel)AttachmentPanel).Children).IndexOf((UIElement)(object)val);
		if (num < 0 || !(((FrameworkElement)this).DataContext is MessageViewModel messageViewModel))
		{
			return;
		}
		Uri uri;
		if (!string.IsNullOrEmpty(messageViewModel.ImageAttachments.First().Id))
		{
			string attachmentIds = string.Join(",", messageViewModel.ImageAttachments.Select((MailAttachment attachment) => attachment.Id));
			string attachmentFbids = string.Join(",", messageViewModel.ImageAttachments.Select((MailAttachment attachment) => attachment.Fbid));
			uri = FacebookUri.GetAttachmentsUri(_messageId, attachmentIds, attachmentFbids, num);
		}
		else
		{
			string filePaths = JsonConvert.SerializeObject(messageViewModel.ImageAttachments.Select((MailAttachment attachment) => attachment.LocalUrl));
			uri = FacebookUri.GetAttachmentsUriFromFilePaths(_messageId, filePaths, num);
		}
		if (NavigateEvent != null)
		{
			NavigateEvent(this, new NavigationEventArgs(uri));
		}
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ImageAttachmentsControl.xaml", UriKind.Relative));
			AttachmentPanel = (WrapPanel)((FrameworkElement)this).FindName("AttachmentPanel");
		}
	}

	static ImageAttachmentsControl()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected Obj, but got Unknown
	}
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;

namespace Microsoft.Phone.Controls
{
	public class ThumbnailedImageViewer : Control
	{
		private enum ImageBindingState
		{
			ScreenSizeThumbnail,
			FullSizePhoto
		}

		private Image _image;

		private FrameworkElement _placeholder;

		private BitmapImage _thumbnailBitmapImage;

		private ImageSource _thumbnailImageSource;

		private BitmapImage _fullResolutionBitmapImage;

		private ImageSource _fullResolutionImageSource;

		public ThumbnailedImageViewer()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected Obj, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected Obj, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected Obj, but got Unknown
			base.DefaultStyleKey = typeof(ThumbnailedImageViewer);
			DependencyProperty val = DependencyProperty.RegisterAttached("DataContextProperty", typeof(object), typeof(FrameworkElement), new PropertyMetadata((PropertyChangedCallback)OnDataContextPropertyChanged));
			base.SetBinding(val, new Binding());
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			DependencyObject templateChild = base.GetTemplateChild("Image");
			_image = (Image)(object)((templateChild is Image) ? templateChild : null);
			DependencyObject templateChild2 = base.GetTemplateChild("Placeholder");
			_placeholder = (FrameworkElement)(object)((templateChild2 is FrameworkElement) ? templateChild2 : null);
			ShowPlaceholder();
		}

		private static void OnDataContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ThumbnailedImageViewer thumbnailedImageViewer = (ThumbnailedImageViewer)(object)d;
			IImage image = e.NewValue as IImage;
			thumbnailedImageViewer.ClearImageSources();
			if (image != null)
			{
				thumbnailedImageViewer.ShowPlaceholder();
				thumbnailedImageViewer.BeginLoadingFullResolutionImage();
			}
			else
			{
				thumbnailedImageViewer._image.Source = null;
			}
		}

		private void OnThumbnailOpened(object sender, EventArgs e)
		{
			_image.Source = null;
			_image.Source = _thumbnailImageSource;
			ClearImageSources();
			HidePlaceholder();
			base.InvalidateMeasure();
		}

		private void OnFullSizeImageOpened(object sender, EventArgs e)
		{
			ImageSource fullResolutionImageSource = _fullResolutionImageSource;
			BitmapImage val = (BitmapImage)(object)((fullResolutionImageSource is BitmapImage) ? fullResolutionImageSource : null);
			if (val != null)
			{
				_image.Source = null;
				_image.Source = (ImageSource)(object)val;
				Messenger.Default.Send(new PhotoLoadingSignal(val.UriSource, PhotoLoadingStatus.Loaded));
				ClearImageSources();
				HidePlaceholder();
				base.InvalidateMeasure();
			}
		}

		private void ClearImageSources()
		{
			if (_thumbnailBitmapImage != null)
			{
				_thumbnailBitmapImage.ImageOpened -= OnThumbnailOpened;
			}
			_thumbnailBitmapImage = null;
			if (_fullResolutionBitmapImage != null)
			{
				_fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
				_fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
			}
			_fullResolutionBitmapImage = null;
			_thumbnailImageSource = null;
			_fullResolutionImageSource = null;
		}

		private bool CurrentImageSizeIsTooSmall(Size availableSize)
		{
			ImageSource source = _image.Source;
			BitmapImage val = (BitmapImage)(object)((source is BitmapImage) ? source : null);
			if (val == null)
			{
				return true;
			}
			return (double)((BitmapSource)val).PixelWidth < availableSize.Width && (double)((BitmapSource)val).PixelHeight < availableSize.Height;
		}

		private void BeginLoadingFullResolutionImage()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected Obj, but got Unknown
			if (base.DataContext is IImage)
			{
				if (_fullResolutionBitmapImage != null)
				{
					_fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
				}
				_fullResolutionBitmapImage = null;
				Uri imageUri = ((IImage)base.DataContext).GetImageUri();
				Messenger.Default.Send(new PhotoLoadingSignal(imageUri, PhotoLoadingStatus.Loading));
				_fullResolutionBitmapImage = new BitmapImage();
				_fullResolutionBitmapImage.ImageOpened += OnFullSizeImageOpened;
				_fullResolutionBitmapImage.CreateOptions = (BitmapCreateOptions)16;
				_fullResolutionBitmapImage.UriSource = imageUri;
				_fullResolutionImageSource = (ImageSource)(object)_fullResolutionBitmapImage;
			}
		}

		private void ShowPlaceholder()
		{
			if (_placeholder != null)
			{
				((UIElement)_placeholder).Visibility = Visibility.Visible;
			}
			if (_image != null)
			{
				((UIElement)_image).Visibility = Visibility.Collapsed;
			}
		}

		private void HidePlaceholder()
		{
			if (_image != null)
			{
				((UIElement)_image).Visibility = Visibility.Visible;
			}
			if (_placeholder != null)
			{
				((UIElement)_placeholder).Visibility = Visibility.Collapsed;
			}
		}
	}
}

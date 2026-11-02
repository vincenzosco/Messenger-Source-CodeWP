using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Facebook.Messenger.WP8.Controls.Photos
{
	public partial class PhotoCroppingPopup : FacebookPopup
	{
		private const double CroppingDiameter = 400.0;

		private double _maxScale = 10.0;

		private int _imageWidth;

		private int _imageHeight;

		private double _scale = 1.0;

		private double _minScale;

		private double _coercedScale;

		private double _originalScale;

		private Size _viewportSize;

		private bool _pinching;

		private Point _screenMidpoint;

		private Point _relativeMidpoint;

		private bool _isAnimating;













		public event EventHandler<BitmapSource> CropCompleted;

		public PhotoCroppingPopup()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			((FrameworkElement)ContentPanel).Width = MaxContentPanelWidth;
			((FrameworkElement)ContentPanel).Height = MaxContentPanelHeight;
			double num = (MaxContentPanelWidth - 400.0) / 2.0;
			double num2 = (MaxContentPanelHeight - 400.0) / 2.0;
			((Control)viewport).Padding = new Thickness(num, num2, num, num2);
			RadialBrush.Center = new Point(MaxContentPanelWidth / 2.0, MaxContentPanelHeight / 2.0);
			RadialBrush.GradientOrigin = new Point(MaxContentPanelWidth / 2.0, MaxContentPanelHeight / 2.0);
		}

		public void SetPhoto(PhotoData photoData)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected Obj, but got Unknown
			WriteableBitmap val = new WriteableBitmap(photoData.Source);
			double num = MaxContentPanelHeight - ((FrameworkElement)CustomApplicationBar).Height;
			double num2 = Math.Min(num / (double)((BitmapSource)val).PixelHeight, MaxContentPanelWidth / (double)((BitmapSource)val).PixelWidth);
			int height = (int)((double)((BitmapSource)val).PixelHeight * num2);
			int width = (int)((double)((BitmapSource)val).PixelWidth * num2);
			val = val.Resize(width, height, WriteableBitmapExtensions.Interpolation.Bilinear);
			RotationAngle rotationAngle = photoData.GetRotationAngle();
			if (rotationAngle != RotationAngle.Portrait)
			{
				val = val.Rotate(rotationAngle.ToDegrees());
			}
			((FrameworkElement)Photo).Height = (_imageHeight = ((BitmapSource)val).PixelHeight);
			((FrameworkElement)Photo).Width = (_imageWidth = ((BitmapSource)val).PixelWidth);
			Photo.Source = (ImageSource)(object)val;
			OnImageOpened();
		}

		private async void CropPhotoButton_OnTap(object sender, GestureEventArgs e)
		{
			if ((int)SendAnimation.GetCurrentState() != 0)
			{
				((UIElement)MessengerProgressBar).Visibility = Visibility.Visible;
				MessengerProgressBar.IsIndeterminate = true;
				await Task.Delay(10);
				SendAnimation.Begin();
				((UIElement)MessengerProgressBar).Visibility = Visibility.Collapsed;
				MessengerProgressBar.IsIndeterminate = false;
			}
		}

		private void SendAnimation_OnCompleted(object sender, EventArgs e)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected Obj, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			if (CropCompleted != null)
			{
				((UIElement)Overlay).Opacity = 0.0;
				Rect val = SnapRectangleToBounds(viewport.Viewport, viewport.Bounds);
				Rect val2 = default(Rect);
				val2.X = (((FrameworkElement)ContentPanel).ActualWidth - val.Width) / 2.0;
				val2.Y = (((FrameworkElement)ContentPanel).ActualHeight - val.Height) / 2.0;
				val2.Width = val.Width;
				val2.Height = val.Height;
				Rect region = val2;
				WriteableBitmap bmp = new WriteableBitmap((UIElement)(object)ContentPanel, (Transform)null);
				bmp = bmp.Crop(region);
				CropCompleted(this, (BitmapSource)(object)bmp);
			}
			Reset();
		}

		protected override void Reset()
		{
			_imageHeight = (_imageWidth = 0);
			Photo.Source = null;
			((UIElement)Overlay).Opacity = 0.5;
			Transform renderTransform = ((UIElement)ContentPanel).RenderTransform;
			CompositeTransform val = (CompositeTransform)(object)((renderTransform is CompositeTransform) ? renderTransform : null);
			if (val != null)
			{
				val.TranslateY = 0.0;
			}
		}

		private void viewport_ViewportChanged(object sender, ViewportChangedEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			Rect val = viewport.Viewport;
			double width = val.Width;
			Rect val2 = viewport.Viewport;
			Size val3 = new Size(width, val2.Height);
			if (val3 != _viewportSize)
			{
				_viewportSize = val3;
				CoerceScale(true);
				ResizeImage(true);
			}
		}

		private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			_pinching = false;
			_originalScale = _scale;
		}

		private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			if (e.PinchManipulation != null)
			{
				e.Handled = true;
				if (!_pinching)
				{
					_pinching = true;
					Point center = e.PinchManipulation.Original.Center;
					_relativeMidpoint = new Point(center.X / ((FrameworkElement)Photo).ActualWidth, center.Y / ((FrameworkElement)Photo).ActualHeight);
					GeneralTransform val = ((UIElement)Photo).TransformToVisual((UIElement)(object)viewport);
					_screenMidpoint = val.Transform(center);
				}
				_scale = _originalScale * e.PinchManipulation.CumulativeScale;
				CoerceScale(false);
				ResizeImage(false);
			}
			else if (_pinching)
			{
				_pinching = false;
				_originalScale = (_scale = _coercedScale);
			}
		}

		private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			_pinching = false;
			_scale = _coercedScale;
		}

		private void OnImageOpened()
		{
			_scale = 0.0;
			CoerceScale(true);
			_scale = _coercedScale;
			ResizeImage(true);
		}

		private void ResizeImage(bool center)
		{
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			if (_coercedScale != 0.0 && _imageHeight != 0 && _imageWidth != 0)
			{
				double num = (((FrameworkElement)canvas).Width = Math.Round((double)_imageWidth * _coercedScale));
				double num3 = num;
				double num4 = (((FrameworkElement)canvas).Height = Math.Round((double)_imageHeight * _coercedScale));
				double num6 = num4;
				ScaleTransform val = xform;
				double scaleX = (xform.ScaleY = _coercedScale);
				val.ScaleX = scaleX;
				viewport.Bounds = new Rect(0.0, 0.0, num3, num6);
				if (center)
				{
					Rect val2 = viewport.Viewport;
					double num7 = Math.Round((num3 - val2.Width) / 2.0);
					Rect val3 = viewport.Viewport;
					double num8 = Math.Round((num6 - val3.Height) / 2.0);
					viewport.SetViewportOrigin(new Point(num7, num8));
				}
				else
				{
					Point val4 = new Point(num3 * _relativeMidpoint.X, num6 * _relativeMidpoint.Y);
					double num9 = val4.X - _screenMidpoint.X;
					Thickness padding = ((Control)viewport).Padding;
					double num10 = num9 + padding.Left;
					double num11 = val4.Y - _screenMidpoint.Y;
					Thickness padding2 = ((Control)viewport).Padding;
					Point viewportOrigin = new Point(num10, num11 + padding2.Top);
					viewport.SetViewportOrigin(viewportOrigin);
				}
			}
		}

		private void CoerceScale(bool recompute)
		{
			if (recompute && _imageWidth != 0 && _imageHeight != 0 && viewport != null)
			{
				double val = 400.0 / (double)_imageWidth;
				double val2 = 400.0 / (double)_imageHeight;
				_minScale = Math.Max(val, val2);
			}
			if (_minScale > _maxScale)
			{
				_maxScale = _minScale;
			}
			_coercedScale = Math.Min(_maxScale, Math.Max(_scale, _minScale));
		}

		private void Viewport_OnManipulationStateChanged(object sender, ManipulationStateChangedEventArgs e)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (_isAnimating)
			{
				Rect val = SnapRectangleToBounds(viewport.Viewport, viewport.Bounds);
				viewport.SetViewportOrigin(new Point(val.X, val.Y));
				_isAnimating = false;
			}
			else if ((int)viewport.ManipulationState == 2)
			{
				_isAnimating = true;
			}
		}

		private Rect SnapRectangleToBounds(Rect source, Rect bounds)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			double num = source.X;
			double num2 = source.Y;
			if (source.Top < bounds.Top)
			{
				num2 = bounds.Top;
			}
			else if (source.Bottom > bounds.Bottom)
			{
				num2 = bounds.Bottom - source.Height;
			}
			if (source.Left < bounds.Left)
			{
				num = bounds.Left;
			}
			else if (source.Right > bounds.Right)
			{
				num = bounds.Right - source.Width;
			}
			return new Rect(num, num2, source.Width, source.Height);
		}

	}
}

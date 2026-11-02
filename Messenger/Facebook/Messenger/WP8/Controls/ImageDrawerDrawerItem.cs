using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ImageDrawerDrawerItem : ImageDrawerListItem
	{









		public event EventHandler SendClicked;

		public event EventHandler EditClicked;

		public event EventHandler OverlayTapped;

		public ImageDrawerDrawerItem()
		{
			InitializeComponent();
		}

		protected override void OnItemValueAssigned()
		{
			ViewModel.PropertyChanged += OnPropertyChanged;
			Photo.Source = ViewModel.GetDisplayImage((int)((FrameworkElement)LayoutRoot).Width, (int)((FrameworkElement)LayoutRoot).Height);
			UpdateTransform();
			SetSelectedOverlay();
		}

		private void UpdateTransform()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected Obj, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected Obj, but got Unknown
			RotationAngle rotationAngle = ViewModel.GetRotationAngle();
			if (rotationAngle != RotationAngle.Portrait)
			{
				BitmapImage val = (BitmapImage)Photo.Source;
				int decodePixelHeight = val.DecodePixelHeight;
				int decodePixelWidth = val.DecodePixelWidth;
				RotateTransform val2 = new RotateTransform();
				val2.Angle = rotationAngle.ToDegrees();
				val2.CenterX = (double)decodePixelWidth / 2.0;
				val2.CenterY = (double)decodePixelHeight / 2.0;
				RotateTransform renderTransform = val2;
				((UIElement)Photo).RenderTransform = (Transform)(object)renderTransform;
			}
		}

		protected override void Recycled(ImageDrawerItemViewModel oldViewModel)
		{
			if (oldViewModel != null)
			{
				oldViewModel.PropertyChanged -= OnPropertyChanged;
			}
			Photo.Source = null;
			((UIElement)Photo).RenderTransform = null;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("IsSelected"))
			{
				SetSelectedOverlay();
			}
		}

		private void SetSelectedOverlay()
		{
			if (ViewModel != null)
			{
				if (ViewModel.IsSelected)
				{
					FadeIn.Begin();
					SendPhotoButton.Reset();
				}
				else
				{
					FadeOut.Begin();
				}
			}
		}

		private void SendPhotoButton_OnTap(object sender, GestureEventArgs e)
		{
			e.Handled = true;
			if (SendClicked != null)
			{
				SendClicked(this, (EventArgs)(object)e);
			}
		}

		private void EditPhotoButton_OnTap(object sender, GestureEventArgs e)
		{
			e.Handled = true;
			if (EditClicked != null)
			{
				EditClicked(this, (EventArgs)(object)e);
			}
		}

		private void Overlay_OnTap(object sender, GestureEventArgs e)
		{
			e.Handled = true;
			if (OverlayTapped != null)
			{
				OverlayTapped(this, (EventArgs)(object)e);
			}
		}

	}
}

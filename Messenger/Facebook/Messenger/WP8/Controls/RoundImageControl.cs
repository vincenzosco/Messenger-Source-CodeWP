using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Extensions;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class RoundImageControl : BaseControl, ITiltable
	{
		public static readonly DependencyProperty ImageUriProperty = DependencyProperty.Register("ImageUri", typeof(Uri), typeof(RoundImageControl), new PropertyMetadata((object)null, (PropertyChangedCallback)OnImageUriChanged));

		public static readonly DependencyProperty PlaceholderUriProperty = DependencyProperty.Register("PlaceholderUri", typeof(Uri), typeof(RoundImageControl), new PropertyMetadata((object)null, (PropertyChangedCallback)OnPlaceHolderChanged));



		public Uri ImageUri
		{
			get
			{
				return (Uri)base.GetValue(ImageUriProperty);
			}
			set
			{
				base.SetValue(ImageUriProperty, (object)value);
			}
		}

		public Uri PlaceholderUri
		{
			get
			{
				return (Uri)base.GetValue(PlaceholderUriProperty);
			}
			set
			{
				base.SetValue(PlaceholderUriProperty, (object)value);
			}
		}

		private static void OnImageUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RoundImageControl roundImageControl = d as RoundImageControl;
			if (roundImageControl == null)
			{
				return;
			}
			if (e.NewValue == null)
			{
				ImageSource source = roundImageControl.RoundImage.Source;
				BitmapImage val = (BitmapImage)(object)((source is BitmapImage) ? source : null);
				if (val != null)
				{
					val.UriSource = null;
				}
				roundImageControl.RoundImage.Source = null;
			}
			((DependencyObject)roundImageControl.RoundImage).SetValue(ImageCacheExtensions.ImageUriProperty, e.NewValue);
		}

		private static void OnPlaceHolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RoundImageControl roundImageControl = d as RoundImageControl;
			if (roundImageControl != null)
			{
				((DependencyObject)roundImageControl.RoundImage).SetValue(ImageCacheExtensions.PlaceholderUriProperty, e.NewValue);
			}
		}

		public RoundImageControl()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			InitializeComponent();
			base.SizeChanged += OnSizeChanged;
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			if (!double.IsNaN(base.Width))
			{
				((DependencyObject)RoundImage).SetValue(RoundContentExtension.RadiusProperty, (object)(base.Width / 2.0));
			}
		}


		static RoundImageControl()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected Obj, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected Obj, but got Unknown
		}
	}
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class ImageButton : ButtonBase
	{
		public static DependencyProperty BackgroundSourceProperty = DependencyProperty.Register("BackgroundSource", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static DependencyProperty SelectedBackgroundSourceProperty = DependencyProperty.Register("SelectedBackgroundSource", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static DependencyProperty SelectedSourceProperty = DependencyProperty.Register("SelectedSource", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ImageButton), new PropertyMetadata((object)false, (PropertyChangedCallback)IsSelectedChangedCallback));

		private bool _pressed;





		public Uri BackgroundSource
		{
			get
			{
				return (Uri)base.GetValue(BackgroundSourceProperty);
			}
			set
			{
				base.SetValue(BackgroundSourceProperty, (object)value);
			}
		}

		public Uri SelectedBackgroundSource
		{
			get
			{
				return (Uri)base.GetValue(BackgroundSourceProperty);
			}
			set
			{
				base.SetValue(BackgroundSourceProperty, (object)value);
			}
		}

		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(SourceProperty);
			}
			set
			{
				base.SetValue(SourceProperty, (object)value);
			}
		}

		public Uri SelectedSource
		{
			get
			{
				return (Uri)base.GetValue(SelectedSourceProperty);
			}
			set
			{
				base.SetValue(SelectedSourceProperty, (object)value);
			}
		}

		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(IsSelectedProperty);
			}
			set
			{
				base.SetValue(IsSelectedProperty, (object)value);
			}
		}

		private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			(dependencyObject as ImageButton)?.Reset();
		}

		private static void IsSelectedChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			ImageButton imageButton = dependencyObject as ImageButton;
			if (imageButton != null)
			{
				bool isSelected = dependencyPropertyChangedEventArgs.NewValue is bool && (bool)dependencyPropertyChangedEventArgs.NewValue;
				imageButton.UpdateImage(isSelected);
			}
		}

		public ImageButton()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			Reset();
		}

		public void Reset()
		{
			UpdateImage(IsSelected);
		}

		protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
		{
			base.OnManipulationStarted(e);
			OnImagePressed(true);
		}

		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
		{
			base.OnManipulationCompleted(e);
			OnImagePressed(false);
		}

		protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
		{
			OnImagePressed(base.IsPressed);
		}

		private void OnImagePressed(bool pressed)
		{
			if (pressed != _pressed)
			{
				_pressed = pressed || IsSelected;
				UpdateImage(_pressed);
			}
		}

		private void UpdateImage(bool isSelected)
		{
			if (isSelected)
			{
				SetSelectedImage();
			}
			else
			{
				SetDefaultImage();
			}
		}

		private void SetSelectedImage()
		{
			SafeSetImageSource(IconImage, SelectedSource ?? Source);
			SafeSetImageSource(BackgroundImage, SelectedBackgroundSource ?? BackgroundSource);
		}

		private void SetDefaultImage()
		{
			SafeSetImageSource(IconImage, Source);
			SafeSetImageSource(BackgroundImage, BackgroundSource);
		}

		private void SafeSetImageSource(Image image, Uri uriSource)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			image.Source = (ImageSource)((uriSource == null) ? ((BitmapImage)null) : new BitmapImage(uriSource));
		}


		static ImageButton()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected Obj, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected Obj, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected Obj, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected Obj, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected Obj, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected Obj, but got Unknown
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected Obj, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Expected Obj, but got Unknown
		}
	}
}

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
	public class ImageButton : ButtonBase
	{
		public static DependencyProperty BackgroundSourceProperty = DependencyProperty.Register("BackgroundSource", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static DependencyProperty SelectedBackgroundSourceProperty = DependencyProperty.Register("SelectedBackgroundSource", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static DependencyProperty SelectedSourceProperty = DependencyProperty.Register("SelectedSource", typeof(Uri), typeof(ImageButton), new PropertyMetadata((object)null, (PropertyChangedCallback)PropertyChangedCallback));

		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ImageButton), new PropertyMetadata((object)false, (PropertyChangedCallback)IsSelectedChangedCallback));

		private bool _pressed;

		internal Grid LayoutRoot;

		internal Image BackgroundImage;

		internal Image IconImage;

		private bool _contentLoaded;

		public Uri BackgroundSource
		{
			get
			{
				return (Uri)((DependencyObject)this).GetValue(BackgroundSourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(BackgroundSourceProperty, (object)value);
			}
		}

		public Uri SelectedBackgroundSource
		{
			get
			{
				return (Uri)((DependencyObject)this).GetValue(BackgroundSourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(BackgroundSourceProperty, (object)value);
			}
		}

		public Uri Source
		{
			get
			{
				return (Uri)((DependencyObject)this).GetValue(SourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(SourceProperty, (object)value);
			}
		}

		public Uri SelectedSource
		{
			get
			{
				return (Uri)((DependencyObject)this).GetValue(SelectedSourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(SelectedSourceProperty, (object)value);
			}
		}

		public bool IsSelected
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsSelectedProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsSelectedProperty, (object)value);
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
			((FrameworkElement)this).Loaded += OnLoaded;
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
			((Control)this).OnManipulationStarted(e);
			OnImagePressed(true);
		}

		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
		{
			((Control)this).OnManipulationCompleted(e);
			OnImagePressed(false);
		}

		protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
		{
			OnImagePressed(((ButtonBase)this).IsPressed);
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

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ImageButton.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				BackgroundImage = (Image)((FrameworkElement)this).FindName("BackgroundImage");
				IconImage = (Image)((FrameworkElement)this).FindName("IconImage");
			}
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

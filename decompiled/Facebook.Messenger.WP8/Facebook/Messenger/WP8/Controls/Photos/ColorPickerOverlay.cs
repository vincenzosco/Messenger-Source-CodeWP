using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Facebook.Messenger.WP8.Controls.Photos;

public class ColorPickerOverlay : UserControl
{
	private const double BrushThicknessDefault = 10.0;

	private const double BrushThicknessMin = 2.0;

	private const double BrushThicknessMax = 30.0;

	private static readonly Color BrushColorDefault = (Color)Application.Current.Resources[(object)"MessengerBlue"];

	public static readonly DependencyProperty BrushThicknessProperty = DependencyProperty.Register("BrushThickness", typeof(double), typeof(ColorPickerOverlay), new PropertyMetadata((object)10.0));

	public static readonly DependencyProperty BrushColorProperty = DependencyProperty.Register("BrushColor", typeof(Color), typeof(ColorPickerOverlay), new PropertyMetadata((object)BrushColorDefault));

	private WriteableBitmap _colorPickerBitmap;

	internal Grid ColorPickerGrid;

	internal Rectangle ColorPicker;

	internal Canvas IndicatorCanvas;

	internal Grid ColorIndicator;

	internal Image ColorScrubberImage;

	internal Ellipse ChosenColor;

	internal TranslateTransform ColorIndicatorTranslateTransform;

	private bool _contentLoaded;

	public double BrushThickness
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(BrushThicknessProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(BrushThicknessProperty, (object)value);
		}
	}

	public Color BrushColor
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (Color)((DependencyObject)this).GetValue(BrushColorProperty);
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DependencyObject)this).SetValue(BrushColorProperty, (object)value);
		}
	}

	public event EventHandler<BrushUpdateArgs> BrushUpdated;

	public ColorPickerOverlay()
	{
		InitializeComponent();
		((FrameworkElement)this).DataContext = this;
	}

	public void Reset()
	{
		_colorPickerBitmap = null;
	}

	private void ColorPicker_OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((UIElement)ColorIndicator).Visibility = (Visibility)0;
		UpdateBrushFromTouchPosition(e.ManipulationOrigin);
		UpdateColorIndicatorPosition(e.ManipulationOrigin);
	}

	private void ColorPicker_OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Point manipulationOrigin = e.ManipulationOrigin;
		double x = manipulationOrigin.X;
		Point translation = e.DeltaManipulation.Translation;
		double num = x + translation.X;
		double y = manipulationOrigin.Y;
		Point translation2 = e.DeltaManipulation.Translation;
		Point position = new Point(num, y + translation2.Y);
		UpdateBrushFromTouchPosition(position);
		UpdateColorIndicatorPosition(e.ManipulationOrigin);
	}

	private void ColorPicker_OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (BrushUpdated != null)
		{
			BrushUpdated(this, new BrushUpdateArgs(BrushThickness, BrushColor));
		}
		((UIElement)ColorIndicator).Visibility = (Visibility)1;
	}

	private void UpdateBrushFromTouchPosition(Point position)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected Obj, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected Obj, but got Unknown
		if (_colorPickerBitmap == null)
		{
			_colorPickerBitmap = new WriteableBitmap((UIElement)(object)ColorPicker, (Transform)null);
		}
		if (!(position.Y <= 0.0) && !(position.Y > (double)((BitmapSource)_colorPickerBitmap).PixelHeight))
		{
			int num = (int)(Math.Max(0.0, position.Y - 1.0) * (double)((BitmapSource)_colorPickerBitmap).PixelWidth + (double)(((BitmapSource)_colorPickerBitmap).PixelWidth / 2));
			int value = _colorPickerBitmap.Pixels[num];
			byte[] bytes = BitConverter.GetBytes(value);
			BrushColor = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
			((Shape)ChosenColor).Fill = (Brush)new SolidColorBrush(BrushColor);
		}
	}

	private void UpdateColorIndicatorPosition(Point position)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Point absolutePosition = TransformToCoordinateSpace((UIElement)(object)ColorPickerGrid, (UIElement)(object)IndicatorCanvas, position);
		ColorIndicatorTranslateTransform.X = Math.Max(absolutePosition.X - 100.0, 0.0);
		ColorIndicatorTranslateTransform.Y = absolutePosition.Y - ((FrameworkElement)ColorScrubberImage).Height / 2.0;
		BrushThickness = GetBrushThicknessFromTouchPosition(absolutePosition);
	}

	private static double GetBrushThicknessFromTouchPosition(Point absolutePosition)
	{
		return 30.0 - absolutePosition.X / Application.Current.Host.Content.ActualWidth * 28.0 + 1.0;
	}

	private static Point TransformToCoordinateSpace(UIElement source, UIElement destination, Point p)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		GeneralTransform val = source.TransformToVisual(destination);
		return val.Transform(p);
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Photos/ColorPickerOverlay.xaml", UriKind.Relative));
			ColorPickerGrid = (Grid)((FrameworkElement)this).FindName("ColorPickerGrid");
			ColorPicker = (Rectangle)((FrameworkElement)this).FindName("ColorPicker");
			IndicatorCanvas = (Canvas)((FrameworkElement)this).FindName("IndicatorCanvas");
			ColorIndicator = (Grid)((FrameworkElement)this).FindName("ColorIndicator");
			ColorScrubberImage = (Image)((FrameworkElement)this).FindName("ColorScrubberImage");
			ChosenColor = (Ellipse)((FrameworkElement)this).FindName("ChosenColor");
			ColorIndicatorTranslateTransform = (TranslateTransform)((FrameworkElement)this).FindName("ColorIndicatorTranslateTransform");
		}
	}

	static ColorPickerOverlay()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected Obj, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected Obj, but got Unknown
	}
}

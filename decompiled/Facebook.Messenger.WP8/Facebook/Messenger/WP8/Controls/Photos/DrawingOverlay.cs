using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Facebook.Messenger.WP8.Controls.Photos;

public class DrawingOverlay : UserControl
{
	private enum BezierPointType
	{
		ControlPoint1,
		ControlPoint2,
		EndPoint
	}

	private const double DrawPointDelta = 5.0;

	private UIElement _drawingSurface;

	private BezierPointType _pointType;

	private PathFigure _path;

	private PolyBezierSegment _currentSegment;

	private Point _lastDrawnPoint;

	public static readonly DependencyProperty BrushThicknessProperty = DependencyProperty.Register("BrushThickness", typeof(double), typeof(DrawingOverlay), new PropertyMetadata((object)0.0));

	public static readonly DependencyProperty BrushColorProperty = DependencyProperty.Register("BrushColor", typeof(Color), typeof(DrawingOverlay), new PropertyMetadata((object)default(Color)));

	internal Canvas DrawCanvas;

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

	public event EventHandler DrawActionStarted;

	public event EventHandler DrawActionComplete;

	public DrawingOverlay()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		InitializeComponent();
		((Control)this).IsEnabledChanged += OnIsEnabledChanged;
	}

	public void SetDrawingSurface(UIElement surface)
	{
		if (_drawingSurface != null)
		{
			DisableDrawing();
		}
		_drawingSurface = surface;
		if (((Control)this).IsEnabled)
		{
			EnableDrawing();
		}
	}

	private void EnableDrawing()
	{
		if (_drawingSurface != null)
		{
			_drawingSurface.ManipulationStarted += DrawingOnManupulationStarted;
			_drawingSurface.ManipulationDelta += DrawingOnManipulationDelta;
			_drawingSurface.ManipulationCompleted += DrawingOnManipulationCompleted;
		}
	}

	private void DisableDrawing()
	{
		if (_drawingSurface != null)
		{
			_drawingSurface.ManipulationStarted -= DrawingOnManupulationStarted;
			_drawingSurface.ManipulationDelta -= DrawingOnManipulationDelta;
			_drawingSurface.ManipulationCompleted -= DrawingOnManipulationCompleted;
		}
	}

	private void DrawingOnManupulationStarted(object sender, ManipulationStartedEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected Obj, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected Obj, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected Obj, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected Obj, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected Obj, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		if (DrawActionStarted != null)
		{
			DrawActionStarted(this, EventArgs.Empty);
		}
		Path val = new Path();
		((Shape)val).Stroke = (Brush)new SolidColorBrush(BrushColor);
		((Shape)val).StrokeThickness = BrushThickness;
		((Shape)val).StrokeStartLineCap = (PenLineCap)2;
		((Shape)val).StrokeEndLineCap = (PenLineCap)2;
		((Shape)val).StrokeLineJoin = (PenLineJoin)2;
		((Shape)val).StrokeDashCap = (PenLineCap)2;
		((UIElement)val).IsHitTestVisible = false;
		Path val2 = val;
		PathGeometry val3 = new PathGeometry();
		val3.Figures = new PathFigureCollection();
		PathGeometry val4 = val3;
		PathFigure val5 = new PathFigure();
		val5.StartPoint = e.ManipulationOrigin;
		_path = val5;
		_lastDrawnPoint = e.ManipulationOrigin;
		((PresentationFrameworkCollection<PathFigure>)(object)val4.Figures).Add(_path);
		val2.Data = (Geometry)(object)val4;
		_pointType = BezierPointType.ControlPoint1;
		_currentSegment = new PolyBezierSegment();
		((PresentationFrameworkCollection<PathSegment>)(object)_path.Segments).Add((PathSegment)(object)_currentSegment);
		AddPathSegment(e.ManipulationOrigin);
		((PresentationFrameworkCollection<UIElement>)(object)((Panel)DrawCanvas).Children).Add((UIElement)(object)val2);
	}

	private void DrawPathUsingManipulation(ManipulationDeltaEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		Point manipulationOrigin = e.ManipulationOrigin;
		double x = manipulationOrigin.X;
		Point translation = e.DeltaManipulation.Translation;
		double num = x + translation.X;
		double y = manipulationOrigin.Y;
		Point translation2 = e.DeltaManipulation.Translation;
		Point val = new Point(num, y + translation2.Y);
		if (Math.Abs(val.X - _lastDrawnPoint.X) > 5.0 || Math.Abs(val.Y - _lastDrawnPoint.Y) > 5.0)
		{
			AddPathSegment(val);
			_lastDrawnPoint = val;
		}
	}

	private void AddPathSegment(Point nextPoint)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		switch (_pointType)
		{
		case BezierPointType.ControlPoint1:
			((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points).Add(nextPoint);
			((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points).Add(nextPoint);
			((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points).Add(nextPoint);
			_pointType = BezierPointType.ControlPoint2;
			break;
		case BezierPointType.ControlPoint2:
			((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points)[((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points).Count - 2] = nextPoint;
			((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points)[((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points).Count - 1] = nextPoint;
			_pointType = BezierPointType.EndPoint;
			break;
		default:
			((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points)[((PresentationFrameworkCollection<Point>)(object)_currentSegment.Points).Count - 1] = nextPoint;
			_pointType = BezierPointType.ControlPoint1;
			break;
		}
	}

	private void DrawingOnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
	{
		DrawPathUsingManipulation(e);
	}

	private void DrawingOnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
	{
		_path = null;
		if (DrawActionComplete != null)
		{
			DrawActionComplete(this, EventArgs.Empty);
		}
	}

	private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs d)
	{
		if ((bool)d.NewValue)
		{
			EnableDrawing();
		}
		else
		{
			DisableDrawing();
		}
	}

	public void Reset()
	{
		((PresentationFrameworkCollection<UIElement>)(object)((Panel)DrawCanvas).Children).Clear();
	}

	public bool HasDrawings()
	{
		return ((IEnumerable<UIElement>)((Panel)DrawCanvas).Children).Any();
	}

	public void RemoveLastDrawing()
	{
		((PresentationFrameworkCollection<UIElement>)(object)((Panel)DrawCanvas).Children).RemoveAt(((PresentationFrameworkCollection<UIElement>)(object)((Panel)DrawCanvas).Children).Count - 1);
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Photos/DrawingOverlay.xaml", UriKind.Relative));
			DrawCanvas = (Canvas)((FrameworkElement)this).FindName("DrawCanvas");
		}
	}

	static DrawingOverlay()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected Obj, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected Obj, but got Unknown
	}
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
	internal class VirtualizedItem
	{
		private const double maxTextureSideLength = 2048.0;

		private FrameworkElement _zoomableContent;

		private ScaleTransform _contentTransform;

		private Point _lastOrigin;

		private double _currentZoomRatio = 1.0;

		private double _zoomRatioDuringPinch;

		private FrameworkElement _rootFrameworkElement;

		private DataTemplate _dataTemplate;

		private Size _size;

		private ViewportControl _viewport;

		private Canvas _canvas;

		private Size _actualContentSize = new Size(0.0, 0.0);

		private bool _pinchInProgress;

		private bool _centerAtNextOpportunity;

		private Point _pinchMidpointInControlCoordinates;

		private Point _pinchMidpointInPercentOfContent;

		public int? RepresentingItemIndex { get; set; }

		public DataTemplate DataTemplate
		{
			get
			{
				return _dataTemplate;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Expected Obj, but got Unknown
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Expected Obj, but got Unknown
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Expected Obj, but got Unknown
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Expected Obj, but got Unknown
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Expected Obj, but got Unknown
				_dataTemplate = value;
				_rootFrameworkElement = (FrameworkElement)_dataTemplate.LoadContent();
				((UIElement)_rootFrameworkElement).Visibility = (Visibility)1;
				_rootFrameworkElement.Height = _size.Height;
				_rootFrameworkElement.Width = _size.Width;
				_zoomableContent = (FrameworkElement)_rootFrameworkElement.FindName("ZoomableContent");
				_contentTransform = (ScaleTransform)_rootFrameworkElement.FindName("ZoomableContentTransform");
				_viewport = (ViewportControl)_rootFrameworkElement.FindName("Viewport");
				_canvas = (Canvas)_rootFrameworkElement.FindName("Canvas");
				((UIElement)_zoomableContent).ManipulationStarted += OnManipulationStarted;
				((UIElement)_zoomableContent).ManipulationDelta += OnManipulationDelta;
				((UIElement)_zoomableContent).ManipulationCompleted += OnManipulationCompleted;
				_zoomableContent.LayoutUpdated += OnZoomableContentLayoutUpdated;
			}
		}

		public FrameworkElement RootFrameworkElement => _rootFrameworkElement;

		public object DataContext
		{
			get
			{
				return _rootFrameworkElement.DataContext;
			}
			set
			{
				_rootFrameworkElement.DataContext = value;
				ZoomAllTheWayOut();
			}
		}

		public Size Size
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _size;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				_size = value;
				_rootFrameworkElement.Height = _size.Height;
				_rootFrameworkElement.Width = _size.Width;
				ZoomAllTheWayOut();
			}
		}

		public bool IsZoomedIn
		{
			get
			{
				if (!(_currentZoomRatio > 1.0))
				{
					return _pinchInProgress;
				}
				return true;
			}
		}

		public event Action<int?> ItemZoomed;

		public event Action<int?> ItemUnzoomed;

		public VirtualizedItem(Size size)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			_size = size;
		}

		public void DoubleTapped()
		{
			if (IsZoomedIn)
			{
				ZoomAllTheWayOut();
			}
			else
			{
				ZoomInToDefaultLevel();
			}
		}

		public void ZoomAllTheWayOut()
		{
			ItemUnzoomed(RepresentingItemIndex);
			_currentZoomRatio = 1.0;
			_centerAtNextOpportunity = true;
			ResizeContent();
		}

		private void ZoomInToDefaultLevel()
		{
			ItemZoomed(RepresentingItemIndex);
			_currentZoomRatio = 2.0;
			_centerAtNextOpportunity = true;
			ResizeContent();
		}

		private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			_pinchInProgress = false;
		}

		private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			if (e.PinchManipulation != null)
			{
				e.Handled = true;
				if (!_pinchInProgress)
				{
					_pinchInProgress = true;
					ItemZoomed(RepresentingItemIndex);
					Point center = e.PinchManipulation.Original.Center;
					_pinchMidpointInPercentOfContent = new Point(center.X / _actualContentSize.Width, center.Y / _actualContentSize.Height);
					GeneralTransform val = ((UIElement)_zoomableContent).TransformToVisual((UIElement)(object)_rootFrameworkElement);
					_pinchMidpointInControlCoordinates = val.Transform(center);
					_lastOrigin = new Point(-1.0, -1.0);
				}
				_zoomRatioDuringPinch = ClampZoomRatioToMinMax(_currentZoomRatio * e.PinchManipulation.CumulativeScale);
				ScaleContentDuringPinch();
			}
			else if (_pinchInProgress)
			{
				FinishPinch();
			}
		}

		private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			if (_pinchInProgress)
			{
				e.Handled = true;
				FinishPinch();
			}
		}

		private void FinishPinch()
		{
			_pinchInProgress = false;
			_currentZoomRatio = _zoomRatioDuringPinch;
			if (_currentZoomRatio == 1.0)
			{
				ItemUnzoomed(RepresentingItemIndex);
			}
			ResizeContent();
		}

		private void ResizeContent()
		{
			double num = Math.Round(_size.Width * _currentZoomRatio);
			double num2 = Math.Round(_size.Height * _currentZoomRatio);
			((FrameworkElement)_canvas).Width = num;
			((FrameworkElement)_canvas).Height = num2;
			_zoomableContent.MaxWidth = num;
			_zoomableContent.MaxHeight = num2;
			_contentTransform.ScaleX = 1.0;
			_contentTransform.ScaleY = 1.0;
		}

		private void ScaleContentDuringPinch()
		{
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			double num = Math.Round(_size.Width * _zoomRatioDuringPinch);
			Math.Round(_size.Height * _zoomRatioDuringPinch);
			double num2 = num / _zoomableContent.MaxWidth;
			double num3 = _actualContentSize.Width * num2;
			double num4 = _actualContentSize.Height * num2;
			_contentTransform.ScaleX = num2;
			_contentTransform.ScaleY = num2;
			_viewport.Bounds = new Rect(0.0, 0.0, num3, num4);
			Point val = new Point(num3 * _pinchMidpointInPercentOfContent.X, num4 * _pinchMidpointInPercentOfContent.Y);
			Point val2 = new Point((double)(int)(val.X - _pinchMidpointInControlCoordinates.X), (double)(int)(val.Y - _pinchMidpointInControlCoordinates.Y));
			if (val2 != _lastOrigin)
			{
				_viewport.SetViewportOrigin(val2);
				_lastOrigin = val2;
			}
		}

		private void OnZoomableContentLayoutUpdated(object sender, EventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			Size desiredSize = ((UIElement)_zoomableContent).DesiredSize;
			if (desiredSize.Height == _actualContentSize.Height)
			{
				Size desiredSize2 = ((UIElement)_zoomableContent).DesiredSize;
				if (desiredSize2.Width == _actualContentSize.Width)
				{
					return;
				}
			}
			_actualContentSize = ((UIElement)_zoomableContent).DesiredSize;
			_viewport.Bounds = new Rect(0.0, 0.0, _actualContentSize.Width, _actualContentSize.Height);
			if (_centerAtNextOpportunity)
			{
				_viewport.SetViewportOrigin(new Point(Math.Round((_actualContentSize.Width - _size.Width) / 2.0), Math.Round((_actualContentSize.Height - _size.Height) / 2.0)));
				_centerAtNextOpportunity = false;
			}
		}

		private double ClampZoomRatioToMinMax(double ratio)
		{
			double val = Math.Min(2048.0 / _size.Width, 2048.0 / _size.Height);
			return Math.Min(val, Math.Max(1.0, ratio));
		}
	}
}

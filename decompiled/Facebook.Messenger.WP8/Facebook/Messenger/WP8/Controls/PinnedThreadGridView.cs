using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls;

public class PinnedThreadGridView : UserControl
{
	private enum ScrollDirection
	{
		None,
		Up,
		Down
	}

	public static readonly DependencyProperty IsRearrangingProperty = DependencyProperty.Register("IsRearranging", typeof(bool), typeof(PinnedThreadGridView), new PropertyMetadata((object)false));

	public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(PinnedThreadGridView), new PropertyMetadata((PropertyChangedCallback)VerticalOffsetPropertyChanged));

	private GroupTileControl _currentGrid;

	private GroupTileControl _gridBeingUnpinned;

	private GroupTileControl _currentTarget;

	private int? _dragHoverIndex;

	private GroupTileControl _dragItem;

	private Point _centerPoint;

	private DispatcherTimer _holdTimer;

	private Dictionary<int, int> _shiftedIndices = new Dictionary<int, int>();

	private Dictionary<object, GroupTileControl> _objectToVisibleElement = new Dictionary<object, GroupTileControl>();

	private ScrollDirection _scrollDirection;

	private bool _playedFirstSlide;

	private bool _isRemovingPin;

	private INotifyCollectionChanged _currentCollection;

	private bool _scrollEnabled = true;

	public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(PinnedThreadGridView), new PropertyMetadata((object)null, (PropertyChangedCallback)OnItemsSourceChanged));

	public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(PinnedThreadGridView), new PropertyMetadata((PropertyChangedCallback)null));

	public static readonly DependencyProperty GridCellSizeProperty = DependencyProperty.Register("GridCellSize", typeof(Size), typeof(PinnedThreadGridView), new PropertyMetadata((PropertyChangedCallback)null));

	internal Grid InterceptionGrid;

	internal ScrollViewer ScrollViewer;

	internal Canvas Canvas;

	internal Canvas DragCanvas;

	internal Image DragIndicator;

	private bool _contentLoaded;

	public bool IsRearranging
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsRearrangingProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsRearrangingProperty, (object)value);
		}
	}

	public double VerticalOffset
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(VerticalOffsetProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(VerticalOffsetProperty, (object)value);
		}
	}

	private bool ScrollViewerIsEnabled
	{
		get
		{
			return _scrollEnabled;
		}
		set
		{
			_scrollEnabled = value;
			((UIElement)ScrollViewer).IsHitTestVisible = _scrollEnabled;
		}
	}

	public IList<ThreadViewModel> ItemsSource
	{
		get
		{
			return (IList<ThreadViewModel>)((DependencyObject)this).GetValue(ItemsSourceProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ItemsSourceProperty, (object)value);
		}
	}

	public DataTemplate ItemTemplate
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			return (DataTemplate)((DependencyObject)this).GetValue(ItemTemplateProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ItemTemplateProperty, (object)value);
		}
	}

	public Size GridCellSize
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (Size)((DependencyObject)this).GetValue(GridCellSizeProperty);
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DependencyObject)this).SetValue(GridCellSizeProperty, (object)value);
		}
	}

	public event EventHandler<ThreadViewModel> PinRemoved;

	private static void VerticalOffsetPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
	{
		PinnedThreadGridView pinnedThreadGridView = sender as PinnedThreadGridView;
		pinnedThreadGridView.ScrollViewer.ScrollToVerticalOffset((double)args.NewValue);
	}

	private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		((PinnedThreadGridView)(object)obj).OnItemsSourceChanged();
	}

	public PinnedThreadGridView()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected Obj, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected Obj, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected Obj, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected Obj, but got Unknown
		InitializeComponent();
		_holdTimer = new DispatcherTimer();
		_holdTimer.Interval = TimeSpan.FromMilliseconds(250.0);
		_holdTimer.Tick += HoldTimerOnTick;
		((Panel)InterceptionGrid).Background = (Brush)new SolidColorBrush(Colors.Transparent);
		((FrameworkElement)InterceptionGrid).UseOptimizedManipulationRouting = false;
		((UIElement)InterceptionGrid).IsHitTestVisible = true;
		((UIElement)InterceptionGrid).Hold += RearrangeCanvas_OnHold;
		((UIElement)InterceptionGrid).Tap += RearrangeCanvas_OnTap;
		((UIElement)InterceptionGrid).ManipulationDelta += OnGridManipulationDelta;
		((UIElement)InterceptionGrid).AddHandler(UIElement.ManipulationStartedEvent, (Delegate)new EventHandler<ManipulationStartedEventArgs>(OnManipulationStarted), true);
		((UIElement)InterceptionGrid).AddHandler(UIElement.ManipulationCompletedEvent, (Delegate)new EventHandler<ManipulationCompletedEventArgs>(OnManipulationCompleted), true);
		((UIElement)InterceptionGrid).AddHandler(UIElement.ManipulationDeltaEvent, (Delegate)new EventHandler<ManipulationDeltaEventArgs>(OnListManipulationDelta), true);
		((FrameworkElement)Canvas).SizeChanged += OnCanvasOrScrollViewerSizeChanged;
		((FrameworkElement)ScrollViewer).SizeChanged += OnCanvasOrScrollViewerSizeChanged;
	}

	private void OnCanvasOrScrollViewerSizeChanged(object sender, SizeChangedEventArgs args)
	{
		if (((FrameworkElement)Canvas).ActualHeight < ((FrameworkElement)ScrollViewer).ActualHeight)
		{
			ScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)0;
		}
		else
		{
			ScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)1;
		}
	}

	private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		OnItemsSourceChanged();
	}

	public void OnItemsSourceChanged()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		if (_currentCollection != null)
		{
			_currentCollection.CollectionChanged -= OnCollectionChanged;
		}
		_currentCollection = ItemsSource as INotifyCollectionChanged;
		if (_currentCollection != null)
		{
			_currentCollection.CollectionChanged += OnCollectionChanged;
		}
		_objectToVisibleElement.Clear();
		((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Clear();
		((UIElement)this).InvalidateMeasure();
		double top = 0.0;
		double left = 0.0;
		foreach (ThreadViewModel item in ItemsSource)
		{
			Add(item, ref top, ref left);
		}
		Canvas canvas = Canvas;
		Size gridCellSize = GridCellSize;
		double num = gridCellSize.Height * Math.Ceiling((double)((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Count / 2.0);
		Thickness margin = ((FrameworkElement)Canvas).Margin;
		double num2 = num + margin.Bottom;
		Thickness margin2 = ((FrameworkElement)Canvas).Margin;
		((FrameworkElement)canvas).Height = num2 + margin2.Top;
	}

	private FrameworkElement Add(ThreadViewModel item, ref double top, ref double left)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		GroupTileControl groupTileControl = ItemTemplate.LoadContent() as GroupTileControl;
		((FrameworkElement)groupTileControl).DataContext = item;
		Canvas.SetTop((UIElement)(object)groupTileControl, top);
		Canvas.SetLeft((UIElement)(object)groupTileControl, left);
		Size gridCellSize = GridCellSize;
		((FrameworkElement)groupTileControl).Width = gridCellSize.Width;
		Size gridCellSize2 = GridCellSize;
		((FrameworkElement)groupTileControl).Height = gridCellSize2.Height;
		AdvanceTilePosition(ref top, ref left);
		_objectToVisibleElement[item] = groupTileControl;
		((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Add((UIElement)(object)groupTileControl);
		return (FrameworkElement)(object)groupTileControl;
	}

	private void AdvanceTilePosition(ref double top, ref double left)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (left == 0.0)
		{
			double num = left;
			Size gridCellSize = GridCellSize;
			left = num + gridCellSize.Width;
		}
		else
		{
			double num2 = top;
			Size gridCellSize2 = GridCellSize;
			top = num2 + gridCellSize2.Height;
			left = 0.0;
		}
	}

	private void HoldTimerOnTick(object sender, EventArgs args)
	{
		_holdTimer.Stop();
		if (_dragItem != null && _currentGrid != _dragItem && _dragItem != null)
		{
			SwapZoomTo(_dragItem);
			ScrollViewerIsEnabled = false;
		}
	}

	private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		if (_isRemovingPin)
		{
			return;
		}
		_holdTimer.Stop();
		GeneralTransform val = e.ManipulationContainer.TransformToVisual(Application.Current.RootVisual);
		Point point = val.Transform(e.ManipulationOrigin);
		point = GetHostCoordinates(point);
		object content = ((ContentControl)ScrollViewer).Content;
		UIElement val2 = (UIElement)((content is UIElement) ? content : null);
		List<UIElement> source = VisualTreeHelper.FindElementsInHostCoordinates(point, val2).ToList();
		GroupTileControl groupTileControl = source.OfType<GroupTileControl>().FirstOrDefault();
		if (groupTileControl != null && groupTileControl == _currentGrid)
		{
			ScrollViewerIsEnabled = false;
		}
		else
		{
			ScrollViewerIsEnabled = true;
		}
		if (_dragItem == null && groupTileControl != null)
		{
			_dragItem = groupTileControl;
			GeneralTransform val3 = ((UIElement)_dragItem).TransformToVisual((UIElement)(object)DragCanvas);
			Point val4 = val3.Transform(new Point(0.0, 0.0));
			Canvas.SetLeft((UIElement)(object)DragIndicator, val4.X);
			Canvas.SetTop((UIElement)(object)DragIndicator, val4.Y);
			Transform renderTransform = ((UIElement)DragIndicator).RenderTransform;
			TranslateTransform val5 = (TranslateTransform)(object)((renderTransform is TranslateTransform) ? renderTransform : null);
			val5.X = 0.0;
			val5.Y = 0.0;
			Image dragIndicator = DragIndicator;
			Size renderSize = ((UIElement)_dragItem).RenderSize;
			((FrameworkElement)dragIndicator).Width = renderSize.Width;
			Image dragIndicator2 = DragIndicator;
			Size renderSize2 = ((UIElement)_dragItem).RenderSize;
			((FrameworkElement)dragIndicator2).Height = renderSize2.Height;
			if (_currentGrid != null && _currentGrid != _dragItem)
			{
				_holdTimer.Start();
			}
		}
	}

	private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
	{
		if (_dragItem == null || _isRemovingPin)
		{
			ResetState();
			return;
		}
		_holdTimer.Stop();
		Transform renderTransform = ((UIElement)DragIndicator).RenderTransform;
		TranslateTransform val = (TranslateTransform)(object)((renderTransform is TranslateTransform) ? renderTransform : null);
		double top = Canvas.GetTop((UIElement)(object)DragIndicator);
		double left = Canvas.GetLeft((UIElement)(object)DragIndicator);
		double num = GetFinalTop((FrameworkElement)(object)_dragItem) - val.Y - ScrollViewer.VerticalOffset;
		double num2 = GetFinalLeft((FrameworkElement)(object)_dragItem) - val.X;
		if (top == num && left == num2)
		{
			CompleteDrag();
		}
		else
		{
			AnimateShift((FrameworkElement)(object)DragIndicator, top, num, left, num2, AnimateDragIndicatorFinished);
		}
	}

	private void AnimateDragIndicatorFinished(object sender, EventArgs eventArgs)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((UIElement)DragIndicator).Visibility == 0)
		{
			((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
			{
				vm.PlayCardLandedSound();
			});
		}
		CompleteDrag();
	}

	private void CompleteDrag()
	{
		if (_dragHoverIndex.HasValue)
		{
			ThreadViewModel item = ((FrameworkElement)_dragItem).DataContext as ThreadViewModel;
			ObservableCollection<ThreadViewModel> observableCollection = ItemsSource as ObservableCollection<ThreadViewModel>;
			int oldIndex = observableCollection.IndexOf(item);
			if (_currentCollection != null)
			{
				_currentCollection.CollectionChanged -= OnCollectionChanged;
			}
			observableCollection.Move(oldIndex, _dragHoverIndex.Value);
			if (_currentCollection != null)
			{
				_currentCollection.CollectionChanged += OnCollectionChanged;
			}
		}
		ResetState();
	}

	private void ResetState()
	{
		((UIElement)DragIndicator).Visibility = (Visibility)1;
		DragIndicator.Source = null;
		if (_dragItem != null)
		{
			((UIElement)_dragItem).Visibility = (Visibility)0;
		}
		if (_currentGrid != null)
		{
			_currentGrid.ResetTransform();
		}
		_dragItem = null;
		_currentTarget = null;
		_dragHoverIndex = null;
		ScrollViewerIsEnabled = true;
		_scrollDirection = ScrollDirection.None;
		_shiftedIndices.Clear();
	}

	private void OnListManipulationDelta(object sender, ManipulationDeltaEventArgs e)
	{
		_holdTimer.Stop();
	}

	private bool NoTranslationPresent(ManipulationDelta e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Point translation = e.Translation;
		if (translation.Y == 0.0)
		{
			Point translation2 = e.Translation;
			return translation2.X == 0.0;
		}
		return false;
	}

	private void OnGridManipulationDelta(object sender, ManipulationDeltaEventArgs e)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected Obj, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected Obj, but got Unknown
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		if (!IsRearranging || _isRemovingPin)
		{
			return;
		}
		e.Handled = true;
		if (ItemsSource == null || ((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Count <= 1 || _dragItem == null || (NoTranslationPresent(e.CumulativeManipulation) && NoTranslationPresent(e.DeltaManipulation)))
		{
			return;
		}
		if (_dragItem != _currentGrid)
		{
			SwapZoomTo(_dragItem, animate: false);
		}
		if (DragIndicator.Source == null)
		{
			_dragItem.ResetTransform();
			Size renderSize = ((UIElement)_dragItem).RenderSize;
			WriteableBitmap val = new WriteableBitmap((int)renderSize.Width, (int)renderSize.Height);
			val.Render((UIElement)(object)_dragItem, (Transform)null);
			val.Invalidate();
			DragIndicator.Source = (ImageSource)(object)val;
			((UIElement)DragIndicator).Visibility = (Visibility)0;
			((UIElement)_dragItem).Visibility = (Visibility)1;
		}
		TranslateTransform val2 = (TranslateTransform)((UIElement)DragIndicator).RenderTransform;
		Point translation = e.CumulativeManipulation.Translation;
		val2.Y = translation.Y;
		Point translation2 = e.CumulativeManipulation.Translation;
		val2.X = translation2.X;
		if (_dragItem != _currentGrid)
		{
			_holdTimer.Start();
		}
		_centerPoint.Y = Canvas.GetTop((UIElement)(object)DragIndicator) + val2.Y + ((FrameworkElement)DragIndicator).Height / 2.0;
		_centerPoint.X = Canvas.GetLeft((UIElement)(object)DragIndicator) + val2.X + ((FrameworkElement)DragIndicator).Width / 2.0;
		double num = Canvas.GetTop((UIElement)(object)DragIndicator) + val2.Y;
		double num2 = Canvas.GetTop((UIElement)(object)DragIndicator) + val2.Y + ((FrameworkElement)DragIndicator).Height;
		if (num < 5.0 && ScrollViewer.VerticalOffset > 0.0)
		{
			_scrollDirection = ScrollDirection.Up;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)AutoScroll);
		}
		else if (num2 + 5.0 > ((FrameworkElement)DragCanvas).ActualHeight && ((FrameworkElement)Canvas).ActualHeight - ((FrameworkElement)ScrollViewer).ActualHeight > ScrollViewer.VerticalOffset)
		{
			_scrollDirection = ScrollDirection.Down;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)AutoScroll);
		}
		else
		{
			_scrollDirection = ScrollDirection.None;
		}
		GeneralTransform val3 = ((UIElement)DragCanvas).TransformToVisual(Application.Current.RootVisual);
		_centerPoint = val3.Transform(_centerPoint);
		_centerPoint = GetHostCoordinates(_centerPoint);
		object content = ((ContentControl)ScrollViewer).Content;
		UIElement val4 = (UIElement)((content is UIElement) ? content : null);
		List<UIElement> source = VisualTreeHelper.FindElementsInHostCoordinates(_centerPoint, val4).ToList();
		GroupTileControl groupTileControl = source.OfType<GroupTileControl>().FirstOrDefault();
		if (_currentTarget != groupTileControl && groupTileControl != null && groupTileControl != _dragItem)
		{
			if (_currentTarget != null)
			{
				((Control)_currentTarget).Background = null;
			}
			_currentTarget = groupTileControl;
			PerformRearrange((FrameworkElement)(object)groupTileControl, (FrameworkElement)(object)_dragItem);
		}
	}

	private double Clamp(double value, double min, double max)
	{
		return Math.Max(min, Math.Min(value, max));
	}

	private void AutoScroll()
	{
		if (_scrollDirection != ScrollDirection.None)
		{
			double num = 0.0;
			switch (_scrollDirection)
			{
			case ScrollDirection.Up:
				num = -1.0;
				break;
			case ScrollDirection.Down:
				num = 1.0;
				break;
			}
			double verticalOffset = ScrollViewer.VerticalOffset;
			double num2 = Clamp(verticalOffset + num, 0.0, ((FrameworkElement)Canvas).ActualHeight - ((FrameworkElement)ScrollViewer).ActualHeight);
			ScrollViewer.ScrollToVerticalOffset(num2);
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)AutoScroll);
		}
	}

	private static Point GetHostCoordinates(Point point)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected Obj, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		PhoneApplicationFrame val = (PhoneApplicationFrame)Application.Current.RootVisual;
		PageOrientation orientation = val.Orientation;
		if ((int)orientation != 18)
		{
			if ((int)orientation == 34)
			{
				double y = point.Y;
				Size renderSize = ((UIElement)val).RenderSize;
				return new Point(y, renderSize.Height - point.X);
			}
			return point;
		}
		Size renderSize2 = ((UIElement)val).RenderSize;
		return new Point(renderSize2.Width - point.Y, point.X);
	}

	private void RearrangeCanvas_OnHold(object sender, GestureEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (!IsRearranging)
		{
			Point position = e.GetPosition(Application.Current.RootVisual);
			object content = ((ContentControl)ScrollViewer).Content;
			UIElement val = (UIElement)((content is UIElement) ? content : null);
			List<UIElement> source = VisualTreeHelper.FindElementsInHostCoordinates(position, val).ToList();
			GroupTileControl groupTileControl = source.OfType<GroupTileControl>().FirstOrDefault();
			if (groupTileControl != null)
			{
				ZoomOut(groupTileControl);
				ScrollViewerIsEnabled = false;
				IsRearranging = true;
			}
		}
	}

	private void RearrangeCanvas_OnTap(object sender, GestureEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (IsRearranging && !_isRemovingPin)
		{
			Point position = e.GetPosition(Application.Current.RootVisual);
			object content = ((ContentControl)ScrollViewer).Content;
			UIElement val = (UIElement)((content is UIElement) ? content : null);
			List<UIElement> source = VisualTreeHelper.FindElementsInHostCoordinates(position, val).ToList();
			GroupTileControl groupTileControl = source.OfType<GroupTileControl>().FirstOrDefault();
			if (groupTileControl != null && _currentGrid != groupTileControl)
			{
				SwapZoomTo(groupTileControl);
			}
			else
			{
				CancelRearranging();
			}
		}
	}

	public void CancelRearranging()
	{
		if (IsRearranging)
		{
			ZoomIn();
			IsRearranging = false;
			ScrollViewerIsEnabled = true;
		}
	}

	private void ZoomOut(GroupTileControl exclude)
	{
		_currentGrid = exclude;
		((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
		{
			vm.PlayCardPickedUpSound();
		});
		MakeActive((UserControl)(object)_currentGrid);
		foreach (UIElement item in (PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children)
		{
			if (item != null && (object)item != exclude)
			{
				StartZoomOutAnimation(item);
			}
			else if ((object)item == exclude)
			{
				StartBounceOutAnimation(exclude);
			}
		}
	}

	private void ZoomIn()
	{
		foreach (GroupTileControl item in from GroupTileControl grid in (IEnumerable)((Panel)Canvas).Children
			where grid.IsZoomedOut()
			select grid)
		{
			StartZoomInAnimation(item);
		}
		MakeInactive((UserControl)(object)_currentGrid);
		_currentGrid = null;
	}

	private void MakeActive(UserControl control)
	{
		if (control != null)
		{
			VisualStateManager.GoToState((Control)(object)control, "Active", true);
		}
	}

	private void MakeInactive(UserControl control)
	{
		if (control != null)
		{
			VisualStateManager.GoToState((Control)(object)control, "Inactive", true);
		}
	}

	private void StartZoomInAnimation(object element)
	{
		object obj = ((FrameworkElement)((element is FrameworkElement) ? element : null)).Resources[(object)"ZoomIn"];
		((Storyboard)((obj is Storyboard) ? obj : null)).Begin();
	}

	private void StartZoomOutAnimation(object element)
	{
		object obj = ((FrameworkElement)((element is FrameworkElement) ? element : null)).Resources[(object)"ZoomOut"];
		((Storyboard)((obj is Storyboard) ? obj : null)).Begin();
	}

	private void StartBounceOutAnimation(object element)
	{
		object obj = ((FrameworkElement)((element is FrameworkElement) ? element : null)).Resources[(object)"BounceOut"];
		((Storyboard)((obj is Storyboard) ? obj : null)).Begin();
	}

	private void StartBounceInAnimation(object element)
	{
		object obj = ((FrameworkElement)((element is FrameworkElement) ? element : null)).Resources[(object)"BounceIn"];
		((Storyboard)((obj is Storyboard) ? obj : null)).Begin();
	}

	public void SwapZoomTo(GroupTileControl grid, bool animate = true)
	{
		_holdTimer.Stop();
		((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
		{
			vm.PlayCardPickedUpSound();
		});
		if (_currentGrid != null)
		{
			MakeInactive((UserControl)(object)_currentGrid);
			StartZoomOutAnimation(_currentGrid);
		}
		_currentGrid = grid;
		MakeActive((UserControl)(object)_currentGrid);
		if (animate)
		{
			StartBounceInAnimation(grid);
		}
		else
		{
			grid.ResetTransform();
		}
	}

	public void PerformRearrange(FrameworkElement target, FrameworkElement origin, bool playSounds = true, bool updateTargetIndex = true)
	{
		int num = ItemsSource.IndexOf((ThreadViewModel)target.DataContext);
		int num2 = ItemsSource.IndexOf((ThreadViewModel)origin.DataContext);
		if (!_shiftedIndices.TryGetValue(num, out var value))
		{
			value = num;
		}
		if (!_shiftedIndices.TryGetValue(num2, out var value2))
		{
			value2 = num2;
		}
		if (updateTargetIndex)
		{
			_dragHoverIndex = value;
		}
		double finalTop = GetFinalTop(target);
		double finalLeft = GetFinalLeft(target);
		StopShiftAnimation(target);
		bool flag = value2 < value;
		int num3 = value2 + ((!flag) ? 1 : (-1));
		do
		{
			num3 = ((!flag) ? (num3 - 1) : (num3 + 1));
			double toLeft = finalLeft;
			double toTop = finalTop;
			int num4 = num3;
			foreach (KeyValuePair<int, int> shiftedIndex in _shiftedIndices)
			{
				if (shiftedIndex.Value == num3)
				{
					num4 = shiftedIndex.Key;
				}
			}
			GroupTileControl currentElement = _objectToVisibleElement[ItemsSource[num4]];
			finalTop = GetFinalTop((FrameworkElement)(object)currentElement);
			finalLeft = GetFinalLeft((FrameworkElement)(object)currentElement);
			_shiftedIndices[num4] = num3 + ((!flag) ? 1 : (-1));
			((UIElement)currentElement).IsHitTestVisible = false;
			AnimateShift((FrameworkElement)(object)currentElement, finalTop, toTop, finalLeft, toLeft, (object sender, EventArgs args) =>
			{
				((UIElement)currentElement).IsHitTestVisible = true;
			});
		}
		while (num3 != value);
		if (playSounds)
		{
			if (_playedFirstSlide)
			{
				((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
				{
					vm.PlayCardSlide2Sound();
				});
				_playedFirstSlide = false;
			}
			else
			{
				((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
				{
					vm.PlayCardSlide1Sound();
				});
				_playedFirstSlide = true;
			}
		}
		_shiftedIndices[num2] = value;
		_currentTarget = null;
	}

	public void StopShiftAnimation(FrameworkElement target)
	{
		if (target.Tag != null)
		{
			object tag = target.Tag;
			Storyboard val = (Storyboard)((tag is Storyboard) ? tag : null);
			val.Stop();
		}
	}

	public void AnimateShift(FrameworkElement target, double fromTop, double toTop, double fromLeft, double toLeft, EventHandler completedHandler = null)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected Obj, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected Obj, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected Obj, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected Obj, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected Obj, but got Unknown
		Storyboard val;
		DoubleAnimation val3;
		DoubleAnimation val5;
		if (target.Tag != null)
		{
			object tag = target.Tag;
			val = (Storyboard)((tag is Storyboard) ? tag : null);
			Timeline val2 = ((PresentationFrameworkCollection<Timeline>)(object)val.Children)[0];
			val3 = (DoubleAnimation)(object)((val2 is DoubleAnimation) ? val2 : null);
			Timeline val4 = ((PresentationFrameworkCollection<Timeline>)(object)val.Children)[1];
			val5 = (DoubleAnimation)(object)((val4 is DoubleAnimation) ? val4 : null);
			val.SkipToFill();
			val.Stop();
		}
		else
		{
			val = new Storyboard();
			val3 = new DoubleAnimation();
			val5 = new DoubleAnimation();
			if (completedHandler != null)
			{
				((Timeline)val).Completed += completedHandler;
			}
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(200.0);
			((Timeline)val3).Duration = Duration.op_Implicit(timeSpan);
			((Timeline)val5).Duration = Duration.op_Implicit(timeSpan);
			Storyboard.SetTarget((Timeline)(object)val3, (DependencyObject)(object)target);
			Storyboard.SetTarget((Timeline)(object)val5, (DependencyObject)(object)target);
			Storyboard.SetTargetProperty((Timeline)(object)val3, new PropertyPath("(Canvas.Top)", new object[0]));
			Storyboard.SetTargetProperty((Timeline)(object)val5, new PropertyPath("(Canvas.Left)", new object[0]));
			((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val3);
			((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val5);
			target.Tag = val;
		}
		val3.From = fromTop;
		val3.To = toTop;
		val5.From = fromLeft;
		val5.To = toLeft;
		val.Begin();
	}

	private double GetCanvasProperty(FrameworkElement element, Func<UIElement, double> func, int animationIndex)
	{
		double num = func((UIElement)(object)element);
		if (element.Tag != null)
		{
			object tag = element.Tag;
			Storyboard val = (Storyboard)((tag is Storyboard) ? tag : null);
			Timeline val2 = ((PresentationFrameworkCollection<Timeline>)(object)val.Children)[animationIndex];
			DoubleAnimation val3 = (DoubleAnimation)(object)((val2 is DoubleAnimation) ? val2 : null);
			num = val3.To ?? num;
		}
		return num;
	}

	private double GetFinalTop(FrameworkElement element)
	{
		return GetCanvasProperty(element, (Func<UIElement, double>)Canvas.GetTop, 0);
	}

	private double GetFinalLeft(FrameworkElement element)
	{
		return GetCanvasProperty(element, (Func<UIElement, double>)Canvas.GetLeft, 1);
	}

	public void AnimatePinAdd(ObservableCollection<ThreadViewModel> pinsToAdd)
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected Obj, but got Unknown
		if (pinsToAdd.Count == 0)
		{
			return;
		}
		INotifyCollectionChanged currentCollection = _currentCollection;
		if (currentCollection != null)
		{
			currentCollection.CollectionChanged -= OnCollectionChanged;
		}
		double top = 0.0;
		double left = 0.0;
		if (ItemsSource.Count > 0)
		{
			ThreadViewModel key = ItemsSource[ItemsSource.Count - 1];
			GroupTileControl groupTileControl = _objectToVisibleElement[key];
			top = Canvas.GetTop((UIElement)(object)groupTileControl);
			left = Canvas.GetLeft((UIElement)(object)groupTileControl);
			AdvanceTilePosition(ref top, ref left);
		}
		foreach (ThreadViewModel item in pinsToAdd)
		{
			ItemsSource.Add(item);
			FrameworkElement val = Add(item, ref top, ref left);
			((UIElement)val).Opacity = 0.0;
		}
		Size gridCellSize = GridCellSize;
		double num = gridCellSize.Height * Math.Ceiling((double)((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Count / 2.0);
		Thickness margin = ((FrameworkElement)Canvas).Margin;
		double num2 = num + margin.Top;
		Thickness margin2 = ((FrameworkElement)Canvas).Margin;
		double num3 = num2 + margin2.Bottom;
		if (((FrameworkElement)Canvas).ActualHeight == num3)
		{
			AnimateToBottom();
		}
		else
		{
			((FrameworkElement)Canvas).Height = num3;
			((FrameworkElement)Canvas).SizeChanged += CanvasOnSizeChanged;
		}
		if (currentCollection != null && currentCollection == _currentCollection)
		{
			_currentCollection.CollectionChanged += OnCollectionChanged;
		}
		pinsToAdd.Clear();
	}

	private void CanvasOnSizeChanged(object sender, SizeChangedEventArgs args)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected Obj, but got Unknown
		((FrameworkElement)Canvas).SizeChanged -= CanvasOnSizeChanged;
		AnimateToBottom();
	}

	private void AnimateToBottom()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected Obj, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected Obj, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected Obj, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected Obj, but got Unknown
		((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
		{
			vm.PlayNewGroupAppearingSound();
		});
		Storyboard val = new Storyboard();
		((Timeline)val).Completed += StoryboardOnCompleted;
		DoubleAnimation val2 = new DoubleAnimation();
		val2.To = ((FrameworkElement)Canvas).Height - ScrollViewer.ViewportHeight;
		val2.From = ScrollViewer.VerticalOffset;
		((Timeline)val2).Duration = Duration.op_Implicit(TimeSpan.FromMilliseconds(500.0));
		CubicEase val3 = new CubicEase();
		((EasingFunctionBase)val3).EasingMode = (EasingMode)0;
		val2.EasingFunction = (IEasingFunction)(object)val3;
		DoubleAnimation val4 = val2;
		Storyboard.SetTarget((Timeline)(object)val4, (DependencyObject)(object)this);
		Storyboard.SetTargetProperty((Timeline)(object)val4, new PropertyPath("VerticalOffset", new object[0]));
		((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val4);
		val.Begin();
	}

	private void StoryboardOnCompleted(object sender, EventArgs eventArgs)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected Obj, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected Obj, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected Obj, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected Obj, but got Unknown
		Storyboard val = (Storyboard)((sender is Storyboard) ? sender : null);
		((Timeline)val).Completed -= StoryboardOnCompleted;
		((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
		{
			vm.PlayNewGroupBounceSound();
		});
		IEnumerable<FrameworkElement> enumerable = ((IEnumerable<UIElement>)((Panel)Canvas).Children).Where((UIElement c) => c.Opacity == 0.0).Cast<FrameworkElement>();
		foreach (FrameworkElement item in enumerable)
		{
			Storyboard val2 = new Storyboard();
			DoubleAnimation val3 = new DoubleAnimation();
			val3.To = 1.0;
			val3.From = 0.0;
			((Timeline)val3).Duration = Duration.op_Implicit(TimeSpan.FromMilliseconds(500.0));
			CubicEase val4 = new CubicEase();
			((EasingFunctionBase)val4).EasingMode = (EasingMode)0;
			val3.EasingFunction = (IEasingFunction)(object)val4;
			DoubleAnimation val5 = val3;
			Storyboard.SetTarget((Timeline)(object)val5, (DependencyObject)(object)item);
			Storyboard.SetTargetProperty((Timeline)(object)val5, new PropertyPath("Opacity", new object[0]));
			((PresentationFrameworkCollection<Timeline>)(object)val2.Children).Add((Timeline)(object)val5);
			val2.Begin();
		}
	}

	public void AnimatePinRemove()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected Obj, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected Obj, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected Obj, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected Obj, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected Obj, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected Obj, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected Obj, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected Obj, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected Obj, but got Unknown
		if (_currentGrid != null && !_isRemovingPin)
		{
			_gridBeingUnpinned = _currentGrid;
			_isRemovingPin = true;
			((FrameworkElement)(object)this).CallWithViewModel((PinnedThreadsViewModel vm) =>
			{
				vm.PlayRemovingPinSound();
			});
			((UIElement)_currentGrid).RenderTransform = (Transform)new ScaleTransform();
			((UIElement)_currentGrid).RenderTransformOrigin = new Point(0.5, 0.5);
			Storyboard val = new Storyboard();
			CubicEase val2 = new CubicEase();
			((EasingFunctionBase)val2).EasingMode = (EasingMode)0;
			CubicEase easingFunction = val2;
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(500.0);
			DoubleAnimation val3 = new DoubleAnimation();
			val3.To = 0.0;
			val3.From = 1.0;
			((Timeline)val3).Duration = Duration.op_Implicit(timeSpan);
			val3.EasingFunction = (IEasingFunction)(object)easingFunction;
			DoubleAnimation val4 = val3;
			DoubleAnimation val5 = new DoubleAnimation();
			val5.To = 0.0;
			val5.From = 1.0;
			((Timeline)val5).Duration = Duration.op_Implicit(timeSpan);
			val5.EasingFunction = (IEasingFunction)(object)easingFunction;
			DoubleAnimation val6 = val5;
			DoubleAnimation val7 = new DoubleAnimation();
			val7.To = 0.0;
			val7.From = 1.0;
			((Timeline)val7).Duration = Duration.op_Implicit(timeSpan);
			val7.EasingFunction = (IEasingFunction)(object)easingFunction;
			DoubleAnimation val8 = val7;
			Storyboard.SetTarget((Timeline)(object)val4, (DependencyObject)(object)_currentGrid);
			Storyboard.SetTarget((Timeline)(object)val6, (DependencyObject)(object)_currentGrid);
			Storyboard.SetTarget((Timeline)(object)val8, (DependencyObject)(object)_currentGrid);
			Storyboard.SetTargetProperty((Timeline)(object)val4, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)", new object[0]));
			Storyboard.SetTargetProperty((Timeline)(object)val6, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", new object[0]));
			Storyboard.SetTargetProperty((Timeline)(object)val8, new PropertyPath("Opacity", new object[0]));
			((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val4);
			((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val6);
			((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val8);
			((Timeline)val).Completed += OnRemoveAnimationComplete;
			val.Begin();
		}
	}

	private async void OnRemoveAnimationComplete(object sender, EventArgs eventArgs)
	{
		Storyboard board = (Storyboard)((sender is Storyboard) ? sender : null);
		((Timeline)board).Completed -= OnRemoveAnimationComplete;
		if (_gridBeingUnpinned == null)
		{
			return;
		}
		GroupTileControl gridUpinned = _gridBeingUnpinned;
		_gridBeingUnpinned = null;
		ThreadViewModel lastObject = ItemsSource[ItemsSource.Count - 1];
		GroupTileControl lastChild = _objectToVisibleElement[lastObject];
		PerformRearrange((FrameworkElement)(object)lastChild, (FrameworkElement)(object)gridUpinned, playSounds: false, updateTargetIndex: false);
		_shiftedIndices.Clear();
		await Task.Delay(200);
		ThreadViewModel element = (ThreadViewModel)((FrameworkElement)gridUpinned).DataContext;
		_objectToVisibleElement.Remove(element);
		((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Remove((UIElement)(object)gridUpinned);
		bool didAnimate = false;
		Size gridCellSize = GridCellSize;
		double num = gridCellSize.Height * Math.Ceiling((double)((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Count / 2.0);
		Thickness margin = ((FrameworkElement)Canvas).Margin;
		double num2 = num + margin.Top;
		Thickness margin2 = ((FrameworkElement)Canvas).Margin;
		double newHeight = num2 + margin2.Bottom;
		if (newHeight != ((FrameworkElement)Canvas).Height && ScrollViewer.VerticalOffset + ScrollViewer.ViewportHeight > newHeight)
		{
			didAnimate = true;
			Storyboard val = new Storyboard();
			((Timeline)val).Completed += ScrollUpdateComplete;
			DoubleAnimation val2 = new DoubleAnimation();
			val2.To = newHeight - ScrollViewer.ViewportHeight;
			val2.From = ScrollViewer.VerticalOffset;
			((Timeline)val2).Duration = Duration.op_Implicit(TimeSpan.FromMilliseconds(500.0));
			CubicEase val3 = new CubicEase();
			((EasingFunctionBase)val3).EasingMode = (EasingMode)0;
			val2.EasingFunction = (IEasingFunction)(object)val3;
			DoubleAnimation val4 = val2;
			Storyboard.SetTarget((Timeline)(object)val4, (DependencyObject)(object)this);
			Storyboard.SetTargetProperty((Timeline)(object)val4, new PropertyPath("VerticalOffset", new object[0]));
			((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val4);
			val.Begin();
		}
		else
		{
			((FrameworkElement)Canvas).Height = newHeight;
		}
		if (_currentCollection != null)
		{
			_currentCollection.CollectionChanged -= OnCollectionChanged;
		}
		int index = ItemsSource.IndexOf(element);
		ItemsSource.Remove(element);
		if (_currentCollection != null)
		{
			_currentCollection.CollectionChanged += OnCollectionChanged;
		}
		if (PinRemoved != null)
		{
			PinRemoved(this, element);
		}
		((DependencyObject)this).Dispatcher.BeginInvoke((Action)(async () =>
		{
			if (didAnimate)
			{
				await Task.Delay(500);
			}
			_isRemovingPin = false;
			ScrollViewerIsEnabled = true;
			if (IsRearranging && ItemsSource != null)
			{
				int count = ItemsSource.Count;
				if (count == 0)
				{
					CancelRearranging();
				}
				else
				{
					index = Math.Min(index, count - 1);
					GroupTileControl grid = _objectToVisibleElement[ItemsSource[index]];
					SwapZoomTo(grid);
				}
			}
		}));
	}

	private void ScrollUpdateComplete(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Storyboard val = new Storyboard();
		((Timeline)val).Completed -= ScrollUpdateComplete;
		Canvas canvas = Canvas;
		Size gridCellSize = GridCellSize;
		double num = gridCellSize.Height * Math.Ceiling((double)((PresentationFrameworkCollection<UIElement>)(object)((Panel)Canvas).Children).Count / 2.0);
		Thickness margin = ((FrameworkElement)Canvas).Margin;
		double num2 = num + margin.Top;
		Thickness margin2 = ((FrameworkElement)Canvas).Margin;
		((FrameworkElement)canvas).Height = num2 + margin2.Bottom;
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/PinnedThreadGridView.xaml", UriKind.Relative));
			InterceptionGrid = (Grid)((FrameworkElement)this).FindName("InterceptionGrid");
			ScrollViewer = (ScrollViewer)((FrameworkElement)this).FindName("ScrollViewer");
			Canvas = (Canvas)((FrameworkElement)this).FindName("Canvas");
			DragCanvas = (Canvas)((FrameworkElement)this).FindName("DragCanvas");
			DragIndicator = (Image)((FrameworkElement)this).FindName("DragIndicator");
		}
	}

	static PinnedThreadGridView()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected Obj, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected Obj, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected Obj, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected Obj, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected Obj, but got Unknown
	}
}

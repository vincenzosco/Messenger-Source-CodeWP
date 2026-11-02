using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Facebook.Base.Logging;
using Facebook.Messenger.WP8.Events;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls.Base;

public class FacebookListView : LongListSelector
{
	protected Dictionary<object, ContentPresenter> ActivePresenters = new Dictionary<object, ContentPresenter>();

	private readonly ScrollableViewportStateCalculator _scrollStateCalculator = new ScrollableViewportStateCalculator();

	private bool _manualScroll;

	private bool _hasRealizedItems;

	public static readonly DependencyProperty InternalItemsSourceProperty = DependencyProperty.Register("InternalItemsSource", typeof(object), typeof(FacebookListView), new PropertyMetadata((object)null, (PropertyChangedCallback)OnInternalItemsSourceChanged));

	protected ViewportControl ViewportControl { get; set; }

	protected ScrollBar VerticalScrollBar { get; set; }

	protected Brush OriginalScrollBackgroundBrush { get; set; }

	protected Brush ScrollBarBrush { get; set; }

	public event EventHandler<ListEventArgs> ListEvent;

	public event EventHandler<ViewportChangedEventArgs> ViewportChanged;

	private static void OnInternalItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
	{
		if (o is FacebookListView facebookListView)
		{
			facebookListView.OnNewItemSource(e.OldValue, e.NewValue);
		}
	}

	public FacebookListView()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected Obj, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected Obj, but got Unknown
		((LongListSelector)this).ItemRealized += OnItemRealized;
		((LongListSelector)this).ItemUnrealized += OnItemUnrealized;
		DependencyProperty internalItemsSourceProperty = InternalItemsSourceProperty;
		Binding val = new Binding();
		val.Source = this;
		val.Path = new PropertyPath("ItemsSource", new object[0]);
		((FrameworkElement)this).SetBinding(internalItemsSourceProperty, val);
		object obj = Application.Current.Resources[(object)"PhoneDisabledBrush"];
		OriginalScrollBackgroundBrush = (Brush)((obj is Brush) ? obj : null);
	}

	protected override Size MeasureOverride(Size availableSize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return ((LongListSelector)this).MeasureOverride(availableSize);
		}
		catch (ArgumentException)
		{
			return ((LongListSelector)this).MeasureOverride(availableSize);
		}
	}

	public override void OnApplyTemplate()
	{
		((LongListSelector)this).OnApplyTemplate();
		if (ViewportControl != null)
		{
			ViewportControl.ViewportChanged -= OnViewportChanged;
		}
		DependencyObject templateChild = ((Control)this).GetTemplateChild("ViewportControl");
		ViewportControl = (ViewportControl)(object)((templateChild is ViewportControl) ? templateChild : null);
		DependencyObject templateChild2 = ((Control)this).GetTemplateChild("VerticalScrollBar");
		VerticalScrollBar = (ScrollBar)(object)((templateChild2 is ScrollBar) ? templateChild2 : null);
		if (VerticalScrollBar != null)
		{
			OriginalScrollBackgroundBrush = ((Control)VerticalScrollBar).Background;
			((Control)VerticalScrollBar).Background = ScrollBarBrush;
		}
		if (ViewportControl != null)
		{
			ViewportControl.ViewportChanged += OnViewportChanged;
		}
	}

	private void OnViewportChanged(object sender, ViewportChangedEventArgs e)
	{
		if (_hasRealizedItems && _scrollStateCalculator.HasTransitionedToBottom(ViewportControl))
		{
			EventHandler<ListEventArgs> listEvent = ListEvent;
			if (listEvent != null)
			{
				((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
				{
					listEvent(this, new ListEventArgs(Facebook.Messenger.WP8.Events.ListEvent.ScrolledToBottom));
				}));
			}
		}
		if (_manualScroll)
		{
			_manualScroll = false;
		}
		else if (ViewportChanged != null)
		{
			ViewportChanged(sender, e);
		}
	}

	public void ScrollToTop(bool animate = false, int duration = 250)
	{
		if (!IsAtTop())
		{
			if (((LongListSelector)this).ListHeader != null)
			{
				SafeScrollTo(((LongListSelector)this).ListHeader);
			}
			else
			{
				ScrollToTopElement();
			}
		}
	}

	public bool IsAtTop()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (((LongListSelector)this).ItemsSource != null && ((LongListSelector)this).ItemsSource.Count > 0)
		{
			object obj = ((LongListSelector)this).ItemsSource[0];
			if (obj != null && ActivePresenters.TryGetValue(obj, out var value))
			{
				double top = Canvas.GetTop((UIElement)(object)value);
				Rect viewport = ViewportControl.Viewport;
				if (top >= viewport.Top)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void SafeScrollTo(object item)
	{
		if (item == null)
		{
			return;
		}
		try
		{
			((LongListSelector)this).ScrollTo(item);
		}
		catch (Exception arg)
		{
			Logger.Warning($"FacebookListView.ScrollTo exception: {arg}", "SafeScrollTo", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\Base\\FacebookListView.cs", 174);
		}
	}

	public void ScrollToTopElement()
	{
		if (((LongListSelector)this).ItemsSource != null && ((LongListSelector)this).ItemsSource.Count > 0)
		{
			SafeScrollTo(((LongListSelector)this).ItemsSource[0]);
		}
	}

	public void MoveElementToView(object element)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		if (((LongListSelector)this).ItemsSource == null || ((LongListSelector)this).ItemsSource.Count <= 0 || element == null || !ActivePresenters.TryGetValue(element, out var value))
		{
			return;
		}
		double num = Canvas.GetTop((UIElement)(object)value);
		double num2 = num;
		Rect viewport = ViewportControl.Viewport;
		if (num2 < viewport.Top)
		{
			if (num < 0.0)
			{
				num = 0.0;
			}
			SetViewportOrigin(new Point(0.0, num));
			return;
		}
		double num3 = num + ((FrameworkElement)value).ActualHeight;
		Rect viewport2 = ViewportControl.Viewport;
		if (!(num3 > viewport2.Bottom))
		{
			return;
		}
		double num4 = num;
		Rect viewport3 = ViewportControl.Viewport;
		double num5 = num4 - (viewport3.Height - ((FrameworkElement)value).ActualHeight);
		double num6 = num5;
		Rect bounds = ViewportControl.Bounds;
		if (num6 < bounds.Top)
		{
			Rect bounds2 = ViewportControl.Bounds;
			num5 = bounds2.Top;
		}
		else
		{
			double num7 = num5;
			Rect bounds3 = ViewportControl.Bounds;
			if (num7 > bounds3.Bottom)
			{
				Rect bounds4 = ViewportControl.Bounds;
				num5 = bounds4.Bottom;
			}
		}
		SetViewportOrigin(new Point(0.0, num5));
	}

	private void SetViewportOrigin(Point origin)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_manualScroll = true;
		ViewportControl.SetViewportOrigin(origin);
	}

	protected virtual void OnItemUnrealized(object sender, ItemRealizationEventArgs args)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		if (((FrameworkElement)args.Container).DataContext != null)
		{
			ActivePresenters.Remove(((FrameworkElement)args.Container).DataContext);
		}
		if ((int)args.ItemKind == 2)
		{
			((FrameworkElement)args.Container).DataContext = null;
			_hasRealizedItems = ((LongListSelector)this).ItemsSource != null && ((LongListSelector)this).ItemsSource.Count > 0;
		}
	}

	protected virtual void OnItemRealized(object sender, ItemRealizationEventArgs args)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		if (((FrameworkElement)args.Container).DataContext != null)
		{
			ActivePresenters[((FrameworkElement)args.Container).DataContext] = args.Container;
		}
		if ((int)args.ItemKind == 2)
		{
			_hasRealizedItems = ((LongListSelector)this).ItemsSource != null && ((LongListSelector)this).ItemsSource.Count > 0;
		}
	}

	private void OnNewItemSource(object oldValue, object newValue)
	{
		_hasRealizedItems = false;
		if (oldValue is INotifyCollectionChanged notifyCollectionChanged)
		{
			notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
		}
		if (newValue is INotifyCollectionChanged notifyCollectionChanged2)
		{
			notifyCollectionChanged2.CollectionChanged += OnCollectionChanged;
		}
		UpdateScrollbarVisibility();
	}

	private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		UpdateScrollbarVisibility();
	}

	private void UpdateScrollbarVisibility()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected Obj, but got Unknown
		if (((LongListSelector)this).ItemsSource != null && ((LongListSelector)this).ItemsSource.Count == 0)
		{
			ScrollBarBrush = (Brush)new SolidColorBrush(Colors.Transparent);
		}
		else
		{
			ScrollBarBrush = OriginalScrollBackgroundBrush;
		}
		if (VerticalScrollBar != null)
		{
			((Control)VerticalScrollBar).Background = ScrollBarBrush;
		}
	}

	static FacebookListView()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected Obj, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
	}
}

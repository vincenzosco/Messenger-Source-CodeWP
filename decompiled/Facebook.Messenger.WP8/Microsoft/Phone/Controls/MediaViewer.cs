using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
	[TemplatePart(Name = "MediaStrip", Type = typeof(Canvas))]
	[TemplatePart(Name = "MediaStripCompositeTransform", Type = typeof(CompositeTransform))]
	public class MediaViewer : Control
	{
		private enum MediaViewerState
		{
			Uninitialized,
			Initialized,
			InertiaAnimating,
			Dragging,
			DraggingAndSquishing,
			Pinching,
			UnsquishAnimating
		}

		private const int _virtualizedItemPoolSize = 3;

		private const double _itemGutter = 18.0;

		private const double _maxDraggingSquishDistance = 150.0;

		private const double _minDraggingSquishScale = 0.9;

		private const int _unsquishAnimationMilliseconds = 100;

		private const double _dragStagnationTimeThreshold = 300.0;

		private const double _dragStagnationDistanceThreshold = 15.0;

		private const double _flickMinInputVelocity = 0.0;

		private const double _flickMaxInputVelocity = 5.0;

		private const double _flickMinOutputMilliseconds = 100.0;

		private const double _flickMaxOutputMilliseconds = 800.0;

		private CompositeTransform _mediaStripCompositeTransform;

		private MediaViewerState _state;

		private List<VirtualizedItem> _virtualizedItemPool;

		private Canvas _mediaStrip;

		private Nullable _size = null;

		private DragState _dragState = new DragState(150.0);

		private Storyboard _dragInertiaAnimation;

		private Storyboard _unsquishAnimation;

		private DoubleAnimation _dragInertiaAnimationTranslation;

		private DoubleAnimation _unsquishAnimationTranslation;

		private VirtualizedItem _displayedVirtualizedItem;

		private FrameworkElement _headerTemplateInstance;

		private FrameworkElement _footerTemplateInstance;

		private FrameworkElement _headerOnMediaStrip;

		private FrameworkElement _footerOnMediaStrip;

		private int? _displayedElementIndex = null;

		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(MediaViewer), new PropertyMetadata((object)null, (PropertyChangedCallback)OnItemTemplatePropertyChanged));

		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<IImage>), typeof(MediaViewer), new PropertyMetadata((PropertyChangedCallback)OnItemsPropertyChanged));

		public static readonly DependencyProperty DisplayedElementProperty = DependencyProperty.Register("DisplayedElement", typeof(DisplayedElementType), typeof(MediaViewer), new PropertyMetadata((object)DisplayedElementType.None));

		public static readonly DependencyProperty DisplayedItemIndexProperty = DependencyProperty.Register("DisplayedItemIndex", typeof(int), typeof(MediaViewer), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty InitiallyDisplayedIndexProperty = DependencyProperty.Register("InitiallyDisplayedIndex", typeof(int), typeof(MediaViewer), new PropertyMetadata((object)0));

		public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(MediaViewer), new PropertyMetadata((object)(Visibility)1, (PropertyChangedCallback)OnHeaderVisibilityPropertyChanged));

		public static readonly DependencyProperty FooterVisibilityProperty = DependencyProperty.Register("FooterVisibility", typeof(Visibility), typeof(MediaViewer), new PropertyMetadata((object)(Visibility)1, (PropertyChangedCallback)OnFooterVisibilityPropertyChanged));

		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(FrameworkElement), typeof(MediaViewer), new PropertyMetadata((object)null, (PropertyChangedCallback)OnHeaderPropertyChanged));

		public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(FrameworkElement), typeof(MediaViewer), new PropertyMetadata((object)null, (PropertyChangedCallback)OnFooterPropertyChanged));

		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(MediaViewer), new PropertyMetadata((PropertyChangedCallback)HeaderTemplateChangedEventHandler));

		public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(MediaViewer), new PropertyMetadata((PropertyChangedCallback)FooterTemplateChangedEventHandler));

		public static readonly DependencyProperty DragEnabledProperty = DependencyProperty.Register("DragEnabled", typeof(bool), typeof(MediaViewer), new PropertyMetadata((object)true));

		private double ScrollOffset
		{
			get
			{
				return _mediaStripCompositeTransform.TranslateX;
			}
			set
			{
				_mediaStripCompositeTransform.TranslateX = value;
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

		public ObservableCollection<IImage> Items
		{
			get
			{
				return (ObservableCollection<IImage>)((DependencyObject)this).GetValue(ItemsProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ItemsProperty, (object)value);
			}
		}

		public DisplayedElementType DisplayedElement
		{
			get
			{
				return (DisplayedElementType)((DependencyObject)this).GetValue(DisplayedElementProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(DisplayedElementProperty, (object)value);
			}
		}

		public int DisplayedItemIndex
		{
			get
			{
				return (int)((DependencyObject)this).GetValue(DisplayedItemIndexProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(DisplayedItemIndexProperty, (object)value);
			}
		}

		public int InitiallyDisplayedIndex
		{
			get
			{
				return (int)((DependencyObject)this).GetValue(InitiallyDisplayedIndexProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(InitiallyDisplayedIndexProperty, (object)value);
			}
		}

		public Visibility HeaderVisibility
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Visibility)((DependencyObject)this).GetValue(HeaderVisibilityProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((DependencyObject)this).SetValue(HeaderVisibilityProperty, (object)value);
			}
		}

		public Visibility FooterVisibility
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Visibility)((DependencyObject)this).GetValue(FooterVisibilityProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((DependencyObject)this).SetValue(FooterVisibilityProperty, (object)value);
			}
		}

		public FrameworkElement Header
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (FrameworkElement)((DependencyObject)this).GetValue(HeaderProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(HeaderProperty, (object)value);
			}
		}

		public FrameworkElement Footer
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (FrameworkElement)((DependencyObject)this).GetValue(FooterProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FooterProperty, (object)value);
			}
		}

		public DataTemplate HeaderTemplate
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (DataTemplate)((DependencyObject)this).GetValue(HeaderTemplateProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(HeaderTemplateProperty, (object)value);
			}
		}

		public DataTemplate FooterTemplate
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (DataTemplate)((DependencyObject)this).GetValue(FooterTemplateProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FooterTemplateProperty, (object)value);
			}
		}

		public bool DragEnabled
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(DragEnabledProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DragEnabledProperty, (object)value);
			}
		}

		public event EventHandler HeaderDisplayed;

		public event EventHandler<ItemDisplayedEventArgs> ItemDisplayed;

		public event EventHandler FooterDisplayed;

		public event EventHandler ItemZoomed;

		public event EventHandler ItemUnzoomed;

		public MediaViewer()
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected Obj, but got Unknown
			((Control)this).DefaultStyleKey = typeof(MediaViewer);
			((FrameworkElement)this).SizeChanged += OnMediaViewerSizeChanged;
		}

		private static void OnHeaderVisibilityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			mediaViewer.InitializeOrReset();
		}

		private static void OnFooterVisibilityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			mediaViewer.InitializeOrReset();
		}

		private static void OnItemsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			ObservableCollection<IImage> observableCollection = eventArgs.OldValue as ObservableCollection<IImage>;
			ObservableCollection<IImage> observableCollection2 = eventArgs.NewValue as ObservableCollection<IImage>;
			if (observableCollection != null)
			{
				observableCollection.CollectionChanged -= mediaViewer.OnItemsCollectionChanged;
			}
			if (observableCollection2 != null)
			{
				observableCollection2.CollectionChanged += mediaViewer.OnItemsCollectionChanged;
			}
			mediaViewer.InitializeOrReset();
		}

		private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				int count2 = e.NewItems.Count;
				ResetMediaStripGeometry();
				PlaceFooter();
				foreach (VirtualizedItem item in _virtualizedItemPool)
				{
					if (item.RepresentingItemIndex.HasValue && item.RepresentingItemIndex >= e.NewStartingIndex)
					{
						item.RepresentingItemIndex += count2;
					}
				}
				if (!_displayedElementIndex.HasValue)
				{
					JumpToElement(0);
				}
				else if (_displayedElementIndex.Value >= e.NewStartingIndex)
				{
					JumpToElement(_displayedElementIndex.Value + count2);
				}
				break;
			}
			case NotifyCollectionChangedAction.Remove:
			{
				int count = e.OldItems.Count;
				ResetMediaStripGeometry();
				PlaceFooter();
				foreach (VirtualizedItem item2 in _virtualizedItemPool)
				{
					if (item2.RepresentingItemIndex.HasValue)
					{
						int? representingItemIndex = item2.RepresentingItemIndex;
						int oldStartingIndex = e.OldStartingIndex;
						if (representingItemIndex.GetValueOrDefault() >= oldStartingIndex && representingItemIndex.HasValue && item2.RepresentingItemIndex < e.OldStartingIndex + count)
						{
							item2.RepresentingItemIndex = null;
							item2.DataContext = null;
						}
					}
					if (item2.RepresentingItemIndex.HasValue && item2.RepresentingItemIndex > e.OldStartingIndex)
					{
						item2.RepresentingItemIndex -= count;
					}
				}
				if (!_displayedElementIndex.HasValue)
				{
					break;
				}
				int elementCount = GetElementCount();
				if (elementCount == 0)
				{
					UpdateDisplayedElement(null);
					break;
				}
				int num = _displayedElementIndex.Value;
				int num2 = (((int)HeaderVisibility == 0) ? (e.OldStartingIndex + 1) : e.OldStartingIndex);
				if (_displayedElementIndex.Value > e.OldStartingIndex)
				{
					int num3 = -1 * Math.Min(count, _displayedElementIndex.Value - num2);
					num += num3;
				}
				if (num >= elementCount)
				{
					num = elementCount - 1;
				}
				JumpToElement(num);
				break;
			}
			case NotifyCollectionChangedAction.Replace:
			case NotifyCollectionChangedAction.Move:
			case NotifyCollectionChangedAction.Reset:
				InitializeOrReset();
				break;
			}
		}

		private static void HeaderTemplateChangedEventHandler(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected Obj, but got Unknown
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			DataTemplate val = (DataTemplate)eventArgs.NewValue;
			if (val != null)
			{
				DependencyObject val2 = val.LoadContent();
				mediaViewer._headerTemplateInstance = (FrameworkElement)(object)((val2 is FrameworkElement) ? val2 : null);
			}
			else
			{
				mediaViewer._headerTemplateInstance = null;
			}
			if (mediaViewer._state == MediaViewerState.Initialized)
			{
				mediaViewer.PlaceHeader();
			}
		}

		private static void FooterTemplateChangedEventHandler(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected Obj, but got Unknown
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			DataTemplate val = (DataTemplate)eventArgs.NewValue;
			if (val != null)
			{
				DependencyObject val2 = val.LoadContent();
				mediaViewer._footerTemplateInstance = (FrameworkElement)(object)((val2 is FrameworkElement) ? val2 : null);
			}
			else
			{
				mediaViewer._footerTemplateInstance = null;
			}
			if (mediaViewer._state == MediaViewerState.Initialized)
			{
				mediaViewer.PlaceFooter();
			}
		}

		private static void OnHeaderPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			if (mediaViewer._state == MediaViewerState.Initialized)
			{
				mediaViewer.PlaceHeader();
			}
		}

		private static void OnFooterPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			if (mediaViewer._state == MediaViewerState.Initialized)
			{
				mediaViewer.PlaceFooter();
			}
		}

		private static void OnItemTemplatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			MediaViewer mediaViewer = (MediaViewer)(object)dependencyObject;
			mediaViewer.InitializeVirtualizationIfReady();
		}

		private void InitializeVirtualizationIfReady()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (_mediaStrip != null && _size.HasValue)
			{
				_virtualizedItemPool = new List<VirtualizedItem>();
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Clear();
				for (int i = 0; i < 3; i++)
				{
					Size value = _size.Value;
					double width = value.Width;
					Size value2 = _size.Value;
					VirtualizedItem virtualizedItem = new VirtualizedItem(new Size(width, value2.Height));
					virtualizedItem.DataTemplate = ItemTemplate;
					virtualizedItem.ItemZoomed += OnItemZoomed;
					virtualizedItem.ItemUnzoomed += OnItemUnzoomed;
					((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Add((UIElement)(object)virtualizedItem.RootFrameworkElement);
					_virtualizedItemPool.Add(virtualizedItem);
				}
				if (_state == MediaViewerState.Uninitialized)
				{
					_state = MediaViewerState.Initialized;
				}
				ResetDisplayedElement();
				ResetItemLayout();
			}
		}

		private void ResetDisplayedElement()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			if ((int)HeaderVisibility == 1 && (int)FooterVisibility == 1 && (Items == null || Items.Count == 0))
			{
				ScrollOffset = 0.0;
				UpdateDisplayedElement(null);
			}
			else
			{
				JumpToElement(InitiallyDisplayedIndex);
			}
		}

		private void UpdateDisplayedElementPropertiesBasedOnIndex()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			if (!_displayedElementIndex.HasValue)
			{
				DisplayedElement = DisplayedElementType.None;
				DisplayedItemIndex = -1;
				return;
			}
			int? displayedElementIndex = _displayedElementIndex;
			if (displayedElementIndex.GetValueOrDefault() == 0 && displayedElementIndex.HasValue && (int)HeaderVisibility == 0)
			{
				DisplayedElement = DisplayedElementType.Header;
				DisplayedItemIndex = -1;
				HeaderDisplayed?.Invoke(this, EventArgs.Empty);
				return;
			}
			int? displayedElementIndex2 = _displayedElementIndex;
			int num = GetElementCount() - 1;
			if (displayedElementIndex2.GetValueOrDefault() == num && displayedElementIndex2.HasValue && (int)FooterVisibility == 0)
			{
				DisplayedElement = DisplayedElementType.Footer;
				DisplayedItemIndex = -1;
				FooterDisplayed?.Invoke(this, EventArgs.Empty);
				return;
			}
			int num2 = _displayedElementIndex.Value;
			if ((int)HeaderVisibility == 0)
			{
				num2--;
			}
			DisplayedElement = DisplayedElementType.Item;
			DisplayedItemIndex = num2;
			ItemDisplayed?.Invoke(this, new ItemDisplayedEventArgs(num2));
		}

		private void PlaceHeader()
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			FrameworkElement val = null;
			if (Header != null)
			{
				val = Header;
			}
			else
			{
				val = _headerTemplateInstance;
				if (val != null && Header != null)
				{
					val.DataContext = Header;
				}
			}
			if (_headerOnMediaStrip != null && _headerOnMediaStrip != val)
			{
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Remove((UIElement)(object)_headerOnMediaStrip);
				_headerOnMediaStrip = null;
			}
			if ((int)HeaderVisibility == 0)
			{
				if (val != null)
				{
					((DependencyObject)val).SetValue(Canvas.TopProperty, (object)0.0);
					((DependencyObject)val).SetValue(Canvas.LeftProperty, (object)0.0);
					FrameworkElement val2 = val;
					Size value = _size.Value;
					val2.Height = value.Height;
					FrameworkElement val3 = val;
					Size value2 = _size.Value;
					val3.Width = value2.Width;
					if (_headerOnMediaStrip != val)
					{
						((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Add((UIElement)(object)val);
						_headerOnMediaStrip = val;
					}
				}
			}
			else if (_headerOnMediaStrip != null)
			{
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Remove((UIElement)(object)_headerOnMediaStrip);
				_headerOnMediaStrip = null;
			}
		}

		private void PlaceFooter()
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			FrameworkElement val = null;
			if (Footer != null)
			{
				val = Footer;
			}
			else
			{
				val = _footerTemplateInstance;
				if (val != null && Footer != null)
				{
					val.DataContext = Footer;
				}
			}
			if (_footerOnMediaStrip != null && _footerOnMediaStrip != val)
			{
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Remove((UIElement)(object)_footerOnMediaStrip);
				_footerOnMediaStrip = null;
			}
			if ((int)FooterVisibility == 0)
			{
				if (val != null)
				{
					((DependencyObject)val).SetValue(Canvas.TopProperty, (object)0.0);
					FrameworkElement val2 = val;
					DependencyProperty leftProperty = Canvas.LeftProperty;
					double width = ((FrameworkElement)_mediaStrip).Width;
					Size value = _size.Value;
					((DependencyObject)val2).SetValue(leftProperty, (object)(width - value.Width));
					FrameworkElement val3 = val;
					Size value2 = _size.Value;
					val3.Height = value2.Height;
					FrameworkElement val4 = val;
					Size value3 = _size.Value;
					val4.Width = value3.Width;
					if (_footerOnMediaStrip != val)
					{
						((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Add((UIElement)(object)val);
						_footerOnMediaStrip = val;
					}
				}
			}
			else if (_footerOnMediaStrip != null)
			{
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)_mediaStrip).Children).Remove((UIElement)(object)_footerOnMediaStrip);
				_footerOnMediaStrip = null;
			}
		}

		private void ResetItemLayout()
		{
			if (_state != MediaViewerState.Uninitialized)
			{
				ResetMediaStripGeometry();
				PlaceHeader();
				PlaceFooter();
				UpdateVirtualizedItemPositions();
			}
		}

		private void UpdateVirtualizedItemSizes()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			foreach (VirtualizedItem item in _virtualizedItemPool)
			{
				item.Size = _size.Value;
			}
		}

		private void UpdateVirtualizedItemPositions()
		{
			int? num = null;
			switch (DisplayedElement)
			{
			case DisplayedElementType.Header:
				num = ((Items == null || Items.Count <= 0) ? ((int?)null) : new int?(0));
				break;
			case DisplayedElementType.Item:
				num = DisplayedItemIndex;
				break;
			case DisplayedElementType.Footer:
				num = ((Items == null || Items.Count <= 0) ? ((int?)null) : new int?(Items.Count - 1));
				break;
			}
			if (!num.HasValue)
			{
				return;
			}
			int num2 = 1;
			int num3 = Math.Max(0, num.Value - num2);
			int num4 = Math.Min(Items.Count - 1, num.Value + num2);
			for (int i = num3; i <= num4; i++)
			{
				bool flag = false;
				double num5 = CalculateItemOffset(i);
				VirtualizedItem virtualizedItem = null;
				foreach (VirtualizedItem item in _virtualizedItemPool)
				{
					if (DisplayedItemIndex != -1 && item.RepresentingItemIndex == DisplayedItemIndex)
					{
						_displayedVirtualizedItem = item;
					}
					if (item.RepresentingItemIndex == i)
					{
						flag = true;
						if ((double)((DependencyObject)item.RootFrameworkElement).GetValue(Canvas.LeftProperty) != num5)
						{
							((DependencyObject)item.RootFrameworkElement).SetValue(Canvas.LeftProperty, (object)num5);
						}
						break;
					}
					if (virtualizedItem == null || !item.RepresentingItemIndex.HasValue)
					{
						virtualizedItem = item;
					}
					else if (virtualizedItem != null && virtualizedItem.RepresentingItemIndex.HasValue)
					{
						int num6 = Math.Abs(virtualizedItem.RepresentingItemIndex.Value - num.Value);
						int num7 = Math.Abs(item.RepresentingItemIndex.Value - num.Value);
						if (num7 > num6)
						{
							virtualizedItem = item;
						}
					}
				}
				if (!flag)
				{
					virtualizedItem.DataContext = Items[i];
					virtualizedItem.RepresentingItemIndex = i;
					((DependencyObject)virtualizedItem.RootFrameworkElement).SetValue(Canvas.LeftProperty, (object)num5);
					((UIElement)virtualizedItem.RootFrameworkElement).Visibility = (Visibility)0;
					if (DisplayedItemIndex != -1 && virtualizedItem.RepresentingItemIndex == DisplayedItemIndex)
					{
						_displayedVirtualizedItem = virtualizedItem;
					}
				}
			}
		}

		private void ResetMediaStripGeometry()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			Canvas mediaStrip = _mediaStrip;
			Size value = _size.Value;
			((FrameworkElement)mediaStrip).Height = value.Height;
			int elementCount = GetElementCount();
			if (elementCount > 0)
			{
				Canvas mediaStrip2 = _mediaStrip;
				Size value2 = _size.Value;
				((FrameworkElement)mediaStrip2).Width = value2.Width * (double)elementCount + 18.0 * (double)(elementCount - 1);
			}
			else
			{
				((FrameworkElement)_mediaStrip).Width = 0.0;
			}
			_dragState.MinDraggingBoundary = 150.0;
			DragState dragState = _dragState;
			double width = ((FrameworkElement)_mediaStrip).Width;
			Size value3 = _size.Value;
			dragState.MaxDraggingBoundary = -1.0 * (width - value3.Width + 150.0);
		}

		public bool JumpToHeader()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)HeaderVisibility == 1)
			{
				return false;
			}
			JumpToFirstElement();
			return true;
		}

		public bool JumpToFooter()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)FooterVisibility == 1)
			{
				return false;
			}
			JumpToLastElement();
			return true;
		}

		public bool JumpToItem(int itemIndex)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (Items == null || itemIndex >= Items.Count || itemIndex < 0)
			{
				return false;
			}
			JumpToElement(((int)HeaderVisibility == 0) ? (itemIndex + 1) : itemIndex);
			return true;
		}

		public bool ScrollLeftOneElement()
		{
			if (!_displayedElementIndex.HasValue || _displayedElementIndex == 0)
			{
				return false;
			}
			AnimateToElement(_displayedElementIndex.Value - 1, new TimeSpan(0, 0, 0, 0, 800));
			return true;
		}

		public object FindNameInHeader(string name)
		{
			if (_headerOnMediaStrip == null)
			{
				return null;
			}
			return _headerOnMediaStrip.FindName(name);
		}

		public object FindNameInFooter(string name)
		{
			if (_footerOnMediaStrip == null)
			{
				return null;
			}
			return _footerOnMediaStrip.FindName(name);
		}

		private void OnMediaViewerSizeChanged(object sender, SizeChangedEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_size = e.NewSize;
			if (_state == MediaViewerState.Uninitialized)
			{
				InitializeVirtualizationIfReady();
				return;
			}
			UpdateVirtualizedItemSizes();
			ResetItemLayout();
			if (_displayedElementIndex.HasValue)
			{
				ScrollOffset = -1.0 * CalculateElementOffset(_displayedElementIndex.Value);
			}
			else
			{
				ScrollOffset = 0.0;
			}
		}

		public override void OnApplyTemplate()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected Obj, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected Obj, but got Unknown
			((FrameworkElement)this).OnApplyTemplate();
			_mediaStrip = (Canvas)((Control)this).GetTemplateChild("MediaStrip");
			_mediaStripCompositeTransform = (CompositeTransform)((Control)this).GetTemplateChild("MediaStripCompositeTransform");
			InitializeVirtualizationIfReady();
		}

		protected override void OnDoubleTap(GestureEventArgs e)
		{
			((Control)this).OnDoubleTap(e);
			if (DisplayedElement == DisplayedElementType.Item)
			{
				_displayedVirtualizedItem.DoubleTapped();
			}
		}

		protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
		{
			((Control)this).OnManipulationStarted(e);
			if (_state == MediaViewerState.InertiaAnimating)
			{
				CompleteDragInertiaAnimation();
			}
		}

		protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnManipulationDelta(e);
			if (e.PinchManipulation == null)
			{
				if (!IsZoomedInToItem() && _state == MediaViewerState.Initialized && DragEnabled && GetElementCount() > 0)
				{
					_state = MediaViewerState.Dragging;
					DragStartedEventHandler();
				}
				if (_state == MediaViewerState.Dragging || _state == MediaViewerState.DraggingAndSquishing)
				{
					Point translation = e.DeltaManipulation.Translation;
					double x = translation.X;
					Point translation2 = e.DeltaManipulation.Translation;
					DragDeltaEventHandler(x, translation2.Y);
				}
			}
		}

		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnManipulationCompleted(e);
			if (_state == MediaViewerState.Dragging || _state == MediaViewerState.DraggingAndSquishing)
			{
				if (!_dragState.GotDragDelta)
				{
					Point translation = e.TotalManipulation.Translation;
					double x = translation.X;
					Point translation2 = e.TotalManipulation.Translation;
					ProcessDragDelta(x, translation2.Y);
				}
				DragCompletedEventHandler();
			}
		}

		private void OnItemZoomed(int? representingItemIndex)
		{
			if (_displayedElementIndex.HasValue && _displayedElementIndex == representingItemIndex)
			{
				ItemZoomed?.Invoke(this, EventArgs.Empty);
			}
		}

		private void OnItemUnzoomed(int? representingItemIndex)
		{
			if (_displayedElementIndex.HasValue && _displayedElementIndex == representingItemIndex)
			{
				ItemUnzoomed?.Invoke(this, EventArgs.Empty);
			}
		}

		private void DragStartedEventHandler()
		{
			int elementCount = GetElementCount();
			_state = MediaViewerState.Dragging;
			_dragState.LastDragUpdateTime = DateTime.Now;
			_dragState.DragStartingMediaStripOffset = ScrollOffset;
			_dragState.NetDragDistanceSincleLastDragStagnation = 0.0;
			_dragState.IsDraggingFirstElement = _displayedElementIndex == 0;
			_dragState.IsDraggingLastElement = _displayedElementIndex == elementCount - 1;
			_dragState.GotDragDelta = false;
		}

		private void DragDeltaEventHandler(double horizontalChange, double verticalChange)
		{
			_dragState.GotDragDelta = true;
			ProcessDragDelta(horizontalChange, verticalChange);
		}

		private void DragCompletedEventHandler()
		{
			switch (_state)
			{
			case MediaViewerState.Dragging:
				StartDragInertiaAnimation();
				break;
			case MediaViewerState.DraggingAndSquishing:
				StartUndoSquishAnimation();
				break;
			}
		}

		private void ProcessDragDelta(double horizontalChange, double verticalChange)
		{
			DateTime now = DateTime.Now;
			double totalMilliseconds = (now - _dragState.LastDragUpdateTime).TotalMilliseconds;
			_dragState.LastDragUpdateTime = now;
			if (totalMilliseconds > 300.0)
			{
				_dragState.NetDragDistanceSincleLastDragStagnation = 0.0;
			}
			double num = 0.0;
			_dragState.LastDragDistanceDelta = horizontalChange;
			num = ScrollOffset + horizontalChange;
			_dragState.NetDragDistanceSincleLastDragStagnation += horizontalChange;
			num = Math.Min(num, _dragState.MinDraggingBoundary);
			num = Math.Max(num, _dragState.MaxDraggingBoundary);
			if (_dragState.IsDraggingFirstElement || _dragState.IsDraggingLastElement)
			{
				HandleSquishingWhileDragging(num);
			}
			ScrollOffset = num;
		}

		private void ConstructDragInertiaAnimation(double animationEndingValue, TimeSpan animationDuration)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected Obj, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected Obj, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected Obj, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			_dragInertiaAnimation = new Storyboard();
			_dragInertiaAnimationTranslation = new DoubleAnimation();
			Storyboard.SetTarget((Timeline)(object)_dragInertiaAnimationTranslation, (DependencyObject)(object)_mediaStripCompositeTransform);
			Storyboard.SetTargetProperty((Timeline)(object)_dragInertiaAnimationTranslation, new PropertyPath((object)CompositeTransform.TranslateXProperty));
			QuadraticEase val = new QuadraticEase();
			((EasingFunctionBase)val).EasingMode = (EasingMode)0;
			_dragInertiaAnimationTranslation.From = ScrollOffset;
			_dragInertiaAnimationTranslation.To = animationEndingValue;
			((Timeline)_dragInertiaAnimationTranslation).Duration = Duration.op_Implicit(animationDuration);
			_dragInertiaAnimationTranslation.EasingFunction = (IEasingFunction)(object)val;
			((PresentationFrameworkCollection<Timeline>)(object)_dragInertiaAnimation.Children).Add((Timeline)(object)_dragInertiaAnimationTranslation);
			((Timeline)_dragInertiaAnimation).Completed += DragInertiaAnimationComplete;
			((Timeline)_dragInertiaAnimation).FillBehavior = (FillBehavior)0;
		}

		private int CalculateDragInertiaAnimationEndingValue()
		{
			bool flag = Math.Abs(_dragState.NetDragDistanceSincleLastDragStagnation) <= 15.0;
			int num = 0;
			if (!flag)
			{
				num = -1 * Math.Sign(_dragState.LastDragDistanceDelta);
				int? displayedElementIndex = _displayedElementIndex;
				if (displayedElementIndex.GetValueOrDefault() == 0 && displayedElementIndex.HasValue && num == -1)
				{
					num = 0;
				}
				else
				{
					int? displayedElementIndex2 = _displayedElementIndex;
					int num2 = GetElementCount() - 1;
					if (displayedElementIndex2.GetValueOrDefault() == num2 && displayedElementIndex2.HasValue && num == 1)
					{
						num = 0;
					}
				}
			}
			return num;
		}

		private TimeSpan CalculateDragInertiaAnimationDuration(TimeSpan lastDragTimeDelta)
		{
			double val = Math.Abs(_dragState.LastDragDistanceDelta / lastDragTimeDelta.TotalMilliseconds);
			val = Math.Min(5.0, val);
			val = Math.Max(0.0, val);
			double num = (val - 0.0) / 5.0;
			int val2 = (int)(700.0 * (1.0 - num) + 100.0);
			val2 = Math.Min(800, val2);
			val2 = Math.Max(100, val2);
			return new TimeSpan(0, 0, 0, 0, val2);
		}

		private void StartDragInertiaAnimation()
		{
			TimeSpan lastDragTimeDelta = DateTime.Now - _dragState.LastDragUpdateTime;
			int num = CalculateDragInertiaAnimationEndingValue();
			TimeSpan animationDuration = CalculateDragInertiaAnimationDuration(lastDragTimeDelta);
			AnimateToElement(_displayedElementIndex.Value + num, animationDuration);
			_state = MediaViewerState.InertiaAnimating;
		}

		private void AnimateToElement(int elementIndex, TimeSpan animationDuration)
		{
			double animationEndingValue = -1.0 * CalculateElementOffset(elementIndex);
			ConstructDragInertiaAnimation(animationEndingValue, animationDuration);
			_state = MediaViewerState.InertiaAnimating;
			_dragInertiaAnimation.Begin();
			_dragState.NewDisplayedElementIndex = elementIndex;
		}

		private void DragInertiaAnimationComplete(object sender, EventArgs e)
		{
			CompleteDragInertiaAnimation();
		}

		private void CompleteDragInertiaAnimation()
		{
			if (_dragInertiaAnimation != null)
			{
				if (_state == MediaViewerState.InertiaAnimating)
				{
					_state = MediaViewerState.Initialized;
				}
				ScrollOffset = _dragInertiaAnimationTranslation.To.Value;
				_dragInertiaAnimation.Stop();
				_dragInertiaAnimation = null;
				_dragInertiaAnimationTranslation = null;
				UpdateDisplayedElement(_dragState.NewDisplayedElementIndex);
			}
		}

		private void HandleSquishingWhileDragging(double newTranslation)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			double num = -1.0 * CalculateElementOffset(GetElementCount() - 1);
			double num2 = 0.0;
			if (newTranslation > 0.0)
			{
				num2 = newTranslation;
				_dragState.UnsquishTranslationAnimationTarget = 0.0;
				((UIElement)_mediaStrip).RenderTransformOrigin = new Point(0.0, 0.0);
			}
			else if (newTranslation < num)
			{
				num2 = num - newTranslation;
				_dragState.UnsquishTranslationAnimationTarget = num;
				((UIElement)_mediaStrip).RenderTransformOrigin = new Point(1.0, 0.0);
			}
			double num3 = 1.0 - num2 / 150.0 * 0.09999999999999998;
			_mediaStripCompositeTransform.ScaleX = num3;
			_state = ((num3 == 1.0) ? MediaViewerState.Dragging : MediaViewerState.DraggingAndSquishing);
		}

		private void StartUndoSquishAnimation()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected Obj, but got Unknown
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected Obj, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected Obj, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected Obj, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			_unsquishAnimation = new Storyboard();
			DoubleAnimation val = new DoubleAnimation();
			_unsquishAnimationTranslation = new DoubleAnimation();
			Storyboard.SetTarget((Timeline)(object)val, (DependencyObject)(object)_mediaStripCompositeTransform);
			Storyboard.SetTarget((Timeline)(object)_unsquishAnimationTranslation, (DependencyObject)(object)_mediaStripCompositeTransform);
			Storyboard.SetTargetProperty((Timeline)(object)val, new PropertyPath((object)CompositeTransform.ScaleXProperty));
			Storyboard.SetTargetProperty((Timeline)(object)_unsquishAnimationTranslation, new PropertyPath((object)CompositeTransform.TranslateXProperty));
			val.From = _mediaStripCompositeTransform.ScaleX;
			_unsquishAnimationTranslation.From = _mediaStripCompositeTransform.TranslateX;
			val.To = 1.0;
			_unsquishAnimationTranslation.To = _dragState.UnsquishTranslationAnimationTarget;
			((Timeline)val).Duration = Duration.op_Implicit(new TimeSpan(0, 0, 0, 0, 100));
			((Timeline)_unsquishAnimationTranslation).Duration = ((Timeline)val).Duration;
			((PresentationFrameworkCollection<Timeline>)(object)_unsquishAnimation.Children).Add((Timeline)(object)val);
			((PresentationFrameworkCollection<Timeline>)(object)_unsquishAnimation.Children).Add((Timeline)(object)_unsquishAnimationTranslation);
			((Timeline)_unsquishAnimation).FillBehavior = (FillBehavior)1;
			((Timeline)_unsquishAnimation).Completed += UnsquishAnimationComplete;
			_state = MediaViewerState.UnsquishAnimating;
			_unsquishAnimation.Begin();
			_mediaStripCompositeTransform.ScaleX = val.To.Value;
			_mediaStripCompositeTransform.TranslateX = _unsquishAnimationTranslation.To.Value;
		}

		private void UnsquishAnimationComplete(object sender, EventArgs e)
		{
			if (_state == MediaViewerState.UnsquishAnimating)
			{
				_state = MediaViewerState.Initialized;
			}
			ScrollOffset = _unsquishAnimationTranslation.To.Value;
			_unsquishAnimation.Stop();
			_unsquishAnimation = null;
			_unsquishAnimationTranslation = null;
		}

		private int GetElementCount()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			if ((int)HeaderVisibility == 0)
			{
				num++;
			}
			if (Items != null)
			{
				num += Items.Count;
			}
			if ((int)FooterVisibility == 0)
			{
				num++;
			}
			return num;
		}

		private void UpdateDisplayedElement(int? newElementIndex)
		{
			_displayedElementIndex = newElementIndex;
			UpdateDisplayedElementPropertiesBasedOnIndex();
			UpdateVirtualizedItemPositions();
		}

		private double RoundOffsetDownToElementStart(double offset)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Size value = _size.Value;
			int num = (int)(offset / (value.Width + 18.0));
			double num2 = num;
			Size value2 = _size.Value;
			return num2 * (value2.Width + 18.0);
		}

		private double CalculateElementOffset(int elementIndex)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			double num = elementIndex;
			Size value = _size.Value;
			return num * (value.Width + 18.0);
		}

		private double CalculateItemOffset(int itemIndex)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			double num = 0.0;
			if ((int)HeaderVisibility == 0)
			{
				double num2 = num;
				Size value = _size.Value;
				num = num2 + (value.Width + 18.0);
			}
			double num3 = num;
			double num4 = itemIndex;
			Size value2 = _size.Value;
			return num3 + num4 * (value2.Width + 18.0);
		}

		private bool IsZoomedInToItem()
		{
			if (_displayedVirtualizedItem != null)
			{
				return _displayedVirtualizedItem.IsZoomedIn;
			}
			return false;
		}

		private void JumpToFirstElement()
		{
			JumpToElement(0);
		}

		private void JumpToLastElement()
		{
			int elementCount = GetElementCount();
			if (elementCount > 0)
			{
				JumpToElement(elementCount - 1);
			}
		}

		private void JumpToElement(int elementIndex)
		{
			if (DisplayedElement == DisplayedElementType.Item && _displayedVirtualizedItem.IsZoomedIn)
			{
				_displayedVirtualizedItem.ZoomAllTheWayOut();
			}
			ScrollOffset = -1.0 * CalculateElementOffset(elementIndex);
			UpdateDisplayedElement(elementIndex);
		}

		private void InitializeOrReset()
		{
			if (_state == MediaViewerState.Uninitialized)
			{
				InitializeVirtualizationIfReady();
				return;
			}
			foreach (VirtualizedItem item in _virtualizedItemPool)
			{
				item.DataContext = null;
				item.RepresentingItemIndex = null;
			}
			ResetDisplayedElement();
			ResetItemLayout();
		}

		static MediaViewer()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected Obj, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected Obj, but got Unknown
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected Obj, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected Obj, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected Obj, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected Obj, but got Unknown
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected Obj, but got Unknown
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Expected Obj, but got Unknown
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Expected Obj, but got Unknown
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected Obj, but got Unknown
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Expected Obj, but got Unknown
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Expected Obj, but got Unknown
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Expected Obj, but got Unknown
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Expected Obj, but got Unknown
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected Obj, but got Unknown
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Expected Obj, but got Unknown
		}
	}
}

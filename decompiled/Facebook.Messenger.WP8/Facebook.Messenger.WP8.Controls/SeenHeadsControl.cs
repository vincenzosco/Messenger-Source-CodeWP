using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls
{
	public class SeenHeadsControl : UserControl
	{
		private const int SeenHeadImageWidth = 20;

		private static readonly Uri PlaceholderUri = new Uri("/Assets/Messages/Silhouette-small.png", UriKind.Relative);

		private FacebookContext _context;

		private int _maxSeenHeads = 5;

		public static readonly DependencyProperty MaxSeenHeadsProperty = DependencyProperty.Register("MaxSeenHeads", typeof(int), typeof(SeenHeadsControl), new PropertyMetadata((PropertyChangedCallback)OnMaxHeadsChanged));

		private List<string> _allReadIds = new List<string>();

		private Queue<KeyValuePair<string, Image>> _displayedQueue = new Queue<KeyValuePair<string, Image>>();

		private Queue<string> _truncatedQueue = new Queue<string>();

		private string _lastAddedSeenHead;

		internal StackPanel SeenHeadsPanel;

		internal Border SeenHeadOverflowBorder;

		internal TextBlock SeenHeadOverflowText;

		private bool _contentLoaded;

		private FacebookContext Context
		{
			get
			{
				return _context ?? (_context = Application.Current.Resources[(object)"FacebookContext"] as FacebookContext);
			}
		}

		public int MaxSeenHeads
		{
			get
			{
				return _maxSeenHeads;
			}
			set
			{
				if (_maxSeenHeads != value)
				{
					_maxSeenHeads = value;
					UpdateSeenHeads(_allReadIds);
				}
			}
		}

		private static void OnMaxHeadsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			SeenHeadsControl seenHeadsControl = o as SeenHeadsControl;
			if (seenHeadsControl != null)
			{
				int? num = e.NewValue as int?;
				if (num.HasValue)
				{
					seenHeadsControl.MaxSeenHeads = num.Value;
				}
			}
		}

		public SeenHeadsControl()
		{
			InitializeComponent();
		}

		public void UpdateSeenHeads(List<string> readIds)
		{
			List<string> allReadIds = _allReadIds;
			_allReadIds = new List<string>(readIds);
			List<string> readIdsToRemove = new List<string>(allReadIds.Except(_allReadIds));
			List<string> readIdsToAdd = new List<string>(_allReadIds.Except(allReadIds));
			RemoveSeenHeads(readIdsToRemove);
			AddSeenHeads(readIdsToAdd);
			UpdateSeenHeadOverflow();
		}

		private void RemoveSeenHeads(List<string> readIdsToRemove)
		{
			Queue<KeyValuePair<string, Image>> queue = new Queue<KeyValuePair<string, Image>>();
			Queue<string> queue2 = new Queue<string>();
			while (_displayedQueue.Count > 0)
			{
				KeyValuePair<string, Image> item = _displayedQueue.Dequeue();
				if (readIdsToRemove.Contains(item.Key))
				{
					((PresentationFrameworkCollection<UIElement>)(object)((Panel)SeenHeadsPanel).Children).Remove((UIElement)(object)item.Value);
					readIdsToRemove.Remove(item.Key);
				}
				else
				{
					queue.Enqueue(item);
				}
			}
			_displayedQueue = queue;
			while (_truncatedQueue.Count > 0)
			{
				string item2 = _truncatedQueue.Dequeue();
				if (readIdsToRemove.Contains(item2))
				{
					readIdsToRemove.Remove(item2);
				}
				else
				{
					queue2.Enqueue(item2);
				}
			}
			_truncatedQueue = queue2;
		}

		private void AddSeenHeads(List<string> readIdsToAdd)
		{
			foreach (string item in readIdsToAdd)
			{
				if (_displayedQueue.Count < MaxSeenHeads)
				{
					AddToUi(item);
				}
				else
				{
					_truncatedQueue.Enqueue(item);
				}
			}
		}

		private void UpdateSeenHeadOverflow()
		{
			while (_truncatedQueue.Any() && _displayedQueue.Count < MaxSeenHeads)
			{
				AddToUi(_truncatedQueue.Dequeue());
			}
			if (_displayedQueue.Count == MaxSeenHeads + 1 && _truncatedQueue.Count > 0)
			{
				RemoveSeenHeads(new List<string> { _lastAddedSeenHead });
				_truncatedQueue.Enqueue(_lastAddedSeenHead);
			}
			if (_truncatedQueue.Any())
			{
				if (_truncatedQueue.Count == 1)
				{
					AddToUi(_truncatedQueue.Dequeue());
					((UIElement)SeenHeadOverflowBorder).Visibility = (Visibility)1;
				}
				else if (_truncatedQueue.Count > 9)
				{
					SeenHeadOverflowText.Text = "9+";
					((UIElement)SeenHeadOverflowBorder).Visibility = (Visibility)0;
				}
				else
				{
					SeenHeadOverflowText.Text = "+" + Enumerable.Count(_truncatedQueue);
					((UIElement)SeenHeadOverflowBorder).Visibility = (Visibility)0;
				}
			}
			else
			{
				((UIElement)SeenHeadOverflowBorder).Visibility = (Visibility)1;
			}
		}

		private void AddToUi(string readId)
		{
			Image val = CreateSeenHead(Context, readId);
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)SeenHeadsPanel).Children).Insert(1, (UIElement)(object)val);
			_displayedQueue.Enqueue(new KeyValuePair<string, Image>(readId, val));
			_lastAddedSeenHead = readId;
		}

		private Image CreateSeenHead(FacebookContext context, string userId)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Image val = new Image();
			((FrameworkElement)val).Height = 20.0;
			((FrameworkElement)val).Width = 20.0;
			((FrameworkElement)val).Margin = new Thickness(1.0, 0.0, 1.0, 0.0);
			Image val2 = val;
			Uri result;
			Uri.TryCreate(ProfilePhotoConverter.CreatePhotoUrl(context, userId, (int)((FrameworkElement)val2).Width), UriKind.Absolute, out result);
			((DependencyObject)val2).SetValue(ImageCacheExtensions.ImageUriProperty, (object)result);
			((DependencyObject)val2).SetValue(RoundContentExtension.RadiusProperty, (object)(((FrameworkElement)val2).Width / 2.0));
			ImageCacheExtensions.SetPlaceholderUri((FrameworkElement)(object)val2, PlaceholderUri);
			return val2;
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
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/SeenHeadsControl.xaml", UriKind.Relative));
				SeenHeadsPanel = (StackPanel)((FrameworkElement)this).FindName("SeenHeadsPanel");
				SeenHeadOverflowBorder = (Border)((FrameworkElement)this).FindName("SeenHeadOverflowBorder");
				SeenHeadOverflowText = (TextBlock)((FrameworkElement)this).FindName("SeenHeadOverflowText");
			}
		}

		static SeenHeadsControl()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected Obj, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected Obj, but got Unknown
		}
	}
}

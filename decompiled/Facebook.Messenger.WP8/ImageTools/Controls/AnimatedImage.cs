using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageTools.Controls
{
	[TemplatePart(Name = "Image", Type = typeof(Image))]
	public class AnimatedImage : Control
	{
		private class DispatcherTimerAccurate
		{
			private Timer _timer;

			private bool _running;

			private TimeSpan _interval;

			public TimeSpan Interval
			{
				get
				{
					return _interval;
				}
				set
				{
					if (_interval != value)
					{
						_interval = value;
						if (_running)
						{
							_timer.Change(value, value);
						}
					}
				}
			}

			public event EventHandler Tick;

			public DispatcherTimerAccurate()
			{
				_timer = new Timer(_tick);
			}

			private void _tick(object state)
			{
				if (_running)
				{
					((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(() =>
					{
						Tick(this, new EventArgs());
					}));
				}
			}

			public void Start()
			{
				_running = true;
				_timer.Change(_interval, _interval);
			}

			public void Stop()
			{
				_running = false;
				_timer.Change(-1, -1);
			}
		}

		public const string ImagePart = "Image";

		private Image _image;

		private DispatcherTimerAccurate _animationTimer;

		private List<KeyValuePair<ImageBase, ImageSource>> _frames;

		private int _animationFrameIndex;

		private bool _isLoadingImage;

		private int _currentRepeatsRemaining;

		public static readonly DependencyProperty RepeatCountProperty = DependencyProperty.Register("RepeatCount", typeof(int), typeof(AnimatedImage), new PropertyMetadata((object)(-1)));

		public static readonly DependencyProperty PauseProperty = DependencyProperty.Register("Pause", typeof(bool), typeof(AnimatedImage), new PropertyMetadata((object)false));

		public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(IImageFilter), typeof(AnimatedImage), new PropertyMetadata((PropertyChangedCallback)OnFilterPropertyChanged));

		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(AnimatedImage), new PropertyMetadata((object)(Stretch)2));

		public static readonly DependencyProperty AutoSizeProperty = DependencyProperty.Register("AutoSize", typeof(bool), typeof(AnimatedImage), (PropertyMetadata)null);

		public static readonly DependencyProperty AnimationModeProperty = DependencyProperty.Register("AnimationMode", typeof(AnimationMode), typeof(AnimatedImage), new PropertyMetadata((object)AnimationMode.Repeat));

		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ExtendedImage), typeof(AnimatedImage), new PropertyMetadata((PropertyChangedCallback)OnSourcePropertyChanged));

		public int RepeatCount
		{
			get
			{
				return (int)((DependencyObject)this).GetValue(RepeatCountProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(RepeatCountProperty, (object)value);
			}
		}

		public bool Pause
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(PauseProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(PauseProperty, (object)value);
			}
		}

		public IImageFilter Filter
		{
			get
			{
				return (IImageFilter)((DependencyObject)this).GetValue(FilterProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FilterProperty, (object)value);
			}
		}

		public Stretch Stretch
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Stretch)((DependencyObject)this).GetValue(StretchProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((DependencyObject)this).SetValue(StretchProperty, (object)value);
			}
		}

		public bool AutoSize
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(AutoSizeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(AutoSizeProperty, (object)value);
			}
		}

		public AnimationMode AnimationMode
		{
			get
			{
				return (AnimationMode)((DependencyObject)this).GetValue(AnimationModeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(AnimationModeProperty, (object)value);
			}
		}

		public ExtendedImage Source
		{
			get
			{
				return (ExtendedImage)((DependencyObject)this).GetValue(SourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(SourceProperty, (object)value);
			}
		}

		public event EventHandler LoadingCompleted;

		public event EventHandler<UnhandledExceptionEventArgs> LoadingFailed;

		protected virtual void OnLoadingCompleted(EventArgs e)
		{
			LoadingCompleted?.Invoke(this, e);
		}

		protected virtual void OnLoadingFailed(UnhandledExceptionEventArgs e)
		{
			LoadingFailed?.Invoke(this, e);
		}

		private static void OnFilterPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			(o as AnimatedImage)?.OnFilterPropertyChanged();
		}

		private void OnFilterPropertyChanged()
		{
			if (Source != null && Source.IsFilled)
			{
				LoadImage(Source);
			}
		}

		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as AnimatedImage)?.OnSourceChanged();
		}

		protected virtual void OnSourceChanged()
		{
			_frames.Clear();
			if (_image != null && Source != null)
			{
				if (!Source.IsFilled || Source.IsLoading)
				{
					Source.LoadingCompleted += image_LoadingCompleted;
					Source.LoadingFailed += image_LoadingFailed;
				}
				else
				{
					LoadImage(Source);
				}
			}
		}

		public AnimatedImage()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected Obj, but got Unknown
			RoutedEventHandler val = null;
			_frames = new List<KeyValuePair<ImageBase, ImageSource>>();
			((Control)this)._002Ector();
			((Control)this).DefaultStyleKey = typeof(AnimatedImage);
			_animationTimer = new DispatcherTimerAccurate();
			_animationTimer.Tick += timer_Tick;
			if (val == null)
			{
				val = (object sender, RoutedEventArgs e) =>
				{
					Stop();
				};
			}
			((FrameworkElement)this).Unloaded += val;
		}

		public override void OnApplyTemplate()
		{
			((FrameworkElement)this).OnApplyTemplate();
			BindXaml();
			OnSourceChanged();
		}

		private void BindXaml()
		{
			DependencyObject templateChild = ((Control)this).GetTemplateChild("Image");
			_image = (Image)(object)((templateChild is Image) ? templateChild : null);
		}

		private void LoadImage(ExtendedImage image)
		{
			if (_isLoadingImage)
			{
				return;
			}
			_isLoadingImage = true;
			if (Filter != null)
			{
				IImageFilter filter = Filter;
				ThreadPool.QueueUserWorkItem((object x) =>
				{
					ExtendedImage filteredImage = ExtendedImage.ApplyFilters(image, filter);
					((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
					{
						AssignImage(filteredImage);
					}));
				});
			}
			else
			{
				AssignImage(image);
			}
		}

		private void AssignImage(ExtendedImage image)
		{
			_isLoadingImage = false;
			if (!image.IsFilled)
			{
				return;
			}
			_frames.Clear();
			BitmapSource bitmapSource = image.BitmapSource;
			if (image.IsAnimated && AnimationMode != AnimationMode.None)
			{
				Stop();
				List<ImageBase> list = new List<ImageBase>();
				list.Add(image);
				list.AddRange(image.Frames.OfType<ImageBase>());
				foreach (ImageBase item in list)
				{
					if (item != null && item.IsFilled)
					{
						_frames.Add(new KeyValuePair<ImageBase, ImageSource>(item, (ImageSource)(object)item.BitmapSource));
					}
				}
				AnimateImage();
				Start();
			}
			else if (_image != null)
			{
				_image.Source = (ImageSource)(object)bitmapSource;
			}
		}

		private void image_LoadingCompleted(object sender, EventArgs e)
		{
			ExtendedImage image = sender as ExtendedImage;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				LoadImage(image);
				image.LoadingCompleted -= image_LoadingCompleted;
				image.LoadingFailed -= image_LoadingFailed;
				OnLoadingCompleted(e);
			}));
		}

		private void image_LoadingFailed(object sender, UnhandledExceptionEventArgs e)
		{
			ExtendedImage image = sender as ExtendedImage;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				image.LoadingCompleted -= image_LoadingCompleted;
				image.LoadingFailed -= image_LoadingFailed;
				OnLoadingFailed(e);
			}));
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (!Pause)
			{
				AnimateImage();
			}
		}

		private void AnimateImage()
		{
			if (_animationFrameIndex >= _frames.Count)
			{
				return;
			}
			KeyValuePair<ImageBase, ImageSource> keyValuePair = _frames[_animationFrameIndex];
			if (keyValuePair.Key == null)
			{
				return;
			}
			if (_image != null)
			{
				_image.Source = keyValuePair.Value;
			}
			_animationTimer.Interval = new TimeSpan(0, 0, 0, 0, keyValuePair.Key.DelayTime * 10);
			_animationFrameIndex++;
			if (_animationFrameIndex != _frames.Count)
			{
				return;
			}
			_animationFrameIndex = 0;
			if (_currentRepeatsRemaining > 0 && AnimationMode == AnimationMode.Repeat)
			{
				_currentRepeatsRemaining--;
			}
			if (AnimationMode == AnimationMode.PlayOnce || (AnimationMode == AnimationMode.Repeat && _currentRepeatsRemaining == 0))
			{
				Stop();
				if (_image != null)
				{
					_image.Source = _frames[_animationFrameIndex].Value;
				}
			}
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (Source != null && Source.IsFilled && AutoSize)
			{
				return new Size((double)Source.PixelWidth, (double)Source.PixelHeight);
			}
			return ((FrameworkElement)this).MeasureOverride(availableSize);
		}

		public void Start()
		{
			Pause = false;
			_currentRepeatsRemaining = RepeatCount;
			_animationTimer.Start();
		}

		public void Stop()
		{
			_animationFrameIndex = 0;
			_animationTimer.Stop();
		}

		static AnimatedImage()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected Obj, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected Obj, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected Obj, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected Obj, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected Obj, but got Unknown
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Expected Obj, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected Obj, but got Unknown
		}
	}
}

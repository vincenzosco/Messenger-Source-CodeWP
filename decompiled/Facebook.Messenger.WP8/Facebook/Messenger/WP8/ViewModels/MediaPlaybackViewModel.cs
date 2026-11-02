using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Facebook.Messenger.WP8.ViewModels
{
	public abstract class MediaPlaybackViewModel
	{
		public enum State
		{
			Loading,
			Playing,
			Paused,
			Stopped
		}

		public interface PlaybackListener
		{
			void StateChanged(State state, TimeSpan position);
		}

		private readonly DispatcherTimer _mediaTimer;

		private PlaybackListener _listener;

		private MediaElement _media;

		public MediaElement Media
		{
			get
			{
				return _media;
			}
			set
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Expected Obj, but got Unknown
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Expected Obj, but got Unknown
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Expected Obj, but got Unknown
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Expected Obj, but got Unknown
				if (_media != null)
				{
					_media.CurrentStateChanged -= MediaElementOnCurrentStateChanged;
					_media.MediaEnded -= PerformReset;
					_media.MediaFailed -= PerformReset;
				}
				_media = value;
				if (_media != null)
				{
					_media.CurrentStateChanged += MediaElementOnCurrentStateChanged;
					_media.MediaEnded += PerformReset;
					_media.MediaFailed += PerformReset;
				}
			}
		}

		protected MediaPlaybackViewModel()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected Obj, but got Unknown
			DispatcherTimer val = new DispatcherTimer();
			val.Interval = TimeSpan.FromSeconds(0.1);
			_mediaTimer = val;
			DispatcherTimer mediaTimer = _mediaTimer;
			EventHandler eventHandler = (object sender, EventArgs args) =>
			{
				NotifyState();
			};
			mediaTimer.Tick += eventHandler;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void ClearListener(PlaybackListener listener)
		{
			if (_listener == listener)
			{
				_listener = null;
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		protected void SetListener(PlaybackListener listener)
		{
			_listener = listener;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		protected void NotifyState(State? overriddenState = null)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected I4, but got Unknown
			if (_listener == null)
			{
				return;
			}
			if (overriddenState.HasValue)
			{
				_listener.StateChanged(overriddenState.Value, TimeSpan.Zero);
				return;
			}
			MediaElementState currentState = Media.CurrentState;
			switch (currentState - 1)
			{
			case 0:
			case 1:
			case 5:
				_listener.StateChanged(State.Loading, TimeSpan.Zero);
				break;
			case 2:
				_listener.StateChanged(State.Playing, Media.Position);
				break;
			case 3:
				_listener.StateChanged(State.Paused, Media.Position);
				break;
			default:
				_listener.StateChanged(State.Stopped, TimeSpan.Zero);
				break;
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		protected virtual void MediaElementOnCurrentStateChanged(object sender, RoutedEventArgs routedEventArgs)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected I4, but got Unknown
			NotifyState();
			MediaElementState currentState = Media.CurrentState;
			switch (currentState - 3)
			{
			case 0:
				_mediaTimer.Start();
				break;
			case 2:
				Reset();
				break;
			case 1:
				break;
			}
		}

		public void ToggleCurrentMedia()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Invalid comparison between Unknown and I4
			if (Media != null && !(Media.Source == null))
			{
				if ((int)Media.CurrentState == 3)
				{
					Media.Pause();
				}
				else
				{
					Media.Play();
				}
			}
		}

		private void PerformReset(object sender, EventArgs e)
		{
			Reset();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public virtual void Reset()
		{
			_mediaTimer.Stop();
			if (Media != null)
			{
				Media.Stop();
			}
			NotifyState(State.Stopped);
		}

		public void RemoveMediaFromVisualTree()
		{
			if (Media != null)
			{
				DependencyObject parent = ((FrameworkElement)Media).Parent;
				Panel val = (Panel)(object)((parent is Panel) ? parent : null);
				if (val != null)
				{
					((PresentationFrameworkCollection<UIElement>)(object)val.Children).Remove((UIElement)(object)Media);
				}
			}
		}
	}
}

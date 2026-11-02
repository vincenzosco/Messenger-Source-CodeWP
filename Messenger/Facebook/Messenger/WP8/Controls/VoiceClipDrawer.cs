using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Facebook.Messenger.Extensions;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class VoiceClipDrawer : ComposerDrawer
	{
		private class AnimationStateManager
		{
			private readonly VoiceClipDrawer _container;

			private bool _durationLimitReached;

			public AnimationStateManager(VoiceClipDrawer parent)
			{
				_container = parent;
			}

			public void Reset()
			{
				_durationLimitReached = false;
				_container.IdleStateAnimation.Begin();
				_container.NewUserInstructionAnimation.Stop();
				_container.RecordingRedTimerText.Stop();
				_container.RecordingStateAnimation.Stop();
				_container.CancelStateAnimation.Stop();
				_container.MoveTimerUpAnimation.Stop();
				_container.MoveTimerDownAnimation.Stop();
				_container.TooShortInstructionAnimation.Stop();
				_container.RecordingTimelimitReachedAnimation.Stop();
				_container.UpdateTimer();
			}

			public void ShowRecordingAnimation()
			{
				_container.RecordingRedTimerText.Begin();
				_container.RecordingStateAnimation.Begin();
				_container.TooShortInstructionAnimation.Stop();
				_container.IdleStateAnimation.Stop();
			}

			public void ShowNewUserInstruction()
			{
				_container.NewUserInstructionAnimation.Begin();
			}

			public void OnDurationLimitReached()
			{
				_durationLimitReached = true;
				UpdateTimelimitAnimations();
				_container.MoveTimerDownAnimation.Begin();
				_container.MoveTimerUpAnimation.Stop();
			}

			public void ShowTooShortAnimation()
			{
				_container.TooShortInstructionAnimation.Begin();
			}

			public async Task ShowSendAnimation()
			{
				_container.RecordingStateAnimation.Stop();
				_container.CancelStateAnimation.Stop();
				_container.NewUserInstructionAnimation.Stop();
				_container.MoveTimerUpAnimation.Stop();
				_container.MoveTimerDownAnimation.Stop();
				_container.RecordingTimelimitReachedAnimation.Stop();
				_container.SendAnimation.Begin();
				Duration duration = ((Timeline)_container.SendAnimation).Duration;
				await Task.Delay(duration.TimeSpan);
				_container.UpdateTimer();
				_container.RecordingRedTimerText.Stop();
				_container.SendAnimation.Stop();
				_durationLimitReached = false;
			}

			public void SetCancelOnRelease()
			{
				if (_container._cancelOnRelease)
				{
					_container.CancelStateAnimation.Begin();
					_container.RecordingStateAnimation.Stop();
					_container.NewUserInstructionAnimation.Stop();
				}
				else
				{
					_container.RecordingStateAnimation.Begin();
					_container.CancelStateAnimation.Stop();
				}
				if (_durationLimitReached)
				{
					UpdateTimelimitAnimations();
				}
				else if (_container._cancelOnRelease)
				{
					_container.MoveTimerDownAnimation.Begin();
					_container.MoveTimerUpAnimation.Stop();
				}
				else
				{
					_container.MoveTimerUpAnimation.Begin();
					_container.MoveTimerDownAnimation.Stop();
				}
			}

			private void UpdateTimelimitAnimations()
			{
				if (_container._cancelOnRelease)
				{
					_container.RecordingRedTimerText.Begin();
					_container.RecordingTimelimitReachedAnimation.Stop();
				}
				else
				{
					_container.RecordingTimelimitReachedAnimation.Begin();
					_container.RecordingRedTimerText.Stop();
				}
			}
		}

		private enum RecordingState
		{
			Idle,
			Recording,
			Processing,
			Cooldown
		}

		private const int TimerInaccuracyBufferMs = 200;

		private const int MinRecordDurationMs = 1200;

		private const int TapTimeoutMs = 250;

		private const string PreferenceShownNuxKey = "ShownNUX";

		private readonly AnimationStateManager _animator;

		private readonly VoiceRecordingHelper _recorder;

		private readonly SemaphoreSlim _recorderLock = new SemaphoreSlim(1, 1);

		private readonly DispatcherTimer _cooldownTimer;

		private readonly DispatcherTimer _displayTimerUpdater;

		public Action<VoiceRecordingHelper.VoiceRecordingResult> OnVoiceClipSend;

		public Action OnVoiceClipCancel;

		private bool _cancelOnRelease;

		private RecordingState _recordingState;

		private DateTime _whenMouseDown;

		private bool _nuxShown;




























		public VoiceClipDrawer()
			: base(DrawerType.Voice)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected Obj, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected Obj, but got Unknown
			DispatcherTimer val = new DispatcherTimer();
			val.Interval = TimeSpan.FromMilliseconds(250.0);
			_cooldownTimer = val;
			DispatcherTimer val2 = new DispatcherTimer();
			val2.Interval = TimeSpan.FromMilliseconds(200.0);
			_displayTimerUpdater = val2;
			_animator = new AnimationStateManager(this);
			InitializeComponent();
			_recorder = new VoiceRecordingHelper(TimeSpan.FromSeconds(60.0), _animator.OnDurationLimitReached);
			DispatcherTimer cooldownTimer = _cooldownTimer;
			EventHandler eventHandler = (object param0, EventArgs param1) =>
			{
				if (_recordingState == RecordingState.Cooldown)
				{
					_recordingState = RecordingState.Idle;
					_cooldownTimer.Stop();
				}
			};
			cooldownTimer.Tick += eventHandler;
			_displayTimerUpdater.Tick += (object obj, EventArgs args) =>
			{
				UpdateTimer();
			};
			UpdateTimer();
			((Timeline)NewUserInstructionAnimation).Completed += (object obj, EventArgs arg) =>
			{
				_nuxShown = true;
			};
		}

		public override void RestorePreferences(IDictionary<string, string> preferences)
		{
			if (preferences != null && preferences.ContainsKey("ShownNUX"))
			{
				bool.TryParse(preferences["ShownNUX"], out _nuxShown);
			}
		}

		public override IDictionary<string, string> GetPreferences()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("ShownNUX", _nuxShown.ToString());
			return dictionary;
		}

		public override IList<string> GetPreferenceKeys()
		{
			List<string> list = new List<string>();
			list.Add("ShownNUX");
			return list;
		}

		public override async void Cleanup()
		{
			((UIElement)RecordButtonHitArea).ReleaseMouseCapture();
			_displayTimerUpdater.Stop();
			await CancelRecording();
			_recordingState = RecordingState.Idle;
		}

		public override void Show()
		{
			base.Visibility = Visibility.Visible;
		}

		public override void Hide()
		{
			base.Visibility = Visibility.Collapsed;
			Cleanup();
		}

		public void UpdateTimer()
		{
			TimerText.Text = $"{_recorder.GetDuration():m\\:ss}";
		}

		private async void RecordButton_OnLeftMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_recordingState == RecordingState.Cooldown)
			{
				_cooldownTimer.Start();
			}
			if (_recordingState == RecordingState.Idle)
			{
				_cooldownTimer.Stop();
				_recordingState = RecordingState.Recording;
				_whenMouseDown = DateTime.Now;
				_cancelOnRelease = false;
				if (!_nuxShown)
				{
					_animator.ShowNewUserInstruction();
				}
				_animator.ShowRecordingAnimation();
				_displayTimerUpdater.Start();
				((UIElement)RecordButtonHitArea).CaptureMouse();
				await _recorderLock.WaitAsync();
				if (!(await _recorder.StartRecording()))
				{
					_recordingState = RecordingState.Idle;
					await CancelRecording();
				}
				_recorderLock.Release();
			}
		}

		private async Task CancelRecording()
		{
			if (_recordingState == RecordingState.Recording)
			{
				_recordingState = RecordingState.Processing;
				await _recorderLock.WaitAsync();
				await _recorder.CancelRecording();
				_recorderLock.Release();
			}
			_animator.Reset();
		}

		private async void RecordButton_OnLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			((UIElement)RecordButtonHitArea).ReleaseMouseCapture();
			_displayTimerUpdater.Stop();
			if (_recordingState != RecordingState.Recording)
			{
				return;
			}
			bool tooShort = DateTime.Now.Subtract(_whenMouseDown).TotalMilliseconds < 1200.0;
			if (tooShort || _cancelOnRelease)
			{
				await CancelRecording();
				OnVoiceClipCancel();
			}
			if (tooShort)
			{
				_animator.ShowTooShortAnimation();
				_recordingState = RecordingState.Cooldown;
				_cooldownTimer.Start();
				return;
			}
			if (!_cancelOnRelease)
			{
				_recordingState = RecordingState.Processing;
				await _recorderLock.WaitAsync();
				VoiceRecordingHelper.VoiceRecordingResult result = await _recorder.CompleteRecording();
				_recorderLock.Release();
				await _animator.ShowSendAnimation();
				if (_recordingState == RecordingState.Processing)
				{
					OnVoiceClipSend(result);
				}
				else
				{
					result.File.DeleteAsync().DoNotWait();
				}
			}
			_recordingState = RecordingState.Idle;
		}

		private void RecordButton_OnMouseMove(object sender, MouseEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (_recordingState == RecordingState.Recording)
			{
				bool flag = VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition((UIElement)null), (UIElement)(object)RecordButtonHitArea).Any();
				if (flag == _cancelOnRelease)
				{
					_cancelOnRelease = !flag;
					_animator.SetCancelOnRelease();
				}
			}
		}

	}
}

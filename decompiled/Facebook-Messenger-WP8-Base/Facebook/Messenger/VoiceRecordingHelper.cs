using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Facebook.Base.Logging;
using Windows.Phone.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger;

public class VoiceRecordingHelper
{
	private class AudioRecorder
	{
		private const string RecordingTempFilename = "temprecordingfile.mp4";

		private AudioVideoCaptureDevice _mic;

		private StorageFile _file;

		private IRandomAccessStream _stream;

		private DateTime _startTime = DateTime.MinValue;

		private DateTime _stopTime = DateTime.MinValue;

		public async Task<bool> Start()
		{
			try
			{
				_mic = await AudioVideoCaptureDevice.OpenForAudioOnlyAsync();
				StorageFolder local = ApplicationData.Current.LocalFolder;
				_file = await local.CreateFileAsync("temprecordingfile.mp4", (CreationCollisionOption)0);
				_stream = await _file.OpenAsync((FileAccessMode)1);
				await _mic.StartRecordingToStreamAsync(_stream);
				_startTime = DateTime.Now;
			}
			catch (Exception ex)
			{
				Logger.Error(this, "Error initializing recorder", "Start", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\VoiceRecordingHelper.cs", 48);
				Logger.Error(this, ex.ToString(), "Start", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\VoiceRecordingHelper.cs", 49);
				Reset();
				return false;
			}
			return true;
		}

		public async Task Stop()
		{
			try
			{
				await _mic.StopRecordingAsync();
			}
			catch (Exception ex)
			{
				Logger.Error(this, "Error when audio recording stops", "Stop", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\VoiceRecordingHelper.cs", 64);
				Logger.Error(this, ex.ToString(), "Stop", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\VoiceRecordingHelper.cs", 65);
			}
			_stopTime = DateTime.Now;
			await ((IOutputStream)_stream).FlushAsync();
		}

		public TimeSpan GetDuration()
		{
			if (_startTime == DateTime.MinValue)
			{
				return TimeSpan.Zero;
			}
			if (_stopTime == DateTime.MinValue)
			{
				return DateTime.Now - _startTime;
			}
			return _stopTime - _startTime;
		}

		public StorageFile GetFile()
		{
			return _file;
		}

		public void Reset()
		{
			_file = null;
			try
			{
				_mic.Dispose();
			}
			catch (Exception ex)
			{
				Logger.Error(this, "Error when disposing mic", "Reset", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\VoiceRecordingHelper.cs", 93);
				Logger.Error(this, ex.ToString(), "Reset", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\VoiceRecordingHelper.cs", 94);
			}
			finally
			{
				_mic = null;
			}
			((IDisposable)_stream).Dispose();
			_stream = null;
			_stopTime = DateTime.MinValue;
			_startTime = DateTime.MinValue;
		}
	}

	public class VoiceRecordingResult
	{
		public readonly StorageFile File;

		public readonly long DurationMs;

		public readonly DateTime Time;

		public VoiceRecordingResult(StorageFile file, long durationMs, DateTime end)
		{
			File = file;
			DurationMs = durationMs;
			Time = end;
		}
	}

	private enum RecordingState
	{
		Idle,
		Setup,
		SetupCancelled,
		Record,
		RecordingStopped
	}

	private readonly AudioRecorder _recorder = new AudioRecorder();

	private readonly TimeSpan _maximumRecordingLength;

	private readonly DispatcherTimer _maxRecordingDurationTimer;

	private readonly Action _durationLimitEvent;

	private RecordingState _recordingState;

	public VoiceRecordingHelper(TimeSpan durationLimit, Action durationLimitEvent = null)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected Obj, but got Unknown
		DispatcherTimer val = new DispatcherTimer();
		val.Interval = TimeSpan.FromMilliseconds(200.0);
		_maxRecordingDurationTimer = val;
		base._002Ector();
		_maximumRecordingLength = durationLimit;
		_maxRecordingDurationTimer.Tick += MaxRecordingDurationTimerTick;
		_durationLimitEvent = durationLimitEvent;
	}

	public async Task<bool> StartRecording()
	{
		if (_recordingState != RecordingState.Idle)
		{
			return false;
		}
		_recordingState = RecordingState.Setup;
		bool started = await _recorder.Start();
		bool setupWasCancelledBeforeCompletion = _recordingState == RecordingState.SetupCancelled;
		_recordingState = RecordingState.Record;
		if (setupWasCancelledBeforeCompletion || !started)
		{
			await CancelRecording();
			return false;
		}
		_maxRecordingDurationTimer.Start();
		return true;
	}

	public TimeSpan GetDuration()
	{
		if (_recordingState == RecordingState.Record || _recordingState == RecordingState.RecordingStopped)
		{
			return _recorder.GetDuration();
		}
		return TimeSpan.Zero;
	}

	public async Task<VoiceRecordingResult> CompleteRecording()
	{
		if (_recordingState == RecordingState.Record)
		{
			await StopRecordingInternal();
		}
		if (_recordingState != RecordingState.RecordingStopped)
		{
			return null;
		}
		StorageFile file = _recorder.GetFile();
		TimeSpan duration = _recorder.GetDuration();
		_recorder.Reset();
		_recordingState = RecordingState.Idle;
		return new VoiceRecordingResult(file, (long)duration.TotalMilliseconds, DateTime.Now);
	}

	public async Task CancelRecording()
	{
		if (_recordingState == RecordingState.Setup)
		{
			_recordingState = RecordingState.SetupCancelled;
			return;
		}
		VoiceRecordingResult result = await CompleteRecording();
		if (result != null)
		{
			await result.File.DeleteAsync();
		}
	}

	private async Task StopRecordingInternal()
	{
		_maxRecordingDurationTimer.Stop();
		await _recorder.Stop();
		_recordingState = RecordingState.RecordingStopped;
	}

	private async void MaxRecordingDurationTimerTick(object sender, EventArgs e)
	{
		if (_recorder.GetDuration() >= _maximumRecordingLength)
		{
			await StopRecordingInternal();
			if (_durationLimitEvent != null)
			{
				_durationLimitEvent();
			}
		}
	}
}

namespace Facebook.Base
{
	public class ApplicationState
	{
		public enum Runtype
		{
			Foreground,
			BackgroundSending
		}

		private long _lastStartTimestamp;

		public bool IsShuttingDown { get; private set; }

		public bool ManuallyTerminated { get; private set; }

		public bool ActivatedFromMessageFailedToast { get; set; }

		public Runtype CurrentRuntype { get; private set; }

		public ApplicationState(Runtype runtype)
		{
			CurrentRuntype = runtype;
		}

		public void AppIsShuttingDown()
		{
			IsShuttingDown = true;
			ActivatedFromMessageFailedToast = false;
		}

		public void AppIsStartingUp()
		{
			IsShuttingDown = false;
			ManuallyTerminated = false;
			_lastStartTimestamp = DateExtensions.MSecsSinceEpoch();
		}

		public void AppWasManuallyTerminated()
		{
			ManuallyTerminated = true;
		}

		public bool IsTimestampBeforeLastStart(long timestamp)
		{
			return timestamp < _lastStartTimestamp;
		}
	}
}

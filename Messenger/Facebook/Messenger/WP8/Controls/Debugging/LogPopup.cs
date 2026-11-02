using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Facebook.Base.Logging;
using Facebook.Messenger.Gatekeepers;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls.Debugging
{
	public partial class LogPopup : UserControl, DebugOverlay.IDebugDisplayElement, ILogListener
	{
		private readonly DebugLogDisplayEnabledProvider _enabledProvider;

		private readonly Queue<LogDisplayInfo> _logQueue = new Queue<LogDisplayInfo>();

		private bool _isShowingLog;







		public LogPopup()
		{
			_enabledProvider = ServiceLocator.Current.GetInstance<DebugLogDisplayEnabledProvider>();
			InitializeComponent();
		}

		public FrameworkElement Get()
		{
			return (FrameworkElement)(object)this;
		}

		public void Load()
		{
			Logger.AddListener(this);
		}

		public void Unload()
		{
			Logger.RemoveListener(this);
		}

		public void Log(LogLevel level, Type type, string message, string member, string filePath, int line)
		{
			if (ShouldDisplayLog(level))
			{
				LogDisplayInfo item = new LogDisplayInfo(level, type, message);
				lock (_logQueue)
				{
					_logQueue.Enqueue(item);
				}
				base.Dispatcher.BeginInvoke((Action)ShowNext);
			}
		}

		public void Log(LogLevel level, string message, string member, string filePath, int line)
		{
			Log(level, null, message, member, filePath, line);
		}

		private bool ShouldDisplayLog(LogLevel level)
		{
			if (!_enabledProvider.Enabled)
			{
				return false;
			}
			switch (level)
			{
			case LogLevel.Error:
			case LogLevel.Critical:
				return true;
			case LogLevel.Debug:
			case LogLevel.Warning:
				return DebuggingUtility.IsTestBuild;
			default:
				return false;
			}
		}

		private void LogContainer_OnTap(object sender, GestureEventArgs e)
		{
			DisplayLogAnimation.SkipToFill();
		}

		private void DisplayLogAnimation_OnCompleted(object sender, EventArgs e)
		{
			DisplayLogAnimation.Stop();
			_isShowingLog = false;
			((UIElement)LogContainer).IsHitTestVisible = false;
			ShowNext();
		}

		private void ShowNext()
		{
			if (_isShowingLog)
			{
				return;
			}
			_isShowingLog = true;
			LogDisplayInfo logDisplayInfo;
			lock (_logQueue)
			{
				if (!_logQueue.Any())
				{
					_isShowingLog = false;
					return;
				}
				logDisplayInfo = _logQueue.Dequeue();
			}
			LogLevelBorder.Background = (Brush)(object)logDisplayInfo.ColorBrush;
			LogLevelText.Text = logDisplayInfo.Title;
			LogText.Text = logDisplayInfo.Description;
			DisplayLogAnimation.Begin();
			((UIElement)LogContainer).IsHitTestVisible = true;
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Messenger.Crash;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.Logging;
using Facebook.Messenger.Navigation;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Models;
using Microsoft.Phone.Controls;
using Microsoft.Practices.ServiceLocation;
using ShakeGestures;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger.WP8.Utilities
{
	public class ExceptionHelper
	{
		private const int MaximumReportsForDebugDialog = 20;

		public static bool RageShakeDialogVisible;

		private readonly Queue<Tuple<StorageFile, string>> _dialogQueue = new Queue<Tuple<StorageFile, string>>();

		public readonly Facebook.Messenger.Logging.Logger Log;

		private bool _showingDialog;

		public ExceptionHelper(Application application)
		{
			Log = new Facebook.Messenger.Logging.Logger();
			Facebook.Base.Logging.Logger.AddListener(Log);
			InitializeExceptionHandling(application);
		}

		private void InitializeExceptionHandling(Application application)
		{
			application.UnhandledException += OnUnhandledException;
			TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
			ExceptionSynchronizationContext.EnableAsyncExceptionHandling(ErrorContextOnUnhandledException);
			ShakeGesturesHelper.Instance.ShakeGesture += OnShakeGesture;
			ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 6;
			ShakeGesturesHelper.Instance.StillCounterThreshold = 5;
			ShakeGesturesHelper.Instance.StillMagnitudeWithoutGravitationThreshold = 0.05;
			ShakeGesturesHelper.Instance.WeakMagnitudeWithoutGravitationThreshold = 2.0;
		}

		public void ResetRageShake(FacebookProfile profile = null)
		{
			ShakeGesturesHelper.Instance.Active = DebuggingUtility.IsTestBuildOrEmployee;
		}

		private void OnShakeGesture(object sender, ShakeGestureEventArgs e)
		{
			if (e.ShakeType == ShakeType.Z)
			{
				return;
			}
			App.Dispatcher.RunAsync(() =>
			{
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Expected Obj, but got Unknown
				if (!RageShakeDialogVisible)
				{
					RageShakeDialogVisible = true;
					IEnumerable<string> logs = Enumerable.Reverse(Log.LogMessages);
					RageShakeControl rageShakeControl = new RageShakeControl();
					Popup val = new Popup();
					val.Child = (UIElement)(object)rageShakeControl;
					Popup popup = val;
					rageShakeControl.ActionChosen += (object o, RageShakeControl.RageShakeAction action) =>
					{
						popup.IsOpen = false;
						RageShakeDialogVisible = false;
						HandleSendEvent(action, logs).DoNotWait();
					};
					popup.IsOpen = true;
				}
			});
		}

		private static async Task HandleSendEvent(RageShakeControl.RageShakeAction action, IEnumerable<string> logs)
		{
			switch (action.ChosenAction)
			{
			case RageShakeControl.Action.Email:
				await CrashSender.PostToEmail(logs: CrashSaver.CreateUserLog("app", action.InputText, logs), inputText: action.InputText);
				break;
			case RageShakeControl.Action.Message:
			{
				string textToSend = action.InputText + "\n\n" + string.Join("\n", logs);
				int maxTextSize = Math.Min(32768, textToSend.Length);
				textToSend = textToSend.Substring(0, maxTextSize);
				Navigator navigator = ServiceLocator.Current.GetInstance<Navigator>();
				FacebookUri uri = FacebookUri.GetComposerUri(null, false, null, null, textToSend);
				navigator.Navigate(uri);
				break;
			}
			}
		}

		public static async Task CheckForCrashLogs()
		{
			List<StorageFile> files = await CrashSaver.GetPersistedCrashes();
			if (files.Count == 0)
			{
				return;
			}
			if (DebuggingUtility.IsTestBuild)
			{
				((DependencyObject)Deployment.Current).Dispatcher.BeginInvoke((Action)(async () =>
				{
					await ShowAppStartReportDialog(files);
				}));
				return;
			}
			FacebookContext context = ServiceLocator.Current.GetInstance<FacebookContext>();
			if (context.IsAuthenticated())
			{
				await CrashSender.PostReportsToServer(files, context.Session.Credentials.AccessToken);
			}
			else
			{
				await CrashSender.PostReportsToServer(files);
			}
		}

		private void ErrorContextOnUnhandledException(object sender, AsyncUnhandledExceptionEventArgs e)
		{
			e.Handled = true;
			HandleException(e.Exception, true, "ErrorContextOnUnhandledException");
		}

		private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			e.SetObserved();
			HandleException(e.Exception, false, "OnUnobservedTaskException");
		}

		private void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			e.Handled = true;
			HandleException(e.ExceptionObject, true, "OnUnhandledException");
		}

		private void HandleException(Exception e, bool observed = true, [CallerMemberName] string member = "")
		{
			Facebook.Base.Logging.Logger.Error(this, member, "HandleException", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\ExceptionHelper.cs", 156);
			Facebook.Base.Logging.Logger.Error(this, e.ToString(), "HandleException", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\ExceptionHelper.cs", 157);
			if (ShouldSkipReportingException(e))
			{
				return;
			}
			List<string> logMessages = Log.LogMessages;
			Tuple<StorageFile, string> tuple = CrashSaver.SaveLog("app", e, observed, logMessages);
			if (tuple == null)
			{
				return;
			}
			if (DebuggingUtility.IsTestBuild)
			{
				if (observed)
				{
					lock (_dialogQueue)
					{
						_dialogQueue.Enqueue(tuple);
					}
					ShowNextErrorDialog();
				}
			}
			else
			{
				FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
				if (instance.IsAuthenticated())
				{
					CrashSender.PostToServerAndDeleteLogFile(tuple.Item2, tuple.Item1, instance.Session.Credentials.AccessToken).DoNotWait();
				}
				else
				{
					CrashSender.PostToServerAndDeleteLogFile(tuple.Item2, tuple.Item1).DoNotWait();
				}
			}
		}

		private bool ShouldSkipReportingException(Exception e)
		{
			Version version = Environment.OSVersion.Version;
			if (version.Major != 8 || version.Minor != 0)
			{
				return false;
			}
			Exception baseException = e.GetBaseException();
			if ((object)baseException.GetType() == typeof(ObjectDisposedException) && baseException.StackTrace.Contains("Microsoft.Devices.Camera"))
			{
				return true;
			}
			return false;
		}

		private void ShowNextErrorDialog()
		{
			if (_showingDialog)
			{
				return;
			}
			Tuple<StorageFile, string> result = null;
			lock (_dialogQueue)
			{
				if (!_dialogQueue.Any())
				{
					return;
				}
				result = _dialogQueue.Dequeue();
			}
			_showingDialog = true;
			App.Dispatcher.RunAsync(() =>
			{
				ShowErrorDialog(result.Item1, result.Item2);
			});
		}

		private void ShowErrorDialog(StorageFile file, string errorLog)
		{
			try
			{
				CustomMessageBox customMessageBox = new CustomMessageBox();
				customMessageBox.Caption = InternalStringsLibrary.Crash_Dialog_Title;
				customMessageBox.Message = InternalStringsLibrary.Crash_Dialog_Body;
				customMessageBox.RightButtonContent = InternalStringsLibrary.Crash_Dialog_Report;
				customMessageBox.LeftButtonContent = InternalStringsLibrary.Crash_Dialog_Ignore;
				CustomMessageBox customMessageBox2 = customMessageBox;
				customMessageBox2.Dismissed += async (object sender, DismissedEventArgs args) =>
				{
					if (args.Result != CustomMessageBoxResult.LeftButton && args.Result == CustomMessageBoxResult.RightButton)
					{
						await CrashSender.PostToEmail(errorLog);
					}
					await CrashSaver.DeleteExceptionFiles(new List<StorageFile> { file });
					_showingDialog = false;
					ShowNextErrorDialog();
				};
				customMessageBox2.Show();
			}
			catch (Exception ex)
			{
				Facebook.Base.Logging.Logger.Error(this, ex.ToString(), "ShowErrorDialog", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\ExceptionHelper.cs", 276);
			}
		}

		private static async Task<string> ReadTextAsync(StorageFile file)
		{
			try
			{
				IRandomAccessStreamWithContentType stream = await file.OpenReadAsync();
				try
				{
					IInputStream input = ((IRandomAccessStream)stream).GetInputStreamAt(0uL);
					try
					{
						DataReader reader = new DataReader(input);
						try
						{
							await (IAsyncOperation<uint>)(object)reader.LoadAsync((uint)((IRandomAccessStream)stream).Size);
							byte[] buf = new byte[((IRandomAccessStream)stream).Size];
							reader.ReadBytes(buf);
							return Encoding.UTF8.GetString(buf, 0, buf.Length);
						}
						finally
						{
							((IDisposable)reader)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)input)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)stream)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				Facebook.Base.Logging.Logger.Error("Error reading crash file: " + ex, "ReadTextAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\ExceptionHelper.cs", 297);
			}
			return null;
		}

		private static async Task ShowAppStartReportDialog(IList<StorageFile> files)
		{
			StringBuilder sb = new StringBuilder();
			foreach (StorageFile file in files.Take(20))
			{
				string content = await ReadTextAsync(file);
				if (content != null)
				{
					sb.AppendLine(file.Name);
					sb.AppendLine();
					sb.AppendLine(content);
				}
			}
			try
			{
				CustomMessageBox customMessageBox = new CustomMessageBox();
				customMessageBox.Caption = InternalStringsLibrary.Crash_Dialog_Existing_Title;
				customMessageBox.Message = InternalStringsLibrary.Crash_Dialog_Existing_Body;
				customMessageBox.RightButtonContent = InternalStringsLibrary.Crash_Dialog_Report;
				customMessageBox.LeftButtonContent = InternalStringsLibrary.Crash_Dialog_Ignore;
				CustomMessageBox customMessageBox2 = customMessageBox;
				customMessageBox2.Dismissed += async (object sender, DismissedEventArgs args) =>
				{
					if (args.Result != CustomMessageBoxResult.LeftButton && args.Result == CustomMessageBoxResult.RightButton)
					{
						await CrashSender.PostToEmail(sb.ToString());
					}
					CrashSaver.DeleteExceptionFiles(files).DoNotWait();
				};
				customMessageBox2.Show();
			}
			catch (Exception ex)
			{
				Facebook.Base.Logging.Logger.Error("Error showing crash dialog: " + ex, "ShowAppStartReportDialog", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\ExceptionHelper.cs", 348);
			}
		}
	}
}

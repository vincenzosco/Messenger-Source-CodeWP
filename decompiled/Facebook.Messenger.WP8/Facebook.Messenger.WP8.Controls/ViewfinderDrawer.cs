using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Models;
using Microsoft.Devices;
using Windows.Phone.Media.Capture;

namespace Facebook.Messenger.WP8.Controls
{
	public class ViewfinderDrawer : ComposerDrawer
	{
		private const string PreferenceCamRotationKey = "CameraRotation";

		private const double RecordingProgressInterval = 0.2;

		private readonly DispatcherTimer _cameraCleanupTimer;

		private Nullable _rotationPreference;

		private DispatcherTimer _recordingProgressTimer;

		private bool _holding;

		private bool _isMouseOver;

		private bool _timeLimitReached;

		private bool _launchCameraTaskWhenFinishedInitializing;

		private Storyboard _expandAnimation;

		private Storyboard _collapseAnimation;

		internal Grid LayoutRoot;

		internal CameraView CameraControl;

		internal Grid DeleteGrid;

		internal TextBlock HelpTextBlock;

		internal Rectangle ProgressRectangle;

		internal SolidColorBrush ProgressColorBrush;

		internal TextBlock ProgressText;

		internal ImageButton CapturePhotoButton;

		internal ImageButton ReinitializeCameraButton;

		internal CircledIcon RotateCameraButton;

		internal CircledIcon ExternalCameraButton;

		private bool _contentLoaded;

		public event EventHandler CameraButtonClicked;

		public event EventHandler<ImageResult> SendClicked;

		public event EventHandler<VideoResult> VideoRecorded;

		public event EventHandler<UserGesture> UserGestured;

		public event EventHandler<Storyboard> ExpandStoryboardCreated;

		public event EventHandler<Storyboard> CollapseStoryboardCreated;

		public ViewfinderDrawer()
			: base(DrawerType.Camera)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected Obj, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected Obj, but got Unknown
			InitializeComponent();
			((UIElement)RotateCameraButton).Visibility = VisibilityUtil.FromBool(CameraControl.CanRotate);
			_cameraCleanupTimer = new DispatcherTimer();
			_cameraCleanupTimer.Tick += CameraCleanupTimerOnTick;
			_cameraCleanupTimer.Interval = TimeSpan.FromSeconds(2.0);
			HelpTextBlock.Text = StringLibrary.VideoRecorder_HelpText_HoldToSend;
			((UIElement)CapturePhotoButton).AddHandler(UIElement.MouseLeftButtonUpEvent, (Delegate)new MouseButtonEventHandler(OnMouseLeftButtonUp), true);
			((UIElement)CapturePhotoButton).AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate)new MouseButtonEventHandler(OnMouseLeftButtonDown), true);
			((UIElement)CapturePhotoButton).Hold += CaptureButton_OnHold;
			((UIElement)HelpTextBlock).Visibility = (Visibility)0;
		}

		public override void Cleanup()
		{
			ResetProgress();
			CameraControl.CleanUp();
			CameraButtons.ShutterKeyPressed -= CameraButtonsOnShutterKeyPressed;
		}

		private void CameraCleanupTimerOnTick(object sender, EventArgs eventArgs)
		{
			Cleanup();
			_cameraCleanupTimer.Stop();
		}

		public override void Show()
		{
			_cameraCleanupTimer.Stop();
			CameraControl.Setup(_rotationPreference);
			((UIElement)this).Visibility = (Visibility)0;
			CameraButtons.ShutterKeyPressed += CameraButtonsOnShutterKeyPressed;
		}

		public override void Hide()
		{
			((UIElement)this).Visibility = (Visibility)1;
			CameraButtons.ShutterKeyPressed -= CameraButtonsOnShutterKeyPressed;
			_cameraCleanupTimer.Start();
		}

		public override void RestorePreferences(IDictionary<string, string> preferences)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			CameraSensorLocation result;
			if (preferences != null && preferences.ContainsKey("CameraRotation") && Enum.TryParse<CameraSensorLocation>(preferences["CameraRotation"], out result))
			{
				_rotationPreference = result;
			}
		}

		public override IDictionary<string, string> GetPreferences()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			string value = (_rotationPreference.HasValue ? ((object)_rotationPreference.Value).ToString() : string.Empty);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("CameraRotation", value);
			return dictionary;
		}

		public override IList<string> GetPreferenceKeys()
		{
			List<string> list = new List<string>();
			list.Add("CameraRotation");
			return list;
		}

		private void CameraButtonsOnShutterKeyPressed(object sender, EventArgs eventArgs)
		{
		}

		public Nullable GetRotationPreference()
		{
			return _rotationPreference;
		}

		private void CameraButton_Tap(object sender, GestureEventArgs e)
		{
			if (CameraControl.IsInitializing)
			{
				_launchCameraTaskWhenFinishedInitializing = true;
			}
			else if (CameraButtonClicked != null)
			{
				CameraButtonClicked(this, null);
			}
		}

		private void RotateCameraButton_OnTap(object sender, GestureEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_rotationPreference = CameraControl.Rotate();
		}

		private void InvokeUserGestured(UserGesture gesture)
		{
			if (UserGestured != null)
			{
				UserGestured(this, gesture);
			}
		}

		private void CapturePhoto()
		{
			WriteableBitmap val = CameraControl.Capture();
			if (SendClicked != null && val != null)
			{
				SendClicked(this, new ImageResult(MediaSource.QuickCam, PhotoData.FromImage((BitmapSource)(object)val)));
			}
		}

		private void CameraControl_OnStateChanged(object sender, CameraView.State e)
		{
			Logger.Info(string.Format("Camera state changed to: {0}", e), "CameraControl_OnStateChanged", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\ViewfinderDrawer.xaml.cs", 168);
			if (_launchCameraTaskWhenFinishedInitializing && e != CameraView.State.Initializing)
			{
				_launchCameraTaskWhenFinishedInitializing = false;
				if (CameraButtonClicked != null)
				{
					CameraButtonClicked(this, null);
				}
			}
			else
			{
				((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
				{
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_003e: Unknown result type (might be due to invalid IL or missing references)
					((UIElement)CapturePhotoButton).Visibility = VisibilityUtil.FromBool(e == CameraView.State.Ready || e == CameraView.State.Recording);
					((UIElement)ReinitializeCameraButton).Visibility = VisibilityUtil.FromBool(e == CameraView.State.Error);
				}));
			}
		}

		public void OnNavigatedFrom()
		{
			CameraButtons.ShutterKeyPressed -= CameraButtonsOnShutterKeyPressed;
			CameraControl.CleanUp();
		}

		private void ReinitializeCameraButton_OnTap(object sender, GestureEventArgs e)
		{
			CameraControl.Setup();
		}

		private Storyboard PrepareAnimationStoryboard(double to, double from)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected Obj, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected Obj, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected Obj, but got Unknown
			Storyboard val = new Storyboard();
			DoubleAnimation val2 = new DoubleAnimation();
			((Timeline)val2).Duration = Duration.op_Implicit(TimeSpan.FromSeconds(0.3));
			val2.To = to;
			val2.From = from;
			CubicEase val3 = new CubicEase();
			((EasingFunctionBase)val3).EasingMode = (EasingMode)0;
			val2.EasingFunction = (IEasingFunction)(object)val3;
			DoubleAnimation val4 = val2;
			Storyboard.SetTarget((Timeline)(object)val4, (DependencyObject)(object)this);
			Storyboard.SetTargetProperty((Timeline)(object)val4, new PropertyPath("Height", new object[0]));
			((PresentationFrameworkCollection<Timeline>)(object)val.Children).Add((Timeline)(object)val4);
			return val;
		}

		private void CaptureButton_OnHold(object sender, GestureEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (_collapseAnimation != null && (int)_collapseAnimation.GetCurrentState() == 0)
			{
				return;
			}
			_holding = true;
			double captureHeight = CameraControl.CaptureHeight;
			if (captureHeight > 0.0)
			{
				HelpTextBlock.Text = StringLibrary.VideoRecorder_HelpText_Send;
				if (_expandAnimation != null)
				{
					((Timeline)_expandAnimation).Completed -= ExpandAnimationOnCompleted;
				}
				if (_collapseAnimation != null)
				{
					((Timeline)_collapseAnimation).Completed -= CollapseAnimationOnCompleted;
				}
				_expandAnimation = PrepareAnimationStoryboard(captureHeight, ((FrameworkElement)this).ActualHeight);
				_collapseAnimation = PrepareAnimationStoryboard(((FrameworkElement)this).ActualHeight, captureHeight);
				if (ExpandStoryboardCreated != null)
				{
					ExpandStoryboardCreated(this, _expandAnimation);
				}
				if (CollapseStoryboardCreated != null)
				{
					CollapseStoryboardCreated(this, _collapseAnimation);
				}
				CameraControl.PopulateExpandStoryboard(_expandAnimation);
				CameraControl.PopulateCollapseStoryboard(_collapseAnimation);
				((Timeline)_collapseAnimation).Completed += CollapseAnimationOnCompleted;
				((Timeline)_expandAnimation).Completed += ExpandAnimationOnCompleted;
				_expandAnimation.Begin();
			}
		}

		private void CollapseAnimationOnCompleted(object sender, EventArgs eventArgs)
		{
			((Control)CapturePhotoButton).IsEnabled = true;
		}

		private async void ExpandAnimationOnCompleted(object sender, EventArgs eventArgs)
		{
			((Control)RotateCameraButton).IsEnabled = false;
			((Control)ExternalCameraButton).IsEnabled = false;
			((UIElement)ProgressText).Visibility = (Visibility)0;
			if (_recordingProgressTimer == null)
			{
				_recordingProgressTimer = new DispatcherTimer();
				_recordingProgressTimer.Tick += RecordingProgressTimerOnTick;
				_recordingProgressTimer.Interval = TimeSpan.FromSeconds(0.2);
			}
			await CameraControl.StartVideoCaptureAsync();
			if (_holding)
			{
				_recordingProgressTimer.Start();
				return;
			}
			await CameraControl.StopVideoCaptureAsync(true);
			ResetProgress();
		}

		private async void RecordingProgressTimerOnTick(object sender, EventArgs e)
		{
			DateTime currentTime = DateTime.Now;
			TimeSpan diff = currentTime - CameraControl.StartTime;
			double milliseconds = diff.TotalMilliseconds;
			if (milliseconds > 15000.0)
			{
				milliseconds = 15000.0;
			}
			((UIElement)ProgressRectangle).Visibility = (Visibility)0;
			((FrameworkElement)ProgressRectangle).Width = Math.Round(milliseconds / 15000.0 * ((FrameworkElement)this).ActualWidth);
			ProgressText.Text = string.Format("0:{0}", diff.TotalSeconds.ToString("00"));
			if (milliseconds == 15000.0)
			{
				_timeLimitReached = true;
				_recordingProgressTimer.Stop();
				await CameraControl.StopVideoCaptureAsync();
				CameraControl.PausePreview();
			}
		}

		private void CapturePhotoButton_OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Invalid comparison between Unknown and I4
			_isMouseOver = ((ButtonBase)CapturePhotoButton).IsPressed;
			if (((ButtonBase)CapturePhotoButton).IsMouseOver && _holding)
			{
				if (_isMouseOver && (int)((UIElement)DeleteGrid).Visibility == 0)
				{
					((UIElement)DeleteGrid).Visibility = (Visibility)1;
					Rectangle progressRectangle = ProgressRectangle;
					object obj = Application.Current.Resources[(object)"MessengerTransparentBlueBrush"];
					((Shape)progressRectangle).Fill = (Brush)((obj is SolidColorBrush) ? obj : null);
					HelpTextBlock.Text = StringLibrary.VideoRecorder_HelpText_Send;
				}
				else if (!_isMouseOver && (int)((UIElement)DeleteGrid).Visibility == 1)
				{
					((UIElement)DeleteGrid).Visibility = (Visibility)0;
					Rectangle progressRectangle2 = ProgressRectangle;
					object obj2 = Application.Current.Resources[(object)"MessengerProgressRedBrush"];
					((Shape)progressRectangle2).Fill = (Brush)((obj2 is SolidColorBrush) ? obj2 : null);
					HelpTextBlock.Text = StringLibrary.VideoRecorder_HelpText_Cancel;
				}
			}
		}

		private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_isMouseOver = true;
			InvokeUserGestured(UserGesture.CameraButtonDown);
		}

		private async void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
		{
			if (_expandAnimation != null && (int)_expandAnimation.GetCurrentState() == 0)
			{
				_expandAnimation.Stop();
				((Timeline)_expandAnimation).Completed -= ExpandAnimationOnCompleted;
				_expandAnimation = null;
				InvokeUserGestured(UserGesture.CancelRecord);
			}
			else if (_holding)
			{
				bool recorded = CameraControl.GetState() == CameraView.State.Recording || (CameraControl.GetState() == CameraView.State.Ready && _timeLimitReached);
				bool recordCancelled = (int)((UIElement)DeleteGrid).Visibility == 0;
				((Control)CapturePhotoButton).IsEnabled = false;
				VideoResult result = null;
				if (recorded)
				{
					await CameraControl.StopVideoCaptureAsync(recordCancelled);
					if (!recordCancelled)
					{
						result = await CameraControl.GetVideoResultAsync();
					}
				}
				InvokeUserGestured((!recordCancelled && recorded) ? UserGesture.CameraButtonUp : UserGesture.CancelRecord);
				if (result != null && VideoRecorded != null)
				{
					VideoRecorded(this, result);
				}
				if (_collapseAnimation != null)
				{
					_collapseAnimation.Begin();
				}
			}
			else if (_isMouseOver)
			{
				await Task.Delay(300);
				InvokeUserGestured(UserGesture.CameraButtonUp);
				CapturePhoto();
			}
			ResetProgress();
			_holding = false;
			_isMouseOver = false;
		}

		private void ResetProgress()
		{
			CameraControl.RestoreDefaults();
			((Control)RotateCameraButton).IsEnabled = true;
			((Control)ExternalCameraButton).IsEnabled = true;
			Rectangle progressRectangle = ProgressRectangle;
			object obj = Application.Current.Resources[(object)"MessengerTransparentBlueBrush"];
			((Shape)progressRectangle).Fill = (Brush)((obj is SolidColorBrush) ? obj : null);
			((FrameworkElement)ProgressRectangle).Width = 0.0;
			((UIElement)ProgressRectangle).Visibility = (Visibility)1;
			((UIElement)ProgressText).Visibility = (Visibility)1;
			ProgressText.Text = "0:00";
			HelpTextBlock.Text = StringLibrary.VideoRecorder_HelpText_HoldToSend;
			((UIElement)DeleteGrid).Visibility = (Visibility)1;
			_timeLimitReached = false;
			if (_recordingProgressTimer != null)
			{
				_recordingProgressTimer.Stop();
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ViewfinderDrawer.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				CameraControl = (CameraView)((FrameworkElement)this).FindName("CameraControl");
				DeleteGrid = (Grid)((FrameworkElement)this).FindName("DeleteGrid");
				HelpTextBlock = (TextBlock)((FrameworkElement)this).FindName("HelpTextBlock");
				ProgressRectangle = (Rectangle)((FrameworkElement)this).FindName("ProgressRectangle");
				ProgressColorBrush = (SolidColorBrush)((FrameworkElement)this).FindName("ProgressColorBrush");
				ProgressText = (TextBlock)((FrameworkElement)this).FindName("ProgressText");
				CapturePhotoButton = (ImageButton)((FrameworkElement)this).FindName("CapturePhotoButton");
				ReinitializeCameraButton = (ImageButton)((FrameworkElement)this).FindName("ReinitializeCameraButton");
				RotateCameraButton = (CircledIcon)((FrameworkElement)this).FindName("RotateCameraButton");
				ExternalCameraButton = (CircledIcon)((FrameworkElement)this).FindName("ExternalCameraButton");
			}
		}
	}
}

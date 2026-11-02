using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Utilities.SystemTrayManager;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger.WP8.Controls
{
	public class StateIndicatingProgressBar : BaseControl
	{
		private bool _isAnimating;

		private readonly SystemTrayManager _systemTrayManager;

		public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register("FontColor", typeof(Brush), typeof(StateIndicatingProgressBar), new PropertyMetadata(Application.Current.Resources[(object)"MessengerBlackBrush"], (PropertyChangedCallback)OnFontColorSet));

		public static DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(StateIndicatingProgressBar), new PropertyMetadata((object)false, (PropertyChangedCallback)IsIndeterminatePropertyChanged));

		internal Storyboard IntermediateIn;

		internal Storyboard NextStateIn;

		internal Storyboard IntermediateOut;

		internal Grid LayoutRoot;

		internal FBMessengerProgressBar FbProgressBar;

		internal TextBlock State;

		internal TranslateTransform StateTransform;

		internal TextBlock CompletedState;

		internal TranslateTransform CompletedStateTransform;

		private bool _contentLoaded;

		public Brush FontColor
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (Brush)((DependencyObject)this).GetValue(FontColorProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FontColorProperty, (object)value);
			}
		}

		public bool IsIndeterminate
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsIndeterminateProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsIndeterminateProperty, (object)value);
			}
		}

		private static void OnFontColorSet(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			StateIndicatingProgressBar stateIndicatingProgressBar = obj as StateIndicatingProgressBar;
			object newValue = args.NewValue;
			Brush val = (Brush)((newValue is Brush) ? newValue : null);
			if (stateIndicatingProgressBar != null && val != null)
			{
				stateIndicatingProgressBar.State.Foreground = val;
				stateIndicatingProgressBar.CompletedState.Foreground = val;
			}
		}

		private static void IsIndeterminatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StateIndicatingProgressBar stateIndicatingProgressBar = d as StateIndicatingProgressBar;
			if (stateIndicatingProgressBar != null)
			{
				bool flag = (e.NewValue as bool?) ?? false;
				stateIndicatingProgressBar.FbProgressBar.IsIndeterminate = flag;
				((UIElement)stateIndicatingProgressBar.FbProgressBar).Opacity = (flag ? 1 : 0);
			}
		}

		public StateIndicatingProgressBar()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnloaded;
			_systemTrayManager = ServiceLocator.Current.GetInstance<SystemTrayManager>();
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			((Timeline)NextStateIn).Completed += NextStateInOnCompleted;
		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_systemTrayManager.RemoveOperationFromStack(((object)this).GetType());
			((Timeline)NextStateIn).Completed -= NextStateInOnCompleted;
		}

		public void Reset()
		{
			_isAnimating = false;
			StopAllAnimations();
			StateTransform.Y = -40.0;
			CompletedStateTransform.Y = -40.0;
		}

		public void StopAllAnimations()
		{
			IntermediateIn.Stop();
			IntermediateOut.Stop();
			NextStateIn.SkipToFill();
		}

		public void SetState(string status)
		{
			Reset();
			_isAnimating = true;
			_systemTrayManager.AddOperationToStack(((object)this).GetType());
			State.Text = status;
			IntermediateIn.Begin();
		}

		public void NextState(string nextStatus)
		{
			if (_isAnimating)
			{
				CompletedState.Text = nextStatus;
				IntermediateIn.Stop();
				NextStateIn.Begin();
				IntermediateOut.Begin();
			}
		}

		private void NextStateInOnCompleted(object sender, EventArgs eventArgs)
		{
			_systemTrayManager.RemoveOperationFromStack(((object)this).GetType());
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
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/StateIndicatingProgressbar.xaml", UriKind.Relative));
				IntermediateIn = (Storyboard)((FrameworkElement)this).FindName("IntermediateIn");
				NextStateIn = (Storyboard)((FrameworkElement)this).FindName("NextStateIn");
				IntermediateOut = (Storyboard)((FrameworkElement)this).FindName("IntermediateOut");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				FbProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("FbProgressBar");
				State = (TextBlock)((FrameworkElement)this).FindName("State");
				StateTransform = (TranslateTransform)((FrameworkElement)this).FindName("StateTransform");
				CompletedState = (TextBlock)((FrameworkElement)this).FindName("CompletedState");
				CompletedStateTransform = (TranslateTransform)((FrameworkElement)this).FindName("CompletedStateTransform");
			}
		}

		static StateIndicatingProgressBar()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected Obj, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected Obj, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected Obj, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected Obj, but got Unknown
		}
	}
}

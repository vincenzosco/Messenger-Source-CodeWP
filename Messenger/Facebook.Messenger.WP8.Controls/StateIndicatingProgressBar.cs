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
	public partial class StateIndicatingProgressBar : BaseControl
	{
		private bool _isAnimating;

		private readonly SystemTrayManager _systemTrayManager;

		public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register("FontColor", typeof(Brush), typeof(StateIndicatingProgressBar), new PropertyMetadata(Application.Current.Resources[(object)"MessengerBlackBrush"], (PropertyChangedCallback)OnFontColorSet));

		public static DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(StateIndicatingProgressBar), new PropertyMetadata((object)false, (PropertyChangedCallback)IsIndeterminatePropertyChanged));











		public Brush FontColor
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (Brush)base.GetValue(FontColorProperty);
			}
			set
			{
				base.SetValue(FontColorProperty, (object)value);
			}
		}

		public bool IsIndeterminate
		{
			get
			{
				return (bool)base.GetValue(IsIndeterminateProperty);
			}
			set
			{
				base.SetValue(IsIndeterminateProperty, (object)value);
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
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
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

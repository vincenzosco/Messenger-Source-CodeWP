using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Extensions;

public class TiltEffect : DependencyObject
{
	private class LogarithmicEase : EasingFunctionBase
	{
		protected override double EaseInCore(double normalizedTime)
		{
			return Math.Log(normalizedTime + 1.0) / 0.693147181;
		}
	}

	private const double MaxAngle = 0.3;

	private const double MaxDepression = 25.0;

	private static readonly TimeSpan TiltReturnAnimationDelay;

	private static readonly TimeSpan TiltReturnAnimationDuration;

	private static FrameworkElement currentTiltElement;

	private static Storyboard tiltReturnStoryboard;

	private static DoubleAnimation tiltReturnXAnimation;

	private static DoubleAnimation tiltReturnYAnimation;

	private static DoubleAnimation tiltReturnZAnimation;

	private static Point currentTiltElementCenter;

	private static bool wasPauseAnimation;

	public static readonly DependencyProperty IsTiltEnabledProperty;

	public static readonly DependencyProperty SuppressTiltProperty;

	public static bool UseLogarithmicEase { get; set; }

	public static List<Type> TiltableItems { get; private set; }

	private TiltEffect()
	{
	}

	static TiltEffect()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected Obj, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected Obj, but got Unknown
		TiltReturnAnimationDelay = TimeSpan.FromMilliseconds(200.0);
		TiltReturnAnimationDuration = TimeSpan.FromMilliseconds(100.0);
		wasPauseAnimation = false;
		IsTiltEnabledProperty = DependencyProperty.RegisterAttached("IsTiltEnabled", typeof(bool), typeof(TiltEffect), new PropertyMetadata((PropertyChangedCallback)OnIsTiltEnabledChanged));
		SuppressTiltProperty = DependencyProperty.RegisterAttached("SuppressTilt", typeof(bool), typeof(TiltEffect), (PropertyMetadata)null);
		TiltableItems = new List<Type>
		{
			typeof(ButtonBase),
			typeof(ListBoxItem),
			typeof(ITiltable)
		};
		UseLogarithmicEase = false;
	}

	public static bool GetIsTiltEnabled(DependencyObject source)
	{
		return (bool)source.GetValue(IsTiltEnabledProperty);
	}

	public static void SetIsTiltEnabled(DependencyObject source, bool value)
	{
		source.SetValue(IsTiltEnabledProperty, (object)value);
	}

	public static bool GetSuppressTilt(DependencyObject source)
	{
		return (bool)source.GetValue(SuppressTiltProperty);
	}

	public static void SetSuppressTilt(DependencyObject source, bool value)
	{
		source.SetValue(SuppressTiltProperty, (object)value);
	}

	private static void OnIsTiltEnabledChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
	{
		if (target is FrameworkElement)
		{
			if ((bool)args.NewValue)
			{
				((UIElement)((target is FrameworkElement) ? target : null)).ManipulationStarted += TiltEffect_ManipulationStarted;
			}
			else
			{
				((UIElement)((target is FrameworkElement) ? target : null)).ManipulationStarted -= TiltEffect_ManipulationStarted;
			}
		}
	}

	private static void TiltEffect_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
	{
		TryStartTiltEffect((FrameworkElement)((sender is FrameworkElement) ? sender : null), e);
	}

	private static void TiltEffect_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
	{
		ContinueTiltEffect((FrameworkElement)((sender is FrameworkElement) ? sender : null), e);
	}

	private static void TiltEffect_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
	{
		EndTiltEffect(currentTiltElement);
	}

	private static void TryStartTiltEffect(FrameworkElement source, ManipulationStartedEventArgs e)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		object originalSource = ((RoutedEventArgs)e).OriginalSource;
		foreach (FrameworkElement visualAncestor in ((FrameworkElement)((originalSource is FrameworkElement) ? originalSource : null)).GetVisualAncestors())
		{
			foreach (Type tiltableItem in TiltableItems)
			{
				if (tiltableItem.IsAssignableFrom(((object)visualAncestor).GetType()) && !(bool)((DependencyObject)visualAncestor).GetValue(SuppressTiltProperty))
				{
					DependencyObject child = VisualTreeHelper.GetChild((DependencyObject)(object)visualAncestor, 0);
					FrameworkElement val = (FrameworkElement)(object)((child is FrameworkElement) ? child : null);
					UIElement manipulationContainer = e.ManipulationContainer;
					FrameworkElement val2 = (FrameworkElement)(object)((manipulationContainer is FrameworkElement) ? manipulationContainer : null);
					if (val != null && val2 != null)
					{
						Point touchPoint = ((UIElement)val2).TransformToVisual((UIElement)(object)val).Transform(e.ManipulationOrigin);
						Point centerPoint = new Point(val.ActualWidth / 2.0, val.ActualHeight / 2.0);
						Point centerToCenterDelta = GetCenterToCenterDelta(val, source);
						BeginTiltEffect(val, touchPoint, centerPoint, centerToCenterDelta);
					}
					return;
				}
			}
		}
	}

	private static Point GetCenterToCenterDelta(FrameworkElement element, FrameworkElement container)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Point val = new Point(element.ActualWidth / 2.0, element.ActualHeight / 2.0);
		Point val3;
		if (container is PhoneApplicationFrame)
		{
			PhoneApplicationFrame val2 = (PhoneApplicationFrame)(object)((container is PhoneApplicationFrame) ? container : null);
			val3 = (((val2.Orientation & 2) != 2) ? new Point(container.ActualWidth / 2.0, container.ActualHeight / 2.0) : new Point(container.ActualHeight / 2.0, container.ActualWidth / 2.0));
		}
		else
		{
			val3 = new Point(container.ActualWidth / 2.0, container.ActualHeight / 2.0);
		}
		Point val4 = ((UIElement)element).TransformToVisual((UIElement)(object)container).Transform(val);
		return new Point(val3.X - val4.X, val3.Y - val4.Y);
	}

	private static void BeginTiltEffect(FrameworkElement element, Point touchPoint, Point centerPoint, Point centerDelta)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (tiltReturnStoryboard != null)
		{
			StopTiltReturnStoryboardAndCleanup();
		}
		if (PrepareControlForTilt(element, centerDelta))
		{
			currentTiltElement = element;
			currentTiltElementCenter = centerPoint;
			PrepareTiltReturnStoryboard(element);
			ApplyTiltEffect(currentTiltElement, touchPoint, currentTiltElementCenter);
		}
	}

	private static bool PrepareControlForTilt(FrameworkElement element, Point centerDelta)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected Obj, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected Obj, but got Unknown
		if (((UIElement)element).Projection != null || (((UIElement)element).RenderTransform != null && (object)((object)((UIElement)element).RenderTransform).GetType() != typeof(MatrixTransform)))
		{
			return false;
		}
		TranslateTransform val = new TranslateTransform();
		val.X = centerDelta.X;
		val.Y = centerDelta.Y;
		((UIElement)element).RenderTransform = (Transform)(object)val;
		PlaneProjection val2 = new PlaneProjection();
		val2.GlobalOffsetX = -1.0 * centerDelta.X;
		val2.GlobalOffsetY = -1.0 * centerDelta.Y;
		((UIElement)element).Projection = (Projection)(object)val2;
		((UIElement)element).ManipulationDelta += TiltEffect_ManipulationDelta;
		((UIElement)element).ManipulationCompleted += TiltEffect_ManipulationCompleted;
		return true;
	}

	private static void RevertPrepareControlForTilt(FrameworkElement element)
	{
		((UIElement)element).ManipulationDelta -= TiltEffect_ManipulationDelta;
		((UIElement)element).ManipulationCompleted -= TiltEffect_ManipulationCompleted;
		((UIElement)element).Projection = null;
		((UIElement)element).RenderTransform = null;
	}

	private static void PrepareTiltReturnStoryboard(FrameworkElement element)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected Obj, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected Obj, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected Obj, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected Obj, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected Obj, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected Obj, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected Obj, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		if (tiltReturnStoryboard == null)
		{
			tiltReturnStoryboard = new Storyboard();
			((Timeline)tiltReturnStoryboard).Completed += TiltReturnStoryboard_Completed;
			tiltReturnXAnimation = new DoubleAnimation();
			Storyboard.SetTargetProperty((Timeline)(object)tiltReturnXAnimation, new PropertyPath((object)PlaneProjection.RotationXProperty));
			((Timeline)tiltReturnXAnimation).BeginTime = TiltReturnAnimationDelay;
			tiltReturnXAnimation.To = 0.0;
			((Timeline)tiltReturnXAnimation).Duration = Duration.op_Implicit(TiltReturnAnimationDuration);
			tiltReturnYAnimation = new DoubleAnimation();
			Storyboard.SetTargetProperty((Timeline)(object)tiltReturnYAnimation, new PropertyPath((object)PlaneProjection.RotationYProperty));
			((Timeline)tiltReturnYAnimation).BeginTime = TiltReturnAnimationDelay;
			tiltReturnYAnimation.To = 0.0;
			((Timeline)tiltReturnYAnimation).Duration = Duration.op_Implicit(TiltReturnAnimationDuration);
			tiltReturnZAnimation = new DoubleAnimation();
			Storyboard.SetTargetProperty((Timeline)(object)tiltReturnZAnimation, new PropertyPath((object)PlaneProjection.GlobalOffsetZProperty));
			((Timeline)tiltReturnZAnimation).BeginTime = TiltReturnAnimationDelay;
			tiltReturnZAnimation.To = 0.0;
			((Timeline)tiltReturnZAnimation).Duration = Duration.op_Implicit(TiltReturnAnimationDuration);
			if (UseLogarithmicEase)
			{
				tiltReturnXAnimation.EasingFunction = (IEasingFunction)(object)new LogarithmicEase();
				tiltReturnYAnimation.EasingFunction = (IEasingFunction)(object)new LogarithmicEase();
				tiltReturnZAnimation.EasingFunction = (IEasingFunction)(object)new LogarithmicEase();
			}
			((PresentationFrameworkCollection<Timeline>)(object)tiltReturnStoryboard.Children).Add((Timeline)(object)tiltReturnXAnimation);
			((PresentationFrameworkCollection<Timeline>)(object)tiltReturnStoryboard.Children).Add((Timeline)(object)tiltReturnYAnimation);
			((PresentationFrameworkCollection<Timeline>)(object)tiltReturnStoryboard.Children).Add((Timeline)(object)tiltReturnZAnimation);
		}
		Storyboard.SetTarget((Timeline)(object)tiltReturnXAnimation, (DependencyObject)(object)((UIElement)element).Projection);
		Storyboard.SetTarget((Timeline)(object)tiltReturnYAnimation, (DependencyObject)(object)((UIElement)element).Projection);
		Storyboard.SetTarget((Timeline)(object)tiltReturnZAnimation, (DependencyObject)(object)((UIElement)element).Projection);
	}

	private static void ContinueTiltEffect(FrameworkElement element, ManipulationDeltaEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		UIElement manipulationContainer = e.ManipulationContainer;
		FrameworkElement val = (FrameworkElement)(object)((manipulationContainer is FrameworkElement) ? manipulationContainer : null);
		if (val != null && element != null)
		{
			Point val2 = ((UIElement)val).TransformToVisual((UIElement)(object)element).Transform(e.ManipulationOrigin);
			Rect val3 = new Rect(0.0, 0.0, currentTiltElement.ActualWidth, currentTiltElement.ActualHeight);
			if (!val3.Contains(val2))
			{
				PauseTiltEffect();
			}
			else
			{
				ApplyTiltEffect(currentTiltElement, e.ManipulationOrigin, currentTiltElementCenter);
			}
		}
	}

	private static void EndTiltEffect(FrameworkElement element)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (element != null)
		{
			((UIElement)element).ManipulationCompleted -= TiltEffect_ManipulationCompleted;
			((UIElement)element).ManipulationDelta -= TiltEffect_ManipulationDelta;
		}
		if (tiltReturnStoryboard != null)
		{
			wasPauseAnimation = false;
			if ((int)tiltReturnStoryboard.GetCurrentState() != 0)
			{
				tiltReturnStoryboard.Begin();
			}
		}
		else
		{
			StopTiltReturnStoryboardAndCleanup();
		}
	}

	private static void TiltReturnStoryboard_Completed(object sender, EventArgs e)
	{
		if (wasPauseAnimation)
		{
			ResetTiltEffect(currentTiltElement);
		}
		else
		{
			StopTiltReturnStoryboardAndCleanup();
		}
	}

	private static void ResetTiltEffect(FrameworkElement element)
	{
		Projection projection = ((UIElement)element).Projection;
		PlaneProjection val = (PlaneProjection)(object)((projection is PlaneProjection) ? projection : null);
		val.RotationY = 0.0;
		val.RotationX = 0.0;
		val.GlobalOffsetZ = 0.0;
	}

	private static void StopTiltReturnStoryboardAndCleanup()
	{
		if (tiltReturnStoryboard != null)
		{
			tiltReturnStoryboard.Stop();
		}
		RevertPrepareControlForTilt(currentTiltElement);
	}

	private static void PauseTiltEffect()
	{
		if (tiltReturnStoryboard != null && !wasPauseAnimation)
		{
			tiltReturnStoryboard.Stop();
			wasPauseAnimation = true;
			tiltReturnStoryboard.Begin();
		}
	}

	private static void ResetTiltReturnStoryboard()
	{
		tiltReturnStoryboard.Stop();
		wasPauseAnimation = false;
	}

	private static void ApplyTiltEffect(FrameworkElement element, Point touchPoint, Point centerPoint)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		ResetTiltReturnStoryboard();
		Point val = new Point(Math.Min(Math.Max(touchPoint.X / (centerPoint.X * 2.0), 0.0), 1.0), Math.Min(Math.Max(touchPoint.Y / (centerPoint.Y * 2.0), 0.0), 1.0));
		double num = Math.Abs(val.X - 0.5);
		double num2 = Math.Abs(val.Y - 0.5);
		double num3 = -Math.Sign(val.X - 0.5);
		double num4 = Math.Sign(val.Y - 0.5);
		double num5 = num + num2;
		double num6 = ((num + num2 > 0.0) ? (num / (num + num2)) : 0.0);
		double num7 = num5 * 0.3 * 180.0 / Math.PI;
		double num8 = (1.0 - num5) * 25.0;
		Projection projection = ((UIElement)element).Projection;
		PlaneProjection val2 = (PlaneProjection)(object)((projection is PlaneProjection) ? projection : null);
		val2.RotationY = num7 * num6 * num3;
		val2.RotationX = num7 * (1.0 - num6) * num4;
		val2.GlobalOffsetZ = 0.0 - num8;
	}
}

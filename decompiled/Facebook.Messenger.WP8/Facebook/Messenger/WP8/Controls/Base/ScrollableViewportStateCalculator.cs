using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Facebook.Messenger.WP8.Controls.Base;

public class ScrollableViewportStateCalculator
{
	private enum State
	{
		NotAtBottom,
		AtBottom
	}

	private const double ViewportBufferMultiplier = 0.3;

	private State _currentState;

	public bool HasTransitionedToBottom(ViewportControl viewportControl)
	{
		if (!ValidViewportControl(viewportControl))
		{
			return false;
		}
		State currentState = _currentState;
		_currentState = CalculateState(viewportControl);
		return currentState == State.NotAtBottom && _currentState == State.AtBottom;
	}

	private static bool ValidViewportControl(ViewportControl viewportControl)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		Rect viewport = viewportControl.Viewport;
		int num;
		if (viewport.Width > 0.0)
		{
			Rect viewport2 = viewportControl.Viewport;
			num = ((viewport2.Height > 0.0) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		Rect bounds = viewportControl.Bounds;
		int num2;
		if (bounds.Width > 0.0)
		{
			Rect bounds2 = viewportControl.Bounds;
			num2 = ((bounds2.Height > 0.0) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool result = (byte)num2 != 0;
		if (flag)
		{
			return result;
		}
		return false;
	}

	private static State CalculateState(ViewportControl viewportControl)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		Rect viewport = viewportControl.Viewport;
		double y = viewport.Y;
		Rect viewport2 = viewportControl.Viewport;
		double num = y + viewport2.Height;
		Rect bounds = viewportControl.Bounds;
		double y2 = bounds.Y;
		Rect bounds2 = viewportControl.Bounds;
		double num2 = y2 + bounds2.Height;
		double num3 = Math.Max(0.0, num2 - num);
		Rect viewport3 = viewportControl.Viewport;
		double num4 = viewport3.Height * 0.3;
		if (!(num3 < num4))
		{
			return State.NotAtBottom;
		}
		return State.AtBottom;
	}

	private static bool ListIsEmpty(ICollection list)
	{
		if (list != null)
		{
			return list.Count == 0;
		}
		return true;
	}
}

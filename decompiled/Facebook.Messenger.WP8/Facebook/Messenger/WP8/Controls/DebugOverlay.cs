using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Facebook.Messenger.WP8.Controls.Debugging;
using Facebook.Models.Signals;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Messenger.WP8.Controls;

public class DebugOverlay
{
	public interface IDebugDisplayElement
	{
		FrameworkElement Get();

		void Load();

		void Unload();
	}

	private readonly List<IDebugDisplayElement> _debugElements;

	private readonly Grid _container;

	public DebugOverlay()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected Obj, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected Obj, but got Unknown
		Grid val = new Grid();
		((FrameworkElement)val).HorizontalAlignment = (HorizontalAlignment)3;
		((FrameworkElement)val).VerticalAlignment = (VerticalAlignment)0;
		((FrameworkElement)val).Width = 480.0;
		_container = val;
		_debugElements = new List<IDebugDisplayElement>
		{
			new LogPopup()
		};
		foreach (FrameworkElement item in _debugElements.Select((IDebugDisplayElement e) => e.Get()))
		{
			Grid.SetRow(item, 0);
			item.VerticalAlignment = (VerticalAlignment)0;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)_container).Children).Add((UIElement)(object)item);
		}
		Popup val2 = new Popup();
		val2.Child = (UIElement)(object)_container;
		val2.IsOpen = true;
		((UIElement)val2).IsHitTestVisible = false;
		SetVisibility();
	}

	private void SetVisibility()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((UIElement)_container).Visibility = VisibilityUtil.FromBool(DebuggingUtility.IsTestBuildOrEmployee);
	}

	public void Load()
	{
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<LoginSignal>(this, OnLogin);
		_debugElements.ForEach((IDebugDisplayElement e) =>
		{
			e.Load();
		});
	}

	public void Unload()
	{
		GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
		_debugElements.ForEach((IDebugDisplayElement e) =>
		{
			e.Unload();
		});
	}

	private void OnLogin(LoginSignal signal)
	{
		SetVisibility();
	}
}

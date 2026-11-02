using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using Facebook.Messenger.WP8.Controls.Base;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Controls
{
	[ContentProperty("Child")]
	public class FacebookPopup : BaseControl
	{
		protected double MaxContentPanelWidth;

		protected double MaxContentPanelHeight;

		protected readonly IApplicationBar ApplicationBar;

		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(FacebookPopup), new PropertyMetadata((object)false, (PropertyChangedCallback)IsOpen_OnChanged));

		public Popup RootPopup = new Popup();

		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(IsOpenProperty);
			}
			set
			{
				base.SetValue(IsOpenProperty, (object)value);
				if (!IsOpen)
				{
					if (PopupDismissed != null)
					{
						PopupDismissed(this, null);
					}
					Reset();
				}
				else
				{
					Setup();
				}
			}
		}

		public UIElement Child
		{
			get
			{
				return RootPopup.Child;
			}
			set
			{
				RootPopup.Child = value;
			}
		}

		public event EventHandler PopupDismissed;

		private static void IsOpen_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FacebookPopup facebookPopup = d as FacebookPopup;
			if (facebookPopup != null)
			{
				facebookPopup.RootPopup.IsOpen = (bool)e.NewValue;
			}
		}

		protected virtual void Setup()
		{
		}

		protected virtual void Reset()
		{
		}

		public FacebookPopup()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected Obj, but got Unknown
			Initialize();
		}

		public FacebookPopup(IApplicationBar applicationBar)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected Obj, but got Unknown
			ApplicationBar = applicationBar;
			Initialize();
		}

		public static void SafeCleanUp(FacebookPopup popup)
		{
			popup?.CleanUp();
		}

		public void CleanUp()
		{
			Application.Current.Host.Content.Resized -= OnApplicationContentOnResized;
		}

		private void Initialize()
		{
			base.Content = (UIElement)(object)RootPopup;
			UpdateSize();
			Application.Current.Host.Content.Resized += OnApplicationContentOnResized;
		}

		private void UpdateSize()
		{
			MaxContentPanelHeight = Application.Current.Host.Content.ActualHeight;
			MaxContentPanelWidth = Application.Current.Host.Content.ActualWidth;
		}

		private void OnApplicationContentOnResized(object sender, EventArgs eventArgs)
		{
			UpdateSize();
		}

		static FacebookPopup()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected Obj, but got Unknown
		}
	}
}

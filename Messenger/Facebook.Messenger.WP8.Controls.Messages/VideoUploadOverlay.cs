using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls.Messages
{
	public partial class VideoUploadOverlay : UserControl, IResetableView
	{
		private RoutedEventHandler m_CancelEvent;





		public event RoutedEventHandler CancelEvent
		{
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected Obj, but got Unknown
				RoutedEventHandler val = this.m_CancelEvent;
				RoutedEventHandler val2;
				do
				{
					val2 = val;
					RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Combine((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref this.m_CancelEvent, value2, val2);
				}
				while (val != val2);
			}
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected Obj, but got Unknown
				RoutedEventHandler val = this.m_CancelEvent;
				RoutedEventHandler val2;
				do
				{
					val2 = val;
					RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Remove((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref this.m_CancelEvent, value2, val2);
				}
				while (val != val2);
			}
		}

		public VideoUploadOverlay()
		{
			InitializeComponent();
		}

		public void ResetView(bool recycled)
		{
			((UIElement)OverlayGrid).Visibility = Visibility.Collapsed;
			((UIElement)CancelTextBlock).Visibility = Visibility.Collapsed;
			UploadProgressBar.IsIndeterminate = false;
		}

		public void UpdateView(MessageViewModel m)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			bool flag = m.State == MailMessage.MessageState.Queued;
			UploadProgressBar.IsIndeterminate = flag;
			((UIElement)OverlayGrid).Visibility = VisibilityUtil.FromBool(flag);
			((UIElement)CancelTextBlock).Visibility = VisibilityUtil.FromBool(IsCancellable(m));
		}

		private bool IsCancellable(MessageViewModel m)
		{
			if (m is ContextualMessageViewModel)
			{
				return ((ContextualMessageViewModel)m).IsCancellable;
			}
			return false;
		}

		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this.m_CancelEvent != null)
			{
				this.m_CancelEvent.Invoke(sender, e);
			}
		}

	}
}

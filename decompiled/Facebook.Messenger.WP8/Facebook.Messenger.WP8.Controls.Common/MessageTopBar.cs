using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Models;

namespace Facebook.Messenger.WP8.Controls.Common
{
	public class MessageTopBar : BaseControl
	{
		public static readonly DependencyProperty RecipientChatOriginProperty = DependencyProperty.Register("RecipientChatOrigin", typeof(RecipientChatOrigin), typeof(MessageTopBar), new PropertyMetadata((object)RecipientChatOrigin.None, (PropertyChangedCallback)RecipientChatOriginChangedCallback));

		public static readonly DependencyProperty IsCanonicalProperty = DependencyProperty.Register("IsCanonical", typeof(bool), typeof(MessageTopBar), new PropertyMetadata((object)true, (PropertyChangedCallback)IsCanonicalChangedCallback));

		public static readonly DependencyProperty ThreadNameProperty = DependencyProperty.Register("ThreadName", typeof(string), typeof(MessageTopBar), new PropertyMetadata((PropertyChangedCallback)ThreadNameChangedCallback));

		public static readonly DependencyProperty TiltEnabledProperty = DependencyProperty.Register("TiltEnabled", typeof(bool), typeof(MessageTopBar), new PropertyMetadata((object)false));

		internal BaseControl TopBar;

		internal StackPanel CanonicalStackPanel;

		internal TextBlock MessengerOriginTextBlock;

		internal TextBlock FacebookOriginTextBlock;

		internal TextBlock ChatStatus;

		internal TextBlock GroupTextBlock;

		internal TextBlock ChatName;

		private bool _contentLoaded;

		public RecipientChatOrigin RecipientChatOrigin
		{
			get
			{
				return (RecipientChatOrigin)((DependencyObject)this).GetValue(RecipientChatOriginProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(RecipientChatOriginProperty, (object)value);
			}
		}

		public bool IsCanonical
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsCanonicalProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsCanonicalProperty, (object)value);
			}
		}

		public string ThreadName
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(ThreadNameProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ThreadNameProperty, (object)value);
			}
		}

		public bool TiltEnabled
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(TiltEnabledProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TiltEnabledProperty, (object)value);
			}
		}

		public MessageTopBar()
		{
			InitializeComponent();
		}

		private static void RecipientChatOriginChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			MessageTopBar messageTopBar = dependencyObject as MessageTopBar;
			if (messageTopBar != null)
			{
				messageTopBar.UpdateOriginTextBlocks();
			}
		}

		private static void IsCanonicalChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			MessageTopBar messageTopBar = dependencyObject as MessageTopBar;
			if (messageTopBar != null)
			{
				messageTopBar.UpdateCanonicalVisibility();
			}
		}

		private static void ThreadNameChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			MessageTopBar messageTopBar = dependencyObject as MessageTopBar;
			if (messageTopBar != null)
			{
				messageTopBar.UpdateThreadName();
			}
		}

		private void UpdateOriginTextBlocks()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			bool visible = RecipientChatOrigin == RecipientChatOrigin.Messenger;
			bool visible2 = RecipientChatOrigin == RecipientChatOrigin.Facebook;
			((UIElement)MessengerOriginTextBlock).Visibility = VisibilityUtil.FromBool(visible);
			((UIElement)FacebookOriginTextBlock).Visibility = VisibilityUtil.FromBool(visible2);
		}

		private void UpdateCanonicalVisibility()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)CanonicalStackPanel).Visibility = VisibilityUtil.FromBool(IsCanonical);
			((UIElement)GroupTextBlock).Visibility = VisibilityUtil.FromBool(!IsCanonical);
		}

		private void UpdateThreadName()
		{
			ChatName.Text = ThreadName;
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
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
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Common/MessageTopBar.xaml", UriKind.Relative));
				TopBar = (BaseControl)((FrameworkElement)this).FindName("TopBar");
				CanonicalStackPanel = (StackPanel)((FrameworkElement)this).FindName("CanonicalStackPanel");
				MessengerOriginTextBlock = (TextBlock)((FrameworkElement)this).FindName("MessengerOriginTextBlock");
				FacebookOriginTextBlock = (TextBlock)((FrameworkElement)this).FindName("FacebookOriginTextBlock");
				ChatStatus = (TextBlock)((FrameworkElement)this).FindName("ChatStatus");
				GroupTextBlock = (TextBlock)((FrameworkElement)this).FindName("GroupTextBlock");
				ChatName = (TextBlock)((FrameworkElement)this).FindName("ChatName");
			}
		}

		static MessageTopBar()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected Obj, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected Obj, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected Obj, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected Obj, but got Unknown
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
		}
	}
}

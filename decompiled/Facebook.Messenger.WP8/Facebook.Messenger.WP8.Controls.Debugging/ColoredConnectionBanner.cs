using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls.Debugging
{
	public class ColoredConnectionBanner : BaseConnectionBanner
	{
		private static readonly SolidColorBrush GreenBrush;

		private static readonly SolidColorBrush RedBrush;

		private static readonly SolidColorBrush OrangeBrush;

		internal Storyboard ShowAnimation;

		internal Storyboard HideAnimation;

		internal Storyboard ShowAndHideAnimation;

		internal Grid ConnectionStatusDisplay;

		internal TextBlock ConnectionStatusTextBlock;

		internal TranslateTransform StatusDisplayTransform;

		private bool _contentLoaded;

		public ColoredConnectionBanner()
		{
			InitializeComponent();
		}

		protected override void UpdateBannerStatus(BannerState newState)
		{
			switch (newState)
			{
			case BannerState.NoInternet:
				ConnectionStatusTextBlock.Text = StringLibrary.NetworkStatus_Banner_No_Internet;
				((Panel)ConnectionStatusDisplay).Background = (Brush)(object)RedBrush;
				break;
			case BannerState.Disconnected:
				ConnectionStatusTextBlock.Text = StringLibrary.NetworkStatus_Banner_Disconnected;
				((Panel)ConnectionStatusDisplay).Background = (Brush)(object)OrangeBrush;
				break;
			case BannerState.Connecting:
				ConnectionStatusTextBlock.Text = StringLibrary.NetworkStatus_Banner_Connecting;
				((Panel)ConnectionStatusDisplay).Background = (Brush)(object)OrangeBrush;
				break;
			case BannerState.Syncing:
				ConnectionStatusTextBlock.Text = StringLibrary.NetworkStatus_Banner_Syncing;
				((Panel)ConnectionStatusDisplay).Background = (Brush)(object)GreenBrush;
				break;
			case BannerState.Connected:
				ConnectionStatusTextBlock.Text = StringLibrary.NetworkStatus_Banner_Connected;
				((Panel)ConnectionStatusDisplay).Background = (Brush)(object)GreenBrush;
				break;
			case BannerState.Hidden:
				((DependencyObject)ConnectionStatusTextBlock).ClearValue(TextBlock.TextProperty);
				((DependencyObject)ConnectionStatusDisplay).ClearValue(Panel.BackgroundProperty);
				break;
			default:
				throw new ArgumentOutOfRangeException("Unexpected BannerState: " + newState);
			}
		}

		protected override void OnRefreshStateTriggered(object sender, EventArgs eventArgs)
		{
			base.OnRefreshStateTriggered(sender, eventArgs);
			ConnectionStatusTextBlock.Text = StringLibrary.NetworkStatus_Banner_UpToDate;
			((Panel)ConnectionStatusDisplay).Background = (Brush)(object)GreenBrush;
			ShowAndHideAnimation.Begin();
		}

		protected override void TriggerShowAnimation()
		{
			ShowAnimation.Begin();
		}

		protected override void TriggerHideAnimation()
		{
			HideAnimation.Begin();
		}

		protected override void TriggerHideWithoutAnimation()
		{
			HideAnimation.SkipToFill();
		}

		protected override void FinishAnimations()
		{
			ShowAnimation.Stop();
			HideAnimation.Stop();
			ShowAndHideAnimation.Stop();
		}

		protected override void OnBannerHidden(object sender, EventArgs e)
		{
			base.OnBannerHidden(sender, e);
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
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Debugging/ColoredConnectionBanner.xaml", UriKind.Relative));
				ShowAnimation = (Storyboard)((FrameworkElement)this).FindName("ShowAnimation");
				HideAnimation = (Storyboard)((FrameworkElement)this).FindName("HideAnimation");
				ShowAndHideAnimation = (Storyboard)((FrameworkElement)this).FindName("ShowAndHideAnimation");
				ConnectionStatusDisplay = (Grid)((FrameworkElement)this).FindName("ConnectionStatusDisplay");
				ConnectionStatusTextBlock = (TextBlock)((FrameworkElement)this).FindName("ConnectionStatusTextBlock");
				StatusDisplayTransform = (TranslateTransform)((FrameworkElement)this).FindName("StatusDisplayTransform");
			}
		}

		static ColoredConnectionBanner()
		{
			object obj = Application.Current.Resources[(object)"MessengerStatusGreenBrush"];
			GreenBrush = (SolidColorBrush)((obj is SolidColorBrush) ? obj : null);
			object obj2 = Application.Current.Resources[(object)"MessengerStatusRedBrush"];
			RedBrush = (SolidColorBrush)((obj2 is SolidColorBrush) ? obj2 : null);
			object obj3 = Application.Current.Resources[(object)"MessengerStatusOrangeBrush"];
			OrangeBrush = (SolidColorBrush)((obj3 is SolidColorBrush) ? obj3 : null);
		}
	}
}

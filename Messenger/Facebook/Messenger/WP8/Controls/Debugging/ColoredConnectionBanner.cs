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
	public partial class ColoredConnectionBanner : BaseConnectionBanner
	{
		private static readonly SolidColorBrush GreenBrush;

		private static readonly SolidColorBrush RedBrush;

		private static readonly SolidColorBrush OrangeBrush;








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

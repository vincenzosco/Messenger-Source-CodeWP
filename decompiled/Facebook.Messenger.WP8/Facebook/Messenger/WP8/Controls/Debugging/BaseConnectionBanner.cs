using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls.Debugging
{
	public abstract class BaseConnectionBanner : SyncConnectionBannerViewModelControl
	{
		private bool _isBannerShowing;

		private bool _savedIsBannerShowing;

		public event EventHandler BannerAnimationStarted;

		public event EventHandler BannerAnimationCompleted;

		protected BaseConnectionBanner()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			AddConnectionBannerToVisualTree();
			((FrameworkElement)this).Unloaded += OnUnloaded;
		}

		protected override void ViewModelUpdated(ConnectionBannerViewModel oldViewModel)
		{
			base.ViewModelUpdated(oldViewModel);
			if (oldViewModel != null)
			{
				oldViewModel.PropertyChanged -= OnPropertyChanged;
			}
			WithViewModel((ConnectionBannerViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
				vm.RefreshStateTriggered += OnRefreshStateTriggered;
			});
			UpdateBanner();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "BannerState")
			{
				UpdateBanner();
			}
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((ConnectionBannerViewModel vm) =>
			{
				vm.PropertyChanged -= OnPropertyChanged;
				vm.RefreshStateTriggered -= OnRefreshStateTriggered;
			});
		}

		private void AddConnectionBannerToVisualTree()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected Obj, but got Unknown
			Grid val = new Grid();
			((FrameworkElement)val).HorizontalAlignment = (HorizontalAlignment)3;
			((FrameworkElement)val).VerticalAlignment = (VerticalAlignment)0;
			((FrameworkElement)val).Width = Application.Current.Host.Content.ActualWidth;
			Grid val2 = val;
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)val2).Children).Add((UIElement)(object)this);
			Popup val3 = new Popup();
			val3.Child = (UIElement)(object)val2;
			val3.IsOpen = true;
			((UIElement)val3).IsHitTestVisible = false;
		}

		private void UpdateBanner()
		{
			WithViewModel((ConnectionBannerViewModel vm) =>
			{
				UpdateBannerStatus(vm.BannerState);
				AnimateBanner(vm.BannerState);
			});
		}

		private void AnimateBanner(BannerState newState)
		{
			if (_isBannerShowing)
			{
				if (newState == BannerState.Connected || newState == BannerState.Hidden)
				{
					HideBanner();
				}
			}
			else if (newState == BannerState.Disconnected || newState == BannerState.NoInternet)
			{
				ShowBanner();
			}
		}

		private void ShowBanner()
		{
			if (BannerAnimationStarted != null)
			{
				BannerAnimationStarted(this, null);
			}
			_isBannerShowing = true;
			TriggerShowAnimation();
		}

		private void HideBanner()
		{
			TriggerHideAnimation();
		}

		public void RestoreBannerState()
		{
			if (_savedIsBannerShowing && !_isBannerShowing)
			{
				ShowBanner();
			}
		}

		public void PreserveBannerState()
		{
			FinishAnimations();
			TriggerHideWithoutAnimation();
			_savedIsBannerShowing = _isBannerShowing;
			if (BannerAnimationCompleted != null)
			{
				BannerAnimationCompleted(this, null);
			}
			_isBannerShowing = false;
		}

		protected abstract void TriggerShowAnimation();

		protected abstract void TriggerHideAnimation();

		protected abstract void TriggerHideWithoutAnimation();

		protected abstract void FinishAnimations();

		protected abstract void UpdateBannerStatus(BannerState newState);

		protected virtual void OnBannerHidden(object sender, EventArgs e)
		{
			if (BannerAnimationCompleted != null)
			{
				BannerAnimationCompleted(this, null);
			}
			_isBannerShowing = false;
		}

		protected virtual void OnRefreshStateTriggered(object sender, EventArgs eventArgs)
		{
			if (!_isBannerShowing)
			{
				if (BannerAnimationStarted != null)
				{
					BannerAnimationStarted(this, null);
				}
				_isBannerShowing = true;
			}
		}
	}
}

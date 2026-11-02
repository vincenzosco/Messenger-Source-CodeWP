using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls.Stickers
{
	public partial class StickerSelector : StickerPackViewModelControl
	{









		public event EventHandler<StickerViewModel> StickerSelected;

		public event EventHandler<StickerPackViewModel> DownloadPackSelected;

		public StickerSelector()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			AddEventsAndUpdate();
		}

		private void AddEventsAndUpdate()
		{
			WithViewModel((StickerPackViewModel vm) =>
			{
				AddEvents(vm);
				UpdateState(vm);
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			WithViewModel(RemoveEvents);
		}

		protected override void ViewModelUpdated(StickerPackViewModel oldViewModel)
		{
			base.ViewModelUpdated(oldViewModel);
			if (oldViewModel != null)
			{
				RemoveEvents(oldViewModel);
			}
			WithViewModel((StickerPackViewModel vm) =>
			{
				AddEvents(vm);
				UpdateState(vm);
			});
		}

		private void AddEvents(StickerPackViewModel vm)
		{
			RemoveEvents(vm);
			vm.PropertyChanged += OnPropertyChanged;
		}

		private void RemoveEvents(StickerPackViewModel vm)
		{
			vm.PropertyChanged -= OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			string propertyName;
			if ((propertyName = args.PropertyName) != null && propertyName == "State")
			{
				WithViewModel(UpdateState);
			}
		}

		private void Sticker_OnTap(object s, GestureEventArgs gestureEventArgs)
		{
			FrameworkElement val = (FrameworkElement)((s is FrameworkElement) ? s : null);
			if (val == null)
			{
				return;
			}
			StickerViewModel stickerViewModel = val.DataContext as StickerViewModel;
			if (stickerViewModel != null)
			{
				EventHandler<StickerViewModel> stickerSelected = StickerSelected;
				if (stickerSelected != null)
				{
					stickerSelected(this, stickerViewModel);
				}
			}
		}

		private void DownloadButton_OnClick(object sender, RoutedEventArgs e)
		{
			EventHandler<StickerPackViewModel> downloadPackSelected = DownloadPackSelected;
			if (downloadPackSelected != null && ViewModel != null)
			{
				downloadPackSelected(this, ViewModel);
			}
		}

		private void UpdateState(StickerPackViewModel vm)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			bool flag = vm.State == StickerPackViewModel.DownloadState.Downloaded;
			bool visible = vm.State == StickerPackViewModel.DownloadState.Downloading;
			((UIElement)StickerList).Visibility = VisibilityUtil.FromBool(flag);
			((UIElement)StickerPackInfoGrid).Visibility = VisibilityUtil.FromBool(!flag);
			((UIElement)ProgressBarGrid).Visibility = VisibilityUtil.FromBool(visible);
		}

	}
}

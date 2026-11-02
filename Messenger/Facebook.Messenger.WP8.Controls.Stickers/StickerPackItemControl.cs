using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls.Stickers
{
	public partial class StickerPackItemControl : AbstractStickerPackItemControl
	{




		public event EventHandler<StickerPackViewModel> StickerPackClicked;

		public StickerPackItemControl()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			UpdateState();
		}

		private void OnItemTapped(object sender, GestureEventArgs e)
		{
			if (StickerPackClicked != null)
			{
				StickerPackClicked(this, ViewModel);
			}
		}

		protected override void OnItemValueAssigned()
		{
			WithViewModel((StickerPackViewModel vm) =>
			{
				vm.PropertyChanged += OnPropertyChanged;
			});
			UpdateState();
		}

		protected override void Recycled(StickerPackViewModel oldViewModel)
		{
			oldViewModel.PropertyChanged -= OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "State")
			{
				UpdateState();
			}
		}

		private void UpdateState()
		{
			WithViewModel((StickerPackViewModel vm) =>
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				bool visible = vm.State == StickerPackViewModel.DownloadState.Downloaded;
				bool visible2 = vm.State == StickerPackViewModel.DownloadState.Downloading;
				((UIElement)DownloadProgressBar).Visibility = VisibilityUtil.FromBool(visible2);
				((UIElement)DownloadedImage).Visibility = VisibilityUtil.FromBool(visible);
			});
		}

	}
}

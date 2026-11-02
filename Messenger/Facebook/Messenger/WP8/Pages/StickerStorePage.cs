using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Facebook.Base.Extensions;
using Facebook.Messenger.WP8.Controls.Stickers;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class StickerStorePage : StickerStoreViewModelPage
	{





		public StickerStorePage()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnloaded;
		}

		private void AddEvents(StickerStoreViewModel vm)
		{
			RemoveEvents(vm);
			vm.PropertyChanged += OnPropertyChanged;
			vm.AvailableStickerPacksUpdated += OnAvailableStickerPacksUpdated;
			vm.OwnedStickerPacksUpdated += OnOwnedStickerPacksUpdated;
		}

		private void RemoveEvents(StickerStoreViewModel vm)
		{
			vm.PropertyChanged -= OnPropertyChanged;
			vm.AvailableStickerPacksUpdated -= OnAvailableStickerPacksUpdated;
			vm.OwnedStickerPacksUpdated -= OnOwnedStickerPacksUpdated;
		}

		private void UpdateAvailableStickerPacksProgress(StickerStoreViewModel vm)
		{
			bool flag = !vm.FeaturedStickerPacks.Any() && string.IsNullOrEmpty(vm.AvailableErrorMessage);
			bool flag2 = !vm.AvailableStickerPacks.Any() && string.IsNullOrEmpty(vm.AvailableErrorMessage);
			FeaturedStickerPackList.IsLoading = vm.IsLoadingAvailable || flag;
			AllStickerPackList.IsLoading = vm.IsLoadingAvailable || flag2;
		}

		private void UpdateOwnedStickerPacksProgress(StickerStoreViewModel vm)
		{
			OwnedStickerPackList.IsLoading = vm.IsLoadingOwned || !vm.OwnedStickerPacks.Any();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((StickerStoreViewModel vm) =>
			{
				AddEvents(vm);
				vm.FetchStickers().DoNotWait();
				vm.RegisterProgressHandler();
				UpdateAvailableStickerPacksProgress(vm);
				UpdateOwnedStickerPacksProgress(vm);
			});
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			WithViewModel((StickerStoreViewModel vm) =>
			{
				vm.UnRegisterProgressHandler();
				RemoveEvents(vm);
			});
		}

		private void OnAvailableStickerPacksUpdated(object sender, EventArgs args)
		{
			WithViewModel((StickerStoreViewModel vm) =>
			{
				UpdateAvailableStickerPacksProgress(vm);
			});
		}

		private void OnOwnedStickerPacksUpdated(object sender, EventArgs args)
		{
			WithViewModel((StickerStoreViewModel vm) =>
			{
				UpdateOwnedStickerPacksProgress(vm);
			});
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			WithViewModel((StickerStoreViewModel vm) =>
			{
				if (e.PropertyName == "IsLoadingAvailable")
				{
					UpdateAvailableStickerPacksProgress(vm);
				}
				else if (e.PropertyName == "IsLoadingOwned")
				{
					UpdateOwnedStickerPacksProgress(vm);
				}
			});
		}

		private void OnStickerPackRemoved(object sender, StickerPackViewModel stickerPack)
		{
			WithViewModel((StickerStoreViewModel vm) =>
			{
				vm.RemoveStickerPack(stickerPack).DoNotWait();
			});
		}

		private void OnStickerPackClicked(object sender, StickerPackViewModel stickerPack)
		{
			WithViewModel((StickerStoreViewModel vm) =>
			{
				vm.NavigateToStickerPack(stickerPack);
			});
		}

	}
}

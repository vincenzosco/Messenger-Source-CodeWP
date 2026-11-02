using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Pages
{
	public partial class StickerStorePackPage : StickerPackViewModelPage
	{
		public static readonly DependencyProperty DownloadStateProperty = DependencyProperty.Register("DownloadState", typeof(StickerPackViewModel.DownloadState), typeof(StickerStorePackPage), new PropertyMetadata((object)StickerPackViewModel.DownloadState.Available, (PropertyChangedCallback)OnDownloadStatePropertyChanged));












		public StickerPackViewModel.DownloadState DownloadState
		{
			get
			{
				return (StickerPackViewModel.DownloadState)base.GetValue(DownloadStateProperty);
			}
			set
			{
				base.SetValue(DownloadStateProperty, (object)value);
			}
		}

		public StickerStorePackPage()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected Obj, but got Unknown
			InitializeComponent();
			base.Loaded += OnLoaded;
			base.Unloaded += OnUnLoaded;
			base.SetBinding(DownloadStateProperty, new Binding("StickerPack.State"));
		}

		private static void OnDownloadStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((StickerStorePackPage)(object)d).UpdateButtons();
		}

		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			WithViewModel((StickerStorePackViewModel vm) =>
			{
				RemoveEvents(vm);
				AddEvents(vm);
				vm.FetchStickerPack().DoNotWait();
			});
			UpdateButtons();
		}

		private void OnUnLoaded(object sender, RoutedEventArgs args)
		{
			WithViewModel((StickerStorePackViewModel vm) =>
			{
				RemoveEvents(vm);
			});
		}

		private void AddEvents(StickerStorePackViewModel vm)
		{
			vm.PropertyChanged += OnPropertyChanged;
		}

		private void RemoveEvents(StickerStorePackViewModel vm)
		{
			vm.PropertyChanged -= OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "IsLoading")
			{
				UpdateButtons();
			}
		}

		private void UpdateButtons()
		{
			WithViewModel((StickerStorePackViewModel vm) =>
			{
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				if (vm.StickerPack != null)
				{
					bool flag = vm.StickerPack.State == StickerPackViewModel.DownloadState.Downloaded;
					bool visible = vm.StickerPack.State == StickerPackViewModel.DownloadState.Downloading;
					bool canRemove = vm.StickerPack.CanRemove;
					bool isEnabled = vm.StickerPack.CanDownload && !vm.IsLoading;
					((UIElement)DownloadButton).Visibility = VisibilityUtil.FromBool(!flag);
					((Control)DownloadButton).IsEnabled = isEnabled;
					((UIElement)DownloadProgressBar).Visibility = VisibilityUtil.FromBool(visible);
					((UIElement)RemoveButton).Visibility = VisibilityUtil.FromBool(canRemove);
				}
			});
		}

		private void OnDownloadButtonClicked(object sender, RoutedEventArgs e)
		{
			WithViewModel((StickerStorePackViewModel vm) =>
			{
				vm.DownloadStickerPack().DoNotWait();
			});
		}

		private async void OnRemoveButtonClicked(object sender, RoutedEventArgs e)
		{
			await WithViewModel(async (StickerStorePackViewModel vm) =>
			{
				if (!(await vm.DeleteStickerPack()))
				{
					MessageBox.Show(StringLibrary.Sticker_Store_Remove_Error);
				}
			});
		}


		static StickerStorePackPage()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected Obj, but got Unknown
		}
	}
}

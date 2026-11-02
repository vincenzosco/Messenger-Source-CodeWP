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
	public class StickerStorePackPage : StickerPackViewModelPage
	{
		public static readonly DependencyProperty DownloadStateProperty = DependencyProperty.Register("DownloadState", typeof(StickerPackViewModel.DownloadState), typeof(StickerStorePackPage), new PropertyMetadata((object)StickerPackViewModel.DownloadState.Available, (PropertyChangedCallback)OnDownloadStatePropertyChanged));

		internal Grid LayoutRoot;

		internal TextBlock ArtistTextBlock;

		internal TextBlock NameTextBlock;

		internal TextBlock DescriptionTextBlock;

		internal FBMessengerProgressBar DownloadProgressBar;

		internal Image PreviewImage;

		internal Button DownloadButton;

		internal TextBlock DownloadButtonTextBlock;

		internal Button RemoveButton;

		internal TextBlock RemoveButtonTextBlock;

		private bool _contentLoaded;

		public StickerPackViewModel.DownloadState DownloadState
		{
			get
			{
				return (StickerPackViewModel.DownloadState)((DependencyObject)this).GetValue(DownloadStateProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DownloadStateProperty, (object)value);
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
			((FrameworkElement)this).Loaded += OnLoaded;
			((FrameworkElement)this).Unloaded += OnUnLoaded;
			((FrameworkElement)this).SetBinding(DownloadStateProperty, new Binding("StickerPack.State"));
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
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected Obj, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/StickerStorePackPage.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				ArtistTextBlock = (TextBlock)((FrameworkElement)this).FindName("ArtistTextBlock");
				NameTextBlock = (TextBlock)((FrameworkElement)this).FindName("NameTextBlock");
				DescriptionTextBlock = (TextBlock)((FrameworkElement)this).FindName("DescriptionTextBlock");
				DownloadProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("DownloadProgressBar");
				PreviewImage = (Image)((FrameworkElement)this).FindName("PreviewImage");
				DownloadButton = (Button)((FrameworkElement)this).FindName("DownloadButton");
				DownloadButtonTextBlock = (TextBlock)((FrameworkElement)this).FindName("DownloadButtonTextBlock");
				RemoveButton = (Button)((FrameworkElement)this).FindName("RemoveButton");
				RemoveButtonTextBlock = (TextBlock)((FrameworkElement)this).FindName("RemoveButtonTextBlock");
			}
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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls.Stickers;

public class StickerSelector : StickerPackViewModelControl
{
	internal Grid StickersContainer;

	internal FBMessengerProgressBar StickerLoadingProgressBar;

	internal FacebookListView StickerList;

	internal Grid StickerPackInfoGrid;

	internal Grid ProgressBarGrid;

	internal FBMessengerProgressBar DownloadProgressBar;

	internal Button DownloadButton;

	internal TextBlock DownloadButtonTextBlock;

	private bool _contentLoaded;

	public event EventHandler<StickerViewModel> StickerSelected;

	public event EventHandler<StickerPackViewModel> DownloadPackSelected;

	public StickerSelector()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
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
		if (val != null && val.DataContext is StickerViewModel e)
		{
			StickerSelected?.Invoke(this, e);
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

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected Obj, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Stickers/StickerSelector.xaml", UriKind.Relative));
			StickersContainer = (Grid)((FrameworkElement)this).FindName("StickersContainer");
			StickerLoadingProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("StickerLoadingProgressBar");
			StickerList = (FacebookListView)((FrameworkElement)this).FindName("StickerList");
			StickerPackInfoGrid = (Grid)((FrameworkElement)this).FindName("StickerPackInfoGrid");
			ProgressBarGrid = (Grid)((FrameworkElement)this).FindName("ProgressBarGrid");
			DownloadProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("DownloadProgressBar");
			DownloadButton = (Button)((FrameworkElement)this).FindName("DownloadButton");
			DownloadButtonTextBlock = (TextBlock)((FrameworkElement)this).FindName("DownloadButtonTextBlock");
		}
	}
}

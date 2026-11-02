using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls.Stickers;

public class StickerDrawer : ComposerDrawer
{
	private const string PreferenceSelectedStickerPackIdKey = "SelectedStickerPackId";

	private string _targetStickerPackId;

	internal Style StickerPackPanelImage;

	internal Style StickerPackPanelElement;

	internal Style StickerSelectorItemStyle;

	internal Image NewStickersIcon;

	internal FBMessengerProgressBar StickerPacksLoadingProgressBar;

	internal ListBox StickerPackSelector;

	internal Pivot StickerPivot;

	internal StickerSelector StickersContainer;

	private bool _contentLoaded;

	public StickerDrawer()
		: base(DrawerType.Sticker)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected Obj, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Unloaded += OnUnloaded;
		((FrameworkElement)this).Loaded += OnLoaded;
	}

	protected override void ViewModelUpdated(object oldViewModel)
	{
		if (oldViewModel is StickerDrawerViewModel vm)
		{
			RemoveEvents(vm);
		}
		((FrameworkElement)(object)this).CallWithViewModel(async (StickerDrawerViewModel stickerDrawerViewModel) =>
		{
			AddEvents(stickerDrawerViewModel);
			await stickerDrawerViewModel.FetchStickerPacks();
			SelectCorrectStickerPack();
		});
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		((FrameworkElement)(object)this).CallWithViewModel((StickerDrawerViewModel vm) =>
		{
			AddEvents(vm);
			vm.FetchHasNewStickerPacks();
		});
		UpdateStickerPacksProgressBar();
		SelectCorrectStickerPack();
	}

	private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
	{
		((FrameworkElement)(object)this).CallWithViewModel<StickerDrawerViewModel>(RemoveEvents);
	}

	private void AddEvents(StickerDrawerViewModel vm)
	{
		RemoveEvents(vm);
		vm.PropertyChanged += OnPropertyChanged;
		vm.StickerPacksUpdated += OnStickerPacksUpdated;
	}

	private void RemoveEvents(StickerDrawerViewModel vm)
	{
		vm.PropertyChanged -= OnPropertyChanged;
		vm.StickerPacksUpdated -= OnStickerPacksUpdated;
	}

	public override void Show()
	{
		((UIElement)this).Visibility = (Visibility)0;
		UpdateStickerPacksProgressBar();
	}

	public override void Hide()
	{
		((UIElement)this).Visibility = (Visibility)1;
		UpdateStickerPacksProgressBar();
	}

	public override void Cleanup()
	{
	}

	public override void RestorePreferences(IDictionary<string, string> preferences)
	{
		if (preferences != null && preferences.ContainsKey("SelectedStickerPackId"))
		{
			_targetStickerPackId = preferences["SelectedStickerPackId"];
			SelectCorrectStickerPack();
		}
	}

	public override IDictionary<string, string> GetPreferences()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		StickerPackViewModel stickerPackViewModel = (StickerPackViewModel)((Selector)StickerPackSelector).SelectedItem;
		if (stickerPackViewModel != null)
		{
			dictionary.Add("SelectedStickerPackId", stickerPackViewModel.Id);
		}
		return dictionary;
	}

	public override IList<string> GetPreferenceKeys()
	{
		List<string> list = new List<string>();
		list.Add("SelectedStickerPackId");
		return list;
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == "IsLoading")
		{
			UpdateStickerPacksProgressBar();
		}
	}

	private void OnStickerPacksUpdated(object sender, EventArgs e)
	{
		UpdateStickerPacksProgressBar();
		SelectCorrectStickerPack();
	}

	private void PivotSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected Obj, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		PivotItem val = (PivotItem)e.RemovedItems[0];
		PivotItem val2 = (PivotItem)e.AddedItems[0];
		if (val != null && val2 != null && ((IEnumerable<object>)((ItemsControl)StickerPackSelector).Items).Any())
		{
			int num = ((PresentationFrameworkCollection<object>)(object)((ItemsControl)StickerPivot).Items).IndexOf((object)val);
			int num2 = ((PresentationFrameworkCollection<object>)(object)((ItemsControl)StickerPivot).Items).IndexOf((object)val2);
			bool flag = (num == 0 && num2 == 1) || (num == 1 && num2 == 2) || (num == 2 && num2 == 0);
			((ContentControl)val).Content = null;
			((ContentControl)val2).Content = StickersContainer;
			int num3 = (flag ? 1 : (-1));
			int num4 = (((Selector)StickerPackSelector).SelectedIndex + num3 + ((PresentationFrameworkCollection<object>)(object)((ItemsControl)StickerPackSelector).Items).Count) % ((PresentationFrameworkCollection<object>)(object)((ItemsControl)StickerPackSelector).Items).Count;
			StickerPackViewModel stickerPackViewModel = ((PresentationFrameworkCollection<object>)(object)((ItemsControl)StickerPackSelector).Items)[num4] as StickerPackViewModel;
			if (stickerPackViewModel != null)
			{
				_targetStickerPackId = stickerPackViewModel.Id;
			}
			((Selector)StickerPackSelector).SelectedItem = stickerPackViewModel;
		}
	}

	private void StickerStoreButton_OnTap(object sender, GestureEventArgs e)
	{
		((FrameworkElement)(object)this).CallWithViewModel((StickerDrawerViewModel vm) =>
		{
			vm.OpenStickerStoreCommand.Execute(null);
		});
	}

	private void StickersContainer_OnStickerSelected(object sender, StickerViewModel stickerItemViewModel)
	{
		((FrameworkElement)(object)this).CallWithViewModel((StickerDrawerViewModel vm) =>
		{
			vm.SendStickerCommand.Execute(stickerItemViewModel);
		});
	}

	private void StickersContainer_OnDownloadPackSelected(object sender, StickerPackViewModel stickerPackViewModel)
	{
		((FrameworkElement)(object)this).CallWithViewModel((StickerDrawerViewModel vm) =>
		{
			vm.DownloadStickerPackCommand.Execute(stickerPackViewModel);
		});
	}

	private void StickerPackSelectorList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		StickerPackViewModel stickerPackViewModel = (StickerPackViewModel)((Selector)StickerPackSelector).SelectedItem;
		if (stickerPackViewModel == null)
		{
			SelectCorrectStickerPack();
		}
		else
		{
			StickerPackSelector.ScrollIntoView(e.AddedItems[0]);
		}
	}

	private void UpdateStickerPacksProgressBar()
	{
		((FrameworkElement)(object)this).CallWithViewModel((StickerDrawerViewModel vm) =>
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			bool flag = (int)((UIElement)this).Visibility == 0;
			bool flag2 = vm.IsLoading && flag;
			((UIElement)StickerPacksLoadingProgressBar).Visibility = VisibilityUtil.FromBool(flag2);
			StickerPacksLoadingProgressBar.IsIndeterminate = flag2;
		});
	}

	private void SelectCorrectStickerPack()
	{
		List<StickerPackViewModel> source = ((IEnumerable)((ItemsControl)StickerPackSelector).Items).Cast<StickerPackViewModel>().ToList();
		StickerPackViewModel target = source.SingleOrDefault((StickerPackViewModel pack) => pack.Id == _targetStickerPackId) ?? source.FirstOrDefault();
		if (target != null && ((Selector)StickerPackSelector).SelectedItem != target)
		{
			_targetStickerPackId = target.Id;
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)(() =>
			{
				((Selector)StickerPackSelector).SelectedItem = target;
			}));
		}
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Stickers/StickerDrawer.xaml", UriKind.Relative));
			StickerPackPanelImage = (Style)((FrameworkElement)this).FindName("StickerPackPanelImage");
			StickerPackPanelElement = (Style)((FrameworkElement)this).FindName("StickerPackPanelElement");
			StickerSelectorItemStyle = (Style)((FrameworkElement)this).FindName("StickerSelectorItemStyle");
			NewStickersIcon = (Image)((FrameworkElement)this).FindName("NewStickersIcon");
			StickerPacksLoadingProgressBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("StickerPacksLoadingProgressBar");
			StickerPackSelector = (ListBox)((FrameworkElement)this).FindName("StickerPackSelector");
			StickerPivot = (Pivot)((FrameworkElement)this).FindName("StickerPivot");
			StickersContainer = (StickerSelector)((FrameworkElement)this).FindName("StickersContainer");
		}
	}
}

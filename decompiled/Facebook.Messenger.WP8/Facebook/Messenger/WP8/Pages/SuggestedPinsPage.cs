using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Facebook.Base.Extensions;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Converters;
using Facebook.Messenger.WP8.ViewModels;
using Windows.Foundation;

namespace Facebook.Messenger.WP8.Pages;

public class SuggestedPinsPage : SuggestedPinsViewModelPage
{
	private readonly double _searchListBottomMargin;

	internal Grid LayoutRoot;

	internal StateIndicatingProgressBar StateProgressBar;

	internal HintTextBox SearchGroupsTextBox;

	internal FacebookListView SearchList;

	internal TextBlock EmptyStateTextBlock;

	private bool _contentLoaded;

	public SuggestedPinsPage()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		RegisterInputElements(new List<UIElement> { (UIElement)(object)SearchGroupsTextBox });
		Thickness margin = ((FrameworkElement)SearchList).Margin;
		_searchListBottomMargin = margin.Bottom;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected Obj, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected Obj, but got Unknown
		TextBlock emptyStateTextBlock = EmptyStateTextBlock;
		DependencyProperty visibilityProperty = UIElement.VisibilityProperty;
		Binding val = new Binding();
		val.Path = new PropertyPath("ContactSearchState", new object[0]);
		val.Converter = (IValueConverter)(object)new SearchStateToVisibilityConverter();
		((FrameworkElement)emptyStateTextBlock).SetBinding(visibilityProperty, val);
	}

	protected override void OnKeyboardVisibilityChanged(bool showing, Rect occludedRect)
	{
		double bottom = (showing ? occludedRect.Height : _searchListBottomMargin);
		UpdateSearchListBottom(bottom);
	}

	private void UpdateSearchListBottom(double bottom)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Thickness margin = ((FrameworkElement)SearchList).Margin;
		((FrameworkElement)SearchList).Margin = new Thickness(margin.Left, margin.Top, margin.Right, bottom);
	}

	private void OnItemTapped(object sender, GestureEventArgs e)
	{
		if (!((Control)SearchGroupsTextBox).IsEnabled)
		{
			return;
		}
		FrameworkElement element = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
		if (element != null)
		{
			WithViewModel(async (SuggestedPinsViewModel vm) =>
			{
				((Control)SearchGroupsTextBox).IsEnabled = false;
				StateProgressBar.SetState(StringLibrary.SuggestedPins_Pinning_StateIndicator);
				await vm.PinThread(element.DataContext as ThreadViewModel);
				((Control)SearchGroupsTextBox).IsEnabled = true;
				StateProgressBar.Reset();
			}).DoNotWait();
		}
	}

	private void SearchGroupsTextBox_OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
	{
		WithViewModel((SuggestedPinsViewModel vm) =>
		{
			vm.SearchFilter = SearchGroupsTextBox.Text;
		});
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Pages/SuggestedPinsPage.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			StateProgressBar = (StateIndicatingProgressBar)((FrameworkElement)this).FindName("StateProgressBar");
			SearchGroupsTextBox = (HintTextBox)((FrameworkElement)this).FindName("SearchGroupsTextBox");
			SearchList = (FacebookListView)((FrameworkElement)this).FindName("SearchList");
			EmptyStateTextBlock = (TextBlock)((FrameworkElement)this).FindName("EmptyStateTextBlock");
		}
	}
}

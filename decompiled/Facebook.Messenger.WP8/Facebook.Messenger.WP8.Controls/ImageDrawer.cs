using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Facebook.Base.Logging;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls
{
	public class ImageDrawer : UserControl
	{
		public static readonly DependencyProperty EmptyListTextProperty = DependencyProperty.Register("EmptyListText", typeof(string), typeof(ImageDrawer), new PropertyMetadata((object)null));

		public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(ImageDrawer), new PropertyMetadata((object)false));

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(ImageDrawer), new PropertyMetadata((object)null));

		internal Grid LayoutRoot;

		internal TextBlock EmptyGalleryText;

		internal FacebookListView ImageList;

		internal FBMessengerProgressBar LoadingBar;

		private bool _contentLoaded;

		public string EmptyListText
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(EmptyListTextProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(EmptyListTextProperty, (object)value);
			}
		}

		public bool IsLoading
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsLoadingProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsLoadingProperty, (object)value);
			}
		}

		public IList ItemsSource
		{
			get
			{
				return (IList)((DependencyObject)this).GetValue(ItemsSourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ItemsSourceProperty, (object)value);
			}
		}

		public event EventHandler<PhotoData> SendClicked;

		public event EventHandler<PhotoData> EditClicked;

		public ImageDrawer()
		{
			InitializeComponent();
			((FrameworkElement)this).DataContext = this;
		}

		public void Reset()
		{
			((DependencyObject)this).Dispatcher.BeginInvoke((Action)ImageList.ScrollToTopElement);
			ResetSelectedItem();
		}

		private void ResetSelectedItem()
		{
			((LongListSelector)ImageList).SelectedItem = null;
		}

		private void SendButtonClicked(object sender, EventArgs e)
		{
			HandleClicked(SendClicked, sender);
		}

		private void EditButtonClicked(object sender, EventArgs e)
		{
			HandleClicked(EditClicked, sender);
		}

		private void HandleClicked(EventHandler<PhotoData> clickEvent, object sender)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			if (clickEvent == null)
			{
				return;
			}
			ImageDrawerDrawerItem imageDrawerDrawerItem = sender as ImageDrawerDrawerItem;
			if (imageDrawerDrawerItem == null)
			{
				return;
			}
			ImageDrawerItemViewModel imageDrawerItemViewModel = ((FrameworkElement)imageDrawerDrawerItem).DataContext as ImageDrawerItemViewModel;
			if (imageDrawerItemViewModel == null || !imageDrawerItemViewModel.CanSend)
			{
				return;
			}
			try
			{
				PhotoData imageToSend = imageDrawerItemViewModel.GetImageToSend();
				if (imageToSend != null)
				{
					clickEvent(this, imageToSend);
				}
			}
			catch (OutOfMemoryException ex)
			{
				Logger.Error(ex.ToString(), "HandleClicked", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Controls\\ImageDrawer.xaml.cs", 95);
				MessageBox.Show(StringLibrary.Error_Gallery_Out_Of_Memory);
			}
			ResetSelectedItem();
		}

		private void OverlayTapped(object sender, EventArgs e)
		{
			ResetSelectedItem();
		}

		private void SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SetSelectedState(e.RemovedItems, false);
			SetSelectedState(e.AddedItems, true);
			if (e.AddedItems != null && e.AddedItems.Count > 0 && e.AddedItems[0] != null)
			{
				ImageList.MoveElementToView(e.AddedItems[0]);
			}
		}

		private void ListboxScrolled(object sender, ViewportChangedEventArgs e)
		{
			ResetSelectedItem();
		}

		private static void SetSelectedState(IEnumerable items, bool selectedState)
		{
			if (items == null)
			{
				return;
			}
			foreach (ImageDrawerItemViewModel item in items.OfType<ImageDrawerItemViewModel>())
			{
				item.IsSelected = selectedState;
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ImageDrawer.xaml", UriKind.Relative));
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				EmptyGalleryText = (TextBlock)((FrameworkElement)this).FindName("EmptyGalleryText");
				ImageList = (FacebookListView)((FrameworkElement)this).FindName("ImageList");
				LoadingBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("LoadingBar");
			}
		}

		static ImageDrawer()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected Obj, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected Obj, but got Unknown
		}
	}
}

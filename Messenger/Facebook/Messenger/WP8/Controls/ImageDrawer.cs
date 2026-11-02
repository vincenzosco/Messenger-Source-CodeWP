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
	public partial class ImageDrawer : UserControl
	{
		public static readonly DependencyProperty EmptyListTextProperty = DependencyProperty.Register("EmptyListText", typeof(string), typeof(ImageDrawer), new PropertyMetadata((object)null));

		public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(ImageDrawer), new PropertyMetadata((object)false));

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(ImageDrawer), new PropertyMetadata((object)null));






		public string EmptyListText
		{
			get
			{
				return (string)base.GetValue(EmptyListTextProperty);
			}
			set
			{
				base.SetValue(EmptyListTextProperty, (object)value);
			}
		}

		public bool IsLoading
		{
			get
			{
				return (bool)base.GetValue(IsLoadingProperty);
			}
			set
			{
				base.SetValue(IsLoadingProperty, (object)value);
			}
		}

		public IList ItemsSource
		{
			get
			{
				return (IList)base.GetValue(ItemsSourceProperty);
			}
			set
			{
				base.SetValue(ItemsSourceProperty, (object)value);
			}
		}

		public event EventHandler<PhotoData> SendClicked;

		public event EventHandler<PhotoData> EditClicked;

		public ImageDrawer()
		{
			InitializeComponent();
			base.DataContext = this;
		}

		public void Reset()
		{
			base.Dispatcher.BeginInvoke((Action)ImageList.ScrollToTopElement);
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

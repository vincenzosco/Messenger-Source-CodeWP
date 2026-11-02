using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace Facebook.Messenger.WP8.Controls.Stickers
{
	public class StickerPackListUserControl : UserControl
	{
		public static readonly DependencyProperty CanRemoveItemsProperty = DependencyProperty.Register("CanRemoveItems", typeof(bool), typeof(StickerPackListUserControl), new PropertyMetadata((object)false));

		public static readonly DependencyProperty StickerPacksProperty = DependencyProperty.Register("StickerPacks", typeof(StickerPackCollection), typeof(StickerPackListUserControl), new PropertyMetadata((PropertyChangedCallback)OnStickerPacksChanged));

		public static readonly DependencyProperty EmptyMessageProperty = DependencyProperty.Register("EmptyMessage", typeof(string), typeof(StickerPackListUserControl), new PropertyMetadata((PropertyChangedCallback)OnEmptyMessageChanged));

		public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(StickerPackListUserControl), new PropertyMetadata((object)false, (PropertyChangedCallback)null));

		internal UserControl StickerPackListUserControlRoot;

		internal Grid LayoutRoot;

		internal TextBlock EmptyTextBlock;

		internal FacebookListView StickerPackList;

		internal FBMessengerProgressBar UpdateProgessBar;

		private bool _contentLoaded;

		public bool CanRemoveItems
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(CanRemoveItemsProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(CanRemoveItemsProperty, (object)value);
			}
		}

		public StickerPackCollection StickerPacks
		{
			get
			{
				return (StickerPackCollection)((DependencyObject)this).GetValue(StickerPacksProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(StickerPacksProperty, (object)value);
			}
		}

		public string EmptyMessage
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(EmptyMessageProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(StickerPacksProperty, (object)value);
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

		public event EventHandler<StickerPackViewModel> StickerPackRemoved;

		public event EventHandler<StickerPackViewModel> StickerPackClicked;

		public StickerPackListUserControl()
		{
			InitializeComponent();
		}

		private static void OnStickerPacksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LongListSelector)((StickerPackListUserControl)(object)d).StickerPackList).ItemsSource = (StickerPackCollection)e.NewValue;
		}

		private static void OnEmptyMessageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			StickerPackListUserControl stickerPackListUserControl = (StickerPackListUserControl)(object)obj;
			string text = (string)args.NewValue;
			stickerPackListUserControl.EmptyTextBlock.Text = text;
			((UIElement)stickerPackListUserControl.EmptyTextBlock).Visibility = VisibilityUtil.FromBool(!string.IsNullOrEmpty(text));
		}

		private void ContextMenu_OnUnloaded(object sender, RoutedEventArgs e)
		{
			ContextMenu contextMenu = sender as ContextMenu;
			if (contextMenu != null)
			{
				((DependencyObject)contextMenu).ClearValue(FrameworkElement.DataContextProperty);
			}
		}

		private void RemoveStickerPack_OnClick(object sender, RoutedEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (StickerPackRemoved != null)
			{
				StickerPackRemoved(this, (StickerPackViewModel)((FrameworkElement)sender).DataContext);
			}
		}

		private void OnStickerPackClicked(object sender, StickerPackViewModel stickerPack)
		{
			if (StickerPackClicked != null)
			{
				StickerPackClicked(this, stickerPack);
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
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Stickers/StickerPackListUserControl.xaml", UriKind.Relative));
				StickerPackListUserControlRoot = (UserControl)((FrameworkElement)this).FindName("StickerPackListUserControlRoot");
				LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
				EmptyTextBlock = (TextBlock)((FrameworkElement)this).FindName("EmptyTextBlock");
				StickerPackList = (FacebookListView)((FrameworkElement)this).FindName("StickerPackList");
				UpdateProgessBar = (FBMessengerProgressBar)((FrameworkElement)this).FindName("UpdateProgessBar");
			}
		}

		static StickerPackListUserControl()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected Obj, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected Obj, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected Obj, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected Obj, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected Obj, but got Unknown
		}
	}
}

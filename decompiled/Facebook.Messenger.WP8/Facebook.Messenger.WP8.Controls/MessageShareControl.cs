using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;
using Facebook.Models;
using Windows.System;

namespace Facebook.Messenger.WP8.Controls
{
	public class MessageShareControl : BaseControl, IResetableView
	{
		private readonly Uri _adminPlaceHolderImageUri = new Uri("/Assets/Messages/AdminMessagePlaceholder.png", UriKind.Relative);

		private static readonly GridLength DefaultHeight = new GridLength(0.0, (GridUnitType)0);

		private static readonly GridLength PrivateShareHeight = new GridLength(0.0, (GridUnitType)1);

		private string _shareLink;

		internal BaseControl MessageShareControlRoot;

		internal Border Border;

		internal Grid MessageShareLayoutRoot;

		internal RowDefinition FirstRow;

		internal Grid ImageGrid;

		internal Image ImageHolder;

		internal TextBlock ShareName;

		internal TextBlock Caption;

		internal TextBlock Description;

		private bool _contentLoaded;

		public MessageShareControl()
		{
			InitializeComponent();
		}

		public void ResetView(bool recycled)
		{
			_shareLink = null;
		}

		public void UpdateView(MessageViewModel m, SolidColorBrush textColor)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			ShareName.Foreground = (Brush)(object)textColor;
			Description.Foreground = (Brush)(object)textColor;
			Caption.Foreground = (Brush)(object)textColor;
			Border.BorderBrush = (Brush)(object)textColor;
			SetCaption(m);
			FirstRow.Height = DefaultHeight;
			if (m.HasSharesHiddenByPrivacy())
			{
				FirstRow.Height = PrivateShareHeight;
				((UIElement)Border).Visibility = (Visibility)0;
				((UIElement)ImageGrid).Visibility = (Visibility)1;
				ShareName.Text = string.Empty;
				Description.Text = StringLibrary.Messages_Share_Not_Viewable;
				((UIElement)Description).Visibility = (Visibility)0;
			}
			else if (m.HasShares())
			{
				MailShare mailShare = m.Shares[0];
				if (mailShare.HasImage())
				{
					((UIElement)ImageGrid).Visibility = (Visibility)0;
					MailShare.Media media = mailShare.MediaList[0];
					ImageCacheExtensions.SetPlaceholderUri((FrameworkElement)(object)ImageHolder, _adminPlaceHolderImageUri);
					ImageCacheExtensions.SetImageUri((FrameworkElement)(object)ImageHolder, new Uri(media.Src, UriKind.Absolute));
					_shareLink = media.Href;
				}
				else
				{
					((UIElement)ImageGrid).Visibility = (Visibility)1;
					_shareLink = m.Shares[0].Href;
				}
				((UIElement)Border).Visibility = VisibilityUtil.FromBool(!string.IsNullOrWhiteSpace(m.Body));
				ShareName.Text = mailShare.Name;
				Description.Text = mailShare.Description;
				((UIElement)Description).Visibility = VisibilityUtil.VisibleIfNotEmpty(Description.Text);
				if (string.IsNullOrWhiteSpace(ShareName.Text) && string.IsNullOrWhiteSpace(Caption.Text) && string.IsNullOrWhiteSpace(Description.Text))
				{
					((UIElement)ImageGrid).Visibility = (Visibility)0;
				}
			}
		}

		private void SetCaption(MessageViewModel m)
		{
			if (m == null || !m.HasShares())
			{
				Caption.Text = string.Empty;
				return;
			}
			MailShare mailShare = m.Shares[0];
			MailShare.Property property = ((mailShare.PropertyList == null) ? null : mailShare.PropertyList.FirstOrDefault());
			bool flag = !string.IsNullOrWhiteSpace(mailShare.Caption) || property == null;
			Caption.Text = (flag ? mailShare.Caption : string.Format("{0}: {1}", property.Name, property.Text));
		}

		private async void MessageShareOnTap(object sender, GestureEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_shareLink))
			{
				e.Handled = true;
				await Launcher.LaunchUriAsync(new Uri(_shareLink));
			}
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected Obj, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected Obj, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected Obj, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/MessageShareControl.xaml", UriKind.Relative));
				MessageShareControlRoot = (BaseControl)((FrameworkElement)this).FindName("MessageShareControlRoot");
				Border = (Border)((FrameworkElement)this).FindName("Border");
				MessageShareLayoutRoot = (Grid)((FrameworkElement)this).FindName("MessageShareLayoutRoot");
				FirstRow = (RowDefinition)((FrameworkElement)this).FindName("FirstRow");
				ImageGrid = (Grid)((FrameworkElement)this).FindName("ImageGrid");
				ImageHolder = (Image)((FrameworkElement)this).FindName("ImageHolder");
				ShareName = (TextBlock)((FrameworkElement)this).FindName("ShareName");
				Caption = (TextBlock)((FrameworkElement)this).FindName("Caption");
				Description = (TextBlock)((FrameworkElement)this).FindName("Description");
			}
		}

		static MessageShareControl()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		}
	}
}

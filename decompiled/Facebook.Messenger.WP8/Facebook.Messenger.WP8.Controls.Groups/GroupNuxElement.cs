using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls.Groups
{
	public class GroupNuxElement : UserControl
	{
		public static readonly DependencyProperty ImageUriProperty = DependencyProperty.Register("ImageUri", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty MembersProperty = DependencyProperty.Register("Members", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty ActivityLabelProperty = DependencyProperty.Register("ActivityLabel", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));

		internal UserControl GroupNuxElementRoot;

		internal RoundImageControl GroupImage;

		internal DynamicTextBlock GroupNameTextBlock;

		internal TextBlock ActivityTextBlock;

		internal DynamicTextBlock GroupMembersTextBlock;

		private bool _contentLoaded;

		public string ImageUri
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(ImageUriProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ImageUriProperty, (object)value);
			}
		}

		public string GroupName
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(GroupNameProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(GroupNameProperty, (object)value);
			}
		}

		public string Members
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(MembersProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(MembersProperty, (object)value);
			}
		}

		public string ActivityLabel
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(ActivityLabelProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ActivityLabelProperty, (object)value);
			}
		}

		public GroupNuxElement()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Groups/GroupNuxElement.xaml", UriKind.Relative));
				GroupNuxElementRoot = (UserControl)((FrameworkElement)this).FindName("GroupNuxElementRoot");
				GroupImage = (RoundImageControl)((FrameworkElement)this).FindName("GroupImage");
				GroupNameTextBlock = (DynamicTextBlock)((FrameworkElement)this).FindName("GroupNameTextBlock");
				ActivityTextBlock = (TextBlock)((FrameworkElement)this).FindName("ActivityTextBlock");
				GroupMembersTextBlock = (DynamicTextBlock)((FrameworkElement)this).FindName("GroupMembersTextBlock");
			}
		}

		static GroupNuxElement()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected Obj, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected Obj, but got Unknown
		}
	}
}

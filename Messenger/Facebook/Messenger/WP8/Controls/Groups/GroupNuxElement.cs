using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls.Groups
{
	public partial class GroupNuxElement : UserControl
	{
		public static readonly DependencyProperty ImageUriProperty = DependencyProperty.Register("ImageUri", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty MembersProperty = DependencyProperty.Register("Members", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty ActivityLabelProperty = DependencyProperty.Register("ActivityLabel", typeof(string), typeof(GroupNuxElement), new PropertyMetadata((PropertyChangedCallback)null));







		public string ImageUri
		{
			get
			{
				return (string)base.GetValue(ImageUriProperty);
			}
			set
			{
				base.SetValue(ImageUriProperty, (object)value);
			}
		}

		public string GroupName
		{
			get
			{
				return (string)base.GetValue(GroupNameProperty);
			}
			set
			{
				base.SetValue(GroupNameProperty, (object)value);
			}
		}

		public string Members
		{
			get
			{
				return (string)base.GetValue(MembersProperty);
			}
			set
			{
				base.SetValue(MembersProperty, (object)value);
			}
		}

		public string ActivityLabel
		{
			get
			{
				return (string)base.GetValue(ActivityLabelProperty);
			}
			set
			{
				base.SetValue(ActivityLabelProperty, (object)value);
			}
		}

		public GroupNuxElement()
		{
			InitializeComponent();
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

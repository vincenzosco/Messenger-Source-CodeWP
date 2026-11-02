using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Controls
{
	public class BubbleContainer : ContentControl
	{
		public static readonly DependencyProperty TopNubVisibilityProperty = DependencyProperty.Register("TopNubVisibility", typeof(Visibility), typeof(BubbleContainer), new PropertyMetadata((object)Visibility.Visible));

		public static readonly DependencyProperty BottomNubVisibilityProperty = DependencyProperty.Register("BottomNubVisibility", typeof(Visibility), typeof(BubbleContainer), new PropertyMetadata((object)Visibility.Collapsed));

		public static readonly DependencyProperty BottomNubOverlayVisibilityProperty = DependencyProperty.Register("BottomNubOverlayVisibility", typeof(Visibility), typeof(BubbleContainer), new PropertyMetadata((object)Visibility.Collapsed));

		public static readonly DependencyProperty BubbleFillProperty = DependencyProperty.Register("BubbleFill", typeof(SolidColorBrush), typeof(BubbleContainer), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty BubbleAlignmentProperty = DependencyProperty.Register("BubbleAlignment", typeof(HorizontalAlignment), typeof(BubbleContainer), new PropertyMetadata((object)(HorizontalAlignment)1));

		public Visibility TopNubVisibility
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Visibility)base.GetValue(TopNubVisibilityProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				base.SetValue(TopNubVisibilityProperty, (object)value);
			}
		}

		public Visibility BottomNubVisibility
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Visibility)base.GetValue(BottomNubVisibilityProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				base.SetValue(BottomNubVisibilityProperty, (object)value);
			}
		}

		public Visibility BottomNubOverlayVisibility
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Visibility)base.GetValue(BottomNubOverlayVisibilityProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				base.SetValue(BottomNubOverlayVisibilityProperty, (object)value);
			}
		}

		public SolidColorBrush BubbleFill
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (SolidColorBrush)base.GetValue(BubbleFillProperty);
			}
			set
			{
				base.SetValue(BubbleFillProperty, (object)value);
			}
		}

		public HorizontalAlignment BubbleAlignment
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (HorizontalAlignment)base.GetValue(BubbleAlignmentProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				base.SetValue(BubbleAlignmentProperty, (object)value);
			}
		}

		public BubbleContainer()
		{
			base.DefaultStyleKey = typeof(BubbleContainer);
		}

		static BubbleContainer()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected Obj, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected Obj, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected Obj, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected Obj, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected Obj, but got Unknown
		}
	}
}

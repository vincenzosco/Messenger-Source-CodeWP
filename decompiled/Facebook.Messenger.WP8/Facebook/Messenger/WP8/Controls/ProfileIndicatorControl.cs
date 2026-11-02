using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Controls
{
	public class ProfileIndicatorControl : UserControl
	{
		public static readonly DependencyProperty IsPartialUserProperty = DependencyProperty.Register("IsPartialUser", typeof(bool), typeof(ProfileIndicatorControl), new PropertyMetadata((object)false, (PropertyChangedCallback)OnIsPartialUserChanged));

		public static readonly DependencyProperty IsMessengerUserProperty = DependencyProperty.Register("IsMessengerUser", typeof(bool), typeof(ProfileIndicatorControl), new PropertyMetadata((object)false, (PropertyChangedCallback)OnIsMessengerUserChanged));

		public static readonly DependencyProperty ImageBorderThicknessProperty = DependencyProperty.Register("ImageBorderThickness", typeof(int), typeof(ProfileIndicatorControl), new PropertyMetadata((object)2, (PropertyChangedCallback)UpdateClipping));

		public static readonly DependencyProperty SetHeightProperty = DependencyProperty.Register("SetHeight", typeof(double), typeof(ProfileIndicatorControl), new PropertyMetadata((object)double.NaN, (PropertyChangedCallback)UpdateClipping));

		public static readonly DependencyProperty SetWidthProperty = DependencyProperty.Register("SetWidth", typeof(double), typeof(ProfileIndicatorControl), new PropertyMetadata((object)double.NaN, (PropertyChangedCallback)UpdateClipping));

		internal Grid Border;

		internal Grid Clipper;

		internal Image FacebookIcon;

		internal Image MessengerIcon;

		private bool _contentLoaded;

		public bool IsPartialUser
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsPartialUserProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsPartialUserProperty, (object)value);
			}
		}

		public bool IsMessengerUser
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsMessengerUserProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsMessengerUserProperty, (object)value);
			}
		}

		public int ImageBorderThickness
		{
			get
			{
				return (int)((DependencyObject)this).GetValue(ImageBorderThicknessProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ImageBorderThicknessProperty, (object)value);
			}
		}

		public double SetHeight
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(SetHeightProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(SetHeightProperty, (object)value);
			}
		}

		public double SetWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(SetWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(SetWidthProperty, (object)value);
			}
		}

		public ProfileIndicatorControl()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected Obj, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected Obj, but got Unknown
			InitializeComponent();
			DependencyProperty setWidthProperty = SetWidthProperty;
			Binding val = new Binding("Width");
			val.Source = this;
			((FrameworkElement)this).SetBinding(setWidthProperty, val);
			DependencyProperty setHeightProperty = SetHeightProperty;
			Binding val2 = new Binding("Height");
			val2.Source = this;
			((FrameworkElement)this).SetBinding(setHeightProperty, val2);
		}

		private static void OnIsMessengerUserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as ProfileIndicatorControl)?.UpdateIcon();
		}

		private static void OnIsPartialUserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as ProfileIndicatorControl)?.UpdateIcon();
		}

		private void UpdateIcon()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			bool flag = !IsMessengerUser && !IsPartialUser;
			((UIElement)MessengerIcon).Visibility = VisibilityUtil.FromBool(IsMessengerUser);
			((UIElement)FacebookIcon).Visibility = VisibilityUtil.FromBool(flag);
			((UIElement)Border).Visibility = VisibilityUtil.FromBool(IsMessengerUser || flag);
		}

		private static void UpdateClipping(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ProfileIndicatorControl)(object)d).UpdateClipping();
		}

		private void UpdateClipping()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected Obj, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected Obj, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			if (!double.IsNaN(((FrameworkElement)this).Width) && !double.IsNaN(((FrameworkElement)this).Height))
			{
				double num = ((FrameworkElement)this).Width / 2.0;
				double num2 = ((FrameworkElement)this).Height / 2.0;
				Grid border = Border;
				EllipseGeometry val = new EllipseGeometry();
				val.Center = new Point(num, num2);
				val.RadiusX = num;
				val.RadiusY = num2;
				((UIElement)border).Clip = (Geometry)(object)val;
				Grid clipper = Clipper;
				EllipseGeometry val2 = new EllipseGeometry();
				val2.Center = new Point(num, num2);
				val2.RadiusX = num - (double)ImageBorderThickness;
				val2.RadiusY = num2 - (double)ImageBorderThickness;
				((UIElement)clipper).Clip = (Geometry)(object)val2;
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
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/ProfileIndicatorControl.xaml", UriKind.Relative));
				Border = (Grid)((FrameworkElement)this).FindName("Border");
				Clipper = (Grid)((FrameworkElement)this).FindName("Clipper");
				FacebookIcon = (Image)((FrameworkElement)this).FindName("FacebookIcon");
				MessengerIcon = (Image)((FrameworkElement)this).FindName("MessengerIcon");
			}
		}

		static ProfileIndicatorControl()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected Obj, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected Obj, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected Obj, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected Obj, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected Obj, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected Obj, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected Obj, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected Obj, but got Unknown
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Expected Obj, but got Unknown
		}
	}
}

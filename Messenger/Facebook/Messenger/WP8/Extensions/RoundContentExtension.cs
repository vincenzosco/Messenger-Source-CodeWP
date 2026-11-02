using System.Windows;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Extensions
{
	public class RoundContentExtension
	{
		public static readonly DependencyProperty RadiusProperty = DependencyProperty.RegisterAttached("Radius", typeof(double), typeof(RoundContentExtension), new PropertyMetadata((object)0.0, (PropertyChangedCallback)OnRadiusChanged));

		public static double GetRadius(FrameworkElement obj)
		{
			return (double)((DependencyObject)obj).GetValue(RadiusProperty);
		}

		public static void SetRadius(FrameworkElement obj, double value)
		{
			((DependencyObject)obj).SetValue(RadiusProperty, (object)value);
		}

		private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			FrameworkElement val = (FrameworkElement)(object)((d is FrameworkElement) ? d : null);
			double num = (e.NewValue as double?) ?? 0.0;
			if (val != null)
			{
				val.Width = 2.0 * num;
				val.Height = 2.0 * num;
				EllipseGeometry val2 = new EllipseGeometry();
				val2.Center = new Point(num, num);
				val2.RadiusX = num;
				val2.RadiusY = num;
				((UIElement)val).Clip = (Geometry)(object)val2;
			}
		}

		static RoundContentExtension()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected Obj, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected Obj, but got Unknown
		}
	}
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Facebook.Messenger.WP8.Controls
{
	public partial class FBMessengerProgressBar : UserControl
	{
		public static DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(FBMessengerProgressBar), new PropertyMetadata((object)false, (PropertyChangedCallback)IsIndeterminatePropertyChanged));

		public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(FBMessengerProgressBar), new PropertyMetadata((object)0.0, (PropertyChangedCallback)ValuePropertyChanged));

		public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(FBMessengerProgressBar), new PropertyMetadata((object)0.0, (PropertyChangedCallback)OnMinimumChanged));

		public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(FBMessengerProgressBar), new PropertyMetadata((object)0.0, (PropertyChangedCallback)OnMaximumChanged));



		public bool IsIndeterminate
		{
			get
			{
				return (bool)base.GetValue(IsIndeterminateProperty);
			}
			set
			{
				base.SetValue(IsIndeterminateProperty, (object)value);
			}
		}

		public double Value
		{
			get
			{
				return (double)base.GetValue(ValueProperty);
			}
			set
			{
				base.SetValue(ValueProperty, (object)value);
			}
		}

		public double Minimum
		{
			get
			{
				return (double)base.GetValue(MinimumProperty);
			}
			set
			{
				base.SetValue(MinimumProperty, (object)value);
			}
		}

		public double Maximum
		{
			get
			{
				return (double)base.GetValue(MaximumProperty);
			}
			set
			{
				base.SetValue(MaximumProperty, (object)value);
			}
		}

		private static void IsIndeterminatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FBMessengerProgressBar fBMessengerProgressBar = d as FBMessengerProgressBar;
			if (fBMessengerProgressBar != null)
			{
				fBMessengerProgressBar.ProgressBar.IsIndeterminate = (e.NewValue as bool?) ?? false;
			}
		}

		private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FBMessengerProgressBar fBMessengerProgressBar = d as FBMessengerProgressBar;
			if (fBMessengerProgressBar != null)
			{
				((RangeBase)fBMessengerProgressBar.ProgressBar).Value = (e.NewValue as double?) ?? 0.0;
			}
		}

		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FBMessengerProgressBar fBMessengerProgressBar = d as FBMessengerProgressBar;
			if (fBMessengerProgressBar != null)
			{
				((RangeBase)fBMessengerProgressBar.ProgressBar).Minimum = (e.NewValue as double?) ?? 0.0;
			}
		}

		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FBMessengerProgressBar fBMessengerProgressBar = d as FBMessengerProgressBar;
			if (fBMessengerProgressBar != null)
			{
				((RangeBase)fBMessengerProgressBar.ProgressBar).Maximum = (e.NewValue as double?) ?? 0.0;
			}
		}

		public FBMessengerProgressBar()
		{
			InitializeComponent();
		}


		static FBMessengerProgressBar()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected Obj, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected Obj, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected Obj, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected Obj, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected Obj, but got Unknown
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Expected Obj, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Expected Obj, but got Unknown
		}
	}
}

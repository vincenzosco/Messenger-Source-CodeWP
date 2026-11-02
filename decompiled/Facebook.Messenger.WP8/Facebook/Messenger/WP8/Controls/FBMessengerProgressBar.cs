using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Facebook.Messenger.WP8.Controls;

public class FBMessengerProgressBar : UserControl
{
	public static DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(FBMessengerProgressBar), new PropertyMetadata((object)false, (PropertyChangedCallback)IsIndeterminatePropertyChanged));

	public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(FBMessengerProgressBar), new PropertyMetadata((object)0.0, (PropertyChangedCallback)ValuePropertyChanged));

	public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(FBMessengerProgressBar), new PropertyMetadata((object)0.0, (PropertyChangedCallback)OnMinimumChanged));

	public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(FBMessengerProgressBar), new PropertyMetadata((object)0.0, (PropertyChangedCallback)OnMaximumChanged));

	internal ProgressBar ProgressBar;

	private bool _contentLoaded;

	public bool IsIndeterminate
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsIndeterminateProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsIndeterminateProperty, (object)value);
		}
	}

	public double Value
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(ValueProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ValueProperty, (object)value);
		}
	}

	public double Minimum
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(MinimumProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(MinimumProperty, (object)value);
		}
	}

	public double Maximum
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(MaximumProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(MaximumProperty, (object)value);
		}
	}

	private static void IsIndeterminatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is FBMessengerProgressBar { ProgressBar: var progressBar })
		{
			progressBar.IsIndeterminate = (e.NewValue as bool?) ?? false;
		}
	}

	private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is FBMessengerProgressBar { ProgressBar: var progressBar })
		{
			((RangeBase)progressBar).Value = (e.NewValue as double?) ?? 0.0;
		}
	}

	private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is FBMessengerProgressBar { ProgressBar: var progressBar })
		{
			((RangeBase)progressBar).Minimum = (e.NewValue as double?) ?? 0.0;
		}
	}

	private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is FBMessengerProgressBar { ProgressBar: var progressBar })
		{
			((RangeBase)progressBar).Maximum = (e.NewValue as double?) ?? 0.0;
		}
	}

	public FBMessengerProgressBar()
	{
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/FBMessengerProgressBar.xaml", UriKind.Relative));
			ProgressBar = (ProgressBar)((FrameworkElement)this).FindName("ProgressBar");
		}
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

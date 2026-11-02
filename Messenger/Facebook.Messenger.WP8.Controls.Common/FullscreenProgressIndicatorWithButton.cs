using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Facebook.Messenger.WP8.Converters;

namespace Facebook.Messenger.WP8.Controls.Common
{
	public partial class FullscreenProgressIndicatorWithButton : UserControl
	{
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FullscreenProgressIndicatorWithButton), new PropertyMetadata((object)string.Empty));

		public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(FullscreenProgressIndicatorWithButton), new PropertyMetadata((object)false));

		public static readonly DependencyProperty ShowButtonProperty = DependencyProperty.Register("ShowButton", typeof(bool), typeof(FullscreenProgressIndicatorWithButton), new PropertyMetadata((object)false));





		public string Text
		{
			get
			{
				return (string)base.GetValue(TextProperty);
			}
			set
			{
				base.SetValue(TextProperty, (object)value);
			}
		}

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

		public bool ShowButton
		{
			get
			{
				return (bool)base.GetValue(ShowButtonProperty);
			}
			set
			{
				base.SetValue(ShowButtonProperty, (object)value);
			}
		}

		public event EventHandler ButtonTapped;

		public FullscreenProgressIndicatorWithButton()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected Obj, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected Obj, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected Obj, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected Obj, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected Obj, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected Obj, but got Unknown
			InitializeComponent();
			TextBlock progressTextBlock = ProgressTextBlock;
			DependencyProperty textProperty = TextBlock.TextProperty;
			Binding val = new Binding();
			val.Source = this;
			val.Path = new PropertyPath("Text", new object[0]);
			((FrameworkElement)progressTextBlock).SetBinding(textProperty, val);
			ProgressBar progressBar = ProgressBar;
			DependencyProperty isIndeterminateProperty = ProgressBar.IsIndeterminateProperty;
			Binding val2 = new Binding();
			val2.Source = this;
			val2.Path = new PropertyPath("IsIndeterminate", new object[0]);
			((FrameworkElement)progressBar).SetBinding(isIndeterminateProperty, val2);
			Button retryButton = RetryButton;
			DependencyProperty visibilityProperty = UIElement.VisibilityProperty;
			Binding val3 = new Binding();
			val3.Source = this;
			val3.Path = new PropertyPath("ShowButton", new object[0]);
			val3.Converter = (IValueConverter)(object)new BoolToVisibilityConverter();
			((FrameworkElement)retryButton).SetBinding(visibilityProperty, val3);
		}

		private void Button_OnTap(object sender, GestureEventArgs e)
		{
			if (ButtonTapped != null)
			{
				ButtonTapped(sender, (EventArgs)(object)e);
			}
		}


		static FullscreenProgressIndicatorWithButton()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected Obj, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected Obj, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected Obj, but got Unknown
		}
	}
}

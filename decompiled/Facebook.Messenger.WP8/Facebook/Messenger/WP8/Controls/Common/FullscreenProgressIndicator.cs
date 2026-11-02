using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Facebook.Messenger.WP8.Converters;

namespace Facebook.Messenger.WP8.Controls.Common
{
	public class FullscreenProgressIndicator : UserControl
	{
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FullscreenProgressIndicator), new PropertyMetadata((object)string.Empty));

		internal TextBlock ProgressTextBlock;

		internal ProgressBar ProgressBar;

		private bool _contentLoaded;

		public string Text
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(TextProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TextProperty, (object)value);
			}
		}

		public FullscreenProgressIndicator()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected Obj, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected Obj, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected Obj, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected Obj, but got Unknown
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
			val2.Path = new PropertyPath("Visibility", new object[0]);
			val2.Converter = (IValueConverter)(object)new VisibilityToBooleanConverter();
			((FrameworkElement)progressBar).SetBinding(isIndeterminateProperty, val2);
		}

		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected Obj, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Common/FullscreenProgressIndicator.xaml", UriKind.Relative));
				ProgressTextBlock = (TextBlock)((FrameworkElement)this).FindName("ProgressTextBlock");
				ProgressBar = (ProgressBar)((FrameworkElement)this).FindName("ProgressBar");
			}
		}

		static FullscreenProgressIndicator()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected Obj, but got Unknown
		}
	}
}

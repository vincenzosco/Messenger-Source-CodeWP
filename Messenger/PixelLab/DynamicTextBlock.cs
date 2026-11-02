using System.Windows;
using System.Windows.Controls;

namespace PixelLab
{
	public class DynamicTextBlock : ContentControl
	{
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DynamicTextBlock), new PropertyMetadata((object)null, (PropertyChangedCallback)OnTextChanged));

		public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(DynamicTextBlock), new PropertyMetadata((object)(TextWrapping)1, (PropertyChangedCallback)OnTextWrappingChanged));

		public static readonly DependencyProperty LineHeightProperty = DependencyProperty.Register("LineHeight", typeof(double), typeof(DynamicTextBlock), new PropertyMetadata((object)0.0, (PropertyChangedCallback)OnLineHeightChanged));

		public static readonly DependencyProperty LineStackingStrategyProperty = DependencyProperty.Register("LineStackingStrategy", typeof(LineStackingStrategy), typeof(DynamicTextBlock), new PropertyMetadata((object)(LineStackingStrategy)1, (PropertyChangedCallback)OnLineStackingStrategyChanged));

		private TextBlock textBlock;

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

		public TextWrapping TextWrapping
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (TextWrapping)base.GetValue(TextWrappingProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				base.SetValue(TextWrappingProperty, (object)value);
			}
		}

		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(LineHeightProperty);
			}
			set
			{
				base.SetValue(LineHeightProperty, (object)value);
			}
		}

		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (LineStackingStrategy)base.GetValue(LineStackingStrategyProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				base.SetValue(LineStackingStrategyProperty, (object)value);
			}
		}

		public string EllipsizedText => textBlock.Text;

		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DynamicTextBlock)(object)d).OnTextChanged(e);
		}

		protected virtual void OnTextChanged(DependencyPropertyChangedEventArgs e)
		{
			base.InvalidateMeasure();
		}

		private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DynamicTextBlock)(object)d).OnTextWrappingChanged(e);
		}

		protected virtual void OnTextWrappingChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			textBlock.TextWrapping = (TextWrapping)e.NewValue;
			base.InvalidateMeasure();
		}

		private static void OnLineHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DynamicTextBlock)(object)d).OnLineHeightChanged(e);
		}

		protected virtual void OnLineHeightChanged(DependencyPropertyChangedEventArgs e)
		{
			textBlock.LineHeight = LineHeight;
			base.InvalidateMeasure();
		}

		private static void OnLineStackingStrategyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DynamicTextBlock)(object)d).OnLineStackingStrategyChanged(e);
		}

		protected virtual void OnLineStackingStrategyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			textBlock.LineStackingStrategy = (LineStackingStrategy)e.NewValue;
			base.InvalidateMeasure();
		}

		public DynamicTextBlock()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected Obj, but got Unknown
			textBlock = new TextBlock();
			base.Content = textBlock;
			base.HorizontalContentAlignment = (HorizontalAlignment)0;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			bool flag = (int)TextWrapping == 2;
			Size val = (flag ? new Size(availableSize.Width, double.PositiveInfinity) : new Size(double.PositiveInfinity, availableSize.Height));
			textBlock.Text = Text;
			Size result = base.MeasureOverride(val);
			if ((flag ? (result.Height < availableSize.Height) : (result.Width < availableSize.Width)) || Text == null)
			{
				return result;
			}
			int num;
			for (num = 1; num < Text.Length; num <<= 1)
			{
			}
			num >>= 1;
			int num2 = num;
			while (num > 0)
			{
				num >>= 1;
				if (num2 > Text.Length)
				{
					num2 -= num;
					continue;
				}
				string text = Text.Substring(0, num2);
				textBlock.Text = text + "...";
				result = base.MeasureOverride(val);
				num2 = ((!(flag ? (result.Height > availableSize.Height) : (result.Width > availableSize.Width))) ? (num2 + num) : (num2 - ((num == 0) ? 1 : num)));
			}
			textBlock.Text = Text.Substring(0, num2) + "...";
			return base.MeasureOverride(availableSize);
		}

		static DynamicTextBlock()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected Obj, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected Obj, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected Obj, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected Obj, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected Obj, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected Obj, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected Obj, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected Obj, but got Unknown
		}
	}
}

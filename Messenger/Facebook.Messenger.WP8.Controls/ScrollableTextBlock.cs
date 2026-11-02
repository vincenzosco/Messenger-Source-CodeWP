using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Controls
{
	public class ScrollableTextBlock : Control
	{
		private const double DefaultPadding = 20.0;

		private StackPanel stackPanel;

		private TextBlock measureBlock;

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ScrollableTextBlock), new PropertyMetadata((object)"ScrollableTextBlock", (PropertyChangedCallback)OnTextPropertyChanged));

		public static readonly DependencyProperty RealBackgroundProperty = DependencyProperty.Register("RealBackground", typeof(SolidColorBrush), typeof(ScrollableTextBlock), new PropertyMetadata((object)new SolidColorBrush(Colors.White), (PropertyChangedCallback)OnRealBackgroundPropertyChanged));

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

		public SolidColorBrush RealBackground
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (SolidColorBrush)base.GetValue(RealBackgroundProperty);
			}
			set
			{
				base.SetValue(RealBackgroundProperty, (object)value);
			}
		}

		public ScrollableTextBlock()
		{
			base.DefaultStyleKey = typeof(ScrollableTextBlock);
		}

		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollableTextBlock scrollableTextBlock = (ScrollableTextBlock)(object)d;
			string value = (string)e.NewValue;
			scrollableTextBlock.ParseText(value);
		}

		private static void OnRealBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected Obj, but got Unknown
			ScrollableTextBlock scrollableTextBlock = (ScrollableTextBlock)(object)d;
			((Control)scrollableTextBlock).Background = (Brush)(SolidColorBrush)e.NewValue;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			DependencyObject templateChild = base.GetTemplateChild("StackPanel");
			stackPanel = (StackPanel)(object)((templateChild is StackPanel) ? templateChild : null);
			ParseText(Text);
		}

		private void ParseText(string value)
		{
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			StringReader stringReader = new StringReader(value);
			if (stackPanel == null)
			{
				return;
			}
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)stackPanel).Children).Clear();
			int maxTextSize = GetMaxTextSize();
			Border val = null;
			if (value.Length < maxTextSize)
			{
				TextBlock textBlock = GetTextBlock();
				textBlock.Text = value;
				val = WrapWithBorder((UIElement)(object)textBlock, true);
				((PresentationFrameworkCollection<UIElement>)(object)((Panel)stackPanel).Children).Add((UIElement)(object)val);
			}
			else
			{
				string text = "";
				bool first = true;
				while (stringReader.Peek() > 0)
				{
					text = stringReader.ReadLine();
					val = ParseLine(text, first);
					first = false;
				}
			}
			if (val != null)
			{
				Border val2 = val;
				Thickness padding = val.Padding;
				double left = padding.Left;
				Thickness padding2 = val.Padding;
				double top = padding2.Top;
				Thickness padding3 = val.Padding;
				val2.Padding = new Thickness(left, top, padding3.Right, 20.0);
			}
		}

		private Border ParseLine(string line, bool first)
		{
			int i = 0;
			int maxLineCount = GetMaxLineCount();
			string text = line;
			StringBuilder stringBuilder = new StringBuilder();
			for (; i < maxLineCount; i++)
			{
				int num = MeasureString(text, (int)base.Width);
				string value = text.Substring(0, num);
				stringBuilder.Append(value);
				text = text.Substring(num, text.Length - num);
			}
			TextBlock textBlock = GetTextBlock();
			textBlock.Text = stringBuilder.ToString();
			Border val = WrapWithBorder((UIElement)(object)textBlock, first);
			((PresentationFrameworkCollection<UIElement>)(object)((Panel)stackPanel).Children).Add((UIElement)(object)val);
			if (text.Length > 0)
			{
				ParseLine(text, first);
			}
			return val;
		}

		private int MeasureString(string text, int desWidth)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			int num2 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			Size val = MeasureString(text);
			if (val.Width > (double)desWidth)
			{
				string[] array = text.Split(' ');
				stringBuilder.Append(array[0]);
				for (int i = 1; i < array.Length; i++)
				{
					stringBuilder.Append(" " + array[i]);
					Size val2 = MeasureString(stringBuilder.ToString());
					num = (int)val2.Width;
					if (num > desWidth)
					{
						stringBuilder.Remove(stringBuilder.Length - array[i].Length, array[i].Length);
						break;
					}
				}
				return stringBuilder.Length;
			}
			return text.Length;
		}

		private Size MeasureString(string text)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (measureBlock == null)
			{
				measureBlock = GetTextBlock();
			}
			measureBlock.Text = text;
			return new Size(((FrameworkElement)measureBlock).ActualWidth, ((FrameworkElement)measureBlock).ActualHeight);
		}

		private int GetMaxTextSize()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Size val = MeasureText(" ");
			int num = (int)(base.Width / val.Width);
			int num2 = (int)(2048.0 / val.Height);
			return num * num2 / 2;
		}

		private int GetMaxLineCount()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Size val = MeasureText(" ");
			double num = base.Width / val.Width;
			return (int)(2048.0 / val.Height) - 5;
		}

		private Border WrapWithBorder(UIElement control, bool firstControl)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			Border val = new Border();
			if (firstControl)
			{
				val.Padding = new Thickness(20.0, 20.0, 20.0, 0.0);
			}
			else
			{
				val.Padding = new Thickness(20.0, 0.0, 20.0, 0.0);
			}
			((FrameworkElement)val).Margin = new Thickness(0.0, -1.0, 0.0, 0.0);
			val.Background = base.Background;
			val.Child = control;
			return val;
		}

		private TextBlock GetTextBlock()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			TextBlock val = new TextBlock();
			val.TextWrapping = (TextWrapping)2;
			val.FontSize = base.FontSize;
			val.FontFamily = base.FontFamily;
			val.FontWeight = base.FontWeight;
			val.Foreground = base.Foreground;
			Size val2 = MeasureText(" ");
			((FrameworkElement)val).Margin = new Thickness(0.0, 0.0, val2.Width, 0.0);
			return val;
		}

		private Size MeasureText(string value)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			TextBlock textBlockOnly = GetTextBlockOnly();
			textBlockOnly.Text = value;
			return new Size(((FrameworkElement)textBlockOnly).ActualWidth, ((FrameworkElement)textBlockOnly).ActualHeight);
		}

		private TextBlock GetTextBlockOnly()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected Obj, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			TextBlock val = new TextBlock();
			val.TextWrapping = (TextWrapping)2;
			val.FontSize = base.FontSize;
			val.FontFamily = base.FontFamily;
			val.FontWeight = base.FontWeight;
			return val;
		}

		static ScrollableTextBlock()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected Obj, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected Obj, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected Obj, but got Unknown
			//IL_006d: Expected Obj, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected Obj, but got Unknown
		}
	}
}

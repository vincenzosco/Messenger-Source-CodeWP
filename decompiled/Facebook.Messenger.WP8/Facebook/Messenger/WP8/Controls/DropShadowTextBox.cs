using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Facebook.Messenger.WP8.Controls;

public class DropShadowTextBox : UserControl
{
	public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(DropShadowTextBox), new PropertyMetadata((object)(TextAlignment)1));

	public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(DropShadowTextBox), new PropertyMetadata((object)(TextWrapping)1));

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DropShadowTextBox), new PropertyMetadata((object)string.Empty, (PropertyChangedCallback)TextPropertyChangedCallback));

	private RoutedEventHandler m_GotFocus;

	private RoutedEventHandler m_LostFocus;

	private TextChangedEventHandler m_TextChanged;

	internal Grid LayoutRoot;

	internal TextBox ShadowTextBox;

	internal TextBox InputTextBox;

	private bool _contentLoaded;

	public TextAlignment TextAlignment
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (TextAlignment)((DependencyObject)this).GetValue(TextAlignmentProperty);
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DependencyObject)this).SetValue(TextAlignmentProperty, (object)value);
		}
	}

	public TextWrapping TextWrapping
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (TextWrapping)((DependencyObject)this).GetValue(TextWrappingProperty);
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DependencyObject)this).SetValue(TextWrappingProperty, (object)value);
		}
	}

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

	public event RoutedEventHandler GotFocus
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			RoutedEventHandler val = this.m_GotFocus;
			RoutedEventHandler val2;
			do
			{
				val2 = val;
				RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Combine((Delegate)(object)val2, (Delegate)(object)value);
				val = Interlocked.CompareExchange(ref this.m_GotFocus, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			RoutedEventHandler val = this.m_GotFocus;
			RoutedEventHandler val2;
			do
			{
				val2 = val;
				RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Remove((Delegate)(object)val2, (Delegate)(object)value);
				val = Interlocked.CompareExchange(ref this.m_GotFocus, value2, val2);
			}
			while (val != val2);
		}
	}

	public event RoutedEventHandler LostFocus
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			RoutedEventHandler val = this.m_LostFocus;
			RoutedEventHandler val2;
			do
			{
				val2 = val;
				RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Combine((Delegate)(object)val2, (Delegate)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LostFocus, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			RoutedEventHandler val = this.m_LostFocus;
			RoutedEventHandler val2;
			do
			{
				val2 = val;
				RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Remove((Delegate)(object)val2, (Delegate)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LostFocus, value2, val2);
			}
			while (val != val2);
		}
	}

	public event TextChangedEventHandler TextChanged
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			TextChangedEventHandler val = this.m_TextChanged;
			TextChangedEventHandler val2;
			do
			{
				val2 = val;
				TextChangedEventHandler value2 = (TextChangedEventHandler)Delegate.Combine((Delegate)(object)val2, (Delegate)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TextChanged, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected Obj, but got Unknown
			TextChangedEventHandler val = this.m_TextChanged;
			TextChangedEventHandler val2;
			do
			{
				val2 = val;
				TextChangedEventHandler value2 = (TextChangedEventHandler)Delegate.Remove((Delegate)(object)val2, (Delegate)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TextChanged, value2, val2);
			}
			while (val != val2);
		}
	}

	public DropShadowTextBox()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).DataContext = this;
	}

	private static void TextPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
	{
		if (dependencyObject is DropShadowTextBox dropShadowTextBox)
		{
			string text = (dependencyPropertyChangedEventArgs.NewValue as string) ?? string.Empty;
			TextBox inputTextBox = dropShadowTextBox.InputTextBox;
			string text2 = (dropShadowTextBox.ShadowTextBox.Text = text);
			inputTextBox.Text = text2;
		}
	}

	private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected Obj, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected Obj, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected Obj, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected Obj, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected Obj, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected Obj, but got Unknown
		((FrameworkElement)ShadowTextBox).SetBinding(Control.FontSizeProperty, new Binding("FontSize"));
		((FrameworkElement)ShadowTextBox).SetBinding(Control.PaddingProperty, new Binding("Padding"));
		((FrameworkElement)InputTextBox).SetBinding(Control.ForegroundProperty, new Binding("Foreground"));
		((FrameworkElement)InputTextBox).SetBinding(Control.FontSizeProperty, new Binding("FontSize"));
		((FrameworkElement)InputTextBox).SetBinding(Control.IsEnabledProperty, new Binding("IsEnabled"));
		((FrameworkElement)InputTextBox).SetBinding(Control.PaddingProperty, new Binding("Padding"));
	}

	protected override void OnGotFocus(RoutedEventArgs e)
	{
		((Control)InputTextBox).Focus();
	}

	private void InputTextBox_OnGotFocus(object sender, RoutedEventArgs e)
	{
		if (this.m_GotFocus != null)
		{
			this.m_GotFocus.Invoke((object)this, e);
		}
	}

	private void InputTextBox_OnLostFocus(object sender, RoutedEventArgs e)
	{
		if (this.m_LostFocus != null)
		{
			this.m_LostFocus.Invoke((object)this, e);
		}
	}

	private void InputTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
	{
		string text = (ShadowTextBox.Text = InputTextBox.Text);
		Text = text;
		if (this.m_TextChanged != null)
		{
			this.m_TextChanged.Invoke((object)this, e);
		}
	}

	public void ClearText()
	{
		TextBox shadowTextBox = ShadowTextBox;
		string text = (InputTextBox.Text = string.Empty);
		string text2 = (shadowTextBox.Text = text);
		Text = text2;
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
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/DropShadowTextBox.xaml", UriKind.Relative));
			LayoutRoot = (Grid)((FrameworkElement)this).FindName("LayoutRoot");
			ShadowTextBox = (TextBox)((FrameworkElement)this).FindName("ShadowTextBox");
			InputTextBox = (TextBox)((FrameworkElement)this).FindName("InputTextBox");
		}
	}

	static DropShadowTextBox()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected Obj, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected Obj, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected Obj, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected Obj, but got Unknown
	}
}

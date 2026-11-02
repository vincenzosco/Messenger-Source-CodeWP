using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Facebook.Messenger.WP8.Controls.Base
{
	public partial class HintTextBox : BaseControl
	{
		private int? _lastCaretPosition;

		private RoutedEventHandler m_TextSelectionChanged;

		private KeyEventHandler m_TextKeyUp;

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(HintTextBox), new PropertyMetadata((object)string.Empty, (PropertyChangedCallback)TextPropertyChanged));

		public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(HintTextBox), new PropertyMetadata((object)string.Empty));

		public static readonly DependencyProperty IsPasswordProperty = DependencyProperty.Register("IsPassword", typeof(bool), typeof(HintTextBox), new PropertyMetadata((object)false));

		public static readonly DependencyProperty UpdateSourceEnabledProperty = DependencyProperty.Register("UpdateSourceEnabled", typeof(bool), typeof(HintTextBox), new PropertyMetadata((object)true));

		public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(HintTextBox), new PropertyMetadata((object)(TextWrapping)1));

		public static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.Register("AcceptsReturn", typeof(bool), typeof(HintTextBox), new PropertyMetadata((object)false));

		public static readonly DependencyProperty HintBorderThicknessProperty = DependencyProperty.Register("HintBorderThickness", typeof(Thickness), typeof(HintTextBox), new PropertyMetadata((object)new Thickness(1.0)));

		public static readonly DependencyProperty PasswordBoxStyleProperty = DependencyProperty.Register("PasswordBoxStyle", typeof(Style), typeof(HintTextBox), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.Register("TextBoxStyle", typeof(Style), typeof(HintTextBox), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty HintStyleProperty = DependencyProperty.Register("HintStyle", typeof(Style), typeof(HintTextBox), new PropertyMetadata((PropertyChangedCallback)null));

		public static readonly DependencyProperty InputScopeProperty = DependencyProperty.Register("InputScope", typeof(InputScope), typeof(HintTextBox), new PropertyMetadata((PropertyChangedCallback)null));






		public string Text
		{
			get
			{
				return ((string)base.GetValue(TextProperty)) ?? string.Empty;
			}
			set
			{
				base.SetValue(TextProperty, (object)value);
			}
		}

		public string Hint
		{
			get
			{
				return (string)base.GetValue(HintProperty);
			}
			set
			{
				base.SetValue(HintProperty, (object)value);
			}
		}

		public bool IsPassword
		{
			get
			{
				return (bool)base.GetValue(IsPasswordProperty);
			}
			set
			{
				base.SetValue(IsPasswordProperty, (object)value);
			}
		}

		public bool UpdateSourceEnabled
		{
			get
			{
				return (bool)base.GetValue(UpdateSourceEnabledProperty);
			}
			set
			{
				base.SetValue(UpdateSourceEnabledProperty, (object)value);
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

		public bool AcceptsReturn
		{
			get
			{
				return (bool)base.GetValue(AcceptsReturnProperty);
			}
			set
			{
				base.SetValue(AcceptsReturnProperty, (object)value);
			}
		}

		public Thickness HintBorderThickness
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Thickness)base.GetValue(HintBorderThicknessProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				base.SetValue(HintBorderThicknessProperty, (object)value);
			}
		}

		public Style PasswordBoxStyle
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (Style)base.GetValue(PasswordBoxStyleProperty);
			}
			set
			{
				base.SetValue(PasswordBoxStyleProperty, (object)value);
			}
		}

		public Style TextBoxStyle
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (Style)base.GetValue(TextBoxStyleProperty);
			}
			set
			{
				base.SetValue(TextBoxStyleProperty, (object)value);
			}
		}

		public Style HintStyle
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (Style)base.GetValue(HintStyleProperty);
			}
			set
			{
				base.SetValue(HintStyleProperty, (object)value);
			}
		}

		public InputScope InputScope
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected Obj, but got Unknown
				return (InputScope)base.GetValue(InputScopeProperty);
			}
			set
			{
				base.SetValue(InputScopeProperty, (object)value);
			}
		}

		public int CaretPosition
		{
			get
			{
				if (IsPassword)
				{
					return 0;
				}
				return TextBox.SelectionStart;
			}
			set
			{
				if (!IsPassword)
				{
					_lastCaretPosition = value;
					TextBox.SelectionStart = value;
				}
			}
		}

		public event RoutedEventHandler TextSelectionChanged
		{
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected Obj, but got Unknown
				RoutedEventHandler val = this.m_TextSelectionChanged;
				RoutedEventHandler val2;
				do
				{
					val2 = val;
					RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Combine((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref this.m_TextSelectionChanged, value2, val2);
				}
				while (val != val2);
			}
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected Obj, but got Unknown
				RoutedEventHandler val = this.m_TextSelectionChanged;
				RoutedEventHandler val2;
				do
				{
					val2 = val;
					RoutedEventHandler value2 = (RoutedEventHandler)Delegate.Remove((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref this.m_TextSelectionChanged, value2, val2);
				}
				while (val != val2);
			}
		}

		public event KeyEventHandler TextKeyUp
		{
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected Obj, but got Unknown
				KeyEventHandler val = this.m_TextKeyUp;
				KeyEventHandler val2;
				do
				{
					val2 = val;
					KeyEventHandler value2 = (KeyEventHandler)Delegate.Combine((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref this.m_TextKeyUp, value2, val2);
				}
				while (val != val2);
			}
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected Obj, but got Unknown
				KeyEventHandler val = this.m_TextKeyUp;
				KeyEventHandler val2;
				do
				{
					val2 = val;
					KeyEventHandler value2 = (KeyEventHandler)Delegate.Remove((Delegate)(object)val2, (Delegate)(object)value);
					val = Interlocked.CompareExchange(ref this.m_TextKeyUp, value2, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<TextChangedEventArgs> TextBoxTextChanged;

		public HintTextBox()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected Obj, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected Obj, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected Obj, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected Obj, but got Unknown
			InitializeComponent();
			((FrameworkElement)LayoutRoot).DataContext = this;
			((UIElement)TextBox).GotFocus += TextBoxOnGotFocus;
			((UIElement)TextBox).LostFocus += TextBoxOnLostFocus;
			TextBox.SelectionChanged += TextBoxOnSelectionChanged;
			((UIElement)TextBox).KeyUp += TextBoxOnKeyUp;
		}

		private void TextBoxOnSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (this.m_TextSelectionChanged != null)
			{
				this.m_TextSelectionChanged.Invoke(sender, e);
			}
		}

		private void TextBoxOnKeyUp(object sender, KeyEventArgs e)
		{
			if (this.m_TextKeyUp != null)
			{
				this.m_TextKeyUp.Invoke(sender, e);
			}
		}

		private void TextBoxOnLostFocus(object sender, RoutedEventArgs e)
		{
			_lastCaretPosition = null;
		}

		private void TextBoxOnGotFocus(object sender, RoutedEventArgs e)
		{
			if (_lastCaretPosition.HasValue)
			{
				TextBox.SelectionStart = _lastCaretPosition.Value;
			}
		}

		private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			HintTextBox hintTextBox = d as HintTextBox;
			if (hintTextBox != null)
			{
				((UIElement)hintTextBox.HintBox).Visibility = VisibilityUtil.FromBool(string.IsNullOrEmpty(hintTextBox.Text));
			}
		}

		protected virtual void HintTextBlock_OnTap(object sender, GestureEventArgs e)
		{
			if (IsPassword)
			{
				((Control)PasswordBox).Focus();
			}
			else
			{
				((Control)TextBox).Focus();
			}
		}

		protected virtual void TextChanged(object sender, TextChangedEventArgs e)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)HintBox).Visibility = VisibilityUtil.FromBool(string.IsNullOrEmpty(TextBox.Text));
			if (UpdateSourceEnabled)
			{
				((FrameworkElement)TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
			}
			if (TextBoxTextChanged != null)
			{
				TextBoxTextChanged(sender, e);
			}
		}

		protected virtual void PasswordChanged(object sender, RoutedEventArgs e)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)HintBox).Visibility = VisibilityUtil.FromBool(string.IsNullOrEmpty(PasswordBox.Password));
			if (UpdateSourceEnabled)
			{
				((FrameworkElement)PasswordBox).GetBindingExpression(PasswordBox.PasswordProperty).UpdateSource();
			}
		}

		private void LayoutRoot_OnGotFocus(object sender, RoutedEventArgs e)
		{
			if (IsPassword)
			{
				((Control)PasswordBox).Focus();
			}
			else
			{
				((Control)TextBox).Focus();
			}
		}


		static HintTextBox()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected Obj, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected Obj, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected Obj, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected Obj, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected Obj, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected Obj, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected Obj, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected Obj, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Expected Obj, but got Unknown
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Expected Obj, but got Unknown
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Expected Obj, but got Unknown
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Expected Obj, but got Unknown
		}
	}
}

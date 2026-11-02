using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Facebook.Messenger.WP8.Extensions;
using PixelLab;

namespace Facebook.Messenger.WP8.Controls.Messages;

public sealed class Token : UserControl, ITiltable
{
	public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register("IsFocused", typeof(bool), typeof(Token), new PropertyMetadata((object)false));

	internal Border Border;

	internal DynamicTextBlock NameTextBox;

	private bool _contentLoaded;

	public bool IsFocused
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsFocusedProperty);
		}
		set
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected Obj, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected Obj, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected Obj, but got Unknown
			((DependencyObject)this).SetValue(IsFocusedProperty, (object)value);
			if (value)
			{
				Border border = Border;
				object obj = Application.Current.Resources[(object)"MessengerBlueBrush"];
				border.Background = (Brush)((obj is SolidColorBrush) ? obj : null);
				((Control)NameTextBox).Foreground = (Brush)new SolidColorBrush(Colors.White);
			}
			else
			{
				Border.Background = (Brush)new SolidColorBrush(Colors.Transparent);
				((Control)NameTextBox).Foreground = (Brush)new SolidColorBrush(Colors.Black);
			}
		}
	}

	public string Text
	{
		get
		{
			return NameTextBox.Text;
		}
		set
		{
			NameTextBox.Text = value;
		}
	}

	public string ClassName => "Token";

	public Token()
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
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/Messages/Token.xaml", UriKind.Relative));
			Border = (Border)((FrameworkElement)this).FindName("Border");
			NameTextBox = (DynamicTextBlock)((FrameworkElement)this).FindName("NameTextBox");
		}
	}

	static Token()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected Obj, but got Unknown
	}
}

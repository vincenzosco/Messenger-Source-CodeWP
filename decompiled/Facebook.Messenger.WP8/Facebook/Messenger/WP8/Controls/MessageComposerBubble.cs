using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Controls.Messages;

namespace Facebook.Messenger.WP8.Controls;

public class MessageComposerBubble : UserControl
{
	private bool _hasText;

	internal BubbleContainer BubbleContainer;

	internal ImageUploadAttachmentControl UploadBar;

	internal ScrollViewer MessageComposeScrollViewer;

	internal HintTextBox ComposerTextBox;

	private bool _contentLoaded;

	public string Text
	{
		get
		{
			return ComposerTextBox.Text;
		}
		set
		{
			ComposerTextBox.Text = value;
		}
	}

	public bool HasText
	{
		get
		{
			return _hasText;
		}
		private set
		{
			if (_hasText != value)
			{
				_hasText = value;
				if (HasTextChanged != null)
				{
					HasTextChanged(this, null);
				}
			}
		}
	}

	public event EventHandler EnterPressed;

	public event EventHandler HasTextChanged;

	public MessageComposerBubble()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected Obj, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
		InitializeComponent();
		((FrameworkElement)this).Loaded += OnLoaded;
		((FrameworkElement)this).Unloaded += OnUnloaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		UpdateHasText();
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected Obj, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		((FrameworkElement)this).Loaded -= OnLoaded;
		((FrameworkElement)this).Unloaded -= OnUnloaded;
	}

	private void UpdateHasText()
	{
		HasText = !string.IsNullOrWhiteSpace(Text);
	}

	public void ClearText()
	{
		Text = string.Empty;
	}

	private void MessageComposerBubble_OnGotFocus(object sender, RoutedEventArgs e)
	{
		((Control)ComposerTextBox).Focus();
	}

	private void ComposerTextBox_OnGotFocus(object sender, RoutedEventArgs e)
	{
		BubbleContainer bubbleContainer = BubbleContainer;
		object obj = Application.Current.Resources[(object)"MessengerBlueBrush"];
		((Control)bubbleContainer).Background = (Brush)((obj is SolidColorBrush) ? obj : null);
	}

	private void ComposerTextBox_OnLostFocus(object sender, RoutedEventArgs e)
	{
		BubbleContainer bubbleContainer = BubbleContainer;
		object obj = Application.Current.Resources[(object)"MessageBubbleBrush"];
		((Control)bubbleContainer).Background = (Brush)((obj is SolidColorBrush) ? obj : null);
	}

	private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		UpdateHasText();
		if (ComposerTextBox.TextBox.SelectionStart == Text.Length)
		{
			((UIElement)MessageComposeScrollViewer).Measure(default(Size));
			MessageComposeScrollViewer.ScrollToVerticalOffset(((FrameworkElement)ComposerTextBox).ActualHeight);
		}
	}

	private void ComposerTextBox_OnKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)e.Key == 3 && EnterPressed != null)
		{
			EnterPressed(this, null);
		}
	}

	private void BubbleTapped(object sender, GestureEventArgs e)
	{
		((Control)ComposerTextBox).Focus();
	}

	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected Obj, but got Unknown
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Application.LoadComponent((object)this, new Uri("/Facebook.Messenger.WP8;component/Controls/MessageComposerBubble.xaml", UriKind.Relative));
			BubbleContainer = (BubbleContainer)((FrameworkElement)this).FindName("BubbleContainer");
			UploadBar = (ImageUploadAttachmentControl)((FrameworkElement)this).FindName("UploadBar");
			MessageComposeScrollViewer = (ScrollViewer)((FrameworkElement)this).FindName("MessageComposeScrollViewer");
			ComposerTextBox = (HintTextBox)((FrameworkElement)this).FindName("ComposerTextBox");
		}
	}
}

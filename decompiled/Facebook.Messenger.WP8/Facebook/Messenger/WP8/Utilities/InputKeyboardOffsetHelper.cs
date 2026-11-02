using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace Facebook.Messenger.WP8.Utilities
{
	public class InputKeyboardOffsetHelper
	{
		public delegate void KeyboardVisibilityChangedDelegate(bool visible, Rect occludedRect);

		private readonly SingleInputPaneHelper _inputPaneHelper;

		private readonly KeyboardVisibilityChangedDelegate _keyboardVisibilityChangedDelegate;

		public InputKeyboardOffsetHelper(KeyboardVisibilityChangedDelegate keyboardVisibilityChangedDelegate)
		{
			_keyboardVisibilityChangedDelegate = keyboardVisibilityChangedDelegate;
			_inputPaneHelper = new SingleInputPaneHelper();
		}

		public void SetListeners()
		{
			_inputPaneHelper.SubscribeToKeyboardEvents(ShowingHandler, HidingHandler);
		}

		public void ClearListeners()
		{
			_inputPaneHelper.UnsubscribeFromKeyboardEvents();
		}

		public void RegisterInputElements(IReadOnlyList<UIElement> inputElements)
		{
		}

		private void ShowingHandler(object sender, InputPaneVisibilityEventArgs e)
		{
			Rect occludedRect = e.OccludedRect;
			int num = CalculateKeyboardHeightPixels(occludedRect.Height, occludedRect.Width);
			InvokeKeyboardVisibilityChangedDelegate(true, new Rect(0.0, 0.0, 0.0, num));
		}

		private static int CalculateKeyboardHeightPixels(double heightDips, double widthDips)
		{
			if ((int)widthDips == 0)
			{
				return 0;
			}
			double actualWidth = Application.Current.Host.Content.ActualWidth;
			double num = actualWidth / widthDips;
			return (int)(heightDips * num);
		}

		private void HidingHandler(InputPane input, InputPaneVisibilityEventArgs e)
		{
			InvokeKeyboardVisibilityChangedDelegate(false, default(Rect));
		}

		private void InputElementOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			InvokeKeyboardVisibilityChangedDelegate(false, default(Rect));
		}

		private void InputElementOnGotFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			int num = InputHelper.GetKeyboardHeight() + GetKeyboardExtraHeight();
			Rect occludedRect = new Rect(0.0, 0.0, Application.Current.Host.Content.ActualWidth, num);
			InvokeKeyboardVisibilityChangedDelegate(true, occludedRect);
		}

		private int GetKeyboardExtraHeight()
		{
			object focusedElement = FocusManager.GetFocusedElement();
			TextBox val = (TextBox)((focusedElement is TextBox) ? focusedElement : null);
			if (val != null && (InputHelper.HasSuggestionBar(val) || Clipboard.ContainsText()))
			{
				return InputHelper.GetKeyboardSuggestionHeight();
			}
			return 0;
		}

		private void InvokeKeyboardVisibilityChangedDelegate(bool visible, Rect occludedRect)
		{
			if (_keyboardVisibilityChangedDelegate != null)
			{
				_keyboardVisibilityChangedDelegate(visible, occludedRect);
			}
		}
	}
}

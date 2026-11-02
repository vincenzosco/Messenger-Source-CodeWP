using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Facebook.Messenger.WP8.Utilities
{
	public class InputHelper
	{
		private const int KeyboardHeight = 336;

		private const int KeyboardSuggestionHeight = 69;

		private const int KeyboardHeightFor81Large = 280;

		private const int KeyboardSuggestionHeightFor81Large = 56;

		private const int KeyboardInputPadding = 20;

		private static readonly List<InputScopeNameValue> InputScopesWithSuggestions = new List<InputScopeNameValue>
		{
			(InputScopeNameValue)49,
			(InputScopeNameValue)55,
			(InputScopeNameValue)51,
			(InputScopeNameValue)57,
			(InputScopeNameValue)50
		};

		public static int GetKeyboardHeight()
		{
			if (DeviceHelper.IsLargeDevice())
			{
				return 280;
			}
			return 336;
		}

		public static int GetKeyboardSuggestionHeight()
		{
			if (DeviceHelper.IsLargeDevice())
			{
				return 56;
			}
			return 69;
		}

		public static double GetRemainingSpace(double spaceRequired)
		{
			return Application.Current.Host.Content.ActualHeight - spaceRequired - (double)GetKeyboardHeight();
		}

		public static int GetOccludedOffset(FrameworkElement inputElement, FrameworkElement rootElement, int keyboardHeight)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			GeneralTransform val = ((UIElement)inputElement).TransformToVisual((UIElement)(object)rootElement);
			Point val2 = val.Transform(new Point(0.0, 0.0));
			double num = val2.Y + inputElement.ActualHeight;
			double num2 = rootElement.ActualHeight - (double)keyboardHeight;
			if (!(num > num2))
			{
				return 0;
			}
			return (int)(num - num2) + 20;
		}

		public static bool HasSuggestionBar(TextBox textBox)
		{
			if (textBox == null || textBox.InputScope == null || textBox.InputScope.Names == null)
			{
				return false;
			}
			IEnumerable<InputScopeName> source = textBox.InputScope.Names.Cast<InputScopeName>();
			return source.Any((InputScopeName name) =>
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return InputScopesWithSuggestions.Contains(name.NameValue);
			});
		}
	}
}

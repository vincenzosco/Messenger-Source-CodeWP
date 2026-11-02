using System.Windows;

namespace Facebook.Messenger.WP8.Utilities
{
	public class MessageBoxUtil
	{
		public static bool Confirm(string text, string caption)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			return (int)MessageBox.Show(text, caption, (MessageBoxButton)1) == 1;
		}
	}
}

using System.Windows;

namespace Facebook.Messenger
{
	public class VisibilityUtil
	{
		public static Visibility FromBool(bool visible)
		{
			if (visible)
			{
				return (Visibility)0;
			}
			return (Visibility)1;
		}

		public static Visibility VisibleIfNotEmpty(string s)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return FromBool(!string.IsNullOrWhiteSpace(s));
		}
	}
}

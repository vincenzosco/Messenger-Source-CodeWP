using Facebook.Messenger.Localization;
using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Localization
{
	public class LocalizedStrings
	{
		private static readonly StringLibrary _stringLibrary = Facebook.Messenger.Localization.LocalizedStrings.GetInstance().StringResources;

		private static readonly InternalStringsLibrary _internalStringResources = Facebook.Messenger.Localization.LocalizedStrings.GetInstance().InternalStringResources;

		public StringLibrary StringLibrary
		{
			get
			{
				return _stringLibrary;
			}
		}

		public InternalStringsLibrary InternalStringsLibrary
		{
			get
			{
				return _internalStringResources;
			}
		}
	}
}

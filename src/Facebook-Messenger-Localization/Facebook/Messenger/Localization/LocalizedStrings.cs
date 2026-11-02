using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.Localization
{
	public class LocalizedStrings
	{
		private static readonly StringLibrary _stringLibrary = new StringLibrary();

		private static readonly InternalStringsLibrary _internalStringsResources = new InternalStringsLibrary();

		private static LocalizedStrings _localizedStrings;

		public StringLibrary StringResources => _stringLibrary;

		public InternalStringsLibrary InternalStringResources => _internalStringsResources;

		private LocalizedStrings()
		{
		}

		public static LocalizedStrings GetInstance()
		{
			if (_localizedStrings == null)
			{
				_localizedStrings = new LocalizedStrings();
			}
			return _localizedStrings;
		}
	}
}

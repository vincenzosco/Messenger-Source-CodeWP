using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Utilities
{
	public class CountryCode
	{
		public int PhonePrefix { get; set; }

		public string RegionCode { get; set; }

		public string CountryName
		{
			get
			{
				return CountryNameStringLibrary.ResourceManager.GetString(RegionCode.ToLowerInvariant());
			}
		}

		public bool IsSelected { get; set; }

		public string FormattedCountryCode
		{
			get
			{
				return string.Format("{0} (+{1})", CountryName, PhonePrefix);
			}
		}

		public string PhonePrefixWithPlus
		{
			get
			{
				return string.Format("+{0}", PhonePrefix);
			}
		}
	}
}

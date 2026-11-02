using Facebook.Messenger.Localization.Resources;

namespace Facebook.Messenger.WP8.Utilities;

public class CountryCode
{
	public int PhonePrefix { get; set; }

	public string RegionCode { get; set; }

	public string CountryName => CountryNameStringLibrary.ResourceManager.GetString(RegionCode.ToLowerInvariant());

	public bool IsSelected { get; set; }

	public string FormattedCountryCode => $"{CountryName} (+{PhonePrefix})";

	public string PhonePrefixWithPlus => $"+{PhonePrefix}";
}

using System;

namespace Facebook.Base;

public static class VersionExtensions
{
	public static string ToThreePartString(this Version version)
	{
		return string.Format("{0}.{1}.{2}", new object[3] { version.Major, version.Minor, version.Build });
	}

	public static bool IsGreaterThan(this Version version, Version compareTo)
	{
		return version.CompareTo(compareTo) > 0;
	}

	public static bool IsGreaterThanOrEqualTo(this Version version, Version compareTo)
	{
		return version.CompareTo(compareTo) >= 0;
	}
}

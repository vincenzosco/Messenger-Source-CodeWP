using System;

namespace Facebook.Base;

public interface IEnvironmentInformation
{
	string GetFacebookApplicationId();

	Version GetApplicationVersion();

	string GetApplicationVersionAsThreePartString();

	string GetApplicationPackageId();

	string GetBuildNumber();

	string GetBuildGitHash();

	string GetBuildGitBranch();

	string GetBuildTime();

	string GetNetworkDescription();

	string GetDeviceModelName();

	string GetDeviceManufacturer();

	string GetDeviceModelDescription();

	string GetOperatingSystemDescription();

	string GetOperatingSystemName();

	string GetOperatingSystemVersion();

	string GetNetworkConnectionType();

	string GetInterfaceDetailsLogString();
}

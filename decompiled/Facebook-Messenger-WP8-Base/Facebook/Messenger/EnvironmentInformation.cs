using System;
using System.Xml.Linq;
using Facebook.Base;
using Facebook.Models;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;

namespace Facebook.Messenger;

public class EnvironmentInformation : IEnvironmentInformation
{
	private readonly Version _applicationVersion;

	private readonly string _applicationId;

	private readonly string _appPackageId;

	public EnvironmentInformation(FacebookConfig config)
	{
		_applicationId = config.ApplicationId;
		try
		{
			XElement xElement = XDocument.Load("WMAppManifest.xml").Root.Element("App");
			if (!Version.TryParse(xElement.Attribute("Version").Value, out _applicationVersion))
			{
				_applicationVersion = new Version(0, 0, 0);
			}
			_appPackageId = xElement.Attribute("ProductID").Value;
		}
		catch
		{
			_applicationVersion = _applicationVersion ?? new Version(0, 0, 0);
			_appPackageId = _appPackageId ?? "Unknown";
		}
	}

	public Version GetApplicationVersion()
	{
		return _applicationVersion;
	}

	public string GetApplicationVersionAsThreePartString()
	{
		return _applicationVersion.ToThreePartString();
	}

	public string GetBuildNumber()
	{
		return "0";
	}

	public string GetBuildGitHash()
	{
		return string.Empty;
	}

	public string GetBuildGitBranch()
	{
		return string.Empty;
	}

	public string GetBuildTime()
	{
		return "0";
	}

	public string GetFacebookApplicationId()
	{
		return _applicationId;
	}

	public string GetNetworkDescription()
	{
		return DeviceNetworkInformation.CellularMobileOperator;
	}

	public string GetDeviceModelName()
	{
		return DeviceStatus.DeviceName;
	}

	public string GetDeviceManufacturer()
	{
		return DeviceStatus.DeviceManufacturer;
	}

	public string GetDeviceModelDescription()
	{
		return $"{DeviceStatus.DeviceManufacturer} {DeviceStatus.DeviceName}";
	}

	public string GetOperatingSystemDescription()
	{
		return Environment.OSVersion.ToString();
	}

	public string GetOperatingSystemName()
	{
		return Environment.OSVersion.Platform.ToString();
	}

	public string GetOperatingSystemVersion()
	{
		return Environment.OSVersion.Version.ToString();
	}

	public string GetApplicationPackageId()
	{
		return _appPackageId;
	}

	public string GetNetworkConnectionType()
	{
		return "mobile";
	}

	public string GetInterfaceDetailsLogString()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		NetworkInterfaceList val = new NetworkInterfaceList();
		string text = string.Empty;
		foreach (NetworkInterfaceInfo item in val)
		{
			text += $"Name: {item.InterfaceName}, State: {item.InterfaceState}, Type: {item.InterfaceType}, SubType: {item.InterfaceSubtype}, Bandwidth: {item.Bandwidth} - ";
		}
		return text;
	}
}

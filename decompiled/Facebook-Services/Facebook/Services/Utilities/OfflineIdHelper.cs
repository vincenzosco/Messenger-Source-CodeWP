using System;
using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Utilities;

public static class OfflineIdHelper
{
	public static string CreateOfflineThreadingId(FacebookContext context)
	{
		long num = DateExtensions.MSecsSinceEpoch();
		int deviceId = GetDeviceId(context);
		long num2 = (num << 32) + deviceId;
		return (num2 & 0x7FFFFFFFFFFFFFFFL).ToString();
	}

	public static bool IsMessageFromThisDevice(MailMessage message, FacebookContext context)
	{
		string offlineThreadingId = message.OfflineThreadingId;
		if (!long.TryParse(offlineThreadingId, out var result))
		{
			return false;
		}
		int num = (int)result;
		int deviceId = GetDeviceId(context);
		return deviceId == num;
	}

	private static int GetDeviceId(FacebookContext context)
	{
		string deviceIdentifier = context.GlobalSettings.DeviceIdentifier;
		return BitConverter.ToInt32(new Guid(deviceIdentifier).ToByteArray(), 0);
	}
}

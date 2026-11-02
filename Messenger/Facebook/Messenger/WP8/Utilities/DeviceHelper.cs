using System;
using System.Windows;
using Facebook.Base.Logging;
using Microsoft.Phone.Info;

namespace Facebook.Messenger.WP8.Utilities
{
	public class DeviceHelper
	{
		public enum Resolution
		{
			WVGA,
			WXGA,
			HD
		}

		public static Resolution GetPhoneResolution()
		{
			int scaleFactor = Application.Current.Host.Content.ScaleFactor;
			switch (scaleFactor)
			{
			case 100:
				return Resolution.WVGA;
			case 160:
				return Resolution.WXGA;
			case 150:
			case 225:
				return Resolution.HD;
			default:
				throw new InvalidOperationException($"No resolution found for unknown scale factor: {scaleFactor}");
			}
		}

		private static double GetScreenSize()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			object obj = default(object);
			object obj2 = default(object);
			object obj3 = default(object);
			if (!DeviceExtendedProperties.TryGetValue("RawDpiX", out obj) || !DeviceExtendedProperties.TryGetValue("RawDpiY", out obj2) || !DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out obj3))
			{
				return -1.0;
			}
			double num = (double)obj;
			double num2 = (double)obj2;
			Size val = (Size)obj3;
			if (!(num > 0.0) || !(num2 > 0.0) || !(val.Width > 0.0) || !(val.Height > 0.0))
			{
				Logger.Warning($"Unable to get screen size, RawDpiX = {num}, RawDpiY = {num2}, PhysicalScreenResolution = {val}", "GetScreenSize", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\DeviceHelper.cs", 55);
				return -1.0;
			}
			return Math.Sqrt(Math.Pow(val.Width / num, 2.0) + Math.Pow(val.Height / num2, 2.0));
		}

		public static bool IsLargeDevice()
		{
			double screenSize = GetScreenSize();
			return screenSize > 5.0;
		}
	}
}

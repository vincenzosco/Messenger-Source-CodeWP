using Facebook.Models;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger
{
	public class DebuggingUtility
	{
		public static bool IsTestBuildOrEmployee
		{
			get
			{
				if (!IsTestBuild)
				{
					return IsEmployee;
				}
				return true;
			}
		}

		public static bool IsEmployee
		{
			get
			{
				FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
				return instance.Session.Profile?.IsEmployee ?? false;
			}
		}

		public static bool IsTestBuild
		{
			get
			{
				if (!IsQualityAssuranceBuild)
				{
					return IsDebugBuild;
				}
				return true;
			}
		}

		public static bool IsQualityAssuranceBuild => false;

		public static bool IsDebugBuild => false;

		public static bool IsDogfoodBuild => false;

		public static bool CanLogSensitiveInfo => IsTestBuild;
	}
}

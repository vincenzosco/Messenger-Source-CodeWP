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
				FacebookProfile profile = instance.Session.Profile;
				if (profile != null)
				{
					return profile.IsEmployee;
				}
				return false;
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

		public static bool IsQualityAssuranceBuild
		{
			get
			{
				return false;
			}
		}

		public static bool IsDebugBuild
		{
			get
			{
				return false;
			}
		}

		public static bool IsDogfoodBuild
		{
			get
			{
				return false;
			}
		}

		public static bool CanLogSensitiveInfo
		{
			get
			{
				return IsTestBuild;
			}
		}
	}
}

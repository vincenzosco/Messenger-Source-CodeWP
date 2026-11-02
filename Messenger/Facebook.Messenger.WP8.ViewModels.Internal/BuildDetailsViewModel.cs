namespace Facebook.Messenger.WP8.ViewModels.Internal
{
	public class BuildDetailsViewModel
	{
		public bool IsDebugBuild
		{
			get
			{
				return DebuggingUtility.IsDebugBuild;
			}
		}

		public bool IsQualityAssuranceBuild
		{
			get
			{
				return DebuggingUtility.IsQualityAssuranceBuild;
			}
		}

		public bool IsDogfoodBuild
		{
			get
			{
				return DebuggingUtility.IsDogfoodBuild;
			}
		}

		public bool IsEmployee
		{
			get
			{
				return DebuggingUtility.IsEmployee;
			}
		}
	}
}

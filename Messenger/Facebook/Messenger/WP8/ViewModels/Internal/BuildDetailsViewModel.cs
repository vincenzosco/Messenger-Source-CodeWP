namespace Facebook.Messenger.WP8.ViewModels.Internal
{
	public class BuildDetailsViewModel
	{
		public bool IsDebugBuild => DebuggingUtility.IsDebugBuild;

		public bool IsQualityAssuranceBuild => DebuggingUtility.IsQualityAssuranceBuild;

		public bool IsDogfoodBuild => DebuggingUtility.IsDogfoodBuild;

		public bool IsEmployee => DebuggingUtility.IsEmployee;
	}
}

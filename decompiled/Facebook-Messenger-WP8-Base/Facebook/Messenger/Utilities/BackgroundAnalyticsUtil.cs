using System.IO.IsolatedStorage;

namespace Facebook.Messenger.Utilities
{
	public class BackgroundAnalyticsUtil
	{
		private const string BackgroundSendingStartedFilename = "BackgroundSendingStarted";

		public static void BackgroundSendingStarted()
		{
			ErrorIgnoringStorageUtils.CreateFileInIsolatedStorage("BackgroundSendingStarted", CreateEmptyFile);
		}

		private static void CreateEmptyFile(IsolatedStorageFile storage, string fileName)
		{
			storage.CreateFile(fileName).Dispose();
		}

		public static bool WasBackgroundSendingInstanceRun()
		{
			return ErrorIgnoringStorageUtils.DeleteFileInIsolateStorage("BackgroundSendingStarted");
		}
	}
}

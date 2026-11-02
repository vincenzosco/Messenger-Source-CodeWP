using System;

namespace Microsoft.Phone.Controls
{
	public class PhotoLoadingSignal
	{
		public Uri ImageUri { get; private set; }

		public PhotoLoadingStatus LoadingStatus { get; private set; }

		public PhotoLoadingSignal(Uri imageUri, PhotoLoadingStatus loadingStatus)
		{
			ImageUri = imageUri;
			LoadingStatus = loadingStatus;
		}
	}
}

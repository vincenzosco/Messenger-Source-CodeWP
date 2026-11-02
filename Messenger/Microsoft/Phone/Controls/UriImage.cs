using System;

namespace Microsoft.Phone.Controls
{
	public class UriImage : IImage
	{
		private Uri _uri;

		public UriImage(Uri uri)
		{
			_uri = uri;
		}

		public Uri GetImageUri()
		{
			return _uri;
		}
	}
}

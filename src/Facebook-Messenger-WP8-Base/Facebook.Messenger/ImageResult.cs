using Facebook.Models;

namespace Facebook.Messenger
{
	public class ImageResult
	{
		public MediaSource Source { get; private set; }

		public PhotoData Image { get; private set; }

		public ImageResult(MediaSource source, PhotoData image)
		{
			Source = source;
			Image = image;
		}
	}
}

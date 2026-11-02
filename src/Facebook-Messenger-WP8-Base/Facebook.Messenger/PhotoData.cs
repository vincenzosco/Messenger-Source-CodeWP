using System.Windows.Media.Imaging;

namespace Facebook.Messenger
{
	public class PhotoData
	{
		public readonly BitmapSource Source;

		public ExifOrientation ExifOrientation { get; private set; }

		public PhotoData(BitmapSource source)
		{
			Source = source;
		}

		public static PhotoData FromImage(BitmapSource source)
		{
			return new PhotoData(source);
		}

		public void SetExifOrientation(ExifOrientation exifOrientation)
		{
			ExifOrientation = exifOrientation;
		}

		public RotationAngle GetRotationAngle()
		{
			return GetRotationAngleFromExifOrientation(ExifOrientation);
		}

		public static RotationAngle GetRotationAngleFromExifOrientation(ExifOrientation exifOrientation)
		{
			switch (exifOrientation)
			{
			case ExifOrientation.TopLeftSide:
			case ExifOrientation.TopRightSide:
				return RotationAngle.Portrait;
			case ExifOrientation.BottomRightSide:
			case ExifOrientation.BottomLeftSide:
				return RotationAngle.PortraitDown;
			case ExifOrientation.LeftSideTop:
			case ExifOrientation.RightSideTop:
				return RotationAngle.LandscapeLeft;
			case ExifOrientation.RightSideBottom:
			case ExifOrientation.LeftSideBottom:
				return RotationAngle.LandscapeRight;
			default:
				return RotationAngle.Portrait;
			}
		}
	}
}

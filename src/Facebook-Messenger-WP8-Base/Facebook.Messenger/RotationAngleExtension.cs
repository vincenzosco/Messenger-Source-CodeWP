using System;

namespace Facebook.Messenger
{
	public static class RotationAngleExtension
	{
		public static int ToDegrees(this RotationAngle angle)
		{
			switch (angle)
			{
			case RotationAngle.Portrait:
				return 0;
			case RotationAngle.LandscapeLeft:
				return 90;
			case RotationAngle.PortraitDown:
				return 180;
			case RotationAngle.LandscapeRight:
				return 270;
			default:
				throw new ArgumentOutOfRangeException("angle");
			}
		}
	}
}

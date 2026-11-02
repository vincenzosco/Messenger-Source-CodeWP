using System;

namespace Facebook.Messenger;

public static class RotationAngleExtension
{
	public static int ToDegrees(this RotationAngle angle)
	{
		return angle switch
		{
			RotationAngle.Portrait => 0, 
			RotationAngle.LandscapeLeft => 90, 
			RotationAngle.PortraitDown => 180, 
			RotationAngle.LandscapeRight => 270, 
			_ => throw new ArgumentOutOfRangeException("angle"), 
		};
	}
}

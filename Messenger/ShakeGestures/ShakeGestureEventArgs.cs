using System;

namespace ShakeGestures
{
	public class ShakeGestureEventArgs : EventArgs
	{
		private ShakeType _shakeType;

		public ShakeType ShakeType => _shakeType;

		public ShakeGestureEventArgs(ShakeType shakeType)
		{
			_shakeType = shakeType;
		}
	}
}

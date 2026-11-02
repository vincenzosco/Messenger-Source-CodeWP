using System;

namespace Facebook.Services.Common
{
	public class JewelRequestedEventArgs : EventArgs
	{
		public JewelType JewelType { get; set; }

		public bool PerformFullSync { get; set; }
	}
}

using System;

namespace Facebook.Services.Common;

public class JewelSyncCompletedEventArgs : EventArgs
{
	public JewelType JewelType { get; set; }

	public bool Success { get; set; }
}

using System;

namespace Microsoft.Phone.Controls
{
	public class ItemDisplayedEventArgs : EventArgs
	{
		public int ItemIndex { get; private set; }

		public ItemDisplayedEventArgs(int itemIndex)
		{
			ItemIndex = itemIndex;
		}
	}
}

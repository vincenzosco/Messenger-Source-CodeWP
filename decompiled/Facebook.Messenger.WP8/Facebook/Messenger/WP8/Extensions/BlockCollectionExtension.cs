using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Facebook.Messenger.WP8.Extensions;

public static class BlockCollectionExtension
{
	public static void ClearWithBugFix(this BlockCollection blocks)
	{
		while (((IEnumerable<Block>)blocks).Any())
		{
			((PresentationFrameworkCollection<Block>)(object)blocks).RemoveAt(0);
		}
	}
}

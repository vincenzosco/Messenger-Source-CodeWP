using System.Collections.Generic;
using System.Linq;

namespace Facebook.Base
{
	public class HashGenerator
	{
		public static int CalculateHashCode(IEnumerable<object> items)
		{
			return items.Where((object item) => item != null).Aggregate(0, (int current, object item) => current ^ item.GetHashCode());
		}
	}
}

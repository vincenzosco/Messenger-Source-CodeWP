using System.Collections;

namespace Thrift.Collections
{
	public class TCollections
	{
		public static bool Equals(IEnumerable first, IEnumerable second)
		{
			if (first == null && second == null)
			{
				return true;
			}
			if (first == null || second == null)
			{
				return false;
			}
			IEnumerator enumerator = first.GetEnumerator();
			IEnumerator enumerator2 = second.GetEnumerator();
			bool flag = enumerator.MoveNext();
			bool flag2 = enumerator2.MoveNext();
			while (flag && flag2)
			{
				IEnumerable enumerable = enumerator.Current as IEnumerable;
				IEnumerable enumerable2 = enumerator2.Current as IEnumerable;
				if (enumerable != null && enumerable2 != null)
				{
					if (!Equals(enumerable, enumerable2))
					{
						return false;
					}
				}
				else
				{
					if ((enumerable == null) ^ (enumerable2 == null))
					{
						return false;
					}
					if (!object.Equals(enumerator.Current, enumerator2.Current))
					{
						return false;
					}
				}
				flag = enumerator.MoveNext();
				flag2 = enumerator2.MoveNext();
			}
			return flag == flag2;
		}

		public static int GetHashCode(IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			int num = 0;
			foreach (object item in enumerable)
			{
				IEnumerable enumerable2 = item as IEnumerable;
				int num2 = ((enumerable2 == null) ? item.GetHashCode() : GetHashCode(enumerable2));
				num = (num * 397) ^ num2;
			}
			return num;
		}
	}
}

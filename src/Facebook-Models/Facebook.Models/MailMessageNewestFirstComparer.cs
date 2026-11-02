using System.Collections.Generic;

namespace Facebook.Models
{
	public class MailMessageNewestFirstComparer : IComparer<MailMessage>
	{
		public static readonly MailMessageNewestFirstComparer Comparer = new MailMessageNewestFirstComparer();

		public int Compare(MailMessage x, MailMessage y)
		{
			long num = x.Timestamp - y.Timestamp;
			if (num != 0)
			{
				if (num <= 0)
				{
					return 1;
				}
				return -1;
			}
			long num2 = x.ActionId - y.ActionId;
			if (num2 == 0)
			{
				return 0;
			}
			if (num2 <= 0)
			{
				return 1;
			}
			return -1;
		}
	}
}

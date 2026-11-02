using System;

namespace Facebook.Models
{
	public class ThreadFbid
	{
		public readonly long Value;

		[Obsolete("We should be converting all FBIDs to long")]
		public ThreadFbid(string value)
		{
			Value = long.Parse(value);
		}

		public ThreadFbid(long value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is ThreadFbid)
			{
				return Value == ((ThreadFbid)obj).Value;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}

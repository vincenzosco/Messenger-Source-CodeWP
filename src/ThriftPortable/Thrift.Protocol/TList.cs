namespace Thrift.Protocol
{
	public struct TList
	{
		private TType elementType;

		private int count;

		public TType ElementType
		{
			get
			{
				return elementType;
			}
			set
			{
				elementType = value;
			}
		}

		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				count = value;
			}
		}

		public TList(TType elementType, int count)
		{
			this = default(TList);
			this.elementType = elementType;
			this.count = count;
		}
	}
}

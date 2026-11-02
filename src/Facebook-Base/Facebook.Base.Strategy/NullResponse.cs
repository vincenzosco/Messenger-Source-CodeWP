namespace Facebook.Base.Strategy
{
	public sealed class NullResponse
	{
		public static NullResponse Null
		{
			get
			{
				return null;
			}
		}

		private NullResponse()
		{
		}
	}
}

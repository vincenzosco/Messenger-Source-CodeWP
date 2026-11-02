namespace Facebook.Models.Signals
{
	public class CacheInboxDataClearedSignal
	{
		public readonly DataTypeCleared DateTypeCleared;

		public CacheInboxDataClearedSignal(DataTypeCleared dataTypeCleared)
		{
			DateTypeCleared = dataTypeCleared;
		}
	}
}

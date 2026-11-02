namespace Facebook.Base.Network
{
	public interface INetworkStatus
	{
		void AddHandler(NetworkStatusHandler handler);

		void RemoveHandler(NetworkStatusHandler handler);

		bool IsConnected();

		void ResetNetworkMap();
	}
}

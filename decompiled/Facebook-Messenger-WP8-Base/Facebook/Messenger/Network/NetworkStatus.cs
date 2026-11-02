using System.Collections.Generic;
using System.Linq;
using Facebook.Base;
using Facebook.Base.Network;
using Microsoft.Phone.Net.NetworkInformation;

namespace Facebook.Messenger.Network;

public class NetworkStatus : INetworkStatus
{
	private readonly object _referencesLock = new object();

	private readonly List<DelegateReference> _references;

	private readonly IDispatcher _dispatcher;

	private readonly Dictionary<string, bool> _networkStateMap;

	private bool _connected;

	public NetworkStatus(IDispatcher dispatcher)
	{
		_dispatcher = dispatcher;
		_references = new List<DelegateReference>();
		_networkStateMap = new Dictionary<string, bool>();
		DeviceNetworkInformation.NetworkAvailabilityChanged += OnNetworkAvailabilityChanged;
		InitializeConnection();
	}

	public void AddHandler(NetworkStatusHandler handler)
	{
		lock (_referencesLock)
		{
			_references.Add(new DelegateReference(handler, keepReferenceAlive: false));
			PruneOldReferences();
		}
	}

	public void RemoveHandler(NetworkStatusHandler handler)
	{
		lock (_referencesLock)
		{
			_references.RemoveAll((DelegateReference d) => d.Target as NetworkStatusHandler == handler);
			PruneOldReferences();
		}
	}

	private void PruneOldReferences()
	{
		lock (_referencesLock)
		{
			_references.RemoveAll((DelegateReference d) => (object)d.Target == null);
		}
	}

	private void OnNetworkAvailabilityChanged(object o, NetworkNotificationEventArgs e)
	{
		MapNetworkInterface(e.NetworkInterface);
		CheckConnection();
	}

	public bool IsConnected()
	{
		return _connected;
	}

	public void ResetNetworkMap()
	{
		lock (_networkStateMap)
		{
			_networkStateMap.Clear();
		}
		InitializeConnection();
	}

	private static bool IsValidInterface(NetworkInterfaceInfo networkInterface)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if ((int)networkInterface.InterfaceSubtype != 0)
		{
			return networkInterface.Bandwidth != -1;
		}
		return false;
	}

	private void InitializeConnection()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected Obj, but got Unknown
		NetworkInterfaceList val = new NetworkInterfaceList();
		foreach (NetworkInterfaceInfo item in val)
		{
			MapNetworkInterface(item);
		}
		CheckConnection();
	}

	private void MapNetworkInterface(NetworkInterfaceInfo networkInterface)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		if (!IsValidInterface(networkInterface))
		{
			return;
		}
		string interfaceName = networkInterface.InterfaceName;
		lock (_networkStateMap)
		{
			if (!_networkStateMap.ContainsKey(interfaceName))
			{
				_networkStateMap.Add(interfaceName, value: false);
			}
			_networkStateMap[interfaceName] = (int)networkInterface.InterfaceState == 1;
		}
	}

	private void CheckConnection()
	{
		bool connected = _connected;
		bool flag;
		lock (_networkStateMap)
		{
			flag = (_connected = (_networkStateMap.Any() ? _networkStateMap.Values.Any((bool interfaceConnected) => interfaceConnected) : DeviceNetworkInformation.IsNetworkAvailable));
		}
		if (connected != flag)
		{
			Notify(flag);
		}
	}

	private void Notify(bool connected)
	{
		lock (_references)
		{
			PruneOldReferences();
			foreach (DelegateReference reference in _references)
			{
				NetworkStatusHandler target = reference.Target as NetworkStatusHandler;
				if (target != null)
				{
					_dispatcher.RunAsync(() =>
					{
						target(this, connected);
					}, DispatcherPriority.Normal);
				}
			}
		}
	}
}

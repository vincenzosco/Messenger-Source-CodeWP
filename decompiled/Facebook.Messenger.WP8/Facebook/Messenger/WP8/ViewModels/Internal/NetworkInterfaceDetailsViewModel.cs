using Microsoft.Phone.Net.NetworkInformation;

namespace Facebook.Messenger.WP8.ViewModels.Internal;

public class NetworkInterfaceDetailsViewModel
{
	public NetworkInterfaceList NetworkInterfaces { get; set; }

	public NetworkInterfaceDetailsViewModel()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected Obj, but got Unknown
		NetworkInterfaces = new NetworkInterfaceList();
	}
}

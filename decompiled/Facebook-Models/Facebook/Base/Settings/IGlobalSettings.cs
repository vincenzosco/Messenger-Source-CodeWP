using System.Threading.Tasks;
using Facebook.Models;

namespace Facebook.Base.Settings;

public interface IGlobalSettings
{
	string FacebookMachineId { get; set; }

	string DeviceIdentifier { get; }

	void ClearSession(string id);

	bool SaveSession(FacebookSession session);

	FacebookSession LoadSession();

	Task PurgeUserFolders(params string[] excludes);
}

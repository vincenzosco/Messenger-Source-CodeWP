using Facebook.Base.Extensions;
using Facebook.Services.Api.Cache;
using Facebook.Services.SyncProtocol;

namespace Facebook.Messenger.WP8.ViewModels.Internal;

public class SyncProtocolSettingsViewModel
{
	private readonly Service _syncService;

	private readonly ThreadCache _threadCache;

	public SyncProtocolSettingsViewModel(Service syncProtocol, ThreadCache threadCache)
	{
		_syncService = syncProtocol;
		_threadCache = threadCache;
	}

	public void SimulateSyncError()
	{
		_syncService.SimulateSyncError();
	}

	public void ClearThreadCache()
	{
		_threadCache.ClearInboxData().DoNotWait();
	}
}

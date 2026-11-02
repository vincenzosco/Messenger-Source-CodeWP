using System.Collections.Generic;
using Facebook.Models;
using GalaSoft.MvvmLight;

namespace Facebook.Messenger.WP8.Utilities.PhoneContacts;

public class ContactSyncStatus : ViewModelBase
{
	private SyncState _status;

	private double _contactSyncProgress;

	private List<FacebookUser> _syncedContactsList;

	public SyncState Status
	{
		get
		{
			return _status;
		}
		private set
		{
			Set(ref _status, value, broadcast: false, "Status");
		}
	}

	public double ContactSyncProgress
	{
		get
		{
			return _contactSyncProgress;
		}
		private set
		{
			Set(ref _contactSyncProgress, value, broadcast: false, "ContactSyncProgress");
		}
	}

	public List<FacebookUser> SyncedContactsList
	{
		get
		{
			return _syncedContactsList;
		}
		private set
		{
			Set(ref _syncedContactsList, value, broadcast: false, "SyncedContactsList");
		}
	}

	public ContactSyncStatus()
	{
		Status = SyncState.NotStarted;
	}

	public void OnContactSyncStarted()
	{
		Status = SyncState.InProgress;
		SyncedContactsList = new List<FacebookUser>();
	}

	public void OnContactSyncComplete(List<FacebookUser> syncedContacts)
	{
		Status = SyncState.Complete;
		SyncedContactsList = syncedContacts;
	}

	public void OnContactSyncCancelled()
	{
		Status = SyncState.Failed;
		SyncedContactsList = new List<FacebookUser>();
	}

	public void OnContactSyncReset()
	{
		Status = SyncState.NotStarted;
		SyncedContactsList = new List<FacebookUser>();
	}

	public void OnProgressUpdate(double progress)
	{
		ContactSyncProgress = progress;
	}
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models.PhoneContacts;

namespace Facebook.Services.Api.Cache
{
	public class PhoneContactCache : ICache
	{
		private const string CacheSaveKey = "phone_contacts_cache";

		private readonly IUserDataStorage _storage;

		private readonly DisposableMutexInit _lock;

		private Dictionary<string, int> _cacheData;

		private bool _dirty;

		public PhoneContactCache(IUserDataStorage storage)
		{
			_storage = storage;
			_lock = new DisposableMutexInit(Init);
		}

		private async Task Init()
		{
			_cacheData = (await SaveCacheUtil.Load<Dictionary<string, int>>(_storage, "phone_contacts_cache")) ?? new Dictionary<string, int>();
		}

		private async Task UnsafeSaveState()
		{
			await SaveCacheUtil.Save(_storage, "phone_contacts_cache", _cacheData);
			_dirty = false;
		}

		public async Task EnsureSave()
		{
			if (!(await _lock.IsInitialized()))
			{
				Logger.Info(this, "Did not save cache: Cache was not initialized.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\PhoneContactCache.cs", 44);
				return;
			}
			Logger.Info(this, "Acquiring Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\PhoneContactCache.cs", 48);
			using (await _lock.WaitAsync())
			{
				Logger.Info(this, "Acquired Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\PhoneContactCache.cs", 51);
				if (_dirty)
				{
					await UnsafeSaveState();
				}
				else
				{
					Logger.Info(this, "Did not save cache: Cache did not change.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\PhoneContactCache.cs", 58);
				}
			}
		}

		public async Task Purge()
		{
			using (await _lock.WaitAsync())
			{
				_cacheData.Clear();
				_dirty = true;
			}
		}

		public async Task<List<FBPhoneContact>> GetAddedOrModifiedContacts(IEnumerable<FBPhoneContact> phoneContacts)
		{
			List<FBPhoneContact> addedOrModifiedContacts = new List<FBPhoneContact>();
			using (await _lock.WaitAsync())
			{
				foreach (FBPhoneContact phoneContact in phoneContacts)
				{
					int value;
					if (!_cacheData.TryGetValue(phoneContact.ContactId, out value) || value != phoneContact.GetHashCode())
					{
						addedOrModifiedContacts.Add(phoneContact);
					}
				}
				return addedOrModifiedContacts;
			}
		}

		public async Task AddContactsToCache(IEnumerable<FBPhoneContact> phoneContacts)
		{
			using (await _lock.WaitAsync())
			{
				foreach (FBPhoneContact phoneContact in phoneContacts)
				{
					_cacheData[phoneContact.ContactId] = phoneContact.GetHashCode();
				}
				_dirty = true;
			}
		}
	}
}

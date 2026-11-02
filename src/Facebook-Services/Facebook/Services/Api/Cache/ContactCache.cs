using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;

namespace Facebook.Services.Api.Cache
{
	public class ContactCache : ICache
	{
		private const string CacheSaveKey = "contact_cache";

		private readonly IUserDataStorage _storage;

		private readonly DisposableMutexInit _lock;

		private ContactCacheData _data;

		private bool _dirty;

		public ContactCache(IUserDataStorage storage)
		{
			_storage = storage;
			_lock = new DisposableMutexInit(Init);
		}

		private async Task Init()
		{
			_data = (await SaveCacheUtil.Load<ContactCacheData>(_storage, "contact_cache")) ?? new ContactCacheData();
		}

		private async Task UnsafeSaveState()
		{
			await SaveCacheUtil.Save(_storage, "contact_cache", _data);
			_dirty = false;
		}

		public async Task EnsureSave()
		{
			if (!(await _lock.IsInitialized()))
			{
				Logger.Info(this, "Did not save cache: Cache was not initialized.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ContactCache.cs", 44);
				return;
			}
			Logger.Info(this, "Acquiring Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ContactCache.cs", 48);
			using (await _lock.WaitAsync())
			{
				Logger.Info(this, "Acquired Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ContactCache.cs", 51);
				if (_dirty)
				{
					await UnsafeSaveState();
				}
				else
				{
					Logger.Info(this, "Did not save cache: Cache did not change.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ContactCache.cs", 58);
				}
			}
		}

		public async Task Purge()
		{
			using (await _lock.WaitAsync())
			{
				_data.Clear();
				_dirty = true;
			}
		}

		public async Task<List<FacebookUser>> GetContacts()
		{
			using (await _lock.WaitAsync())
			{
				return DeepCopy(_data.Contacts.Values.ToList());
			}
		}

		public async Task<bool> Update(List<FacebookUser> newContacts, List<FacebookUser> deletedContacts)
		{
			using (await _lock.WaitAsync())
			{
				bool flag = false;
				if (newContacts != null && newContacts.Any())
				{
					flag = true;
					foreach (FacebookUser newContact in newContacts)
					{
						_data.Contacts[newContact.Id] = DeepCopy(newContact);
					}
				}
				if (deletedContacts != null && deletedContacts.Any())
				{
					flag = true;
					foreach (string item in deletedContacts.Select((FacebookUser u) => u.Id))
					{
						_data.Contacts.Remove(item);
					}
				}
				if (flag)
				{
					_dirty = true;
				}
				return flag;
			}
		}

		public async Task<FacebookUser> GetUser(string uid)
		{
			using (await _lock.WaitAsync())
			{
				return _data.Contacts.ContainsKey(uid) ? DeepCopy(_data.Contacts[uid]) : null;
			}
		}

		private static IEnumerable<string> CalculateFilters(string filterString)
		{
			return (filterString ?? string.Empty).Split(new string[4] { " ", "\t", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
		}

		private static bool UserMatchesFilters(FacebookUser user, IEnumerable<string> filters)
		{
			if (!filters.Any())
			{
				return true;
			}
			string[] fields = new string[4] { user.Name, user.FirstName, user.MiddleName, user.LastName };
			return filters.All((string filter) => fields.Any((string field) => field != null && CultureInfo.CurrentUICulture.CompareInfo.IndexOf(field, filter, CompareOptions.IgnoreCase) >= 0));
		}

		public async Task<List<FacebookUser>> GetUserFromFilter(string filterString)
		{
			using (await _lock.WaitAsync())
			{
				IEnumerable<string> filters = CalculateFilters(filterString);
				IEnumerable<FacebookUser> source = _data.Contacts.Values.Where((FacebookUser user) => UserMatchesFilters(user, filters));
				return DeepCopy(source.ToList());
			}
		}

		public async Task<long> GetLastSyncTime()
		{
			using (await _lock.WaitAsync())
			{
				return _data.LastSyncTime;
			}
		}

		public async Task<long> GetLastFullSyncTime()
		{
			using (await _lock.WaitAsync())
			{
				return _data.LastFullSyncTime;
			}
		}

		public async Task SetLastSyncTime(long time)
		{
			using (await _lock.WaitAsync())
			{
				_data.LastSyncTime = time;
				_dirty = true;
			}
		}

		public async Task SetLastFullSyncTime(long time)
		{
			using (await _lock.WaitAsync())
			{
				_data.LastFullSyncTime = time;
				_dirty = true;
			}
		}

		[Obsolete("Should be removed when FacebookUser becomes immutable")]
		private FacebookUser DeepCopy(FacebookUser u)
		{
			return new MailUser(u);
		}

		[Obsolete("Should be removed when FacebookUser becomes immutable")]
		private List<FacebookUser> DeepCopy(List<FacebookUser> users)
		{
			return users.Select(DeepCopy).ToList();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Localization;
using GalaSoft.MvvmLight.Messaging;

namespace Facebook.Services.Api.Cache
{
	public class ThreadCache : ICache
	{
		private const string CacheSaveKey = "thread_cache";

		private readonly IUserDataStorage _storage;

		private readonly IDispatcher _uiDispatcher;

		private readonly IThreadSnippetGenerator _snippetGenerator;

		private readonly DisposableMutexInit _lock;

		private ThreadCachedData _data;

		public ThreadCache(IUserDataStorage storage, IDispatcher uiDispatcher, IThreadSnippetGenerator snippetGenerator)
		{
			_storage = storage;
			_uiDispatcher = uiDispatcher;
			_snippetGenerator = snippetGenerator;
			_lock = new DisposableMutexInit(Init);
		}

		private async Task Init()
		{
			_data = (await SaveCacheUtil.Load<ThreadCachedData>(_storage, "thread_cache")) ?? new ThreadCachedData();
		}

		private async Task UnsafeSaveState()
		{
			Logger.Info(this, "Trimming Threads and Messages", "UnsafeSaveState", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 55);
			_data.TrimThreadsAndMessages();
			Logger.Info(this, "Trimmed Threads and Messages", "UnsafeSaveState", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 57);
			await SaveCacheUtil.Save(_storage, "thread_cache", _data);
		}

		public async Task EnsureSave()
		{
			if (!(await _lock.IsInitialized()))
			{
				Logger.Info(this, "Did not save cache: Cache was not initialized.", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 65);
				return;
			}
			Logger.Info(this, "Acquiring Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 69);
			using (await _lock.WaitAsync())
			{
				Logger.Info(this, "Acquired Lock", "EnsureSave", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 72);
				await UnsafeSaveState();
			}
		}

		public async Task Purge()
		{
			using (await _lock.WaitAsync())
			{
				_data.Clear();
			}
		}

		public async Task ClearAllData()
		{
			using (await _lock.WaitAsync())
			{
				_data.Clear();
				UnsafeSignalInboxDataCleared(DataTypeCleared.All);
			}
		}

		public async Task ClearAllMessages()
		{
			using (await _lock.WaitAsync())
			{
				_data.ClearAllMessages();
				UnsafeSignalInboxDataCleared(DataTypeCleared.Messages);
			}
		}

		public async Task ClearInboxData()
		{
			using (await _lock.WaitAsync())
			{
				_data.ClearInboxData();
				UnsafeSignalInboxDataCleared(DataTypeCleared.ThreadsAndMessages);
			}
		}

		private void UnsafeSignalMessagesChanged(string threadId, MailMessage message, bool shouldNotify)
		{
			UnsafeSignalMessagesChanged(threadId, new List<MailMessage> { message }, shouldNotify);
		}

		private void UnsafeSignalMessagesChanged(string threadId, List<MailMessage> message, bool shouldNotify)
		{
			MailThread thread = CopyToProtectUntilImmutable(_data.Threads[threadId].Thread);
			message = CopyToProtectUntilImmutable(message);
			_uiDispatcher.RunAsync(() =>
			{
				Messenger.Default.Send(new CacheMessagesUpdatedSignal(thread, message, shouldNotify));
			});
		}

		private void UnsafeSignalThreadChanged(MailThread thread)
		{
			thread = CopyToProtectUntilImmutable(thread);
			_uiDispatcher.RunAsync(() =>
			{
				Messenger.Default.Send(new CacheThreadUpdatedSignal(thread));
			});
		}

		private void UnsafeSignalPinnedThreadChanged(PinnedThread thread)
		{
			_uiDispatcher.RunAsync(() =>
			{
				Messenger.Default.Send(new CachePinnedThreadUpdatedSignal(thread));
			});
		}

		private void UnsafeSignalMarkedQueuedThreadsFailed()
		{
			_uiDispatcher.RunAsync(() =>
			{
				Messenger.Default.Send(new CacheMarkedQueuedThreadsFailedSignal());
			});
		}

		private void UnsafeSignalInboxDataCleared(DataTypeCleared dateTypeCleared)
		{
			_uiDispatcher.RunAsync(() =>
			{
				Messenger.Default.Send(new CacheInboxDataClearedSignal(dateTypeCleared));
			});
		}

		public async Task<MailFolderCount> GetThreadCounts(string folder)
		{
			using (await _lock.WaitAsync())
			{
				if (folder != "inbox")
				{
					throw new NotImplementedException();
				}
				MailFolderCount counts = _data.Counts;
				return CopyToProtectUntilImmutable(counts);
			}
		}

		public async Task SetThreadCounts(MailFolderCount counts)
		{
			counts = CopyToProtectUntilImmutable(counts);
			using (await _lock.WaitAsync())
			{
				if (counts.Folder != "inbox")
				{
					throw new NotImplementedException();
				}
				if (_data.Counts.LastActionId > counts.LastActionId)
				{
					Logger.Error(this, "Storing folder counts with older action ID", "SetThreadCounts", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 171);
				}
				counts.LargestSavedThreadActionId = _data.Counts.LargestSavedThreadActionId;
				_data.Counts = counts;
			}
		}

		public async Task<long> GetLargestSequenceId()
		{
			using (await _lock.WaitAsync())
			{
				return _data.LargestSyncSequenceId;
			}
		}

		public async Task ClearLargestSequenceId()
		{
			using (await _lock.WaitAsync())
			{
				_data.ClearLargestSequenceId();
			}
		}

		public async Task<SequenceIdState> DetermineSequenceIdStateAndUpdate(long sequenceId, long extraCount)
		{
			using (await _lock.WaitAsync())
			{
				Logger.Info(this, string.Format("Determining sequence Id state, Current sequence Id: {0}, new sequence Id: {1}, extra count: {2}", new object[3] { _data.LargestSyncSequenceId, sequenceId, extraCount }), "DetermineSequenceIdStateAndUpdate", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 203);
				long largestSyncSequenceId = _data.LargestSyncSequenceId;
				if (sequenceId < largestSyncSequenceId)
				{
					return SequenceIdState.AlreadySeen;
				}
				if (sequenceId > largestSyncSequenceId + 1)
				{
					return SequenceIdState.Unexpected;
				}
				_data.LargestSyncSequenceId = sequenceId + extraCount;
				return SequenceIdState.Contiguous;
			}
		}

		public async Task<MailThread> GetThreadByThreadId(string threadId)
		{
			using (await _lock.WaitAsync())
			{
				MailThread thread = (_data.Threads.ContainsKey(threadId) ? _data.Threads[threadId].Thread : null);
				return CopyToProtectUntilImmutable(thread);
			}
		}

		public async Task<string> GetThreadIdByThreadFbid(long threadFbid)
		{
			using (await _lock.WaitAsync())
			{
				return UnsafeGetThreadIdByThreadFbId(threadFbid);
			}
		}

		public async Task<string> GetThreadIdBySingleRecipientId(string recipientId)
		{
			using (await _lock.WaitAsync())
			{
				MailThread mailThread = UnsafeGetThreadBySingleRecipientId(recipientId);
				return (mailThread != null) ? mailThread.Id : null;
			}
		}

		public async Task<MailThread> GetThreadBySingleRecipientId(string recipientId)
		{
			using (await _lock.WaitAsync())
			{
				MailThread thread = UnsafeGetThreadBySingleRecipientId(recipientId);
				return CopyToProtectUntilImmutable(thread);
			}
		}

		private MailThread UnsafeGetThreadBySingleRecipientId(string recipientId)
		{
			CachedThreadInfo cachedThreadInfo = _data.Threads.Values.SingleOrDefaultLogErrorIfNotSingle((CachedThreadInfo i) => i.Thread.SingleRecipient == recipientId, "UnsafeGetThreadBySingleRecipientId");
			return (cachedThreadInfo != null) ? cachedThreadInfo.Thread : null;
		}

		public async Task<List<MailThread>> GetThreadsByActionId(long actionId)
		{
			using (await _lock.WaitAsync())
			{
				List<MailThread> threads = (from i in _data.Threads.Values
					where i.Thread.ActionId > actionId
					select i.Thread into i
					orderby i.ActionId descending
					select i).ToList();
				return CopyToProtectUntilImmutable(threads);
			}
		}

		public async Task<List<MailThread>> GetThreadsByMaxTimestamp(long maxTimestamp, int limit)
		{
			using (await _lock.WaitAsync())
			{
				if (maxTimestamp == 0)
				{
					maxTimestamp = long.MaxValue;
				}
				List<MailThread> threads = (from i in (from i in _data.Threads.Values
						where i.Thread.Timestamp < maxTimestamp
						orderby i.Thread.Timestamp descending
						select i).Take(limit)
					select i.Thread).ToList();
				return CopyToProtectUntilImmutable(threads);
			}
		}

		public async Task<List<string>> GetThreadsIdsWithFailedSends()
		{
			using (await _lock.WaitAsync())
			{
				return (from t in _data.Threads.Values
					where t.HasFailedMessage()
					select t.Thread.Id).ToList();
			}
		}

		public async Task<ActionIdResult> GetActionIdsForThreadByThreadId(string threadId)
		{
			using (await _lock.WaitAsync())
			{
				ActionIdResult actionIdResult = new ActionIdResult();
				if (_data.Threads.ContainsKey(threadId))
				{
					CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
					actionIdResult.ThreadLastAddActionId = cachedThreadInfo.Thread.LastVisibleAddActionId;
					actionIdResult.MaxMessageActionId = (cachedThreadInfo.Messages.Any() ? cachedThreadInfo.Messages.Max((MailMessage m) => (!m.HasStaleData()) ? m.ActionId : 0) : 0);
					actionIdResult.HasStaleMessages = cachedThreadInfo.Messages.Any((MailMessage m) => m.State == MailMessage.MessageState.Incomplete);
				}
				return actionIdResult;
			}
		}

		public async Task UpdateExistingThread(MailThread thread)
		{
			thread = CopyToProtectUntilImmutable(thread);
			using (await _lock.WaitAsync())
			{
				if (_data.Threads.ContainsKey(thread.Id))
				{
					_data.Threads[thread.Id].UpdateThread(thread);
					UnsafeUpdateLargestSaveThreadActionId(thread.ActionId);
					UnsafeUpdateLargestSyncSequenceId(thread.SyncSequenceId);
					UnsafeSignalThreadChanged(_data.Threads[thread.Id].Thread);
				}
			}
		}

		public async Task AddThread(MailThread thread)
		{
			thread = CopyToProtectUntilImmutable(thread);
			using (await _lock.WaitAsync())
			{
				if (thread.Deleted || thread.Archived)
				{
					UnsafeRemoveThreadAndPinnedThreadByThreadId(thread.Id);
					return;
				}
				if (!thread.IsThreadOffline() && thread.IsCanonical())
				{
					string deviceGeneratedId = UnsafeGetIdAndRemoveThreadIfDeviceGenerated(thread.SingleRecipient);
					thread.DeviceGeneratedId = deviceGeneratedId;
				}
				if (_data.Threads.ContainsKey(thread.Id))
				{
					_data.Threads[thread.Id].UpdateThread(thread);
				}
				else
				{
					_data.Threads[thread.Id] = new CachedThreadInfo(thread);
				}
				UnsafeUpdateLargestSaveThreadActionId(thread.ActionId);
				UnsafeUpdateLargestSyncSequenceId(thread.SyncSequenceId);
				UnsafeUpdatePinnedThreadDetails(thread);
				UnsafeSignalThreadChanged(_data.Threads[thread.Id].Thread);
			}
		}

		private void UnsafeUpdateLargestSaveThreadActionId(long actionId)
		{
			if (_data.Counts.LargestSavedThreadActionId < actionId)
			{
				_data.Counts.LargestSavedThreadActionId = actionId;
			}
		}

		private void UnsafeUpdateLargestSyncSequenceId(long syncSequenceId)
		{
			if (_data.LargestSyncSequenceId < syncSequenceId)
			{
				_data.LargestSyncSequenceId = syncSequenceId;
			}
		}

		private string UnsafeGetIdAndRemoveThreadIfDeviceGenerated(string singleRecipientId)
		{
			MailThread mailThread = UnsafeGetThreadBySingleRecipientId(singleRecipientId);
			if (mailThread == null || !mailThread.IsThreadOffline())
			{
				return null;
			}
			UnsafeRemoveThreadAndPinnedThreadByThreadId(mailThread.Id);
			return mailThread.DeviceGeneratedId;
		}

		public async Task MarkThreadAsRead(string threadId)
		{
			await SafeOperateOnThreadIfExists(threadId, ThreadCacheHelper.MarkThreadAsRead, "MarkThreadAsRead");
		}

		public async Task MarkAllThreadsRead()
		{
			using (await _lock.WaitAsync())
			{
				foreach (CachedThreadInfo value in _data.Threads.Values)
				{
					UnsafeOperateOnThreadIfExists(value.Thread.Id, ThreadCacheHelper.MarkThreadAsRead);
				}
			}
		}

		public async Task MarkThreadAsUnread(string threadId)
		{
			await SafeOperateOnThreadIfExists(threadId, ThreadCacheHelper.MarkThreadAsUnread, "MarkThreadAsUnread");
		}

		public async Task MarkAllThreadsUnread()
		{
			using (await _lock.WaitAsync())
			{
				foreach (CachedThreadInfo value in _data.Threads.Values)
				{
					UnsafeOperateOnThreadIfExists(value.Thread.Id, ThreadCacheHelper.MarkThreadAsUnread);
				}
			}
		}

		public async Task AddReadReceiptByThreadId(string threadId, ReadReceipt receipt, bool shouldNotify)
		{
			receipt = CopyToProtectUntilImmutable(receipt);
			using (await _lock.WaitAsync())
			{
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				if (cachedThreadInfo != null)
				{
					MailThread thread = cachedThreadInfo.Thread;
					List<MailMessage> message = ThreadCacheHelper.SetReadReceipt(cachedThreadInfo, receipt);
					UnsafeSignalMessagesChanged(threadId, message, false);
					if (shouldNotify)
					{
						UnsafeSignalThreadChanged(thread);
					}
				}
			}
		}

		public async Task UpdateDeliveryReceipt(string threadId, DeliveryReceipt deliveryReceipt)
		{
			deliveryReceipt = CopyToProtectUntilImmutable(deliveryReceipt);
			using (await _lock.WaitAsync())
			{
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				if (cachedThreadInfo != null)
				{
					MailThread thread = cachedThreadInfo.Thread;
					thread.LastMessageState = MailMessage.MessageState.Delivered;
					thread.LastMessageReadUserIds.Clear();
					ThreadCacheHelper.SetDeliveryReceipt(thread, deliveryReceipt);
					UnsafeSignalThreadChanged(thread);
				}
			}
		}

		public async Task AddThreadParticipants(string threadId, IEnumerable<FacebookUser> participants)
		{
			await SafeOperateOnThreadIfExists(threadId, (MailThread t) =>
			{
				ThreadCacheHelper.AddParticipants(t, participants);
			}, "AddThreadParticipants");
		}

		public async Task RemoveThreadParticipants(string threadId, IEnumerable<string> participantIds)
		{
			await SafeOperateOnThreadIfExists(threadId, (MailThread t) =>
			{
				ThreadCacheHelper.RemoveParticipants(t, participantIds);
			}, "RemoveThreadParticipants");
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					Logger.Error(this, "Thread not found: RemoveMessage", "RemoveThreadParticipants", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 488);
					return;
				}
				List<MailMessage> message = ThreadCacheHelper.RemoveParticipantsReadReceipt(_data.Threads[threadId], participantIds);
				UnsafeSignalMessagesChanged(threadId, message, false);
			}
		}

		public async Task MarkThreadMuteState(string threadId, long muteUntil)
		{
			await SafeOperateOnThreadIfExists(threadId, (MailThread t) =>
			{
				ThreadCacheHelper.MarkThreadMuteState(t, muteUntil);
			}, "MarkThreadMuteState");
		}

		public async Task SetThreadName(string threadId, string name)
		{
			await SafeOperateOnThreadIfExists(threadId, (MailThread t) =>
			{
				ThreadCacheHelper.SetThreadName(t, name);
			}, "SetThreadName");
		}

		public async Task SetThreadPicture(string threadId, string hash)
		{
			await SafeOperateOnThreadIfExists(threadId, (MailThread t) =>
			{
				ThreadCacheHelper.SetThreadPictureHash(t, hash);
			}, "SetThreadPicture");
		}

		public async Task ClearThreadPicture(string threadId)
		{
			await SafeOperateOnThreadIfExists(threadId, ThreadCacheHelper.ClearPicture, "ClearThreadPicture");
		}

		private async Task SafeOperateOnThreadIfExists(string threadId, Action<MailThread> action, [CallerMemberName] string methodName = "unknown")
		{
			using (await _lock.WaitAsync())
			{
				UnsafeOperateOnThreadIfExists(threadId, action, methodName);
			}
		}

		private void UnsafeOperateOnThreadIfExists(string threadId, Action<MailThread> action, string methodName = "unknown")
		{
			bool flag = UnsafeOperateOnCachedThreadIfExists(threadId, action);
			bool flag2 = UnsafeOperateOnPinnedThreadIfExists(threadId, action);
			if (!flag && !flag2)
			{
				Logger.Error(this, "Thread not found: " + methodName, "UnsafeOperateOnThreadIfExists", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 532);
			}
		}

		private bool UnsafeOperateOnCachedThreadIfExists(string threadId, Action<MailThread> action)
		{
			if (!_data.Threads.ContainsKey(threadId))
			{
				return false;
			}
			MailThread thread = _data.Threads[threadId].Thread;
			action(thread);
			UnsafeSignalThreadChanged(thread);
			return true;
		}

		public async Task RemoveThreadByThreadId(string threadId)
		{
			using (await _lock.WaitAsync())
			{
				UnsafeRemoveThreadByThreadId(threadId);
			}
		}

		public async Task RemoveThreadAndPinnedThreadByThreadId(string threadId)
		{
			using (await _lock.WaitAsync())
			{
				UnsafeRemoveThreadAndPinnedThreadByThreadId(threadId);
			}
		}

		public async Task RemoveThreadAndPinnedThreadByThreadFbId(long threadFbId)
		{
			using (await _lock.WaitAsync())
			{
				UnsafeRemoveThreadAndPinnedThreadByThreadFbId(threadFbId);
			}
		}

		public async Task RemoveThreadByRecipientId(string recipientId)
		{
			using (await _lock.WaitAsync())
			{
				UnsafeRemoveThreadByRecipientId(recipientId);
			}
		}

		private void UnsafeRemoveThreadByThreadId(string threadId)
		{
			if (_data.Threads.ContainsKey(threadId))
			{
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				cachedThreadInfo.Thread.SyncChangeType = "deleted";
				_data.Threads.Remove(threadId);
				UnsafeSignalThreadChanged(cachedThreadInfo.Thread);
			}
		}

		private void UnsafeRemoveThreadAndPinnedThreadByThreadId(string threadId)
		{
			UnsafeRemoveThreadByThreadId(threadId);
			UnsafeRemovePinnedThreadByThreadId(threadId);
		}

		private void UnsafeRemoveThreadByRecipientId(string recipientId)
		{
			List<MailThread> list = (from i in _data.Threads.Values
				where i.Thread.SingleRecipient != null && i.Thread.SingleRecipient == recipientId
				select i.Thread).ToList();
			if (!list.Any())
			{
				return;
			}
			foreach (MailThread item in list)
			{
				UnsafeRemoveThreadByThreadId(item.Id);
			}
		}

		private string UnsafeGetThreadIdByThreadFbId(long threadFbId)
		{
			CachedThreadInfo cachedThreadInfo = _data.Threads.Values.SingleOrDefaultLogErrorIfNotSingle((CachedThreadInfo i) => i.Thread.Fbid.Value == threadFbId, "UnsafeGetThreadIdByThreadFbId");
			if (cachedThreadInfo == null)
			{
				return null;
			}
			return cachedThreadInfo.Thread.Id;
		}

		private void UnsafeRemoveThreadAndPinnedThreadByThreadFbId(long threadFbId)
		{
			string text = UnsafeGetThreadIdByThreadFbId(threadFbId);
			string text2 = UnsafeGetPinnedThreadIdByThreadFbId(threadFbId);
			if (text != null)
			{
				UnsafeRemoveThreadByThreadId(text);
			}
			if (text2 != null)
			{
				UnsafeRemovePinnedThreadByThreadId(text2);
			}
		}

		public async Task<List<PinnedThread>> GetPinnedThreads()
		{
			using (await _lock.WaitAsync())
			{
				List<PinnedThread> threads = _data.PinnedThreads.OrderBy((PinnedThread t) => t.Position).ToList();
				return CopyToProtectUntilImmutable(threads);
			}
		}

		public async Task AddPinnedThread(PinnedThread pinnedThread)
		{
			pinnedThread = CopyToProtectUntilImmutable(pinnedThread);
			using (await _lock.WaitAsync())
			{
				UnsafeAddPinnedThread(pinnedThread);
			}
		}

		private void UnsafeAddPinnedThread(PinnedThread pinnedThread)
		{
			PinnedThread pinnedThread2 = _data.PinnedThreads.SingleOrDefaultLogErrorIfNotSingle((PinnedThread p) => p.Id == pinnedThread.Id, "UnsafeAddPinnedThread");
			if (pinnedThread2 != null)
			{
				_data.PinnedThreads.Remove(pinnedThread2);
			}
			_data.PinnedThreads.Add(pinnedThread);
			UnsafeSignalPinnedThreadChanged(pinnedThread);
		}

		private bool UnsafeOperateOnPinnedThreadIfExists(string threadId, Action<MailThread> action)
		{
			PinnedThread pinnedThread = _data.PinnedThreads.SingleOrDefaultLogErrorIfNotSingle((PinnedThread p) => p.Id == threadId, "UnsafeOperateOnPinnedThreadIfExists");
			if (pinnedThread == null)
			{
				return false;
			}
			action(pinnedThread);
			UnsafeSignalPinnedThreadChanged(pinnedThread);
			return true;
		}

		public async Task AddPinnedThreadAtEnd(MailThread thread)
		{
			thread = CopyToProtectUntilImmutable(thread);
			using (await _lock.WaitAsync())
			{
				int position = 0;
				long updateTime = 0L;
				if (_data.PinnedThreads.Any())
				{
					position = _data.PinnedThreads.Max((PinnedThread p) => p.Position) + 1;
					updateTime = _data.PinnedThreads.Max((PinnedThread p) => p.UpdateTime);
				}
				PinnedThread pinnedThread = new PinnedThread(thread);
				pinnedThread.Position = position;
				pinnedThread.UpdateTime = updateTime;
				PinnedThread pinnedThread2 = pinnedThread;
				UnsafeAddPinnedThread(pinnedThread2);
			}
		}

		public async Task UpdatePinnedThreadDetails(MailThread thread)
		{
			thread = CopyToProtectUntilImmutable(thread);
			using (await _lock.WaitAsync())
			{
				UnsafeUpdatePinnedThreadDetails(thread);
			}
		}

		private void UnsafeUpdatePinnedThreadDetails(MailThread thread)
		{
			PinnedThread pinnedThread = _data.PinnedThreads.SingleOrDefaultLogErrorIfNotSingle((PinnedThread p) => p.Id == thread.Id, "UnsafeUpdatePinnedThreadDetails");
			if (pinnedThread != null)
			{
				pinnedThread.Timestamp = thread.Timestamp;
				pinnedThread.Name = thread.Name;
				pinnedThread.PictureHash = thread.PictureHash;
				pinnedThread.Participants = thread.Participants;
				UnsafeSignalPinnedThreadChanged(pinnedThread);
			}
		}

		public async Task RemovePinnedThreadsByThreadId(IEnumerable<string> threadIds)
		{
			using (await _lock.WaitAsync())
			{
				foreach (string threadId in threadIds)
				{
					UnsafeRemovePinnedThreadByThreadId(threadId);
				}
			}
		}

		public async Task UpdatePinnedThreadsOrder(IEnumerable<string> orderedThreadIds)
		{
			using (await _lock.WaitAsync())
			{
				int num = 0;
				foreach (PinnedThread item in from threadId in orderedThreadIds
					select _data.PinnedThreads.SingleOrDefaultLogErrorIfNotSingle((PinnedThread p) => p.Id == threadId, "UpdatePinnedThreadsOrder") into pinnedThread
					where pinnedThread != null
					select pinnedThread)
				{
					item.Position = num;
					num++;
					UnsafeSignalPinnedThreadChanged(item);
				}
			}
		}

		public async Task ClearPinnedThreads()
		{
			using (await _lock.WaitAsync())
			{
				_data.PinnedThreads.Clear();
			}
		}

		private string UnsafeGetPinnedThreadIdByThreadFbId(long threadFbId)
		{
			PinnedThread pinnedThread = _data.PinnedThreads.SingleOrDefaultLogErrorIfNotSingle((PinnedThread p) => p.Fbid.Value == threadFbId, "UnsafeGetPinnedThreadIdByThreadFbId");
			if (pinnedThread == null)
			{
				return null;
			}
			return pinnedThread.Id;
		}

		private void UnsafeRemovePinnedThreadByThreadId(string threadId)
		{
			PinnedThread pinnedThread = _data.PinnedThreads.SingleOrDefaultLogErrorIfNotSingle((PinnedThread p) => p.Id == threadId, "UnsafeRemovePinnedThreadByThreadId");
			if (pinnedThread != null)
			{
				_data.PinnedThreads.Remove(pinnedThread);
				pinnedThread.SyncChangeType = "deleted";
				UnsafeSignalPinnedThreadChanged(pinnedThread);
			}
		}

		public async Task<List<MailMessage>> GetMessagesByActionIdAndThreadId(string threadId, long actionId)
		{
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					return null;
				}
				List<MailMessage> messages = _data.Threads[threadId].Messages.Where((MailMessage m) => m.ActionId > actionId).OrderBy((MailMessage m) => m, MailMessageNewestFirstComparer.Comparer).ToList();
				return CopyToProtectUntilImmutable(messages);
			}
		}

		public async Task<List<MailMessage>> GetMessagesByMaxtimestampAndThreadId(string threadId, long maxTimestamp, int limit)
		{
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					return null;
				}
				if (maxTimestamp == 0)
				{
					maxTimestamp = long.MaxValue;
				}
				List<MailMessage> messages = (from m in _data.Threads[threadId].Messages
					where m.Timestamp < maxTimestamp
					orderby m.Timestamp descending
					select m).Take(limit).ToList();
				return CopyToProtectUntilImmutable(messages);
			}
		}

		public async Task<MailMessage> GetMessageByIdAndThreadId(string threadId, string messageId, string offlineThreadId)
		{
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					return null;
				}
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				MailMessage mailMessage = cachedThreadInfo.Messages.FirstOrDefault((MailMessage m) => (m.Id != null && m.Id == messageId) || (m.OfflineThreadingId != null && m.OfflineThreadingId == offlineThreadId));
				if (mailMessage == null)
				{
					return null;
				}
				mailMessage.Recipients = cachedThreadInfo.Thread.Participants;
				return CopyToProtectUntilImmutable(mailMessage);
			}
		}

		public async Task<int> GetMessageCountByThreadId(string threadId)
		{
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					return 0;
				}
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				return cachedThreadInfo.Messages.Count;
			}
		}

		public async Task<bool> AddMessage(MailMessage message, bool shouldNotify)
		{
			message = CopyToProtectUntilImmutable(message);
			using (await _lock.WaitAsync())
			{
				if (string.IsNullOrEmpty(message.ThreadId) || !_data.Threads.ContainsKey(message.ThreadId))
				{
					Logger.Error(this, "Thread not found: AddMessage", "AddMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 859);
					return false;
				}
				if (message is MailThreadAction)
				{
					MailMessage mailMessage = new MailMessage();
					mailMessage.CopyFrom(message);
					message = mailMessage;
				}
				_data.Threads[message.ThreadId].AddMessage(message);
				UnsafeSignalMessagesChanged(message.ThreadId, message, shouldNotify);
			}
			return true;
		}

		public async Task AddMessageAndUpdateThread(MailMessage message, bool shouldNotify, bool isOtherSender = false)
		{
			await AddMessage(message, shouldNotify);
			using (await _lock.WaitAsync())
			{
				if (string.IsNullOrEmpty(message.ThreadId) || !_data.Threads.ContainsKey(message.ThreadId))
				{
					Logger.Error(this, "Thread not found: AddMessageAndUpdateThread", "AddMessageAndUpdateThread", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 886);
					return;
				}
				_data.Counts.LargestSavedThreadActionId = Math.Max(_data.Counts.LargestSavedThreadActionId, message.ActionId);
				CachedThreadInfo cachedThreadInfo = _data.Threads[message.ThreadId];
				MailThread thread = cachedThreadInfo.Thread;
				if (isOtherSender)
				{
					List<MailMessage> message2 = ThreadCacheHelper.SetReadReceipt(cachedThreadInfo, new ReadReceipt
					{
						Id = message.Sender.Id,
						Timestamp = message.Timestamp
					});
					UnsafeSignalMessagesChanged(thread.Id, message2, false);
				}
				ThreadCacheHelper.UpdateThreadDetailsWithMessage(thread, message, _snippetGenerator.GenerateSnippet(message, thread));
				UnsafeSignalThreadChanged(thread);
			}
		}

		public async Task UpdateThreadLastMessageState(string threadId, string offlineThreadingId, MailMessage.MessageState state)
		{
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					Logger.Error(this, "Can't find thread to mark message", "UpdateThreadLastMessageState", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 916);
					return;
				}
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				if (cachedThreadInfo.Thread.LastMessageState != state)
				{
					cachedThreadInfo.Thread.LastMessageState = state;
					cachedThreadInfo.Thread.LastMessageReadUserIds.Clear();
				}
			}
		}

		public async Task<IEnumerable<MailMessage>> GetAllOutgoingMessages()
		{
			using (await _lock.WaitAsync())
			{
				MailMessage.MessageState[] desiredStates = new MailMessage.MessageState[2]
				{
					MailMessage.MessageState.Failed,
					MailMessage.MessageState.Queued
				};
				IEnumerable<MailMessage> messages = _data.Threads.Values.SelectMany((CachedThreadInfo thread) => thread.Messages.Where((MailMessage message) => desiredStates.Contains(message.State)));
				return CopyToProtectUntilImmutable(messages);
			}
		}

		public async Task MarkAllQueuedMessagesFailed()
		{
			using (await _lock.WaitAsync())
			{
				List<MailMessage> source = _data.Threads.Values.SelectMany((CachedThreadInfo t) => t.MarkAllQueuedMessagesFailed()).ToList();
				if (!source.Any())
				{
					return;
				}
				UnsafeSignalMarkedQueuedThreadsFailed();
				IEnumerable<string> enumerable = source.Select((MailMessage m) => m.ThreadId).Distinct();
				foreach (string threadId in enumerable)
				{
					Func<MailMessage, bool> predicate = (MailMessage m) => m.ThreadId == threadId;
					List<MailMessage> message = source.Where(predicate).ToList();
					UnsafeSignalMessagesChanged(threadId, message, false);
				}
			}
		}

		public async Task PatchSentMessageAndUpdateThread(MailMessage message)
		{
			using (await _lock.WaitAsync())
			{
				if (string.IsNullOrEmpty(message.ThreadId) || !_data.Threads.ContainsKey(message.ThreadId))
				{
					Logger.Error(this, "Thread not found: AddMessageAndUpdateThread", "PatchSentMessageAndUpdateThread", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 967);
					return;
				}
				CachedThreadInfo cachedThreadInfo = _data.Threads[message.ThreadId];
				MailMessage mailMessage = cachedThreadInfo.PatchSentMessage(message);
				if (mailMessage != null)
				{
					UnsafeSignalMessagesChanged(message.ThreadId, message, false);
					MailThread thread = cachedThreadInfo.Thread;
					thread.LastMessageState = MailMessage.MessageState.Sent;
					thread.LastMessageReadUserIds.Clear();
					_data.Counts.LargestSavedThreadActionId = Math.Max(_data.Counts.LargestSavedThreadActionId, mailMessage.ActionId);
					ThreadCacheHelper.UpdateThreadDetailsWithMessage(thread, mailMessage, _snippetGenerator.GenerateSnippet(mailMessage, thread));
					UnsafeSignalThreadChanged(thread);
				}
			}
		}

		public async Task MarkMessageSent(Dictionary<string, List<string>> threadsToRequestIds)
		{
			using (await _lock.WaitAsync())
			{
				foreach (string key in threadsToRequestIds.Keys)
				{
					if (_data.Threads.ContainsKey(key))
					{
						CachedThreadInfo cachedThreadInfo = _data.Threads[key];
						List<MailMessage> message = cachedThreadInfo.MarkQueuedMessagesSent(threadsToRequestIds[key]);
						UnsafeSignalMessagesChanged(key, message, false);
					}
				}
			}
		}

		public async Task MarkMessagesReadByTimestamp(string threadId, long readTimestamp)
		{
			using (await _lock.WaitAsync())
			{
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				List<MailMessage> list = cachedThreadInfo.Messages.Where((MailMessage message) => (message.State == MailMessage.MessageState.Delivered || message.State == MailMessage.MessageState.Sent) && message.Timestamp <= readTimestamp).ToList();
				foreach (MailMessage item in list)
				{
					item.State = MailMessage.MessageState.Seen;
				}
				UnsafeSignalMessagesChanged(threadId, list, false);
			}
		}

		public async Task MarkMessagesDeliveredByTimestamp(string threadId, long deliveredWatermarkTimestampMs)
		{
			await MarkMessagesDelivered(threadId, (MailMessage m) => m.State == MailMessage.MessageState.Sent && m.Timestamp <= deliveredWatermarkTimestampMs);
		}

		public async Task MarkMessagesDeliveredByIds(string threadId, List<string> messageIds)
		{
			await MarkMessagesDelivered(threadId, (MailMessage m) => messageIds.Contains(m.Id));
		}

		private async Task MarkMessagesDelivered(string threadId, Func<MailMessage, bool> predicate)
		{
			using (await _lock.WaitAsync())
			{
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				List<MailMessage> list = cachedThreadInfo.Messages.Where(predicate).ToList();
				cachedThreadInfo.MarkMessagesAsDelivered(list);
				UnsafeSignalMessagesChanged(threadId, list, false);
			}
		}

		public async Task RemoveMessage(string threadId, string messageId)
		{
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					Logger.Error(this, "Thread not found: RemoveMessage", "RemoveMessage", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 1052);
					return;
				}
				CachedThreadInfo cachedThreadInfo = _data.Threads[threadId];
				bool flag = cachedThreadInfo.Messages.Any() && cachedThreadInfo.Messages.OrderByDescending((MailMessage m) => m.Timestamp).First().Id == messageId;
				MailMessage mailMessage = cachedThreadInfo.RemoveMessage(messageId);
				if (mailMessage == null)
				{
					return;
				}
				if (cachedThreadInfo.Messages.Any())
				{
					if (flag)
					{
						MailMessage mailMessage2 = cachedThreadInfo.Messages.OrderByDescending((MailMessage m) => m.Timestamp).First();
						long lastTimestamp = mailMessage2.Timestamp;
						if (!_data.Threads.Any((KeyValuePair<string, CachedThreadInfo> t) => t.Value.Thread.Timestamp < lastTimestamp))
						{
							UnsafeRemoveThreadAndPinnedThreadByThreadId(cachedThreadInfo.Thread.Id);
							return;
						}
						UpdateSnippetAndTimestamp(cachedThreadInfo, mailMessage2, lastTimestamp);
					}
					if (mailMessage.MostRecentMessageReadByUserIds.Any())
					{
						MailMessage mailMessage3 = ThreadCacheHelper.HandleDeletionReadReceipt(cachedThreadInfo, mailMessage.Timestamp, mailMessage.MostRecentMessageReadByUserIds);
						if (mailMessage3 != null)
						{
							UnsafeSignalMessagesChanged(threadId, mailMessage3, false);
						}
					}
				}
				mailMessage.SyncChangeType = "deleted";
				UnsafeSignalMessagesChanged(threadId, mailMessage, false);
			}
		}

		public async Task RemoveMessagesByThreadId(string threadId)
		{
			using (await _lock.WaitAsync())
			{
				if (!_data.Threads.ContainsKey(threadId))
				{
					Logger.Error(this, "Thread not found: RemoveMessagesByThreadId", "RemoveMessagesByThreadId", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\ThreadCache.cs", 1104);
					return;
				}
				List<MailMessage> messages = _data.Threads[threadId].Messages;
				_data.Threads[threadId].Messages.Clear();
				foreach (MailMessage item in messages)
				{
					item.SyncChangeType = "deleted";
				}
				UnsafeSignalMessagesChanged(threadId, messages, false);
			}
		}

		private void UpdateSnippetAndTimestamp(CachedThreadInfo threadInfo, MailMessage lastMessage, long lastTimestamp)
		{
			MailThread thread = threadInfo.Thread;
			ThreadSnippetInfo threadSnippetInfo = _snippetGenerator.GenerateSnippet(lastMessage, thread);
			thread.Timestamp = lastTimestamp;
			thread.Snippet = threadSnippetInfo.Snippet;
			thread.SnippetFromMe = threadSnippetInfo.FromLoggedInUser;
			thread.SnippetMessageId = lastMessage.Id;
			thread.SnippetSender = lastMessage.Sender;
			thread.AdminSnippet = threadSnippetInfo.AdminSnippet;
			UnsafeSignalThreadChanged(thread);
		}

		private static MailFolderCount CopyToProtectUntilImmutable(MailFolderCount counts)
		{
			if (counts == null)
			{
				return null;
			}
			return MailFolderCount.CreateCopy(counts);
		}

		private static MailThread CopyToProtectUntilImmutable(MailThread thread)
		{
			if (thread == null)
			{
				return null;
			}
			return new MailThread(thread);
		}

		private static List<MailThread> CopyToProtectUntilImmutable(IEnumerable<MailThread> threads)
		{
			if (threads == null)
			{
				return null;
			}
			return threads.Select(CopyToProtectUntilImmutable).ToList();
		}

		private static PinnedThread CopyToProtectUntilImmutable(PinnedThread thread)
		{
			if (thread == null)
			{
				return null;
			}
			return new PinnedThread(thread);
		}

		private static List<PinnedThread> CopyToProtectUntilImmutable(IEnumerable<PinnedThread> threads)
		{
			if (threads == null)
			{
				return null;
			}
			return threads.Select(CopyToProtectUntilImmutable).ToList();
		}

		private static MailMessage CopyToProtectUntilImmutable(MailMessage message)
		{
			if (message == null)
			{
				return null;
			}
			return MailMessage.CreateCopy(message);
		}

		private static List<MailMessage> CopyToProtectUntilImmutable(IEnumerable<MailMessage> messages)
		{
			if (messages == null)
			{
				return null;
			}
			return messages.Select(CopyToProtectUntilImmutable).ToList();
		}

		private static ReadReceipt CopyToProtectUntilImmutable(ReadReceipt receipt)
		{
			if (receipt == null)
			{
				return null;
			}
			return ReadReceipt.CreateCopy(receipt);
		}

		private static DeliveryReceipt CopyToProtectUntilImmutable(DeliveryReceipt receipt)
		{
			if (receipt == null)
			{
				return null;
			}
			return DeliveryReceipt.CreateCopy(receipt);
		}

		private static List<ReadReceipt> CopyToProtectUntilImmutable(IEnumerable<ReadReceipt> receipts)
		{
			if (receipts == null)
			{
				return null;
			}
			return new List<ReadReceipt>(receipts);
		}
	}
}

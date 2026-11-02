using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol;

public static class ThreadDeltaUtil
{
	private const string MessageIdPrefix = "m_";

	private const string ActionMessageIdPrefix = "m_action";

	public static async Task<List<FetchRequired>> CreateFetchesForThreadsIfMissing(ThreadCache cache, IEnumerable<ThreadKey> threadKeys)
	{
		List<FetchRequired> fetchesRequired = new List<FetchRequired>();
		foreach (ThreadKey key in threadKeys)
		{
			fetchesRequired.Add(await CreateFetchRequiredForThread(cache, key));
		}
		return fetchesRequired;
	}

	public static async Task<List<FetchRequired>> CreateFetchForThreadIfMissing(ThreadCache cache, ThreadKey key)
	{
		return (await CreateFetchRequiredForThread(cache, key)).ToList();
	}

	private static async Task<FetchRequired> CreateFetchRequiredForThread(ThreadCache cache, ThreadKey key)
	{
		string threadId = await GetThreadId(cache, key);
		MailThread mailThread = ((threadId == null) ? null : (await cache.GetThreadByThreadId(threadId)));
		MailThread thread = mailThread;
		return (thread == null || thread.ActionId == 0) ? FetchRequired.FetchThread(key) : FetchRequired.NoFetchRequired;
	}

	public static async Task<string> GetThreadId(ThreadCache cache, ThreadKey key)
	{
		if (key.__isset.otherUserFbId)
		{
			return await cache.GetThreadIdBySingleRecipientId(key.OtherUserFbId.ToString());
		}
		if (key.__isset.threadFbId)
		{
			return await cache.GetThreadIdByThreadFbid(key.ThreadFbId);
		}
		return null;
	}

	public static async Task<MailThread> GetThread(ThreadCache cache, ThreadKey key)
	{
		string threadId = await GetThreadId(cache, key);
		if (threadId == null)
		{
			throw new CacheMissException();
		}
		MailThread thread = await cache.GetThreadByThreadId(threadId);
		if (thread == null)
		{
			throw new CacheMissException();
		}
		return thread;
	}

	public static string PrepareMessageId(string messageId, long actionId)
	{
		if (!string.IsNullOrWhiteSpace(messageId))
		{
			return PrepareMessageId(messageId);
		}
		return "m_action" + actionId;
	}

	public static string PrepareMessageId(string messageId)
	{
		if (string.IsNullOrWhiteSpace(messageId))
		{
			return messageId;
		}
		if (messageId.StartsWith("m_"))
		{
			return messageId;
		}
		if (messageId.StartsWith("m_action"))
		{
			return messageId;
		}
		return "m_" + messageId;
	}

	public static bool AreThreadKeysEqual(ThreadKey threadKey1, ThreadKey threadKey2)
	{
		if (threadKey1 == null || threadKey2 == null)
		{
			return false;
		}
		if (threadKey1.OtherUserFbId == threadKey2.OtherUserFbId)
		{
			return threadKey1.ThreadFbId == threadKey2.ThreadFbId;
		}
		return false;
	}

	public static int GetThreadKeyHashCode(ThreadKey threadKey)
	{
		if (threadKey == null)
		{
			return 0;
		}
		return threadKey.OtherUserFbId.GetHashCode() + threadKey.ThreadFbId.GetHashCode();
	}
}

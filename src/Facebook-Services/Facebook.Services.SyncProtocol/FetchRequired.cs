using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol
{
	public class FetchRequired
	{
		public enum FetchState
		{
			NoFetchRequired,
			FetchMessage,
			FetchThread,
			FetchThreadAndMessage,
			FetchEverything
		}

		public static readonly FetchRequired NoFetchRequired = new FetchRequired(FetchState.NoFetchRequired);

		public static readonly FetchRequired FetchEverything = new FetchRequired(FetchState.FetchEverything);

		public static readonly List<FetchRequired> NoFetchRequiredList = NoFetchRequired.ToList();

		public static readonly List<FetchRequired> FetchEverythingList = FetchEverything.ToList();

		public static readonly Task<List<FetchRequired>> NoFetchRequiredTaskResult = Task.FromResult(NoFetchRequiredList);

		public static readonly Task<List<FetchRequired>> FetchEverythingTaskResult = Task.FromResult(FetchEverythingList);

		public readonly FetchState State;

		public ThreadKey ThreadKey { get; private set; }

		public string MessageId { get; private set; }

		public bool HasRequiredFetch
		{
			get
			{
				return State != FetchState.NoFetchRequired;
			}
		}

		public bool ShouldFetchMessage
		{
			get
			{
				if (State != FetchState.FetchMessage)
				{
					return State == FetchState.FetchThreadAndMessage;
				}
				return true;
			}
		}

		public bool ShouldFetchThread
		{
			get
			{
				if (State != FetchState.FetchThread)
				{
					return State == FetchState.FetchThreadAndMessage;
				}
				return true;
			}
		}

		private FetchRequired(FetchState state)
		{
			State = state;
		}

		public static FetchRequired FetchMessage(string messageId, ThreadKey threadKey)
		{
			FetchRequired fetchRequired = new FetchRequired(FetchState.FetchMessage);
			fetchRequired.MessageId = messageId;
			fetchRequired.ThreadKey = threadKey;
			return fetchRequired;
		}

		public static FetchRequired FetchThread(ThreadKey threadKey)
		{
			FetchRequired fetchRequired = new FetchRequired(FetchState.FetchThread);
			fetchRequired.ThreadKey = threadKey;
			return fetchRequired;
		}

		public static FetchRequired FetchMessageAndThread(string messageId, ThreadKey threadKey)
		{
			FetchRequired fetchRequired = new FetchRequired(FetchState.FetchThreadAndMessage);
			fetchRequired.MessageId = messageId;
			fetchRequired.ThreadKey = threadKey;
			return fetchRequired;
		}

		public override bool Equals(object obj)
		{
			FetchRequired fetchRequired = obj as FetchRequired;
			if (fetchRequired == null)
			{
				return false;
			}
			bool flag = ThreadDeltaUtil.AreThreadKeysEqual(fetchRequired.ThreadKey, ThreadKey);
			bool flag2 = fetchRequired.MessageId == MessageId;
			bool result = fetchRequired.State == State;
			if (flag && flag2)
			{
				return result;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int threadKeyHashCode = ThreadDeltaUtil.GetThreadKeyHashCode(ThreadKey);
			int num = ((MessageId != null) ? MessageId.GetHashCode() : 0);
			int hashCode = State.GetHashCode();
			return threadKeyHashCode + num + hashCode;
		}
	}
}

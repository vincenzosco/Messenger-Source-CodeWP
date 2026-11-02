using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Base.Mqtt;
using Facebook.Base.Strategy;
using Facebook.Models.SyncProtocol;
using Facebook.Models.Utilities;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.SyncProtocol.Handler;

namespace Facebook.Services.SyncProtocol
{
	public class DeltaHandlerManager
	{
		public enum Result
		{
			Success,
			RecreateQueue,
			ResetEverything
		}

		private readonly ThreadCache _cache;

		private readonly Facebook.Services.Api.Service _apiService;

		private readonly IDictionary<DeltaType, DeltaHandler> _handlerLookup;

		private readonly DeltaHandlerContext _deltaHandlerContext;

		public DeltaHandlerManager(ThreadCache cache, Facebook.Services.Api.Service apiService, LoggedInUserUtils loggedInUserUtils)
		{
			_cache = cache;
			_apiService = apiService;
			_handlerLookup = CreateHandlerLookup();
			_deltaHandlerContext = new DeltaHandlerContext(cache, loggedInUserUtils);
		}

		private static IDictionary<DeltaType, DeltaHandler> CreateHandlerLookup()
		{
			return new DeltaHandler[22]
			{
				new DeltaAdminTextMessageHandler(),
				new DeltaForcedFetchHandler(),
				new DeltaMarkReadHandler(),
				new DeltaMarkUnreadHandler(),
				new DeltaMessageDeleteHandler(),
				new DeltaNewGroupThreadHandler(),
				new DeltaNewMessageHandler(),
				new DeltaNoOpHandler(),
				new DeltaParticipantLeftGroupThreadHandler(),
				new DeltaParticipantsAddedToGroupThreadHandler(),
				new DeltaReadReceiptHandler(),
				new DeltaThreadActionHandler(),
				new DeltaThreadDeleteHandler(),
				new DeltaThreadFolderHandler(),
				new DeltaThreadImageHandler(),
				new DeltaThreadMuteSettingsHandler(),
				new DeltaThreadNameHandler(),
				new DeltaVideoCallHandler(),
				new DeltaRtcEventLogHandler(),
				new DeltaBroadcastMessageHandler(),
				new DeltaSentMessageHandler(),
				new DeltaDeliveryReceiptHandler()
			}.ToDictionary((DeltaHandler d) => d.HandlerType);
		}

		public async Task<Result> HandleDeltas(IList<DeltaWrapper> deltas, long firstDeltaSequenceId, string accessToken, CancellationToken cancellationToken, SyncConnectionState syncConnectionState, bool shouldNotify)
		{
			if (deltas.Any((DeltaWrapper d) => d.IsEmptyForcedFetchDelta()))
			{
				Logger.Warning(this, "[SYNC] Received full force fetch delta request", "HandleDeltas", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 86);
				return Result.ResetEverything;
			}
			List<SequenceIdUtil.SequenceAndDeltaInformation> deltasInfo = SequenceIdUtil.CalculateDeltasSequenceIds(deltas, firstDeltaSequenceId).ToList();
			Tuple<Result, Dictionary<SequenceIdUtil.SequenceAndDeltaInformation, List<FetchRequired>>> fetchesForDeltas = await GetFetchesForDeltas(deltasInfo);
			Result result = fetchesForDeltas.Item1;
			Dictionary<SequenceIdUtil.SequenceAndDeltaInformation, List<FetchRequired>> deltaToFetchMap = fetchesForDeltas.Item2;
			if (result != Result.Success)
			{
				Logger.Warning(this, "[SYNC] One of the deltas failed to Ensure", "HandleDeltas", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 98);
				return result;
			}
			Tuple<Result, Dictionary<FetchRequired, RequiredFetchResponse>> fetchResponses = await ExecuteRequiredFetches(deltaToFetchMap, accessToken, cancellationToken);
			result = fetchResponses.Item1;
			Dictionary<FetchRequired, RequiredFetchResponse> fetchToResponseMap = fetchResponses.Item2;
			if (result != Result.Success)
			{
				Logger.Warning(this, "[SYNC] One of the required fetches failed", "HandleDeltas", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 108);
				return result;
			}
			try
			{
				await HandleRequiredDeltas(deltasInfo, deltaToFetchMap, fetchToResponseMap, shouldNotify);
			}
			catch (CacheMissException ex)
			{
				Logger.Warning(this, string.Format("Trying to handle delta where thread was not in cache. {0}", new object[1] { ex }), "HandleDeltas", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 118);
				return Result.ResetEverything;
			}
			return Result.Success;
		}

		private async Task<Tuple<Result, List<FetchRequired>>> EnsureDelta(SequenceIdUtil.SequenceAndDeltaInformation deltaInfo)
		{
			DeltaWrapper delta = deltaInfo.Delta;
			DeltaType deltaType = delta.GetDeltaType();
			SequenceIdState deltaState = await _cache.DetermineSequenceIdStateAndUpdate(deltaInfo.SequenceId, deltaInfo.ExtraSequenceCount);
			Logger.Info(this, string.Format("[SYNC] Delta wrapper of type {0} with sequence ID {1} and state {2}", new object[3] { deltaType, deltaInfo.SequenceId, deltaState }), "EnsureDelta", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 132);
			switch (deltaState)
			{
			case SequenceIdState.AlreadySeen:
				return Tuple.Create(Result.Success, FetchRequired.NoFetchRequiredList);
			case SequenceIdState.Unexpected:
				Logger.Critical(this, "[SYNC] Delta received out of order.", "EnsureDelta", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 141);
				return Tuple.Create(Result.RecreateQueue, FetchRequired.NoFetchRequiredList);
			default:
				throw new NotImplementedException("[SYNC] New unhandled type of state");
			case SequenceIdState.Contiguous:
			{
				DeltaHandler handler = _handlerLookup[deltaType];
				List<FetchRequired> fetchRequiredList = await handler.Ensure(_deltaHandlerContext, delta);
				Result result = (fetchRequiredList.Any((FetchRequired f) => f.State == FetchRequired.FetchState.FetchEverything) ? Result.ResetEverything : Result.Success);
				return Tuple.Create(result, fetchRequiredList);
			}
			}
		}

		private async Task HandleDelta(SequenceIdUtil.SequenceAndDeltaInformation deltaInfo, bool shouldNotify)
		{
			DeltaWrapper delta = deltaInfo.Delta;
			DeltaType deltaType = delta.GetDeltaType();
			DeltaHandler handler = _handlerLookup[deltaType];
			Logger.Info(string.Format("[SYNC] Handling Delta - Delta Type: {0}", new object[1] { deltaType }), "HandleDelta", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 164);
			await handler.Handle(_deltaHandlerContext, delta, shouldNotify);
			Logger.Info(string.Format("[SYNC] Handled Delta - Delta Type: {0}", new object[1] { deltaType }), "HandleDelta", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 166);
		}

		private async Task<Tuple<Result, Dictionary<SequenceIdUtil.SequenceAndDeltaInformation, List<FetchRequired>>>> GetFetchesForDeltas(IEnumerable<SequenceIdUtil.SequenceAndDeltaInformation> deltaInfos)
		{
			Dictionary<SequenceIdUtil.SequenceAndDeltaInformation, List<FetchRequired>> deltaToFetchMap = new Dictionary<SequenceIdUtil.SequenceAndDeltaInformation, List<FetchRequired>>();
			Result result = Result.Success;
			foreach (SequenceIdUtil.SequenceAndDeltaInformation deltaInfo in deltaInfos)
			{
				Tuple<Result, List<FetchRequired>> response = await EnsureDelta(deltaInfo);
				result = response.Item1;
				List<FetchRequired> fetchRequired = response.Item2;
				if (result == Result.Success)
				{
					deltaToFetchMap.Add(deltaInfo, fetchRequired);
					continue;
				}
				break;
			}
			return Tuple.Create(result, deltaToFetchMap);
		}

		private async Task<Tuple<Result, Dictionary<FetchRequired, RequiredFetchResponse>>> ExecuteRequiredFetches(Dictionary<SequenceIdUtil.SequenceAndDeltaInformation, List<FetchRequired>> deltaToFetchMap, string accessToken, CancellationToken cancellationToken)
		{
			List<FetchRequired> fetches = deltaToFetchMap.Values.SelectMany((List<FetchRequired> fetchRequiredList) => fetchRequiredList.Where((FetchRequired fetchRequired) => fetchRequired.HasRequiredFetch)).Distinct().ToList();
			Tuple<ExecutionResult, Dictionary<FetchRequired, RequiredFetchResponse>> response = await RequiredFetchesHandler.HandleRequiredFetches(fetches, _apiService, accessToken, cancellationToken, _cache);
			ExecutionResult executionResult = response.Item1;
			Dictionary<FetchRequired, RequiredFetchResponse> fetchToResponseMap = response.Item2;
			Result result = ((executionResult != ExecutionResult.Success) ? Result.ResetEverything : Result.Success);
			return Tuple.Create(result, fetchToResponseMap);
		}

		private async Task HandleRequiredDeltas(IEnumerable<SequenceIdUtil.SequenceAndDeltaInformation> deltaInfos, IReadOnlyDictionary<SequenceIdUtil.SequenceAndDeltaInformation, List<FetchRequired>> deltaToFetchMap, IReadOnlyDictionary<FetchRequired, RequiredFetchResponse> fetchToResponseMap, bool shouldNotify)
		{
			List<SequenceIdUtil.SequenceAndDeltaInformation> deltaInfoList = deltaInfos.ToList();
			Dictionary<ThreadKey, long> deletedThreadKeys = GetDeletedThreadKeys(deltaInfoList);
			foreach (SequenceIdUtil.SequenceAndDeltaInformation deltaInfo in deltaInfoList)
			{
				List<FetchRequired> fetchRequiredList = deltaToFetchMap[deltaInfo];
				List<FetchRequired> fetchesNotSkipped = new List<FetchRequired>();
				foreach (FetchRequired item in fetchRequiredList)
				{
					bool flag = IsThreadDeletedInFuture(item, deletedThreadKeys, deltaInfo);
					if ((!item.HasRequiredFetch || !fetchToResponseMap[item].SkipHandle) && !flag)
					{
						fetchesNotSkipped.Add(item);
					}
				}
				if (!fetchesNotSkipped.Any())
				{
					Logger.Info(string.Format("[SYNC] Skipping Handle for delta of type {0}", new object[1] { deltaInfo.Delta.GetDeltaType() }), "HandleRequiredDeltas", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 237);
				}
				else
				{
					Logger.Info("[SYNC] Attempting to Handle Delta", "HandleRequiredDeltas", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\DeltaHandlerManager.cs", 241);
					await HandleDelta(deltaInfo, shouldNotify);
				}
			}
		}

		private static bool IsThreadDeletedInFuture(FetchRequired fetchRequired, Dictionary<ThreadKey, long> deletedThreadKeys, SequenceIdUtil.SequenceAndDeltaInformation deltaInfo)
		{
			ThreadKey threadKey = fetchRequired.ThreadKey;
			if (threadKey == null)
			{
				return false;
			}
			bool result = deletedThreadKeys.Keys.Any((ThreadKey t) => t.OtherUserFbId == threadKey.OtherUserFbId && t.ThreadFbId == threadKey.ThreadFbId);
			if (deletedThreadKeys.ContainsKey(threadKey) && deletedThreadKeys[threadKey] > deltaInfo.SequenceId)
			{
				return result;
			}
			return false;
		}

		private Dictionary<ThreadKey, long> GetDeletedThreadKeys(List<SequenceIdUtil.SequenceAndDeltaInformation> deltaInfos)
		{
			Dictionary<ThreadKey, long> dictionary = new Dictionary<ThreadKey, long>();
			foreach (SequenceIdUtil.SequenceAndDeltaInformation item in deltaInfos.Where((SequenceIdUtil.SequenceAndDeltaInformation d) => d.Delta.GetDeltaType() == DeltaType.THREAD_DELETE))
			{
				foreach (ThreadKey threadKey in item.Delta.DeltaThreadDelete.ThreadKeys)
				{
					long num = (dictionary.ContainsKey(threadKey) ? dictionary[threadKey] : 0);
					if (item.SequenceId > num)
					{
						dictionary[threadKey] = item.SequenceId;
					}
				}
			}
			return dictionary;
		}
	}
}

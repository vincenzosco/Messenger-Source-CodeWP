using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Base.Strategy;
using Facebook.Models;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.FetchThreadsAndMessages;

namespace Facebook.Services.SyncProtocol;

public class RequiredFetchesHandler
{
	private class FetchParameters
	{
		public readonly IEnumerable<FetchRequired> FetchesRequired;

		public readonly Facebook.Services.Api.Service ApiService;

		public readonly string AccessToken;

		public readonly ThreadCache ThreadCache;

		public FetchParameters(IEnumerable<FetchRequired> fetchesRequired, Facebook.Services.Api.Service apiService, string accessToken, ThreadCache threadCache)
		{
			FetchesRequired = fetchesRequired;
			ApiService = apiService;
			AccessToken = accessToken;
			ThreadCache = threadCache;
		}
	}

	private const int MaxAttempts = 4;

	public static async Task<Tuple<ExecutionResult, Dictionary<FetchRequired, RequiredFetchResponse>>> HandleRequiredFetches(List<FetchRequired> fetchesRequired, Facebook.Services.Api.Service apiService, string accessToken, CancellationToken cancellationToken, ThreadCache threadCache)
	{
		Dictionary<FetchRequired, RequiredFetchResponse> fetchToResponseMap = null;
		ExecutionResult result = ExecutionResult.Success;
		if (fetchesRequired.Any())
		{
			FetchParameters parameters = new FetchParameters(fetchesRequired, apiService, accessToken, threadCache);
			ExponentialFallback<FetchParameters, Dictionary<FetchRequired, RequiredFetchResponse>> strategy = new ExponentialFallback<FetchParameters, Dictionary<FetchRequired, RequiredFetchResponse>>(HandleRequiredFetches);
			StrategyResponse<Dictionary<FetchRequired, RequiredFetchResponse>> response = await strategy.Execute(cancellationToken, parameters);
			result = response.ExecutionResult;
			fetchToResponseMap = response.ReturnValue;
		}
		return Tuple.Create(result, fetchToResponseMap);
	}

	private static async Task<StrategyResponse<Dictionary<FetchRequired, RequiredFetchResponse>>> HandleRequiredFetches(int attempt, CancellationToken token, FetchParameters paramters)
	{
		Dictionary<FetchRequired, RequiredFetchResponse> fetchToResponseMap = null;
		ExecutionResult result;
		if (attempt > 4)
		{
			result = ExecutionResult.Cancelled;
		}
		else
		{
			Tuple<ExecutionResult, Dictionary<FetchRequired, RequiredFetchResponse>> response = await FetchThreadsAndMessages(paramters, token);
			result = response.Item1;
			fetchToResponseMap = response.Item2;
		}
		return StrategyResponse.Create(result, fetchToResponseMap);
	}

	private static async Task<Tuple<ExecutionResult, Dictionary<FetchRequired, RequiredFetchResponse>>> FetchThreadsAndMessages(FetchParameters parameters, CancellationToken cancellationToken)
	{
		List<FetchRequired> requiredFetches = parameters.FetchesRequired.ToList();
		Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request request = GetFetchThreadsAndMessageRequest(parameters, cancellationToken, requiredFetches);
		Facebook.Services.Api.Methods.FetchThreadsAndMessages.Response response = await parameters.ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request, Facebook.Services.Api.Methods.FetchThreadsAndMessages.Response>(request);
		if (response.HasError())
		{
			LogError(request, response, response.Threads, response.Messages);
			return Tuple.Create<ExecutionResult, Dictionary<FetchRequired, RequiredFetchResponse>>(ExecutionResult.Failed, null);
		}
		return Tuple.Create(ExecutionResult.Success, await MapFetchesToResponses(requiredFetches, response.Threads, response.Messages, parameters.ThreadCache));
	}

	private static Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request GetFetchThreadsAndMessageRequest(FetchParameters parameters, CancellationToken cancellationToken, List<FetchRequired> requiredFetches)
	{
		IEnumerable<long> threadFbIds = from f in requiredFetches
			where f.ShouldFetchThread && f.ThreadKey.__isset.threadFbId
			select f.ThreadKey.ThreadFbId;
		IEnumerable<long> threadRecipientIds = from f in requiredFetches
			where f.ShouldFetchThread && f.ThreadKey.__isset.otherUserFbId
			select f.ThreadKey.OtherUserFbId;
		IEnumerable<string> messageIds = from f in requiredFetches
			where f.ShouldFetchMessage
			select f.MessageId;
		Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request request = new Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request(threadFbIds, threadRecipientIds, messageIds);
		request.AccessToken = parameters.AccessToken;
		request.CancelToken = cancellationToken;
		return request;
	}

	private static async Task<Dictionary<FetchRequired, RequiredFetchResponse>> MapFetchesToResponses(IEnumerable<FetchRequired> fetchesRequired, List<MailThread> threads, List<MailMessage> messages, ThreadCache cache)
	{
		Dictionary<FetchRequired, RequiredFetchResponse> fetchToResponseMap = new Dictionary<FetchRequired, RequiredFetchResponse>();
		foreach (FetchRequired fetchRequired in fetchesRequired)
		{
			ExecutionResult messageExecutionResult = ExecutionResult.Success;
			ExecutionResult threadExecutionResult = ExecutionResult.Success;
			bool skipHandle = false;
			if (fetchRequired.ShouldFetchMessage)
			{
				MailMessage message = messages.FirstOrDefault((MailMessage m) => m.Id == fetchRequired.MessageId);
				message.Source = MailMessage.MessageSource.Sync;
				Tuple<ExecutionResult, bool> response = await HandleFetchedMessage(cache, message, fetchRequired.ThreadKey);
				messageExecutionResult = response.Item1;
				skipHandle |= response.Item2;
				Logger.Info($"Fetched Message with MessageId {fetchRequired.MessageId}, ThreadId {fetchRequired.ThreadKey}, MessageBody {message.Body}, Attachment Count {message.Attachments.Count}, Message Class {MessageClassifier.Classify(message)}", "MapFetchesToResponses", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\RequiredFetchesHandler.cs", 144);
			}
			if (fetchRequired.ShouldFetchThread)
			{
				MailThread thread = (fetchRequired.ThreadKey.__isset.otherUserFbId ? threads.FirstOrDefault((MailThread t) => t.SingleRecipient == fetchRequired.ThreadKey.OtherUserFbId.ToString()) : threads.FirstOrDefault((MailThread t) => t.Fbid.Value == fetchRequired.ThreadKey.ThreadFbId));
				Tuple<ExecutionResult, bool> response2 = await HandleFetchedThread(cache, thread, fetchRequired.ThreadKey);
				threadExecutionResult = response2.Item1;
				skipHandle |= response2.Item2;
			}
			fetchToResponseMap.Add(fetchRequired, new RequiredFetchResponse(messageExecutionResult, threadExecutionResult, skipHandle));
		}
		return fetchToResponseMap;
	}

	private static async Task<Tuple<ExecutionResult, bool>> HandleFetchedThread(ThreadCache cache, MailThread thread, ThreadKey threadKey)
	{
		if (thread == null)
		{
			if (!threadKey.__isset.otherUserFbId)
			{
				await cache.RemoveThreadAndPinnedThreadByThreadFbId(threadKey.ThreadFbId);
			}
			else
			{
				await cache.RemoveThreadByRecipientId(threadKey.OtherUserFbId.ToString());
			}
			return Tuple.Create(ExecutionResult.Success, item2: true);
		}
		await cache.AddThread(thread);
		return Tuple.Create(ExecutionResult.Success, item2: false);
	}

	private static async Task<Tuple<ExecutionResult, bool>> HandleFetchedMessage(ThreadCache cache, MailMessage message, ThreadKey threadKey)
	{
		await cache.AddMessageAndUpdateThread(message, shouldNotify: false);
		return Tuple.Create(ExecutionResult.Success, item2: false);
	}

	private static void LogError(Facebook.Services.Api.Methods.FetchThreadsAndMessages.Request request, Facebook.Base.Response response, object threadData, object messageData)
	{
		Logger.Warning(string.Format("Could not fetch data. Fetch Message Ids: {0}, Fetch Thread Recipient Ids: {1}, Fetch Thread Fb Ids: {2}, Response Type: ({3}), Error: ({4}), Is thread data null: ({5}), Is message data null: ({6})", string.Join(", ", request.MessageIds), string.Join(", ", request.ThreadRecipientIds), string.Join(", ", request.ThreadFbIds), response.GetType(), response, threadData == null, messageData == null), "LogError", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\RequiredFetchesHandler.cs", 190);
	}
}

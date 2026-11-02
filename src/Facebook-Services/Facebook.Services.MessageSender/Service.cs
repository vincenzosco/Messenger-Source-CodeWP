using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Base.Strategy;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchMessage;
using Facebook.Services.Api.Methods.MarkMessageFailedById;
using Facebook.Services.Api.Methods.SaveOutgoingMessage;
using Facebook.Services.Api.Methods.SendMessage;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Services.MessageSender
{
	public class Service : BackgroundService
	{
		private const int MaxSendRetryAttempts = 5;

		public const string ContextAttachmentQueueTag = "-attachments";

		private readonly object _lock = new object();

		private readonly Dictionary<string, SendContext> _idToContext;

		private readonly IFileAccess _access;

		private readonly IPendingRequestStorage<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response> _pendingMessageStorage;

		private FacebookContext _context;

		private readonly ExponentialFallback<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response> _sendMessageWithBackoff;

		private readonly AutoRenewCancellationTokenSource _cancellationTokenSource = new AutoRenewCancellationTokenSource();

		private Facebook.Services.Api.Service _apiService;

		private Facebook.Services.Api.Service ApiService
		{
			get
			{
				return _apiService ?? (_apiService = ServiceLocator.Current.GetInstance<Facebook.Services.Api.Service>());
			}
		}

		public Service(IDispatcher dispatcher, IFileAccess access, IPendingRequestStorage<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response> pendingMessageStorage)
			: base(dispatcher)
		{
			_access = access;
			_pendingMessageStorage = pendingMessageStorage;
			_idToContext = new Dictionary<string, SendContext>();
			_sendMessageWithBackoff = new ExponentialFallback<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(SendMessageWithBackoff, GetCancelledResponse);
			Register<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(ProcessMessage);
		}

		private Facebook.Services.Api.Methods.SendMessage.Response GetCancelledResponse()
		{
			Error error = Error.FromException(new TaskCanceledException("Sending a message was cancelled by the user"));
			Facebook.Services.Api.Methods.SendMessage.Response response = new Facebook.Services.Api.Methods.SendMessage.Response();
			response.Error = error;
			return response;
		}

		public bool HasQueuedRequests()
		{
			lock (_lock)
			{
				return _idToContext.Any((KeyValuePair<string, SendContext> entry) => entry.Value.HasOutstandingRequests());
			}
		}

		public int GetNumQueuedRequests()
		{
			lock (_lock)
			{
				return _idToContext.Sum((KeyValuePair<string, SendContext> entry) => entry.Value.GetNumOutstandingRequests());
			}
		}

		public override void StartService(FacebookContext context)
		{
			_context = context;
			if (_context.ApplicationState.CurrentRuntype == ApplicationState.Runtype.Foreground)
			{
				_pendingMessageStorage.MarkSuccessfulResponses();
			}
		}

		public override void StopService()
		{
			_cancellationTokenSource.CancelAndRenew();
			if (_context.ApplicationState.CurrentRuntype == ApplicationState.Runtype.Foreground)
			{
				_pendingMessageStorage.SavePendingRequests(GetAllOutstandingRequests());
			}
		}

		private void RemoveContext(string id)
		{
			lock (_lock)
			{
				_idToContext.Remove(id);
			}
		}

		public SendContext GetContext(string id, bool isLocalId = false)
		{
			SendContext value = null;
			lock (_lock)
			{
				_idToContext.TryGetValue(id, out value);
				if (value == null)
				{
					value = new SendContext(id, isLocalId);
					_idToContext.Add(id, value);
				}
			}
			return value;
		}

		public List<Facebook.Services.Api.Methods.SendMessage.Request> GetOutstandingRequestsByContextId(string id)
		{
			SendContext context = GetContext(id);
			return context.GetOutstandingRequests();
		}

		private List<Facebook.Services.Api.Methods.SendMessage.Request> GetAllOutstandingRequests()
		{
			List<Facebook.Services.Api.Methods.SendMessage.Request> list = new List<Facebook.Services.Api.Methods.SendMessage.Request>();
			IEnumerable<KeyValuePair<string, SendContext>> enumerable = _idToContext.Where((KeyValuePair<string, SendContext> context) => context.Value.HasOutstandingRequests());
			foreach (KeyValuePair<string, SendContext> item in enumerable)
			{
				list.AddRange(GetOutstandingRequestsByContextId(item.Value.Id));
			}
			return list;
		}

		private void UpdateContextId(Facebook.Services.Api.Methods.SendMessage.Request request, SendContext context, string id)
		{
			string id2 = context.Id;
			context.Id = id;
			context.IdIsLocal = false;
			context.OverrideServerThreadId(id);
			lock (_lock)
			{
				if (request.Users.Count != 1)
				{
					_idToContext.Remove(id2);
				}
				_idToContext.Remove(id);
				_idToContext.Add(id, context);
			}
		}

		public string GetContextIdForRequest(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			string text = null;
			if (request.HasServerThreadId())
			{
				text = request.ServerThreadId;
			}
			else
			{
				text = ((request.Users.Count != 1) ? Guid.NewGuid().ToString() : request.Users[0].Id);
			}
			if (request.HasFiles())
			{
				text += "-attachments";
			}
			return text;
		}

		private SendContext GetContextForRequest(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			SendContext context = GetContext(GetContextIdForRequest(request));
			context.IdIsLocal = !request.HasServerThreadId();
			return context;
		}

		private async Task<StrategyResponse<Facebook.Services.Api.Methods.SendMessage.Response>> SendMessageWithBackoff(int attempt, CancellationToken token, Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			if (attempt > 5)
			{
				Error error = Error.FromException(new TaskCanceledException("Max message send retry attempts"));
				return new StrategyResponse<Facebook.Services.Api.Methods.SendMessage.Response>(ExecutionResult.Cancelled, new Facebook.Services.Api.Methods.SendMessage.Response
				{
					Error = error
				});
			}
			request.RetryAttempt = attempt;
			Facebook.Services.Api.Methods.SendMessage.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(request);
			ExecutionResult state = ((response != null && !response.HasError()) ? ExecutionResult.Success : ExecutionResult.Failed);
			return StrategyResponse.Create(state, response);
		}

		private async Task<Facebook.Services.Api.Methods.SendMessage.Response> ProcessMessage(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			SendContext context = GetContextForRequest(request);
			ExecutionResult state = ExecutionResult.Failed;
			Facebook.Services.Api.Methods.SendMessage.Response response = null;
			try
			{
				context.EnqueueRequest(request);
				await SaveToDatabaseAndUpdateRequest(request, context);
				CancellationToken exponentialBackoffToken = _cancellationTokenSource.Token;
				await context.WaitToSendAsync();
				CancellationTokenSource linkedToken = CancellationTokenSource.CreateLinkedTokenSource(request.CancelToken, exponentialBackoffToken);
				StrategyResponse<Facebook.Services.Api.Methods.SendMessage.Response> result = await _sendMessageWithBackoff.Execute(linkedToken.Token, request, 1000, 5000);
				response = result.ReturnValue;
				state = result.ExecutionResult;
				Facebook.Services.Api.Methods.SendMessage.Response successfulResponseFromBackground = _pendingMessageStorage.GetSuccessfulResponse(request);
				if (response.HasError() && successfulResponseFromBackground != null)
				{
					response = successfulResponseFromBackground;
				}
				if (context.IdIsLocal && request.Users.Count != 1 && response.HasError())
				{
					RemoveContext(context.Id);
				}
				else if (context.IdIsLocal && !response.HasError())
				{
					if (string.IsNullOrWhiteSpace(response.ThreadId))
					{
						RemoveContext(context.Id);
					}
					else
					{
						UpdateContextId(request, context, response.ThreadId);
					}
				}
			}
			finally
			{
				context.ReleaseSend(request);
			}
			bool succeeded = state == ExecutionResult.Success;
			bool requestWasCancelledByUser = request.CancelToken.IsCancellationRequested;
			bool shouldDeleteFiles = succeeded || requestWasCancelledByUser;
			if (request.HasFiles() && shouldDeleteFiles)
			{
				foreach (Facebook.Services.Api.Methods.SendMessage.Attachment attachment in request.FilePaths)
				{
					IDispatcher uiDispatcher = UiDispatcher;
					Action action = async () =>
					{
						await Task.Delay(5000);
						_access.DeleteFileAtPath(attachment.ThumbnailPath).DoNotWait();
						_access.DeleteFileAtPath(attachment.Path).DoNotWait();
					};
					uiDispatcher.Schedule(action);
				}
			}
			if (!succeeded)
			{
				await MarkRequestAsFailed(request);
			}
			else
			{
				await SendMessageSentSignal(response.MessageId, response.ThreadId, request.OfflineThreadingId);
			}
			return response;
		}

		private async Task MarkRequestAsFailed(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			Facebook.Services.Api.Methods.MarkMessageFailedById.Request markMessageFailedRequest = new Facebook.Services.Api.Methods.MarkMessageFailedById.Request
			{
				ThreadId = (request.HasServerThreadId() ? request.ServerThreadId : request.LocalThreadId),
				OfflineThreadingId = request.OfflineThreadingId
			};
			await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MarkMessageFailedById.Request, Facebook.Services.Api.Methods.MarkMessageFailedById.Response>(markMessageFailedRequest);
		}

		private async Task SendMessageSentSignal(string messageId, string threadId, string offlineThreadingId)
		{
			Facebook.Services.Api.Methods.FetchMessage.Request request = new Facebook.Services.Api.Methods.FetchMessage.Request
			{
				AccessToken = _context.Session.Credentials.AccessToken,
				MessageId = messageId,
				ThreadId = threadId,
				OfflineThreadingId = offlineThreadingId
			};
			Facebook.Services.Api.Methods.FetchMessage.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.FetchMessage.Request, Facebook.Services.Api.Methods.FetchMessage.Response>(request);
			if (response.Message != null)
			{
				MessageSentSignal payload = new MessageSentSignal(response.Message);
				Signal("inbox", payload).DoNotWait();
			}
		}

		private async Task SaveToDatabaseAndUpdateRequest(Facebook.Services.Api.Methods.SendMessage.Request request, SendContext context)
		{
			try
			{
				await context.WaitToSaveAsync();
				Facebook.Services.Api.Methods.SaveOutgoingMessage.Response response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SaveOutgoingMessage.Response>(request);
				if (response.HasError())
				{
					Logger.Error(this, "Couldn't save queued message to database", "SaveToDatabaseAndUpdateRequest", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\MessageSender\\Service.cs", 325);
					Logger.Error(response.Error.Message, "SaveToDatabaseAndUpdateRequest", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\MessageSender\\Service.cs", 326);
					if (response.Error.Exception != null)
					{
						Logger.Error(response.Error.Exception.ToString(), "SaveToDatabaseAndUpdateRequest", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\MessageSender\\Service.cs", 330);
					}
				}
				else if (response.DidGenerateLocalThreadId())
				{
					context.OverrideLocalThreadId(response.GeneratedLocalThreadId);
				}
			}
			finally
			{
				context.ReleaseSave();
			}
		}
	}
}

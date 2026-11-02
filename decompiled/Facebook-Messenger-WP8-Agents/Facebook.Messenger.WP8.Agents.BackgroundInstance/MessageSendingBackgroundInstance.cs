using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Messenger.Utilities;
using Facebook.Messenger.WP8.Agents.Helpers;
using Facebook.Models;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.MessageSender;

namespace Facebook.Messenger.WP8.Agents.BackgroundInstance
{
	public class MessageSendingBackgroundInstance : IBackgroundInstance
	{
		public const int MqttConnectDelayMs = 5000;

		private readonly FacebookContext _context;

		private readonly Facebook.Services.MessageSender.Service _sendingService;

		private readonly object _pendingRequestsCounterLock = new object();

		private readonly object _cancelLock = new object();

		private readonly ServiceEngine _serviceEngine;

		private readonly List<Facebook.Services.Api.Methods.SendMessage.Response> _successfulMessages = new List<Facebook.Services.Api.Methods.SendMessage.Response>();

		private readonly SingleExecutionRuntimeLock _runtimeLock = new SingleExecutionRuntimeLock();

		private readonly AutoRenewCancellationTokenSource _cancellationTokenSource = new AutoRenewCancellationTokenSource();

		private int _numberOfPendingRequests;

		public bool IsInstanceRunning { get; private set; }

		public EventHandler ExecutionComplete { get; set; }

		public MessageSendingBackgroundInstance(FacebookContext context, ServiceEngine serviceEngine, Facebook.Services.MessageSender.Service sendingService)
		{
			_context = context;
			_serviceEngine = serviceEngine;
			_sendingService = sendingService;
		}

		public async Task Execute()
		{
			CancellationToken token = _cancellationTokenSource.Token;
			try
			{
				int num = default(int);
				int num2 = num;
				int num3 = 0;
				try
				{
					if (_context.IsAuthenticated() && !token.IsCancellationRequested)
					{
						_runtimeLock.Acquire();
						IsInstanceRunning = true;
						List<Facebook.Services.Api.Methods.SendMessage.Request> outgoingMessages = GetOutgoingMessages();
						BackgroundAnalyticsUtil.BackgroundSendingStarted();
						_serviceEngine.StartServices(_context);
						await MqttConnectionWaiter.WaitAsync(5000);
						SendOutgoingMessages(outgoingMessages).Wait(token);
					}
				}
				catch (OperationCanceledException arg)
				{
					Logger.Warning(string.Format("An operation was cancelled when executing the background sender: {0}", arg), "Execute", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Agents\\BackgroundInstance\\MessageSendingBackgroundInstance.cs", 67);
				}
			}
			finally
			{
				lock (_pendingRequestsCounterLock)
				{
					if (_numberOfPendingRequests > 0)
					{
						ToastHelper.ShowPendingSendsToast();
					}
				}
				if (ExecutionComplete != null)
				{
					ExecutionComplete(this, null);
				}
				CleanUp();
			}
		}

		private List<Facebook.Services.Api.Methods.SendMessage.Request> GetOutgoingMessages()
		{
			List<Facebook.Services.Api.Methods.SendMessage.Request> outstandingRequestsFromIsolatedStorage = BackgroundRequestUtil.GetOutstandingRequestsFromIsolatedStorage();
			_numberOfPendingRequests = outstandingRequestsFromIsolatedStorage.Count;
			BackgroundRequestUtil.DeleteOutstandingRequestsFromIsolatedStorage();
			return outstandingRequestsFromIsolatedStorage;
		}

		public void Cancel()
		{
			CleanUp();
		}

		private void CleanUp()
		{
			lock (_cancelLock)
			{
				if (IsInstanceRunning)
				{
					_cancellationTokenSource.CancelAndRenew();
					_serviceEngine.StopServices();
					_runtimeLock.TryRelease();
					IsInstanceRunning = false;
				}
			}
		}

		private Task SendOutgoingMessages(List<Facebook.Services.Api.Methods.SendMessage.Request> outgoingMessages)
		{
			if (outgoingMessages == null)
			{
				return null;
			}
			List<Task> tasks = outgoingMessages.Select(CreateMessageTask).ToList();
			return Task.WhenAll(tasks);
		}

		private async Task CreateMessageTask(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			if (!request.HasVideo())
			{
				request.RetryAttempt = 0;
				AddSuccessfulRequestAction(await _sendingService.ExecuteAsync<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>(request));
			}
		}

		private void AddSuccessfulRequestAction(Facebook.Services.Api.Methods.SendMessage.Response response)
		{
			if (!response.HasError())
			{
				lock (_pendingRequestsCounterLock)
				{
					_numberOfPendingRequests--;
				}
				_successfulMessages.Add(response);
				BackgroundRequestUtil.SaveSuccessfulResponsesInIsolatedStorage(_successfulMessages);
			}
		}
	}
}

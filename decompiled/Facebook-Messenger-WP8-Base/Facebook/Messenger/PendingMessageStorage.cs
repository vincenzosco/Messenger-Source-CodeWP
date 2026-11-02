using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Messenger.Utilities;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.MarkMessagesSent;
using Facebook.Services.Api.Methods.MarkQueuedMessagesFailed;
using Facebook.Services.Api.Methods.SendMessage;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Messenger
{
	public class PendingMessageStorage : IPendingRequestStorage<Facebook.Services.Api.Methods.SendMessage.Request, Facebook.Services.Api.Methods.SendMessage.Response>
	{
		private readonly List<Facebook.Services.Api.Methods.SendMessage.Response> _successfulBackgroundMessages = new List<Facebook.Services.Api.Methods.SendMessage.Response>();

		private Facebook.Services.Api.Service _apiService;

		private Facebook.Services.Api.Service ApiService => _apiService ?? (_apiService = ServiceLocator.Current.GetInstance<Facebook.Services.Api.Service>());

		public async Task MarkSuccessfulResponses()
		{
			List<Facebook.Services.Api.Methods.SendMessage.Response> successfulMessages = BackgroundRequestUtil.GetSuccessfulResponsesFromIsolatedStorage();
			BackgroundRequestUtil.DeleteSuccessfulResponsesFromIsolatedStorage();
			if (successfulMessages != null && successfulMessages.Any())
			{
				await MarkSuccessfulMessagesAsSent(successfulMessages);
			}
			MarkQueuedMessagesAsFailed().DoNotWait();
		}

		public void SavePendingRequests(List<Facebook.Services.Api.Methods.SendMessage.Request> requests)
		{
			BackgroundRequestUtil.SaveOutstandingRequestsInIsolatedStorage(requests);
		}

		public Facebook.Services.Api.Methods.SendMessage.Response GetSuccessfulResponse(Facebook.Services.Api.Methods.SendMessage.Request request)
		{
			return _successfulBackgroundMessages.FirstOrDefault((Facebook.Services.Api.Methods.SendMessage.Response r) => r.OfflineId == request.OfflineThreadingId);
		}

		public async Task MarkQueuedMessagesAsFailed()
		{
			Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.Request request = new Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.Request();
			await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.Request, Facebook.Services.Api.Methods.MarkQueuedMessagesFailed.Response>(request);
		}

		public async Task MarkSuccessfulMessagesAsSent(List<Facebook.Services.Api.Methods.SendMessage.Response> sentRequests)
		{
			_successfulBackgroundMessages.Clear();
			_successfulBackgroundMessages.AddRange(sentRequests);
			Facebook.Services.Api.Methods.MarkMessagesSent.Request request = new Facebook.Services.Api.Methods.MarkMessagesSent.Request
			{
				SentMessages = sentRequests
			};
			await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MarkMessagesSent.Request, Facebook.Services.Api.Methods.MarkMessagesSent.Response>(request);
		}
	}
}

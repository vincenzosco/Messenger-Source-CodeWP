using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Models;
using Facebook.Services.Api.Methods.MediaItemUpload;
using Facebook.Services.Api.Methods.SendMessage;
using Facebook.Services.Common;

namespace Facebook.Services.Analytics.Logging
{
	public class SendEventLogger
	{
		public enum SendMethod
		{
			MQTT,
			Graph
		}

		private const string SendEventsModule = "send_events";

		public static void LogQueueEvent(Facebook.Services.Api.Methods.SendMessage.Request request, Service service)
		{
			AddEventRequest addEventRequest = new AddEventRequest("message_send_queued", "send_events");
			AddMessageIdentifiers(request, addEventRequest);
			AddAttachmentInformation(request, addEventRequest);
			service.ExecuteAsync<AddEventRequest, BoolResponse>(addEventRequest).DoNotWait();
		}

		public static void LogSendEvent(Facebook.Services.Api.Methods.SendMessage.Request request, Service service)
		{
			AddEventRequest addEventRequest = new AddEventRequest("message_send_attempt", "send_events");
			AddMessageIdentifiers(request, addEventRequest);
			AddAttachmentInformation(request, addEventRequest);
			service.ExecuteAsync<AddEventRequest, BoolResponse>(addEventRequest).DoNotWait();
		}

		public static void LogMediaUploadEvent(Facebook.Services.Api.Methods.SendMessage.Request sendRequest, Facebook.Services.Api.Methods.MediaItemUpload.Request uploadRequest, Service service, Facebook.Services.Api.Methods.MediaItemUpload.Response response, long uploadDuration, int attachmentIndex, ExtraFailureReason extraFailureReason)
		{
			AddEventRequest addEventRequest = new AddEventRequest("message_media_upload", "send_events").AddData("media_type", uploadRequest.Type.ToString()).AddData("media_source", uploadRequest.MediaAttachment.Source.ToString()).AddData("media_size_bytes", uploadRequest.MediaAttachment.AttachmentSizeBytes.ToString())
				.AddData("upload_duration_ms", uploadDuration.ToString())
				.AddData("attachment_number", attachmentIndex.ToString())
				.AddData("send_state", response.HasError() ? "failed" : "succeeded")
				.AddData("extra_failure_reason", extraFailureReason.ToString());
			if (response.HasError())
			{
				AddErrorData(response.Error, addEventRequest);
			}
			AddMessageIdentifiers(sendRequest, addEventRequest);
			service.ExecuteAsync<AddEventRequest, BoolResponse>(addEventRequest).DoNotWait();
		}

		public static void LogSendState(Facebook.Services.Api.Methods.SendMessage.Request request, Service service, SendMethod method, Facebook.Base.Response response, ExtraFailureReason extraFailureReason)
		{
			AddEventRequest addEventRequest = new AddEventRequest("message_send_state", "send_events").AddData("method", method.ToString()).AddData("attempt", request.RetryAttempt.ToString()).AddData("send_state", response.HasError() ? "failed" : "succeeded")
				.AddData("extra_failure_reason", extraFailureReason.ToString());
			if (response.HasError())
			{
				AddErrorData(response.Error, addEventRequest);
			}
			AddMessageIdentifiers(request, addEventRequest);
			service.ExecuteAsync<AddEventRequest, BoolResponse>(addEventRequest).DoNotWait();
		}

		private static void AddMessageIdentifiers(Facebook.Services.Api.Methods.SendMessage.Request request, AddEventRequest e)
		{
			e.AddData("offline_threading_id", request.OfflineThreadingId);
			e.AddData("server_thread_id", request.ServerThreadId);
		}

		private static void AddAttachmentInformation(Facebook.Services.Api.Methods.SendMessage.Request request, AddEventRequest e)
		{
			long num = 0L;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			MediaSource mediaSource = MediaSource.Unknown;
			if (request.FilePaths != null)
			{
				foreach (Facebook.Services.Api.Methods.SendMessage.Attachment filePath in request.FilePaths)
				{
					if (filePath.IsPhoto)
					{
						num2++;
					}
					if (filePath.IsAudio)
					{
						num3++;
					}
					if (filePath.IsVideo)
					{
						num4++;
					}
					mediaSource = filePath.Source;
					num += filePath.AttachmentSizeBytes;
				}
			}
			e.AddData("attachment_count_stickers", ((!string.IsNullOrEmpty(request.ObjectAttachmentId)) ? 1 : 0).ToString());
			e.AddData("attachment_count_photos", num2.ToString());
			e.AddData("attachment_count_audio", num3.ToString());
			e.AddData("attachment_count_video", num4.ToString());
			e.AddData("attachment_source", mediaSource.ToString());
			e.AddData("attachment_size_total_bytes", num.ToString());
		}

		private static void AddErrorData(Error error, AddEventRequest e)
		{
			e.AddData("error_title", error.Title ?? string.Empty).AddData("error_message", error.Message ?? string.Empty).AddData("error_code", error.Code.ToString())
				.AddData("error_exception", (error.Exception == null) ? string.Empty : ((object)error.Exception).GetType().Name);
		}
	}
}

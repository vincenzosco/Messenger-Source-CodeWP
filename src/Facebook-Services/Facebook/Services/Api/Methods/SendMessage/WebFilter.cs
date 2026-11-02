using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SendMessage
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request graph;
			if (request.HasServerThreadId())
			{
				graph = CreateGraphRequest(request).SetHttpMethod(Method.Post).AddParam("id", request.ServerThreadId);
			}
			else
			{
				List<string> ids = request.Users.Select((MailUser user) => user.Id).ToList();
				graph = CreateGraphRequest(request, "me/threads").SetHttpMethod(Method.Post).AddParam("to", CreateRecipientList(ids));
			}
			graph.AddParam("message", request.Message ?? string.Empty).AddParam("return_structure", "true").AddParam("offline_threading_id", request.OfflineThreadingId);
			if (request.Location != null && request.Location.IsValidLocation())
			{
				graph.AddParam("coordinates", request.Location.CreateLocationObject());
			}
			if (!string.IsNullOrWhiteSpace(request.ObjectAttachmentId))
			{
				graph.AddParam("object_attachment", request.ObjectAttachmentId);
			}
			if (request.HasMedia())
			{
				graph.AddParam("media", JsonConvert.SerializeObject(request.MediaFbids));
			}
			else if (request.HasFiles())
			{
				graph.ExtendTimeout = true;
				if (request.FilePaths.Count == 1)
				{
					Attachment attachment = request.FilePaths[0];
					if (attachment.IsAudio)
					{
						graph.AddParam("audio_type", "VOICE_MESSAGE");
						graph.AddParam("audio_duration", attachment.AudioDurationMs.ToString());
					}
				}
				graph.AddParam("attached_files", CreatePhotoList(request.FilePaths));
				for (int num = 0; num < request.FilePaths.Count; num++)
				{
					graph.AddParam($"file_{num}", request.FilePaths[num].Path, request.FilePaths[num].MimeType, request.FilePaths[num].Name);
				}
			}
			if (request.CopyMessageId != null)
			{
				graph.AddParam("copy_message", request.CopyMessageId);
			}
			if (request.CopyAttachmentId != null)
			{
				graph.AddParam("copy_attachment", request.CopyAttachmentId);
			}
			long startMs = DateExtensions.MSecsSinceEpoch();
			Response response = ParseResponse<Response>(await ExecuteHttpRequest(graph));
			SendEventLogger.LogSendState(extraFailureReason: request.GetExtraFailureReason(startMs, Context.ApplicationState), request: request, service: Context.AnalyticsService, method: SendEventLogger.SendMethod.Graph, response: response);
			if (!response.HasError() && !string.IsNullOrWhiteSpace(request.ServerThreadId))
			{
				response.ThreadId = request.ServerThreadId;
			}
			if (response.OfflineId == null)
			{
				response.OfflineId = request.OfflineThreadingId;
			}
			return response;
		}

		private static string CreatePhotoList(List<Attachment> photosAttachments)
		{
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartArray();
			for (int i = 0; i < photosAttachments.Count; i++)
			{
				jsonTextWriter.WriteValue($"file_{i}");
			}
			jsonTextWriter.WriteEndArray();
			return stringWriter.ToString();
		}

		private static string CreateRecipientList(IEnumerable<string> ids)
		{
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartArray();
			foreach (string id in ids)
			{
				jsonTextWriter.WriteStartObject();
				jsonTextWriter.WriteProperty("type", "id");
				jsonTextWriter.WriteProperty("id", id);
				jsonTextWriter.WriteEndObject();
			}
			jsonTextWriter.WriteEndArray();
			return stringWriter.ToString();
		}
	}
}

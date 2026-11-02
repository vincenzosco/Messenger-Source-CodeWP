using System;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.MediaItemUpload
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		private const string MediaFileKey = "media_file";

		private const string MediaThumbnailFileKey = "video_thumbnail";

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request graph;
			switch (request.Type)
			{
			case MediaType.Image:
				graph = CreateGraphRequest(request, "me/message_images");
				graph.AddParam("image_type", GetImageType(request.MediaAttachment.Source));
				break;
			case MediaType.Video:
				graph = CreateGraphRequest(request, "messagevideoattachment");
				graph.AddParam("video_type", GetVideoType(request.MediaAttachment.Source));
				break;
			default:
				throw new Exception("Unknown media type upload attempted.");
			}
			graph.SetHttpMethod(Method.Post);
			graph.ExtendTimeout = true;
			graph.AddParam("attached_files", CreateAttachedFilesList(request));
			graph.AddParam("media_file", request.MediaAttachment.Path, request.MediaAttachment.MimeType, request.MediaAttachment.Name);
			if (request.Type == MediaType.Video && request.MediaAttachment.HasThumbnail)
			{
				string thumbnailPath = request.MediaAttachment.ThumbnailPath;
				graph.AddParam("video_thumbnail", thumbnailPath, FileUtil.GetImageMimeTypeFromFilePath(thumbnailPath), FileUtil.GetName(thumbnailPath));
			}
			return await ExecuteHttpRequestAndParseResponse<Response>(graph);
		}

		private string GetImageType(MediaSource source)
		{
			if (source == MediaSource.QuickCam)
			{
				return "MESSENGER_CAM";
			}
			return "FILE_ATTACHMENT";
		}

		private string GetVideoType(MediaSource source)
		{
			if (source == MediaSource.QuickCam)
			{
				return "RECORDED_VIDEO";
			}
			return "FILE_ATTACHMENT";
		}

		private string CreateAttachedFilesList(Request request)
		{
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartArray();
			jsonTextWriter.WriteValue("media_file");
			if (request.MediaAttachment.HasThumbnail)
			{
				jsonTextWriter.WriteValue("video_thumbnail");
			}
			jsonTextWriter.WriteEndArray();
			return stringWriter.ToString();
		}
	}
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Methods.MediaItemUpload;

namespace Facebook.Services.Api.Methods.SendMessage
{
	[Bundle(BundleType.BackgroundTransfer, typeof(Request), typeof(Response))]
	public class MediaUploadFilter : FacebookApiFilter<Request, Response>
	{
		public MediaUploadFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			if (request.HasUploadableMedia())
			{
				request.InvokeIsCancellableAction(true);
				Error error = null;
				List<string> fbids = new List<string>();
				try
				{
					for (int i = 0; i < request.FilePaths.Count; i++)
					{
						Attachment attachment = request.FilePaths[i];
						Facebook.Services.Api.Methods.MediaItemUpload.Request itemRequest = Facebook.Services.Api.Methods.MediaItemUpload.Request.CreateFrom(request);
						itemRequest.MediaAttachment = attachment;
						if (attachment.IsPhoto)
						{
							itemRequest.Type = MediaType.Image;
						}
						else if (attachment.IsVideo)
						{
							itemRequest.Type = MediaType.Video;
						}
						long startMs = DateExtensions.MSecsSinceEpoch();
						Facebook.Services.Api.Methods.MediaItemUpload.Response itemResponse = await Context.ApiService.ExecuteAsync<Facebook.Services.Api.Methods.MediaItemUpload.Request, Facebook.Services.Api.Methods.MediaItemUpload.Response>(itemRequest);
						long endMs = DateExtensions.MSecsSinceEpoch();
						SendEventLogger.LogMediaUploadEvent(extraFailureReason: request.GetExtraFailureReason(startMs, Context.ApplicationState), sendRequest: request, uploadRequest: itemRequest, service: Context.AnalyticsService, response: itemResponse, uploadDuration: endMs - startMs, attachmentIndex: i);
						if (itemResponse != null && !itemResponse.HasError())
						{
							fbids.Add(itemResponse.Fbid);
							continue;
						}
						error = ((itemResponse != null) ? itemResponse.Error : new Error
						{
							Message = string.Format("Failed to upload media at path: {0}", new object[1] { attachment.Path })
						});
						break;
					}
				}
				finally
				{
					request.InvokeIsCancellableAction(false);
				}
				if (error != null)
				{
					Response response = new Response();
					response.Error = error;
					return response;
				}
				request.MediaFbids = fbids;
			}
			return await next.ExecuteAsync<Request, Response>(request);
		}
	}
}

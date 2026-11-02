using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Common;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.SetThreadPicture
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
			Facebook.Services.Http.Request batchRequest = CreateBatchRequest(request);
			Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(batchRequest);
			Type[] responseBodyTypes = new Type[2]
			{
				typeof(BoolResponse),
				typeof(ThreadPictureResponse)
			};
			BatchGraphResponse batchResponse = FacebookApiFilter<Request, Response>.ParseBatchGraphResponse(httpResponse, responseBodyTypes);
			if (batchResponse.HasError())
			{
				Response response = new Response();
				response.Error = batchResponse.Error;
				return response;
			}
			if (batchResponse.Responses == null || batchResponse.Responses.Count != 2)
			{
				Error error = new Error();
				error.Message = "Graph Batch API didn't return a valid number of responses.";
				Error error2 = error;
				Response response2 = new Response();
				response2.Error = error2;
				return response2;
			}
			BoolResponse setThreadPictureResponse = batchResponse.Responses[0].Body as BoolResponse;
			if (setThreadPictureResponse == null || !setThreadPictureResponse.Succeeded)
			{
				Error error3;
				if (setThreadPictureResponse != null)
				{
					error3 = setThreadPictureResponse.Error;
				}
				else
				{
					Error error4 = new Error();
					error4.Message = "No result for setthreadimage was returned by the server";
					error3 = error4;
				}
				Error error5 = error3;
				Response response3 = new Response();
				response3.Error = error5;
				return response3;
			}
			Response response4 = new Response();
			ThreadPictureResponse fetchThreadPictureResponse = batchResponse.Responses[1].Body as ThreadPictureResponse;
			if (fetchThreadPictureResponse != null && !string.IsNullOrEmpty(fetchThreadPictureResponse.PictureHash))
			{
				response4.PictureHash = fetchThreadPictureResponse.PictureHash;
			}
			return response4;
		}

		private Facebook.Services.Http.Request CreateSetThreadPictureRequest(Request request)
		{
			return CreateGraphRequest(request, "method/messaging.setthreadimage").SetHttpMethod(Method.Post).AddParam("name", "set-thread-image").AddParam("tid", request.ThreadId)
				.AddParam("attached_files", "image");
		}

		private Facebook.Services.Http.Request CreateFetchThreadPictureRequest(Request request)
		{
			return CreateGraphRequest(request, "fql").AddParam("q", string.Format("SELECT pic_hash FROM unified_thread WHERE thread_id = '{0}'", new object[1] { request.ThreadId })).AddParam("name", "fetch-thread-image").AddParam("depends_on", "set-thread-image");
		}

		private Facebook.Services.Http.Request CreateBatchRequest(Request request)
		{
			List<Facebook.Services.Http.Request> list = new List<Facebook.Services.Http.Request>();
			list.Add(CreateSetThreadPictureRequest(request));
			list.Add(CreateFetchThreadPictureRequest(request));
			List<Facebook.Services.Http.Request> requests = list;
			string imageMimeTypeFromFilePath = FileUtil.GetImageMimeTypeFromFilePath(request.ImagePath);
			string fileName = "image_file" + Path.GetExtension(request.ImagePath);
			return CreateBatchGraphRequest(request, requests).AddParam("image", request.ImagePath, imageMimeTypeFromFilePath, fileName);
		}
	}
}

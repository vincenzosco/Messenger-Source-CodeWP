using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Methods.FetchThread;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.CreateGroup
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		private Facebook.Services.Http.Request CreateGroupRequest(Request request)
		{
			string value = string.Format("name={0}&recipients={1}", new object[2]
			{
				Url.Encode(request.ThreadName),
				Url.Encode(JsonConvert.SerializeObject(request.ThreadParticipants))
			});
			return CreateGraphRequest(request, "me/group_threads").SetHttpMethod(Method.Post).AddParam("body", value).AddParam("name", "create-group");
		}

		private Tuple<Facebook.Services.Http.Request, string, string> CreatePhotoRequest(Request request)
		{
			if (string.IsNullOrWhiteSpace(request.ImagePath))
			{
				return null;
			}
			Facebook.Services.Http.Request request2 = CreateGraphRequest(request, "method/messaging.setthreadimage").SetHttpMethod(Method.Post).AddParam("tid", "{result=create-group:$.thread_id}").AddParam("name", "set-picture")
				.AddParam("depends_on", "create-group");
			string imageMimeTypeFromFilePath = FileUtil.GetImageMimeTypeFromFilePath(request.ImagePath);
			string text = "image_file" + Path.GetExtension(request.ImagePath);
			if (string.IsNullOrWhiteSpace(imageMimeTypeFromFilePath) || string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			request2.AddParam("attached_files", "image");
			return Tuple.Create(request2, imageMimeTypeFromFilePath, text);
		}

		private Facebook.Services.Http.Request CreateThreadBatchRequest(Request request)
		{
			List<Facebook.Services.Http.Request> list = new List<Facebook.Services.Http.Request>();
			list.Add(CreateGroupRequest(request));
			Tuple<Facebook.Services.Http.Request, string, string> tuple = CreatePhotoRequest(request);
			string text = ((tuple != null) ? tuple.Item2 : null);
			string text2 = ((tuple != null) ? tuple.Item3 : null);
			if (tuple != null)
			{
				list.Add(tuple.Item1);
			}
			Facebook.Services.Http.Request request2 = CreateBatchGraphRequest(request, list);
			if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2))
			{
				request2.AddParam("image", request.ImagePath, text, text2);
			}
			return request2;
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request batchRequest = CreateThreadBatchRequest(request);
			Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(batchRequest);
			Type[] responseBodyTypes = new Type[2]
			{
				typeof(CreateGroupThreadResponse),
				null
			};
			BatchGraphResponse response = FacebookApiFilter<Request, Response>.ParseBatchGraphResponse(httpResponse, responseBodyTypes);
			if (response.HasError())
			{
				Response response2 = new Response();
				response2.Error = response.Error;
				return response2;
			}
			if (response.Responses == null || response.Responses.Count < 1)
			{
				Error error = new Error();
				error.Message = "Graph Batch API didn't return a valid number of responses";
				Error error2 = error;
				Response response3 = new Response();
				response3.Error = error2;
				return response3;
			}
			CreateGroupThreadResponse createThreadResponse = response.Responses[0].Body as CreateGroupThreadResponse;
			if (createThreadResponse == null || createThreadResponse.HasError())
			{
				Error error3;
				if (createThreadResponse != null)
				{
					error3 = createThreadResponse.Error;
				}
				else
				{
					Error error4 = new Error();
					error4.Message = "No create thread result was returned by the server";
					error3 = error4;
				}
				Error error5 = error3;
				Response response4 = new Response();
				response4.Error = error5;
				return response4;
			}
			Facebook.Services.Api.Methods.FetchThread.Request threadRequest = Facebook.Services.Api.Methods.FetchThread.Request.ByThreadId(createThreadResponse.ThreadId);
			threadRequest.AccessToken = request.AccessToken;
			threadRequest.CancelToken = request.CancelToken;
			Facebook.Services.Api.Methods.FetchThread.Response threadResponse = await ExecuteApiRequest<Facebook.Services.Api.Methods.FetchThread.Request, Facebook.Services.Api.Methods.FetchThread.Response>(threadRequest);
			MailThread thread = null;
			if (!threadResponse.HasError())
			{
				thread = threadResponse.Thread;
			}
			return new Response
			{
				Thread = thread
			};
		}
	}
}

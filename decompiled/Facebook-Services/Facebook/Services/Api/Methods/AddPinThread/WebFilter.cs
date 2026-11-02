using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Methods.FetchThread;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.AddPinThread;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		List<Facebook.Services.Http.Request> requests = new List<Facebook.Services.Http.Request>();
		Facebook.Services.Http.Request pin = GetPinRequest(request);
		Facebook.Services.Http.Request image = GetPictureRequest(request);
		Facebook.Services.Http.Request name = GetNameRequest(request);
		requests.Add(pin);
		if (image != null)
		{
			requests.Add(image);
		}
		if (name != null)
		{
			requests.Add(name);
		}
		Facebook.Services.Http.Request batchRequest = CreateBatchGraphRequest(request, requests);
		if (image != null)
		{
			string imageMimeTypeFromFilePath = FileUtil.GetImageMimeTypeFromFilePath(request.ImagePath);
			string fileName = "image_file" + Path.GetExtension(request.ImagePath);
			batchRequest.AddParam("image", request.ImagePath, imageMimeTypeFromFilePath, fileName);
		}
		Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(batchRequest);
		Type[] responseBodyTypes = new Type[1] { typeof(Response) };
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
		Response pinResponse = response.Responses[0].Body as Response;
		if (pinResponse != null && !pinResponse.HasError())
		{
			Facebook.Services.Api.Methods.FetchThread.Request threadRequest = Facebook.Services.Api.Methods.FetchThread.Request.ByThreadId(request.ThreadId);
			threadRequest.AccessToken = request.AccessToken;
			threadRequest.CancelToken = request.CancelToken;
			threadRequest.ForceToServer = true;
			Facebook.Services.Api.Methods.FetchThread.Response threadResponse = await ExecuteApiRequest<Facebook.Services.Api.Methods.FetchThread.Request, Facebook.Services.Api.Methods.FetchThread.Response>(threadRequest);
			if (!threadResponse.HasError())
			{
				pinResponse.Thread = threadResponse.Thread;
			}
		}
		return pinResponse ?? new Response
		{
			Error = new Error
			{
				Message = "No create thread result was returned by the server"
			}
		};
	}

	private Facebook.Services.Http.Request GetPinRequest(Request request)
	{
		string path = $"{Url.Encode(request.ThreadId)}/pin";
		return CreateGraphRequest(request, path).SetHttpMethod(Method.Post);
	}

	private Facebook.Services.Http.Request GetNameRequest(Request request)
	{
		if (string.IsNullOrWhiteSpace(request.NewName))
		{
			return null;
		}
		string value = $"id={Url.Encode(request.ThreadId)}&tid={Url.Encode(request.ThreadId)}&name={Url.Encode(request.NewName)}&offline_threading_id={Url.Encode(request.OfflineThreadingId)}";
		return CreateGraphRequest(request, "threadname").AddParam("body", value).SetHttpMethod(Method.Post);
	}

	private Facebook.Services.Http.Request GetPictureRequest(Request request)
	{
		if (string.IsNullOrWhiteSpace(request.ImagePath))
		{
			return null;
		}
		string imageMimeTypeFromFilePath = FileUtil.GetImageMimeTypeFromFilePath(request.ImagePath);
		string value = "image_file" + Path.GetExtension(request.ImagePath);
		if (string.IsNullOrWhiteSpace(imageMimeTypeFromFilePath) || string.IsNullOrWhiteSpace(value))
		{
			return null;
		}
		return CreateGraphRequest(request, "picture").AddParam("id", request.ThreadId).AddParam("tid", request.ThreadId).AddParam("name", request.NewName)
			.AddParam("attached_files", "image")
			.SetHttpMethod(Method.Post);
	}
}

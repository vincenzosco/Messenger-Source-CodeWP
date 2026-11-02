using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.BackgroundTransfer;
using Facebook.Base.Extensions;
using Facebook.Base.Network;
using Facebook.Models;
using Facebook.Services.Http;

namespace Facebook.Services.BackgroundTransfer;

public class Service : Facebook.Services.Http.Service
{
	private readonly AbstractBackgroundTransferClient _backgroundTransferClient;

	private AbstractBackgroundTransferOperation _backgroundTransferOperation;

	public Service(FacebookContext context, IFileAccess access, INetworkStatus status, AbstractBackgroundTransferClient backgroundTransferClient, IUserAgentGenerator generator)
		: base(context, access, status, generator)
	{
		_backgroundTransferClient = backgroundTransferClient;
		Register<Request, Response>(CreateBackgroundTransferTask);
	}

	private async Task<Response> CreateBackgroundTransferTask(Request request)
	{
		int num = default;
		_ = num;
		_ = 0;
		try
		{
			if (request.Params == null || !request.Params.Any())
			{
				return null;
			}
			_backgroundTransferClient.InitializeBackgroundTransfer(request.TransferOperation);
			_backgroundTransferClient.SetRequestHeaders(request.Headers);
			List<KeyValuePair<string, Facebook.Services.Http.Request.Param>> fileParams = request.Params.Where((KeyValuePair<string, Facebook.Services.Http.Request.Param> p) => !string.IsNullOrEmpty(p.Value.FileName) && !string.IsNullOrEmpty(p.Value.FilePath)).ToList();
			List<KeyValuePair<string, Facebook.Services.Http.Request.Param>> formDataParams = request.Params.Except(fileParams).ToList();
			List<FileDataParameter> fileDataParameters = fileParams.Select((KeyValuePair<string, Facebook.Services.Http.Request.Param> kvp) => new FileDataParameter
			{
				Filename = kvp.Value.FileName,
				Filepath = kvp.Value.FilePath,
				ContentType = kvp.Value.MimeType,
				ParameterName = kvp.Value.Name
			}).ToList();
			List<FormDataParameter> formDataParameters = formDataParams.Select((KeyValuePair<string, Facebook.Services.Http.Request.Param> kvp) => new FormDataParameter
			{
				Body = kvp.Value.Value,
				ContentType = kvp.Value.MimeType,
				ParameterName = kvp.Value.Name
			}).ToList();
			_backgroundTransferOperation = await _backgroundTransferClient.CreateTransferOperationAsync(request.TransferOperation, Facebook.Services.Http.Service.GetHttpUrl(request), fileDataParameters, formDataParameters, request.FailureMessage);
			_backgroundTransferOperation.ProgressCallback = request.ProgressCallback;
			_backgroundTransferOperation.StartTransfer(request.CancelToken).DoNotWait();
			return new Response
			{
				Operation = _backgroundTransferOperation
			};
		}
		catch (Exception)
		{
			Response response = new Response();
			response.Error = new Error
			{
				Message = "Transfer failed"
			};
			response.Operation = _backgroundTransferOperation;
			return response;
		}
	}
}

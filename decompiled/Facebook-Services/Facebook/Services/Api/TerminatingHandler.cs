using System;
using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Services.Api;

internal class TerminatingHandler : IHandler
{
	public Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : Request where TResponse : Response
	{
		string message = string.Format("Request {0} wanting a response {1} was unhandled", new object[2]
		{
			typeof(TRequest).FullName,
			typeof(TResponse).FullName
		});
		throw new NotImplementedException(message);
	}
}

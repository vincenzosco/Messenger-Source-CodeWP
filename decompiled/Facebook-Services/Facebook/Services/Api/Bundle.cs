using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Services.Api;

public class Bundle : IFilter
{
	private readonly object[] _args;

	private readonly Dictionary<string, Lazy<object>> _handlerInstances = new Dictionary<string, Lazy<object>>();

	public Bundle(params object[] args)
	{
		_args = args;
	}

	private string GetHash(Type request, Type response)
	{
		return string.Format("{0}_{1}", new object[2]
		{
			request.GetHashCode(),
			response.GetHashCode()
		});
	}

	public void Add(Type request, Type response, Type handler)
	{
		_handlerInstances[GetHash(request, response)] = new Lazy<object>(() => Activator.CreateInstance(handler, _args), isThreadSafe: true);
	}

	public virtual async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, IHandler next) where TRequest : Request where TResponse : Response
	{
		string hash = GetHash(typeof(TRequest), typeof(TResponse));
		if (_handlerInstances.ContainsKey(hash))
		{
			IFilterImpl<TRequest, TResponse> handlerInstance = (IFilterImpl<TRequest, TResponse>)_handlerInstances[hash].Value;
			return await handlerInstance.ExecuteAsync(request, next);
		}
		return await next.ExecuteAsync<TRequest, TResponse>(request);
	}
}

using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Services.Api;

public class FilterChain : IHandler
{
	private readonly IFilter _filter;

	private readonly IHandler _next;

	public FilterChain(IFilter filter, IHandler next)
	{
		_filter = filter;
		_next = next;
	}

	public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : Request where TResponse : Response
	{
		return await _filter.ExecuteAsync<TRequest, TResponse>(request, _next);
	}
}

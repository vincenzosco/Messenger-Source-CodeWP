using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Services.Api;

public abstract class Filter<TRequest, TResponse> : IFilterImpl<TRequest, TResponse> where TRequest : Request where TResponse : Response
{
	protected ServiceContext Context { get; private set; }

	protected Filter(ServiceContext context)
	{
		Context = context;
	}

	public abstract Task<TResponse> ExecuteAsync(TRequest request, IHandler next);
}

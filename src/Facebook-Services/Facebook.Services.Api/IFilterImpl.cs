using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Services.Api
{
	public interface IFilterImpl<in TRequest, TResponse> where TRequest : Request where TResponse : Response
	{
		Task<TResponse> ExecuteAsync(TRequest request, IHandler next);
	}
}

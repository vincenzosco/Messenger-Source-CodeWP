using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Services.Api;

public interface IFilter
{
	Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, IHandler next) where TRequest : Request where TResponse : Response;
}

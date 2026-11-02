using System.Threading.Tasks;
using Facebook.Base;

namespace Facebook.Services.Api;

public interface IHandler
{
	Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : Request where TResponse : Response;
}
